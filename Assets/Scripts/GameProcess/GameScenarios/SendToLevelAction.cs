using ShadowWithNoPast.Entities;
using ShadowWithNoPast.GameProcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class SendToLevelAction : ITriggerAction
{
    public SceneName scene;
    public void Action(GridObject trigerer)
    {
        Game.SceneLoader.SwitchScene(scene);
    }
}
