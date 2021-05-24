using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public float fPosOffsetRight = 0f;
    public float fPosOffsetForward = -5f;
    public float fPosOffsetUp = 2f;
    public float fLookAtOffsetRight = 0f;
    public float fLookAtOffsetForward = 0f;
    public float fLookAtOffsetUp = 2f;
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
            + fPosOffsetRight * goPlayer.transform.right
            + fPosOffsetForward * goPlayer.transform.forward
            + fPosOffsetUp * goPlayer.transform.up;
        transform.LookAt(
            goPlayer.transform.position
            + fLookAtOffsetRight * goPlayer.transform.right
            + fLookAtOffsetForward * goPlayer.transform.forward
            + fLookAtOffsetUp * goPlayer.transform.up
        );
    }

    // ------------------------------------------------------------------------------------------------

}
