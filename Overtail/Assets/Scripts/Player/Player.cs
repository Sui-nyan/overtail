using UnityEngine;
using Overtail;


namespace Overtail
{
    [RequireComponent(typeof(global::PlayerMovement))]
    public class Player : MonoBehaviour
    {
        global::PlayerMovement movement;
        [SerializeField] private PersistentPlayerData persistantData;

        private void Start()
        {
            gameObject.transform.position = persistantData.playerSerializable.Position;
            movement = gameObject.GetComponent<global::PlayerMovement>();
        }

        /*
        void LateUpdate()
        {
            if (movement.IsMoving)
                persistantData.playerSerializable.Position = gameObject.transform.position;
        }*/
    }
}