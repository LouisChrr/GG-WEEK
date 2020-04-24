using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ElevateurTeleport : MonoBehaviour, IInteractable
{
    public TMP_InputField codeInputField;

    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public GameObject Player;
    public Transform Teleport;
    public bool waitingForCode;

    public GameObject CodeGO;
    public DialogueScriptableObject textSO;
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
    }

    public void OnEndHover()
    {
        GetComponent<Renderer>().material = defaultMat;
    }

 

    public void checkCode()
    {

        string code = codeInputField.text;

        if (textSO.codeApres && textSO.code == code && waitingForCode)
        {
            
            codeInputField.text = "";
            CodeGO.SetActive(false);
            waitingForCode = false;
        }

    }
}
