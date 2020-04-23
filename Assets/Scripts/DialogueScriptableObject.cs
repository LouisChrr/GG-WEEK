using UnityEngine;

[CreateAssetMenu(fileName = "Dialogue", menuName = "Creer un nouveau objet dialogue", order = 1)]

public class DialogueScriptableObject : ScriptableObject
{
    [Header("!! RENOMMER L'OBJET !!")]
    //public string nomDuDialogue;
    [TextArea]
    [Space(10)]
    public string dialogue;
    [Header("Dialogue a afficher apres?")]
    public DialogueScriptableObject nextDialogue;

    [Header("Choix")]
    public bool choixApres;
    public string choix1, choix2;
    public DialogueScriptableObject nextDialogueChoice1, nextDialogueChoice2;

    [Header("Code")]
    public bool codeApres;
    public string code;

    [Header("Exit pc")]
    public bool exitPc;
}