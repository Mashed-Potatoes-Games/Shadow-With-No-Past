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
            player.Moved += Follow;
        }

        private void Follow(GridObject obj)
        {
            Vector3 newPos = obj.transform.position;
            newPos.z = transform.position.z;
            transform.position = newPos;
        }
    }
}