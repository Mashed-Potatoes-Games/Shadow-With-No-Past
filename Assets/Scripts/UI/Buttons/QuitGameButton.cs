using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGameButton : ButtonInstructions
{
    protected override void OnClick() => Application.Quit();
}
