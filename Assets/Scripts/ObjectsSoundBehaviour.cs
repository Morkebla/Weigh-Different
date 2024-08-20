using UnityEngine;

public class ObjectsSoundBehaviour : MonoBehaviour
{
    public AudioClip pickUp;
    public AudioClip scaleUp;
    public AudioClip scaleDown;

    private AudioSource audioSource;
    private bool wasDragging = false;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        // Ensure the audio source component is present
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    void Update()
    {
        DragController dragControler = FindObjectOfType<DragController>();
        if (dragControler != null)
        {
            if (dragControler.isDragging && !wasDragging)
            {
                PlaySound(pickUp);
                wasDragging = true;
            }
            if (!dragControler.isDragging)
            {
                wasDragging = false;
            }
        }
    }

    public void PlayScaleUpSound()
    {
        PlaySound(scaleUp);
    }

    public void PlayScaleDownSound()
    {
        PlaySound(scaleDown);
    }

    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
