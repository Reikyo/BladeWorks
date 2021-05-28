using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private float fInputHorz;
    private float fInputVert;
    private Vector3 v3PosBeforeRotation;
    private Vector3 v3PosOffset;
    private Vector3 v3LookAt;
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
        v3PosOffset =
            fPosOffsetRight * goPlayer.transform.right
            + fPosOffsetForward * goPlayer.transform.forward
            + fPosOffsetUp * goPlayer.transform.up;
        transform.position =
            goPlayer.transform.position
            + v3PosOffset;
    }

    // ------------------------------------------------------------------------------------------------

    // LateUpdate is called once per frame
    void LateUpdate()
    {
        fInputHorz = Input.GetAxis("Horizontal Camera");
        fInputVert = Input.GetAxis("Vertical Camera");

        // Debug.Log(string.Format("Horz:{0} Vert:{1}", fInputHorz, fInputVert));

        transform.position =
            goPlayer.transform.position
            + v3PosOffset;

        v3LookAt =
            goPlayer.transform.position
            + fLookAtOffsetRight * goPlayer.transform.right
            + fLookAtOffsetForward * goPlayer.transform.forward
            + fLookAtOffsetUp * goPlayer.transform.up;

        if (Math.Abs(fInputHorz) > 0f)
        {
            transform.RotateAround(v3LookAt, Vector3.up, fInputHorz * -50f * Time.deltaTime);
            v3PosOffset = transform.position - goPlayer.transform.position;
        }

        if (    (fInputVert < 0f && transform.position.y > 0.3f)
            ||  (fInputVert > 0f && transform.position.y < 3f) )
        {
            v3PosBeforeRotation = transform.position;
            transform.RotateAround(v3LookAt, transform.right, fInputVert * 50f * Time.deltaTime);
            if (    (transform.position.y < 0.3f)
                ||  (transform.position.y > 3f) )
            {
                transform.position = v3PosBeforeRotation;
            }
            else
            {
                v3PosOffset = transform.position - goPlayer.transform.position;
            }
        }

        transform.LookAt(v3LookAt);
    }

    // ------------------------------------------------------------------------------------------------

}
