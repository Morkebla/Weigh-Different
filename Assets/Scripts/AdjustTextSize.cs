using UnityEngine;

public class AdjustTextSize : MonoBehaviour
{
    private TextMesh textMesh;
    private float originalSize;

    void Start()
    {
        textMesh = GetComponent<TextMesh>();
        originalSize = textMesh.characterSize; 
    }

    void Update()
    {
        if (textMesh.text.Length > 3)
        {
            textMesh.characterSize = originalSize * (3f / textMesh.text.Length);
        }
        else
        {
            textMesh.characterSize = originalSize;
        }
    }
}
