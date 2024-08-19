using System.Collections;
using System.Collections.Generic;
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
    public ScaleButtonUI scaleButtonUI;   
    private Rigidbody2D rb;

    public int CurrentScaleIndex => currentScaleIndex;
    public int MaxScaleIndex => scaleMultipliers.Length - 1;
    public float CurrentScaleMultiplier => scaleMultipliers[currentScaleIndex];

    void Start()
    {
        Canvas canvas = FindObjectOfType<Canvas>();

        originalScale = transform.localScale;
        scaleButtonUI = Instantiate<ScaleButtonUI>(scaleButtonPrefab);
        scaleButtonUI.buttonScaler = this;
        scaleButtonUI.transform.parent = canvas.transform;

        rb = GetComponent<Rigidbody2D>();
        originalMass = rb.mass;

        SetScale(currentScaleIndex);
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

}
