using ShadowWithNoPast.GameProcess;
using UnityEngine;

public class MainMenuButton : ButtonInstructions
{
    [SerializeField]
    private PauseMenuWrapper pauseMenu;
    protected override void OnClick()
    {
        pauseMenu.Resume();
        Game.SceneLoader.SwitchScene(SceneName.MainMenu);
    }
}