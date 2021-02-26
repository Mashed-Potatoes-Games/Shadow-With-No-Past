using UnityEngine;
using UnityEditor;
using System.Collections;
using ShadowWithNoPast.GridObjects;

//This script will always be loaded
[InitializeOnLoad()]
class ObjectsPlacement : Editor
{
    
	static ObjectsPlacement()
    {
        //while editing files, this script will be loaded again, so we are removing the function from event.
		SceneView.duringSceneGui -= EditSceneGUI;
        //We are adding our own GUI handler to modify scene behaviour.
		SceneView.duringSceneGui += EditSceneGUI;
	}

	public static void EditSceneGUI(SceneView sceneView)
	{
		Event current = Event.current;

        if (current.type == EventType.DragPerform)
        {
            //DragAndDrop contains all the info about drag operation
            Object obj = DragAndDrop.objectReferences[0];

            if(obj is GameObject)
            {
                GameObject gameObject = (GameObject)obj;
                //We need to know, if the dragged game object has GridObject component, and is needed to be placed in the grid.
                GridObject gridObject = gameObject.GetComponent<GridObject>();

                //If our game object can't be placed on our grid, we just don't touch it at all, and the GUI behaves as usual.
                if (gridObject == null)
                {
                    return;
                }

                GridManagement grid = GetSelectedGrid();

                //The game will contain at least 2 grids for objects, so we need to be sure, that needed grid is selected in the scene hierarchy.
                if (grid == null)
                {
                    Debug.LogWarning("Select grid, you want your object be associated to!");
                }
                else
                {
                    //Trying to get the mouse position according to a scene, 
                    //while in the editor is a pain in the ass, Input don't work here, just let this code be...
                    Vector3 mousePos = Event.current.mousePosition;
                    Vector3 worldPos = HandleUtility.GUIPointToWorldRay(mousePos).origin;

                    //Remember, that Grid cells have left-bottom pivot.
                    Vector2Int cellPos = GridManagement.CellFromWorld(worldPos);
                    //If cell is occupied, we do nothing and notify the editor about this mistake on his part.
                    if (grid.GetCellStatus(cellPos) != CellStatus.Free)
                    {
                        Debug.LogWarning("You can set your object only to a free cell!");
                    }
                    else
                    {
                        InstantiateAndConnect(gameObject, grid, cellPos);
                    }
                }
                //This method "eats" the event, so no actious will be performed on DragRelease in in scene view.
                //Because of that nothing happens, when grid is not selected or cell isn't free.
                current.Use();
            }
        }
	}

    private static void InstantiateAndConnect(GameObject gameObject, GridManagement grid, Vector2Int cellPos)
    {
        //Drag and drop contains prefab reference, 
        //so we need to actually instantiate new one instead of modifying the referenced one.
        GameObject InstantiatedObj = (GameObject)PrefabUtility.InstantiatePrefab(gameObject);
        GridObject InstantiatedGridObj = InstantiatedObj.GetComponent<GridObject>();

        //We tweak instantiated obj values, so it will stick to a grid and occur on mouse position.
        InstantiatedObj.transform.SetParent(grid.ObjGrid.transform);
        InstantiatedGridObj.WorldGrid = grid;
        InstantiatedGridObj.CurrentPos = cellPos;

        grid.SetNewObjectTo(InstantiatedGridObj, cellPos);

        //We need to tell the editor helper of this object, so it can start working.
        ObjectEditor EditorScript = InstantiatedObj.GetComponent<ObjectEditor>();
        if (EditorScript != null)
        {
            EditorScript.IsConnectedToGrid = true;
        }
    }

    private static GridManagement GetSelectedGrid()
    {
        GridManagement MainGrid = null;
        //Selection containt information about the GameObject selected in the hierarchy and is always available.
        if (Selection.activeGameObject != null)
        {
            MainGrid = Selection.activeGameObject.GetComponent<GridManagement>();
            //First we try to see, is the selected object a MainGrid.
            if (MainGrid == null)
            {
                //User still can choose the GridThat contains only objects,
                //so the script will work exact way if either Main or Objects grid is selected.
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
