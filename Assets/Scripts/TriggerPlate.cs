using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TriggerPlate : MonoBehaviour
{
    [SerializeField] private float PlateMass;
    [SerializeField] private TextMesh PlateText;
    [SerializeField] private PlateManager plateManager;

    private class TrackedObject
    {
        public Rigidbody2D Rigidbody;
        public bool IsStable;

        public TrackedObject(Rigidbody2D rb)
        {
            Rigidbody = rb;
            IsStable = false;
        }
    }

    private List<TrackedObject> objectsInField = new List<TrackedObject>();
    private float totalMass = 0f;

    private void Awake()
    {
        PlateText.text = PlateMass.ToString();
        plateManager.RegisterPlate(this);
    }

    private void Update()
    {
        RecalculateTotalMass();
        plateManager.CheckPlates();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableObject draggableObject = other.GetComponent<DraggableObject>();
        if (draggableObject != null)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                objectsInField.Add(new TrackedObject(rb));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        DraggableObject draggableObject = other.GetComponent<DraggableObject>();
        if (draggableObject != null)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                for (int i = 0; i < objectsInField.Count; i++)
                {
                    if (objectsInField[i].Rigidbody == rb)
                    {
                        objectsInField.RemoveAt(i);
                        break;
                    }
                }
            }
        }
    }

    private void RecalculateTotalMass()
    {
        totalMass = 0f;
        bool allObjectsStable = true;

        foreach (TrackedObject trackedObject in objectsInField)
        {
            Rigidbody2D rb = trackedObject.Rigidbody;
            if (rb.velocity.magnitude < 0.1f && !rb.isKinematic)
            {
                trackedObject.IsStable = true;
            }
            else
            {
                trackedObject.IsStable = false;
                allObjectsStable = false;
            }

            if (trackedObject.IsStable)
            {
                Collider2D objectCollider = rb.GetComponent<Collider2D>();
                if (objectCollider != null && IsObjectPartiallyInside(objectCollider))
                {
                    totalMass += rb.mass;
                }
            }
        }
    }

    public bool IsCorrectMass()
    {
        return Mathf.Approximately(totalMass, PlateMass);
    }

    private bool IsObjectPartiallyInside(Collider2D objectCollider)
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        return triggerCollider.bounds.Intersects(objectCollider.bounds);
    }
}
