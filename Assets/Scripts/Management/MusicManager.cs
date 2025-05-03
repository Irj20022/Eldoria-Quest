using UnityEngine;
using UnityEngine.SceneManagement;



public class MusicManager : MonoBehaviour
{
    public UnityEngine.Audio.AudioMixer audioMixer;

    public static MusicManager Instance;

    [System.Serializable]
    public class SceneMusic
    {
        public string sceneName;
        public AudioClip musicClip;
    }

    [Header("Music Tracks")]
    public SceneMusic[] sceneMusicList;
    private AudioSource audioSource;

    private void Awake()
    {
        
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        audioSource = GetComponent<AudioSource>();
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        PlayMusicForScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        PlayMusicForScene(scene.name);
    }

    private void PlayMusicForScene(string sceneName)
    {
        foreach (SceneMusic sm in sceneMusicList)
        {
            if (sm.sceneName == sceneName)
            {
                if (audioSource.clip != sm.musicClip)
                {
                    audioSource.clip = sm.musicClip;
                    audioSource.loop = true;
                    audioSource.Play();
                }
                return;
            }
        }
    }
}
