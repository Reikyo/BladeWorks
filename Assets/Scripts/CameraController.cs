using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float fOffsetRight = 0f;
    public float fOffsetForward = -5f;
    public float fOffsetUp = 2f;
    private GameObject goPlayer;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");
    }

    // ------------------------------------------------------------------------------------------------

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        transform.position =
            goPlayer.transform.position
            + fOffsetRight * goPlayer.transform.right
            + fOffsetForward * goPlayer.transform.forward
            + fOffsetUp * goPlayer.transform.up;
        transform.LookAt(goPlayer.transform.position);
    }

    // ------------------------------------------------------------------------------------------------

}
