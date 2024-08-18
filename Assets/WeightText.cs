using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeightText : MonoBehaviour
{
    [SerializeField] private Object WeightTextPrefab;
    Rigidbody2D rb;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        WeightTextPrefab.GetComponentInChildren<TextMesh>().text = rb.mass.ToString();
    
    }


    void Update()
    {

            WeightTextPrefab.GetComponentInChildren<TextMesh>().text = rb.mass.ToString();
    }
}
