using UnityEngine;

public class SoundBar : MonoBehaviour
{
    [SerializeField] public AudioClip DoorOpen;
    [SerializeField] public AudioClip DoorClosed;
    [SerializeField] public AudioClip DoorLocked;
    [SerializeField] public AudioClip DoorUnlocked;

    [SerializeField] public AudioClip PinError;
    [SerializeField] public AudioClip PinInput;
    [SerializeField] public AudioClip PinStart;

    [SerializeField] public AudioClip MenuAction;
    [SerializeField] public AudioClip PickItem;

    [SerializeField] public AudioClip MindModeTheme;

    [SerializeField] public AudioSource Player;

    public void PlayOnCharacter(AudioClip audio)
    {
        Player.PlayOneShot(audio);
    }
}
