using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Vector3 v3PosBeforeRotVert;
    private Vector3 v3PosWRTPlayer;
    private Vector3 v3LookAt;
    private Vector3 v3LookAtWRTPlayer;
    public float fPosWRTPlayerRight = 0f;
    public float fPosWRTPlayerForward = -5f;
    public float fPosWRTPlayerUp = 2f;
    public float fLookAtWRTPlayerRight = 0f;
    public float fLookAtWRTPlayerForward = 0f;
    public float fLookAtWRTPlayerUp = 2f;
    public float fPosYLower = 0.3f;
    public float fPosYUpper = 3f;
    public float fDegPerSec = 50f;
    private float fInputHorz;
    private float fInputVert;
    private GameObject goPlayer;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");
        v3PosWRTPlayer =
            fPosWRTPlayerRight * goPlayer.transform.right
            + fPosWRTPlayerForward * goPlayer.transform.forward
            + fPosWRTPlayerUp * goPlayer.transform.up;
        v3LookAtWRTPlayer =
            fLookAtWRTPlayerRight * goPlayer.transform.right
            + fLookAtWRTPlayerForward * goPlayer.transform.forward
            + fLookAtWRTPlayerUp * goPlayer.transform.up;
    }

    // ------------------------------------------------------------------------------------------------

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        transform.position =
            goPlayer.transform.position
            + v3PosWRTPlayer;

        v3LookAt =
            goPlayer.transform.position
            + v3LookAtWRTPlayer;

        fInputHorz = Input.GetAxis("Horizontal Camera"); // Left is negative and right is positive (invert IS NOT enabled in project settings)
        fInputVert = Input.GetAxis("Vertical Camera"); // Down is negative and up is positive (invert IS enabled in project settings)
        // Debug.Log(string.Format("Horz:{0} Vert:{1}", fInputHorz, fInputVert));

        if (Math.Abs(fInputHorz) > 0f)
        {
            transform.RotateAround(goPlayer.transform.position, Vector3.up, fInputHorz * -fDegPerSec * Time.deltaTime);
            v3PosWRTPlayer = transform.position - goPlayer.transform.position;
        }

        if (    (fInputVert < 0f && transform.position.y > fPosYLower)
            ||  (fInputVert > 0f && transform.position.y < fPosYUpper) )
        {
            v3PosBeforeRotVert = transform.position;
            transform.RotateAround(goPlayer.transform.position, transform.right, fInputVert * fDegPerSec * Time.deltaTime);
            if (    (transform.position.y < fPosYLower)
                ||  (transform.position.y > fPosYUpper) )
            {
                transform.position = v3PosBeforeRotVert;
            }
            else
            {
                v3PosWRTPlayer = transform.position - goPlayer.transform.position;
            }
        }

        transform.LookAt(v3LookAt);
    }

    // ------------------------------------------------------------------------------------------------

}
