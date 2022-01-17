using System.Collections;
using System.Collections.Generic;
using Overtail.Dialogue;
using TMPro;
using UnityEngine;

public class InteractablePopUp : MonoBehaviour
{
    public GameObject popup;


    [SerializeReference] private GameObject nearestInteractable;
    public bool readOnlyNearbyInteractable = false;
    [SerializeField] private float radius = .5f;
    [SerializeField] private Vector2 offset;
    
    void FixedUpdate()
    {
        Reset();

        var obj = LocateNearest();

        if(obj != null) Set(obj);


        popup.SetActive(nearestInteractable is not null);
    }

    private GameObject LocateNearest()
    {
        var pos = gameObject.transform.position;
        if (Physics2D.OverlapCircle(pos, radius) == null) return null;

        var nearby = Physics2D.OverlapCircleAll(pos, radius);

        float dist = Mathf.Infinity;

        foreach (var obj in nearby)
        {
            if (!obj.gameObject.TryGetComponent<IInteractable>(out var component)) continue;

            var d = Vector2.Distance(pos, obj.transform.position);

            if (d > dist) continue;

            dist = d;
            return obj.gameObject;
        }

        return null;
    }

    void Reset()
    {
        nearestInteractable = null;
        readOnlyNearbyInteractable = false;
    }

    void Set(GameObject interactable)
    {
        readOnlyNearbyInteractable = true;
        nearestInteractable = interactable;
        popup.transform.position = (Vector2)interactable.transform.position + offset;
        popup.GetComponentInChildren<TextMeshProUGUI>().text = "(x) " + interactable.name;
    }
}