using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCatch : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public Transform playerCam;
    bool beingCarried = false;

    public Material highlightMat, defaultMat;

    // Les 3 fonctions IInteractable à implementer 
    public void OnStartHover()
    {
        GetComponent<Renderer>().material = highlightMat;
    }

    public void OnInteract()
    {
        if (!beingCarried)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = playerCam;
            beingCarried = true;
        }
        else if (beingCarried)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = null;
            beingCarried = false;
        }
    }

    public void OnEndHover()
    {
        GetComponent<Renderer>().material = defaultMat;
    }
    


}
