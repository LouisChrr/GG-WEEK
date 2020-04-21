using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRaycasting : MonoBehaviour
{
    // Start is called before the first frame update
    public float range;
    private IInteractable currentTarget;
    private Camera mainCamera;

    void Awake()
    {
        mainCamera = Camera.main;
    }

    private void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        RaycastForInteractable();

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (currentTarget != null)
            {
                currentTarget.OnInteract();
            }
        }

    }

    private void RaycastForInteractable()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit whatIsHit;

        if (Physics.Raycast(ray, out whatIsHit, range))
        {
            IInteractable interactable = whatIsHit.collider.GetComponent<IInteractable>();

            if (interactable != null)
            {
                if (whatIsHit.distance <= interactable.MaxRange)
                {
                    if (interactable == currentTarget)
                    {
                        return;
                    }
                    else if (currentTarget != null)
                    {
                        currentTarget.OnEndHover();
                        currentTarget = interactable;
                        currentTarget.OnStartHover();
                        return;
                    }
                    else
                    {
                        currentTarget = interactable;
                        currentTarget.OnStartHover();
                        return;
                    }
                }
                else
                {
                    if (currentTarget != null)
                    {
                        currentTarget.OnEndHover();
                        currentTarget = null;
                        return;
                    }
                }
            }
            else
            {
                if (currentTarget != null)
                {
                    currentTarget.OnEndHover();
                    currentTarget = null;
                    return;
                }
            }
        }
        else
        {
            if (currentTarget != null)
            {
                currentTarget.OnEndHover();
                currentTarget = null;
                return;
            }
        }

    }

}
