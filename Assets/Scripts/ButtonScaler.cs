using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{
    private Vector2 originalScale;
    private float originalMass;
    private int currentScaleIndex = 1;
    public float[] scaleMultipliers = { 0.5f, 1f, 2f, 5f };
    public float[] massMultipliers = { 0.1f, 1f, 10f, 100f };
    public ScaleButtonUI scaleButtonPrefab;
    private ScaleButtonUI scaleButtonUI;
    private Rigidbody2D rb;

    public int CurrentScaleIndex => currentScaleIndex;
    public int MaxScaleIndex => scaleMultipliers.Length - 1;
    public float CurrentScaleMultiplier => scaleMultipliers[currentScaleIndex];

    private void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        originalScale = transform.localScale;
        scaleButtonUI = Instantiate(scaleButtonPrefab);
        scaleButtonUI.buttonScaler = this;
        scaleButtonUI.transform.SetParent(canvas.transform, false);
        scaleButtonUI.gameObject.SetActive(false);

        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;

        SetScale(currentScaleIndex);

        var collider = gameObject.AddComponent<PolygonCollider2D>();
        collider.isTrigger = true;
    }

    public void IncreaseSize()
    {
        if (currentScaleIndex < scaleMultipliers.Length - 1)
        {
            currentScaleIndex++;
            SetScale(currentScaleIndex);
        }
    }

    public void DecreaseSize()
    {
        if (currentScaleIndex > 0)
        {
            currentScaleIndex--;
            SetScale(currentScaleIndex);
        }
    }

    private void SetScale(int index)
    {
        transform.localScale = originalScale * scaleMultipliers[index];
        rb.mass = originalMass * massMultipliers[index];
    }

    public void ShowButtons()
    {
        if (scaleButtonUI != null)
        {
            scaleButtonUI.gameObject.SetActive(true);
        }
    }

    public void HideButtons()
    {
        if (scaleButtonUI != null)
        {
            scaleButtonUI.gameObject.SetActive(false);
        }
    }

    private void OnMouseEnter()
    {
        ShowButtons();
    }

    private void OnMouseExit()
    {
        HideButtons();
    }
}
