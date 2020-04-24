using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class MainMenu : MonoBehaviour
{
    public GameObject ALLCANVAS;
    public VideoPlayer startVideoPlayer;

    public void Start()
    {
        startVideoPlayer.loopPointReached += CheckOver;
    }

    void CheckOver(UnityEngine.Video.VideoPlayer vp)
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void PlayGame()
    {
        startVideoPlayer.enabled = true;
        ALLCANVAS.SetActive(false);
    }
    public void QuitGame()
    {
        Application.Quit();
    }


}
