using Overtail.Dialogue;
using UnityEngine;

namespace Overtail.PlayerModule
{
    public class Interactor : MonoBehaviour
    {
        private IInteractable nearbyInteractable;
        [SerializeField] private float radius = .5f;

        [SerializeField] private string debugName;

        void Start()
        {
            if (InputManager.Instance != null)
            {
                InputManager.Instance.KeyConfirm += Act;
            }
        }
        
        void FixedUpdate()
        {
            nearbyInteractable = null;
            debugName = "";

            var pos = gameObject.transform.position;
            if (Physics2D.OverlapCircle(pos, radius) == null) return;

            var nearby = Physics2D.OverlapCircleAll(pos, radius);

            float dist = Mathf.Infinity;

            foreach (var obj in nearby)
            {
                if (!obj.gameObject.TryGetComponent<IInteractable>(out var component)) continue;

                var d = Vector2.Distance(pos, obj.transform.position);

                if (d > dist) continue;

                dist = d;
                nearbyInteractable = component;
                debugName = obj.name;
            }
        }

        public void Act()
        {
            nearbyInteractable?.Interact(this);
        }

        public bool CanAct()
        {
            return nearbyInteractable != null;
        }


        public IInteractable test()
        {
            return nearbyInteractable;
        }
    }
}
