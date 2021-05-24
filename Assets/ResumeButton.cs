using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResumeButton : ButtonInstructions
{
    [SerializeField]
    private PauseMenuWrapper pauseMenu;
    protected override void OnClick()
    {
        pauseMenu.Resume();
    }
}
