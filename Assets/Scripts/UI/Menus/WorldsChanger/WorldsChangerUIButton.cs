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

    private void Start()
    {
        button.onClick.AddListener(() => Game.WorldsChanger.ToggleActive());
    }
}
