using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectsSoundBehaviour : MonoBehaviour
{
    DragController dragControler;

    [SerializeField] AudioClip pickUp;
    [SerializeField] AudioClip ScaleUp;
    [SerializeField] AudioClip ScaleDown;

    private AudioSource audioSource;

    private bool wasDragging = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        dragControler = FindObjectOfType<DragController>();
    }

    void Update()
    {
        if (dragControler != null)
        {
            if (dragControler.isDragging && !wasDragging)
            {
                audioSource.clip = pickUp;
                audioSource.Play();

                wasDragging = true;
            }
            if (!dragControler.isDragging)
            {
                wasDragging = false;
            }
        }
    }
}
