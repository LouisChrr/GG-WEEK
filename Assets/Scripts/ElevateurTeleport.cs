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
    private float timer = 0f;
    public GameObject CodeGO;
    public string textSO;
    private bool isElevator;

    // Les 3 fonctions IInteractable à implementer 
    private void Update()
    {
       

        if (Input.GetKeyDown(KeyCode.E) &&  isElevator)
        {
            CodeGO.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
            waitingForCode = false;
            isElevator = false;
        }
        checkCode();
        if (waitingForCode)
        {
            return;
        }
        
        
    }

    public void OnStartHover()
    {
        GameManager.Instance.interactIcon.SetActive(true);
    }

    public void OnInteract()
    {
        CodeGO.SetActive(true);
        Cursor.lockState = CursorLockMode.Confined;
        waitingForCode = true;
        Invoke("isOK", 0.5f);
    }

    public void OnEndHover()
    {
        GameManager.Instance.interactIcon.SetActive(false);
    }

    public void isOK()
    {
        isElevator = !isElevator;
    }
 

    public void checkCode()
    {

        string code = codeInputField.text;

        if (textSO == code && waitingForCode)
        {
            
            codeInputField.text = "";
            CodeGO.SetActive(false);
            waitingForCode = false;
            Cursor.lockState = CursorLockMode.Locked;
            Player.transform.position = Teleport.position;
        }

    }

}
