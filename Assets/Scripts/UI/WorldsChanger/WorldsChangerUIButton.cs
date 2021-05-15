using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using ShadowWithNoPast.GameProcess;

[RequireComponent(typeof(Button))]
public class WorldsChangerUIButton : MonoBehaviour
{

    [SerializeField]
    private Button button;
    [SerializeField]
    private Image buttonImage;

    private WorldsChanger changer;

    private void Awake()
    {
        changer = FindObjectOfType<WorldsChanger>();
        button.onClick.AddListener(() => changer.ToggleActive());
    }

    
}
