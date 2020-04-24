using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ComputerInteraction : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update

    [Header("Game Design")]
    [Space(10)]
    [Range(0.0f, 0.5f)] public float TimeBetweenLetters;
    [Range(0.0f, 5.0f)] public float TimeBeforeVanish;
    public bool NextDialWithKey;
    public KeyCode keyToSkip;
    [Header("Canvas")]
    public GameObject ChoixGO, CodeGO;
    public GameObject ComputerCanvas;
    public TextMeshProUGUI ReferenceText, Choix1Text, Choix2Text, PressToContinueText;

    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet

    [Header("Autre")]
    public TMP_InputField codeInputField;
    public bool waitingForCode, waitingForChoice;
    public float maxRange;
    private float timer;
    private float charCount;
    private bool textBeingWrited;
    private bool nextDialBool;
    public DialogueScriptableObject textSO;
    public DialogueScriptableObject[] dialoguesSO;
    
    [SerializeField] bool isInComputer;
    public GameObject playerCam, compCam;
    private Vector3 cameraPos;
    private Quaternion cameraRot;
    private float lerpValue;
    bool entering, exiting;
    public AudioSource computerAudioSource;
    public AudioClip dialSound;

    private void Start()
    {
        cameraPos = compCam.transform.position;
        cameraRot = compCam.transform.rotation;
        ComputerCanvas.SetActive(false);
        nextDialBool = false;

        List<DialogueScriptableObject> list = new List<DialogueScriptableObject>();
        foreach (DialogueScriptableObject SO in Resources.LoadAll<DialogueScriptableObject>("Dialogues/")) list.Add(SO);
        dialoguesSO = list.ToArray();
        ReferenceText.maxVisibleCharacters = 0;
        DisplayDialogue("Dialogue0");
        PressToContinueText.maxVisibleCharacters = 0;
    }

    private void Update()
    {
        ComputerAnim();
        if (!isInComputer || entering || exiting) return;
        DialogueUpdate();
    }

    void ComputerAnim()
    {
        if (isInComputer)
        {
            if (entering)
            {
                compCam.transform.position = Vector3.Slerp(playerCam.transform.position, cameraPos, lerpValue);
                compCam.transform.rotation = Quaternion.Slerp(playerCam.transform.rotation, cameraRot, lerpValue);
                lerpValue += Time.deltaTime * 1f;
                if (lerpValue > 1)
                {
                    compCam.transform.rotation = cameraRot;
                    compCam.transform.position = cameraPos;
                    ComputerCanvas.SetActive(true);
                    Cursor.lockState = CursorLockMode.Confined;
                    entering = false;
                }
            }
            if (exiting)
            {
                compCam.transform.position = Vector3.Slerp(cameraPos, playerCam.transform.position, lerpValue);
                compCam.transform.rotation = Quaternion.Slerp(cameraRot, playerCam.transform.rotation, lerpValue);
                lerpValue += Time.deltaTime * 1f;
                if (lerpValue > 1)
                {
                    isInComputer = false;
                    GameManager.Instance.canPlayerMove = true;
                    playerCam.SetActive(true);
                    compCam.SetActive(false);
                    exiting = false;
                }
            }

            if (Input.GetKeyDown(KeyCode.E) && !exiting)
            {
                // On quitte le pc
                Cursor.lockState = CursorLockMode.Locked;
                ComputerCanvas.SetActive(false);
                lerpValue = 0;
                exiting = true;
            }
        }
    }

    void DialogueUpdate()
    {
        if (textBeingWrited)
        {
            TypeWriteText(); // Typewriter effect
        }
        else if (waitingForCode)
        {
            return;
        }
        else if (nextDialBool)
        {
            if (Input.GetKey(keyToSkip)) // KeyCode pour skip dialogue
            {
             //   print("On affiche le dialogue d'après (skip): " + textSO.nextDialogue.name);
                DisplayDialogue(textSO.nextDialogue.name);
                //textSO = null;
                nextDialBool = false;
                PressToContinueText.maxVisibleCharacters = 0;
            }
        }
        else
        {
            if (ReferenceText.maxVisibleCharacters != 0)
            {
                timer += Time.deltaTime;
                if (timer >= TimeBeforeVanish)
                {
                    timer = 0;
                    if (textSO == null)
                    {
                        textSO = null;
                        Debug.LogError("textSO is NULL ????");
                        ReferenceText.maxVisibleCharacters = 0; 
                        return;
                    }
                    else
                    {
                        if (NextDialWithKey)
                        {
                            if (textSO.codeApres)
                            {
                                CodeGO.SetActive(true);
                                waitingForCode = true;
                            }
                            else if (textSO.choixApres)
                            {
                                Choix1Text.text = textSO.choix1;
                                Choix2Text.text = textSO.choix2;
                                waitingForChoice = true;
                                ChoixGO.SetActive(true);
                            }else
                            {
                                nextDialBool = true;
                              //  print("En attente du skip..");
                                PressToContinueText.maxVisibleCharacters = 100;
                            }
                        }
                        else
                        {
                          //  print("On affiche le dialogue d'après: " + textSO.nextDialogue.name);
                            DisplayDialogue(textSO.nextDialogue.name);
                            //textSO = null;
                            ReferenceText.maxVisibleCharacters = 0;
                        }
                    }
                }
            }
        }
    }
   
    public void OnStartHover()
    {
        if (isInComputer) return;
        GameManager.Instance.interactIcon.SetActive(true);
    }

    public void OnInteract()
    {
        if (GameManager.Instance.playerCarryingObject) return;
        if (isInComputer) return;
        GameManager.Instance.interactIcon.SetActive(false);
        // On entre dans le pc
        GameManager.Instance.canPlayerMove = false;
        isInComputer = true;
        
        lerpValue = 0;
        entering = true;
        compCam.transform.position = playerCam.transform.position;
        compCam.transform.rotation = playerCam.transform.rotation;
        compCam.SetActive(true);
        playerCam.SetActive(false);

    }

    public void OnEndHover()
    {
        GameManager.Instance.interactIcon.SetActive(false);

    }

    public void DisplayDialogue(string nomDuDialogue) // A appeller à chaque fois qu'on souhaite afficher un nv texte
    {
        textSO = dialogueNameToSO(nomDuDialogue);
        if (textSO == null)
        {
            print("DialogueSO non trouvé.");
            return;
        }

        string newText = textSO.dialogue;
       // print("On affiche le dialogue: " + nomDuDialogue);
        ReferenceText.text = "";
        ReferenceText.maxVisibleCharacters = 0;
        ReferenceText.text = newText;
        timer = 0;
        charCount = newText.Length;
        textBeingWrited = true;
    }

    DialogueScriptableObject dialogueNameToSO(string name)
    {
        for (int i = 0; i < dialoguesSO.Length; i++)
        {
            if (dialoguesSO[i].name == name)
                return dialoguesSO[i];
        }
        print("Dialogue non trouvé: '" + name + "'" + " -- Mauvais nom dans le code ou dans les assets ?");
        return null;
    }

    void TypeWriteText()
    {
        timer += Time.deltaTime;
           if (timer >= TimeBetweenLetters)
        {
            timer = 0;
            ReferenceText.maxVisibleCharacters += 1;
            computerAudioSource.pitch = Random.Range(-1.0f, 1.0f);
            computerAudioSource.PlayOneShot(dialSound);
            if (ReferenceText.maxVisibleCharacters >= charCount)
            {
                    textBeingWrited = false;
                    timer = 0;
            }
        }
    }

    public void checkCode()
    {

        string code = codeInputField.text;
        if (textSO.codeApres && textSO.code == code && waitingForCode)
        {

            //print("Code trouver!");
            //print("On affiche le dialogue d'après (skip): " + textSO.nextDialogue.name);
            DisplayDialogue(textSO.nextDialogue.name);
            //textSO = null;
            nextDialBool = false;
            codeInputField.text = "";
            CodeGO.SetActive(false);
            waitingForCode = false;
        }
        
    }

    public void Choice1()
    {
        if (waitingForChoice)
        {
            DisplayDialogue(textSO.nextDialogueChoice1.name);
            ChoixGO.SetActive(false);
            waitingForChoice = false;
        }
        
    }

    public void Choice2()
    {
        if (waitingForChoice)
        {
            DisplayDialogue(textSO.nextDialogueChoice2.name);
            ChoixGO.SetActive(false);
            waitingForChoice = false;
        }
    }

}
