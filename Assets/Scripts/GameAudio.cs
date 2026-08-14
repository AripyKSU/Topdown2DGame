using UnityEngine;

public class GameAudio : MonoBehaviour
{
    public static GameAudio I { get; private set; }

    [SerializeField] private AudioSource bgmSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioClip bgmClip;
    [SerializeField] private AudioClip moneyPickupClip;
    [SerializeField] private AudioClip hurtClip;

    private void Awake()
    {
        if (I != null && I != this)
        {
            Destroy(gameObject);
            return;
        }

        I = this;
        DontDestroyOnLoad(gameObject);

        if (bgmSource != null && bgmClip != null)
        {
            bgmSource.clip = bgmClip;
            bgmSource.loop = true;
            if (!bgmSource.isPlaying) bgmSource.Play();
        }
    }

    public void PlayMoneyPickup()
    {
        if (sfxSource != null && moneyPickupClip != null)
            sfxSource.PlayOneShot(moneyPickupClip);
    }

    public void PlayHurt()
    {
        if (sfxSource != null && hurtClip != null)
            sfxSource.PlayOneShot(hurtClip);
    }

    private void OnDestroy()
    {
        if (I == this) I = null;
    }
}
