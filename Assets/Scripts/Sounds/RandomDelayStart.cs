using UnityEngine;

public class RandomDelayStart : MonoBehaviour
{
    [SerializeField] private float maxDelay;
    [SerializeField] private AudioSource audio;

    private void Start()
    {
        if (maxDelay == -1)
            maxDelay = audio.clip.length;
        float rand = Random.Range(0, maxDelay);

        Invoke(nameof(StartAudio), rand);

    }

    private void StartAudio()
    {
        audio.Play();
    }
}
