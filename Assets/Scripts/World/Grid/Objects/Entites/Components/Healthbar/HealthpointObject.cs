using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewHealthbarSprites", menuName = "HealthbarSprites", order = 10)]
public class HealthpointObject : ScriptableObject
{
    public Sprite HealthpointBackground;
    public Sprite HealthpointFill;

    public List<Sprite> MasksToCreateParticles;
}
