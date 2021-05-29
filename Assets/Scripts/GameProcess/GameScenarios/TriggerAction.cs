
using ShadowWithNoPast.Entities;

public interface ITriggerAction
{
    public void Action(GridObject trigerer, WorldPos startPos, WorldPos endPos);
}