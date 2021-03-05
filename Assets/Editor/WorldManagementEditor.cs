using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

[CustomEditor(typeof(WorldManagement))]
class WorldManagementEditor : Editor
{
    private WorldManagement world;
    private const string labelsName = "CoordinateLabels";

    private void OnEnable()
    {
        world = (WorldManagement)target;
        if(world.ShowTileCoordinates)
        {
            AddCoordinateLabels();
        }
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        if(GUI.changed)
        {
            if (world.ShowTileCoordinates)
            {
                AddCoordinateLabels();
            } 
            else
            {
                DestroyCoordinateLabels();
            }
        }
    }

    #region Canvas and labels creation and deletion logic
    private void AddCoordinateLabels()
    {
        GameObject Canvas = CreateCanvas();
        SetupCanvas(Canvas);
        AddLabelsToCanvas(Canvas);
    }

    private void DestroyCoordinateLabels()
    {
        Transform labelsTransform = world.transform.Find(labelsName);
        if (labelsTransform != null)
        {
            DestroyImmediate(labelsTransform.gameObject);
        }
    }

    private GameObject CreateCanvas()
    {
        GameObject Canvas;
        Transform LabelsTransform = world.transform.Find(labelsName);

        if (LabelsTransform != null)
        {
            DestroyImmediate(LabelsTransform.gameObject);
        }
        Canvas = new GameObject(labelsName)
        {
            tag = "EditorOnly",
        };
        return Canvas;
    }

    private void SetupCanvas(GameObject Canvas)
    {
        Canvas canvas = Canvas.AddComponent<Canvas>();

        canvas.renderMode = RenderMode.WorldSpace;
        canvas.referencePixelsPerUnit = 256;
        Canvas.transform.SetParent(world.transform);

        RectTransform canvasRect = canvas.GetComponent<RectTransform>();

        canvasRect.anchoredPosition = new Vector3(0, 0, 0);
        canvasRect.sizeDelta = new Vector2(100, 100);
    }

    private void AddLabelsToCanvas(GameObject Canvas)
    {
        var groundTilemap = (GroundTilemap)serializedObject.FindProperty("ground").objectReferenceValue;
        var tilemap = groundTilemap.ground;
        for (int x = tilemap.cellBounds.min.x; x < tilemap.cellBounds.max.x; x++)
        {
            for (int y = tilemap.cellBounds.min.y; y < tilemap.cellBounds.max.y; y++)
            {
                if (tilemap.GetTile(new Vector3Int(x, y, 0)) != null)
                {
                    GameObject Label = CreateLabelOnCanvas(Canvas);
                    SetupLabel(Label, x, y);
                }
            }
        }
    }

    private static GameObject CreateLabelOnCanvas(GameObject LabelsCanvas)
    {
        GameObject Label = new GameObject("Label");
        Label.transform.parent = LabelsCanvas.transform;
        return Label;
    }

    private static void SetupLabel(GameObject Label, int x, int y)
    {
        Label.transform.position = new Vector3(x + WorldManagement.TileOffset, y + WorldManagement.TileOffset, 0);
        Label.transform.localScale = new Vector3(0.01f, 0.01f, 1);
        Text textComp = Label.AddComponent<Text>();
        textComp.text = x + "," + y;
        textComp.font = Resources.GetBuiltinResource<Font>("Arial.ttf");
        textComp.fontSize = 42;
        textComp.alignment = TextAnchor.MiddleCenter;

        EditorUtility.SetDirty(Label);
    }
    #endregion
}
