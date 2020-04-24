using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeCatch : MonoBehaviour, IInteractable
{
    // Start is called before the first frame update
    public float MaxRange { get { return maxRange; } } // Range d'interact personnel à l'objet
    public float maxRange;
    public Transform objectPos;
    bool beingCarried = false;
    private GameManager gm;
    public Material highlightMat, defaultMat;
    private BoxCollider BoxCollider;
    private float timer;
    bool reset;
    private AudioSource cubeAudioSource;
    public AudioClip dropSound;

    public void Start()
    {
        BoxCollider = GetComponent<BoxCollider>();
        gm = GameManager.Instance;
        cubeAudioSource = GetComponent<AudioSource>();
    }

    // Les 3 fonctions IInteractable à implementer 
    public void OnStartHover()
    {
        //GetComponent<Renderer>().material = highlightMat;
        GameManager.Instance.interactIcon.SetActive(true);
    }

    public void OnInteract()
    {
        if (!beingCarried && reset)
        {
            GetComponent<Rigidbody>().isKinematic = true;
            transform.parent = objectPos;
            transform.localPosition = Vector3.zero;
            StartCoroutine("resetBeingCarried", true);
            BoxCollider.enabled = false;
            timer = 0;
            reset = false;
        }
    }

    public void OnEndHover()
    {
        //GetComponent<Renderer>().material = defaultMat;
        GameManager.Instance.interactIcon.SetActive(false);

    }

    public void Update()
    {
        if (!reset)
        {
            timer += Time.deltaTime;
            if(timer > 0.2f)
            {
                reset = true;
            }
        }
        if (beingCarried && Input.GetKey(KeyCode.E) && reset)
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.parent = null;
            beingCarried = false;
            BoxCollider.enabled = true;
            StartCoroutine("resetBeingCarried", false);
            cubeAudioSource.PlayOneShot(dropSound);
            reset = false;
        }
    }

    IEnumerator resetBeingCarried(bool state)
    {

        yield return new WaitForFixedUpdate();
        beingCarried = state;
        gm.playerCarryingObject = state;
    }

}
