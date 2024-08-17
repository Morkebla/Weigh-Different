using System.Collections.Generic;
using UnityEngine;

public class TriggerPlate : MonoBehaviour
{
    [SerializeField] private float PlateMass;

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

            // Check if the mass has changed
            if (rb.mass != trackedObject.LastKnownMass)
            {
                Debug.Log("Mass updated for " + rb.gameObject.name + ": new mass = " + rb.mass);
                trackedObject.LastKnownMass = rb.mass; 
            }

            
            Collider2D objectCollider = rb.GetComponent<Collider2D>();
            if (objectCollider != null && IsObjectPartiallyInside(objectCollider))
            {
                totalMass += rb.mass;
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
