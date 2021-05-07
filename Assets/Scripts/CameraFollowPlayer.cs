using ShadowWithNoPast.Entities;
using UnityEngine;

namespace Assets.Scripts
{
    [RequireComponent(typeof(Camera))]
    public class CameraFollowPlayer : MonoBehaviour
    {
        private GridObject player;

        private void Start()
        {
            player = GameObject.FindGameObjectWithTag("Player").GetComponent<GridObject>();
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