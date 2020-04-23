using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; protected set; }
    public GameObject Player, playerCam;
    public GameObject interactIcon;
    public bool canPlayerMove = true;
    
    
    
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
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
