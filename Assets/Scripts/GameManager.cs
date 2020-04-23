using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public static GameManager Instance { get; protected set; }

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



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
