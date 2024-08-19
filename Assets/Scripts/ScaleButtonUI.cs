using UnityEngine;
using UnityEngine.UI;

public class ScaleButtonUI : MonoBehaviour
{
    private Vector2 originalPlusButtonScale;
    private Vector2 originalMinusButtonScale;
    public ButtonScaler buttonScaler = null;
    public Button buttonPlus;
    public Button buttonMinus;
    public float distanceMultiplier = 5000f;

    private void Start()
    {
        originalPlusButtonScale = buttonPlus.transform.localScale;
        originalMinusButtonScale = buttonMinus.transform.localScale;
        UpdateButtonStates();
        UpdateButtonPositions();
        UpdateButtonSizes();
    }

    private void Update()
    {
        RectTransform canvasRect = transform.parent as RectTransform;
        Vector3 canvasSize = canvasRect.sizeDelta;

        Vector3 newPosition = Camera.main.WorldToScreenPoint(buttonScaler.transform.localPosition);
        transform.localPosition = newPosition - canvasSize * 0.5f;
    }

    public void IncreaseSize()
    {
        buttonScaler.IncreaseSize();
        UpdateButtonStates();
        UpdateButtonSizes();
        UpdateButtonPositions();
    }

    public void DecreaseSize()
    {
        buttonScaler.DecreaseSize();
        UpdateButtonStates();
        UpdateButtonSizes();
        UpdateButtonPositions();
    }

    private void UpdateButtonPositions()
    {
        Vector2 offset = new Vector2(buttonScaler.transform.localScale.x * distanceMultiplier, 0);

        RectTransform Panel = buttonPlus.transform.parent as RectTransform;
        Vector2 size = Panel.sizeDelta;
        size.x = 80 + offset.x * 50;
        Panel.sizeDelta = size;
    }

    private void UpdateButtonSizes()
    {
        float scale = Mathf.Pow(buttonScaler.CurrentScaleMultiplier, 0.3f);
        buttonPlus.transform.localScale = originalPlusButtonScale * scale;
        buttonMinus.transform.localScale = originalMinusButtonScale * scale;
    }

    private void UpdateButtonStates()
    {
        buttonMinus.interactable = buttonScaler.CurrentScaleIndex > 0;
        buttonPlus.interactable = buttonScaler.CurrentScaleIndex < buttonScaler.MaxScaleIndex;
    }
}
