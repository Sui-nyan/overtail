using UnityEngine;
using Overtail;


namespace Overtail
{
    [RequireComponent(typeof(global::PlayerMove))]
    public class Player : MonoBehaviour
    {
        global::PlayerMove movement;
        [SerializeField] private PersistentPlayerData persistantData;

        private void Start()
        {
            gameObject.transform.position = persistantData.playerSerializable.Position;
            movement = gameObject.GetComponent<global::PlayerMove>();
        }

        /*
        void LateUpdate()
        {
            if (movement.IsMoving)
                persistantData.playerSerializable.Position = gameObject.transform.position;
        }*/
    }
}