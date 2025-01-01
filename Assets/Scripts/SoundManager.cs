using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance; // Singleton para acceder desde cualquier lugar

    [Header("Audio Sources")]
    public AudioSource sfxSource; // Para efectos de sonido
    public AudioSource loopSource; // Para sonidos en bucle
    public AudioSource musicSource; // Para música de fondo

    [Header("Clips de sonido")]
    public AudioClip[] soundClips; // Lista de sonidos

    private void Awake()
    {
        // Configurar Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persistir entre escenas
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Reproducir efecto de sonido
    public void PlaySFX(string clipName, float volume = 1.0f)
    {
        AudioClip clip = GetClipByName(clipName);
        
        if (clip != null && sfxSource != null)
        {
            sfxSource.volume = Mathf.Clamp01(volume); // Asegura que esté entre 0 y 1
            sfxSource.PlayOneShot(clip);
        }
        else
        {
            Debug.LogWarning($"Clip '{clipName}' no encontrado o AudioSource no asignado.");
        }
    }

        // Reproducir sonidos en bucle
    public void PlayLoopSound(string clipName, float volume = 1.0f)
    {
        
        if (loopSource != null){
            loopSource.volume = Mathf.Clamp01(volume); // Asegura que esté entre 0 y 1
            AudioClip clip = GetClipByName(clipName);
            if (loopSource.clip != clip)
            {
                loopSource.clip = clip;
                loopSource.loop = true;
                loopSource.Play();
            }else if (!loopSource.isPlaying)
            {
                loopSource.Play(); // Reanuda si está detenido
            }
        }

    }

    public void StopLoopSound()
    {
        loopSource.Stop();
    }

    // Reproducir música
    public void PlayMusic(string clipName, float volume = 1.0f)
    {
        AudioClip clip = GetClipByName(clipName);
        if (clip != null && musicSource != null)
        {
            musicSource.clip = clip;
            musicSource.volume = Mathf.Clamp01(volume); // Asegura que esté entre 0 y 1
            musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"Clip '{clipName}' no encontrado o AudioSource no asignado.");
        }
    }

    // Buscar clip por nombre
    private AudioClip GetClipByName(string clipName)
    {
        foreach (AudioClip clip in soundClips)
        {
            if (clip.name == clipName)
                return clip;
        }
        return null;
    }
}
