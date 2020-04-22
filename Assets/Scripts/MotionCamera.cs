using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MotionCamera : MonoBehaviour
{
    public GameObject PlayerObj;
    public Transform InitialPos;
    private PlayerMovement PlayerMovement;
    private Rigidbody PlayerRb;

    public float ShakeDuration;
    public int magnitude;

    public float XOffset = 0;
    public float YOffset = 0;

    Vector3 DesiredPosition;

    public float Timed = 0;
    private Vector3 Offset;

    void Start()
    {
        PlayerMovement = PlayerObj.GetComponent<PlayerMovement>();
        PlayerRb = PlayerObj.GetComponent<Rigidbody>();
        Offset = new Vector3(0, PlayerRb.velocity.y * 0.15f, 0);
    }

    void Update()
    {
        Offset.y = PlayerRb.velocity.y * 0.05f;
        if (transform.position != InitialPos.position + Offset)
            transform.position = Vector3.Lerp(transform.position, InitialPos.position + Offset, Time.deltaTime / 2.0f);
    }



    void ShakeCam(int magnitude)
    {
        ShakeDuration -= Time.deltaTime;

        XOffset = Random.Range(-0.1f, 0.1f) * magnitude / 2;
        YOffset = Random.Range(-0.1f, 0.1f) * magnitude / 2;
    }

}