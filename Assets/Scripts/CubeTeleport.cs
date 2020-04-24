using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeTeleport : MonoBehaviour, IInteractable
{
    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public GameObject Player;
    public Transform Teleport;
    

    public Material highlightMat, defaultMat;

    // Les 3 fonctions IInteractable à implementer 
    public void OnStartHover()
    {
        GetComponent<Renderer>().material = highlightMat;
    }

    public void OnInteract()
    {
        GetComponent<AudioSource>().Play();
        Player.transform.position = Teleport.position;
        Debug.Log("Interagi");
    }

    public void OnEndHover()
    {
        GetComponent<Renderer>().material = defaultMat;
    }


}
