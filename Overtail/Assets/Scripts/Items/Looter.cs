using UnityEngine;

namespace Overtail.Items
{
    public class Looter : MonoBehaviour
    {
        [SerializeReference] private Lootable nearestLootable;
        [SerializeField] private float radius = .5f;

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) nearestLootable?.Interact();
        }

        void FixedUpdate()
        {
            nearestLootable = null;

            var pos = gameObject.transform.position;
            if (Physics2D.OverlapCircle(pos, radius) == null) return;

            var nearby = Physics2D.OverlapCircleAll(pos, radius);

            float dist = Mathf.Infinity;

            foreach (var obj in nearby)
            {
                if (!obj.gameObject.TryGetComponent<Lootable>(out var component)) continue;

                var d = Vector2.Distance(pos, obj.transform.position);

                if (d > dist) continue;

                dist = d;
                nearestLootable = component;
            }
        }
    }
}
