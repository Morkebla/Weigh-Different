using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    [SerializeField] private float PlateMass;
    [SerializeField] private Object PlateText;
    private class TrackedObject
    {
        public Rigidbody2D Rigidbody;
        public float LastKnownMass;

        public TrackedObject(Rigidbody2D rb)
        {
            Rigidbody = rb;
            LastKnownMass = rb.mass;
        }
    }

    private List<TrackedObject> objectsInField = new List<TrackedObject>();  
    private float totalMass = 0f;

    private void Awake()
    {
        PlateText.GetComponent<TextMesh>().text = PlateMass.ToString();
    }
    private void Update()
    {
        RecalculateTotalMass();
        CheckMass();
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
                Debug.Log("Object entered: " + other.name + " | Current total mass: " + totalMass);
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
                Debug.Log("Object exited: " + other.name + " | Current total mass: " + totalMass);
            }
        }
    }

    private void RecalculateTotalMass()
    {
        totalMass = 0f;

        foreach (TrackedObject trackedObject in objectsInField)
        {
            Rigidbody2D rb = trackedObject.Rigidbody;

            bool isObjectGrounded = rb.velocity.magnitude < 0.1f && !rb.isKinematic;
            float objectMass = isObjectGrounded ? rb.mass : 0f;

            // Check if the mass has changed
            if (objectMass != trackedObject.LastKnownMass)
            {
                Debug.Log("Mass updated for " + rb.gameObject.name + ": new mass = " + rb.mass);
                trackedObject.LastKnownMass = objectMass;
            }
            
            Collider2D objectCollider = rb.GetComponent<Collider2D>();
            if (objectCollider != null && IsObjectPartiallyInside(objectCollider))
            {
                totalMass += objectMass;
            }
        }

        Debug.Log("Recalculated total mass: " + totalMass);
    }

    private void CheckMass()
    {
        if (totalMass == PlateMass)
        {
            Debug.Log("Correct total mass detected: " + totalMass);
        }
        else
        {
            Debug.Log("Incorrect total mass detected: " + totalMass);
        }
    }

    private bool IsObjectPartiallyInside(Collider2D objectCollider)
    {
        Collider2D triggerCollider = GetComponent<Collider2D>();
        return triggerCollider.bounds.Intersects(objectCollider.bounds);
    }
}
