using System.Collections.Generic;
using UnityEngine;

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
        if (plateManager != null)
        {
            plateManager.RegisterPlate(this);
        }
        else
        {
            Debug.LogError("PlateManager reference is missing.");
        }
    }

    private void Update()
    {
        RecalculateTotalMass();
        plateManager?.CheckPlates();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        DraggableObject draggableObject = other.GetComponent<DraggableObject>();
        if (draggableObject != null)
        {
            Rigidbody2D rb = other.GetComponent<Rigidbody2D>();
            if (rb != null && !objectsInField.Exists(o => o.Rigidbody == rb))
            {
                objectsInField.Add(new TrackedObject(rb));
                Debug.Log("Object added: " + rb.name + " with mass: " + rb.mass);
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
                TrackedObject trackedObject = objectsInField.Find(o => o.Rigidbody == rb);
                if (trackedObject != null)
                {
                    objectsInField.Remove(trackedObject);
                    Debug.Log("Object removed: " + rb.name);
                }
            }
        }
    }

    private void RecalculateTotalMass()
    {
        totalMass = 0f;

        foreach (TrackedObject trackedObject in objectsInField)
        {
            Rigidbody2D rb = trackedObject.Rigidbody;
            trackedObject.IsStable = rb.velocity.magnitude < 0.1f && !rb.isKinematic;

            if (trackedObject.IsStable)
            {
                Collider2D objectCollider = rb.GetComponent<Collider2D>();
                if (objectCollider != null && IsObjectPartiallyInside(objectCollider))
                {
                    totalMass += rb.mass;
                    Debug.Log($"Adding mass of object: {rb.name}, mass: {rb.mass}. Total mass now: {totalMass}");
                }
            }
        }

        Debug.Log($"Recalculated Total Mass: {totalMass}, Plate Mass: {PlateMass}");
    }

    public bool IsCorrectMass()
    {
        bool isCorrect = Mathf.Approximately(totalMass, PlateMass);
        Debug.Log($"Is Correct Mass: {isCorrect}");
        return isCorrect;
    }

    private bool IsObjectPartiallyInside(Collider2D objectCollider)
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        return triggerCollider.bounds.Intersects(objectCollider.bounds);
    }
}
