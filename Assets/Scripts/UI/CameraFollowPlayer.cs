using ShadowWithNoPast.Entities;
using UnityEngine;

namespace ShadowWithNoPast.Utils
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowPlayer : MonoBehaviour
    {
        private GridEntity player;

        private void Start()
        {
            player = Player.Entity;
            player.Moved += (obj, start, end) => Follow(obj);
        }

        private void Follow(GridObject obj)
        {
            Vector3 Vector3Pos = obj.transform.position;
            Vector3Pos.z = transform.position.z;
            transform.position = Vector3Pos;
        }
    }
}