using UnityEngine;

public class DragControler : MonoBehaviour
{
    private GameObject draggedObject;
    private Vector2 offset;
    public bool isDragging = false;

    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleMouseInput(mousePos);
        
        if (isDragging)
        {
            DragObjectToMouse(mousePos);
        }
    }

    private void HandleMouseInput(Vector2 mousePos)
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!isDragging)
            {
                TryStartDragging(mousePos);
            }
        }
        else if (Input.GetMouseButtonUp(0) && isDragging)
        {
            StopDragging();
        }
    }

    private void TryStartDragging(Vector2 mousePos)
    {
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

        if (hit.collider != null && hit.collider.gameObject.GetComponent<DraggableObject>() != null)
        {
            draggedObject = hit.collider.gameObject;
            StartDragging(draggedObject, mousePos);
        }
    }

    private void StartDragging(GameObject obj, Vector2 mousePos)
    {
        offset = (Vector2)draggedObject.transform.position - mousePos;
        isDragging = true;

        var rigidBody = draggedObject.GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            rigidBody.isKinematic = true;
        }
    }

    private void DragObjectToMouse(Vector2 mousePos)
    {
        if (draggedObject != null)
        {
            draggedObject.transform.position = mousePos + offset;
        }
    }

    private void StopDragging()
    {
        var rigidBody = draggedObject.GetComponent<Rigidbody2D>();
        if (rigidBody)
        {
            rigidBody.isKinematic = false;
        }

        draggedObject = null;
        isDragging = false;
    }
}
