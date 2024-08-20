using UnityEngine;

public class DragController : MonoBehaviour
{
    private GameObject draggedObject;
    private Vector2 offset;
    public bool isDragging = false;

    public float hoverBufferDistance = 5f;

    private void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        HandleMouseInput(mousePos);

        if (isDragging)
        {
            DragObjectToMouse(mousePos);
        }
        else
        {
            CheckForHover(mousePos);
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

    private void CheckForHover(Vector2 mousePos)
    {
        ButtonScaler closestScaler = null;
        float closestDistance = float.MaxValue;

        foreach (ButtonScaler scaler in FindObjectsOfType<ButtonScaler>())
        {
            float distance = Vector2.Distance(mousePos, (Vector2)scaler.transform.position);

            if (distance < hoverBufferDistance && distance < closestDistance)
            {
                closestDistance = distance;
                closestScaler = scaler;
            }
        }

        foreach (ButtonScaler scaler in FindObjectsOfType<ButtonScaler>())
        {
            if (scaler == closestScaler)
            {
                scaler.ShowButtons();
            }
            else
            {
                scaler.HideButtons();
            }
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
