using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TongueStroke : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public Animation animationTongue;
    public Animation animationInteract;
    public Material highlightMat, defaultMat;

    // Les 3 fonctions IInteractable à implementer 
    public void OnStartHover()
    {
        GetComponent<Renderer>().material = highlightMat;
    }

    public void OnInteract()
    {
        
    }

    public void OnEndHover()
    {
        GetComponent<Renderer>().material = defaultMat;
    }

}
