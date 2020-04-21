using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeDestroy : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;

    public Material highlightMat, defaultMat;

    // Les 3 fonctions IInteractable à implementer 
    public void OnStartHover()
    {
        GetComponent<Renderer>().material = highlightMat;
    }

    public void OnInteract()
    {
        this.gameObject.SetActive(false);
    }

    public void OnEndHover()
    {
        GetComponent<Renderer>().material = defaultMat;
    }




}
