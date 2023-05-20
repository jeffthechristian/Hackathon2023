using UnityEngine;

public class SoundProximity : MonoBehaviour
{
    public Transform soundObject;
    public AudioSource audioSource;
    public float maxDistance = 10f;

    private void Update()
    {
        float distance = Vector3.Distance(transform.position, soundObject.position);
        float volume = Mathf.Lerp(1f, 0f, distance / maxDistance);
        audioSource.volume = volume;
    }
}
