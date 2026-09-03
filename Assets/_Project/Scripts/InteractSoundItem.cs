using UnityEngine;

public class InteractSoundItem : MonoBehaviour, IInteractable
{
    private AudioSource _audioSource;
    [SerializeField] private AudioClip _clip;

    private void Start()
    {
        _audioSource = gameObject.AddComponent<AudioSource>();
        _audioSource.clip = _clip;
    }

    public void Interact()
    {
        _audioSource.Stop();
        _audioSource.Play();
    }
}
