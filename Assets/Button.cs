using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScaler : MonoBehaviour
{
    private Vector2 originalScale;
    private Vector2 originalPlusButtonScale;
    private Vector2 originalMinusButtonScale;
     private Vector2 originalPlusButtonPosition;
    private Vector2 originalMinusButtonPosition;
    private int currentScaleIndex = 1;
    private float[] scaleMultipliers = { 0.5f, 1f, 2f, 5f };
    public Button buttonPlus;
    public Button buttonMinus;
    public float distanceMultiplier = 1.5f;

    void Start()
    {
        originalScale = transform.localScale;
        originalPlusButtonScale = buttonPlus.transform.localScale;
        originalMinusButtonScale = buttonMinus.transform.localScale;

        originalPlusButtonPosition = buttonPlus.transform.localPosition;
        originalMinusButtonPosition = buttonMinus.transform.localPosition;

        SetScale(currentScaleIndex);
        UpdateButtonStates();
    }

    public void IncreaseSize()
    {
        if (currentScaleIndex < scaleMultipliers.Length - 1)
        {
            currentScaleIndex++;
            SetScale(currentScaleIndex);
            UpdateButtonStates();
        }
    }

    public void DecreaseSize()
    {
        if (currentScaleIndex > 0)
        {
            currentScaleIndex--;
            SetScale(currentScaleIndex);
            UpdateButtonStates();
        }
    }

    private void SetScale(int index)
    {
        
        transform.localScale = originalScale * scaleMultipliers[index];

        buttonPlus.transform.localScale = originalPlusButtonScale * scaleMultipliers[index];
        buttonMinus.transform.localScale = originalMinusButtonScale * scaleMultipliers[index];

        UpdateButtonPositions(index);
    }
    private void UpdateButtonPositions(int index)
    {
        
        Vector2 offset = new Vector2(originalScale.x * distanceMultiplier * scaleMultipliers[index], 0);

        
        buttonPlus.transform.localPosition = originalPlusButtonPosition - offset;
        buttonMinus.transform.localPosition = originalMinusButtonPosition + offset;
    }


    private void UpdateButtonStates()
    {
        
        buttonMinus.interactable = currentScaleIndex > 0;
        buttonPlus.interactable = currentScaleIndex < scaleMultipliers.Length - 1;
    }
}
