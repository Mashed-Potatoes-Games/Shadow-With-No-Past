using UnityEngine;
using UnityEditor;
using System.Collections;
using ShadowWithNoPast.GridObjects;

[InitializeOnLoad()]
class ObjectsPlacement : Editor
{
	void OnEnable()
	{
		SceneView.duringSceneGui += OnSceneGUI;
	}

	static ObjectsPlacement()
    {
		SceneView.duringSceneGui -= OnSceneGUI;
		SceneView.duringSceneGui += OnSceneGUI;
	}
	void OnDisable()
	{
		SceneView.duringSceneGui -= OnSceneGUI;
	}

	public static void OnSceneGUI(SceneView sceneView)
	{
		Event current = Event.current;

        if (current.type == EventType.DragPerform)
        {
            Object obj = DragAndDrop.objectReferences[0];

            if(obj is GameObject)
            {
                GameObject gameObject = (GameObject)obj;
                GridObject gridObject = gameObject.GetComponent<GridObject>();

                if (gridObject != null)
                {
                    GridManagement grid = GetSelectedGrid();

                    if (grid != null)
                    {
                        Vector3 mousePos = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition).origin;
                        Vector2Int gridPos = new Vector2Int(Mathf.RoundToInt(mousePos.x - 0.5f), 
                                                            Mathf.RoundToInt(mousePos.y - 0.5f));
                        if(grid.GetCellStatus(gridPos) == GridManagement.CellStatus.Free)
                        {
                            GameObject InstantiatedObj = (GameObject)PrefabUtility.InstantiatePrefab(gameObject);
                            GridObject InstantiatedGridObj = InstantiatedObj.GetComponent<GridObject>();

                            InstantiatedObj.transform.SetParent(grid.ObjGrid.transform);
                            InstantiatedGridObj.GlobalParentGrid = grid;
                            InstantiatedGridObj.CurrentPos = gridPos;

                            grid.SetNewObjectTo(InstantiatedGridObj, gridPos);

                            ObjectEditor EditorScript = InstantiatedObj.GetComponent<ObjectEditor>();
                            if(EditorScript != null)
                            {
                                EditorScript.IsConnectedToGrid = true;
                            }
                        }
                        else
                        {
                            Debug.LogWarning("You can set your object only to a free cell!");
                        }
                    }
                    else
                    {
                        Debug.LogWarning("Select grid, you want your object be associated to!");
                    }
                    current.Use();
                }
            }
            
        }
	}

    private static GridManagement GetSelectedGrid()
    {
        GridManagement MainGrid = null;

        if (Selection.activeGameObject != null)
        {
            MainGrid = Selection.activeGameObject.GetComponent<GridManagement>();
            if (MainGrid == null)
            {
                ObjectsGrid ObjGrid = Selection.activeGameObject.GetComponent<ObjectsGrid>();
                if (ObjGrid != null)
                {
                    MainGrid = ObjGrid.GetComponentInParent<GridManagement>();
                }
            }
        }
        return MainGrid;
    }


}
