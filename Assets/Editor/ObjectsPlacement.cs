using UnityEngine;
using UnityEditor;
using System.Collections;
using ShadowWithNoPast.Entities;
using ShadowWithNoPast.GameProcess;

//This script will always be loaded
[InitializeOnLoad()]
class ObjectsPlacement : Editor
{
    
	static ObjectsPlacement()
    {
		SceneView.duringSceneGui -= EditSceneGUI;
		SceneView.duringSceneGui += EditSceneGUI;
	}

	public static void EditSceneGUI(SceneView sceneView)
	{
		Event current = Event.current;

        if (current.type == EventType.DragPerform)
        {
            //DragAndDrop contains all the info about drag operation
            Object obj = DragAndDrop.objectReferences[0];

            if(obj is GameObject gameObject)
            {
                //We need to know, if the dragged game object has GridObject component, and is needed to be placed in the grid.
                GridObject gridObject = gameObject.GetComponent<GridObject>();

                //If our game object can't be placed on our grid, we just don't touch it at all, and the GUI behaves as usual.
                if (gridObject == null)
                {
                    return;
                }
                WorldsChanger changer = FindObjectOfType<WorldsChanger>();
                WorldManagement world = changer != null ? changer.CurrentlyActive : null;

                if (world == null)
                {
                    Debug.LogWarning("Worlds changer should be on scene and contain an active world.");
                }
                else
                {
                    //Trying to get the mouse position according to a scene, 
                    //while in the editor is a pain in the ass, Input don't work here, just let this code be...
                    Vector3 mousePos = Event.current.mousePosition;
                    Vector3 worldPos = HandleUtility.GUIPointToWorldRay(mousePos).origin;

                    //Remember, that Grid cells have left-bottom pivot.
                    Vector2Int cellPos = WorldManagement.CellFromWorld(worldPos);
                    //If cell is occupied, we do nothing and notify the editor about this mistake on his part.
                    if (world.GetCellStatus(cellPos) != CellStatus.Free)
                    {
                        Debug.LogWarning("You can set your object only to a free cell!");
                    }
                    else
                    {
                        InstantiateAndConnect(gameObject, new WorldPos(world, cellPos));
                    }
                }
                //This method "eats" the event, so no actious will be performed on DragRelease in in scene view.
                //Because of that nothing happens, when grid is not selected or cell isn't free.
                current.Use();
            }
        }
	}

    private static void InstantiateAndConnect(GameObject gameObject, WorldPos Pos)
    {
        //Drag and drop contains prefab reference, 
        //so we need to actually instantiate new one instead of modifying the referenced one.
        GameObject InstantiatedObj = (GameObject)PrefabUtility.InstantiatePrefab(gameObject);
        GridObject InstantiatedGridObj = InstantiatedObj.GetComponent<GridObject>();

        //We tweak instantiated obj values, so it will stick to a grid and occur on mouse position.
        Transform objGrid = Pos.World.GetComponentInChildren<ObjectsGrid>().transform;
        InstantiatedObj.transform.SetParent(objGrid);
        InstantiatedGridObj.SetNewPosition(Pos);

        Pos.World.SetNewObjectTo(InstantiatedGridObj, Pos.Vector);

        EditorUtility.SetDirty(InstantiatedGridObj);

        //We need to tell the editor helper of this object, so it can start working.
        ObjectEditor EditorScript = InstantiatedObj.GetComponent<ObjectEditor>();
        if (EditorScript != null)
        {
            EditorScript.IsConnectedToGrid = true;
        }
    }
}
