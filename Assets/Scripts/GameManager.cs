using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; protected set; }
    public GameObject Player, playerCam;
    public GameObject interactIcon;
    public bool canPlayerMove = true;
    public bool playerCarryingObject;
    public VideoPlayer startVideoPlayer;
    public AudioSource gmAudioSource;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("2 GameManager?");
        }
        Instance = this;
        
    }

    public void ShakeCam(float duration)
    {
        playerCam.GetComponent<MotionCamera>().ShakeDuration = duration;
    }


    void Start()
    {
        startVideoPlayer.loopPointReached += CheckOver;
        //gmAudioSource = GetComponent<AudioSource>();
    }

    public void launchEndVideo()
    {
        startVideoPlayer.enabled = true;
        canPlayerMove = false;
        gmAudioSource.Stop();
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

}
