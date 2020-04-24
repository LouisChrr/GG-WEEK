using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FinishElevator : MonoBehaviour, IInteractable
{
    public TMP_InputField codeInputField;

    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public bool waitingForCode;
    public GameObject CodeGO;
    public string textSO;
    public Material highlightMat, defaultMat;
    private bool isElevator;

    // Les 3 fonctions IInteractable à implementer 
    private void Update()
    {


        if (Input.GetKeyDown(KeyCode.E) && isElevator)
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
        GetComponent<Renderer>().material = highlightMat;
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
        GetComponent<Renderer>().material = defaultMat;
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
            GameManager.Instance.launchEndVideo();
        }

    }

}
