using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private bool bInMotionLastFrame = false;
    private bool bInMotionThisFrame = false;
    private float fMetresPerSecWalk = 5f;
    private float fInputHorz;
    private float fInputVert;
    private Vector3 v3DirectionMove;
    private Vector3 v3DirectionLook;
    private Animator anPlayer;
    public Animator[] anPlayerChildren;
    private float fSpeedAnPlayerChild;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        anPlayer = GetComponent<Animator>();
        anPlayerChildren = GetComponentsInChildren<Animator>(); // n.b. This only gets the component of the first child in the tree
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {

        // ------------------------------------------------------------------------------------------------

        fInputHorz = Input.GetAxis("Horizontal");
        fInputVert = Input.GetAxis("Vertical");

        if (Math.Abs(fInputHorz) + Math.Abs(fInputVert) > 0f)
        {
            bInMotionThisFrame = true;

            v3DirectionMove = ((fInputHorz * Vector3.right) + (fInputVert * Vector3.forward)).normalized;
            v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fMetresPerSecWalk * Time.deltaTime, 0f);

            transform.position = Vector3.MoveTowards(transform.position, transform.position + v3DirectionMove, fMetresPerSecWalk * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(v3DirectionLook);
        }
        else
        {
            bInMotionThisFrame = false;
        }

        // ------------------------------------------------------------------------------------------------

        if (bInMotionLastFrame != bInMotionThisFrame)
        {
            Debug.Log("Here");
            if (bInMotionThisFrame)
            {
                fSpeedAnPlayerChild = 1f;
            }
            else
            {
                fSpeedAnPlayerChild = 0f;
            }
            // Only needed if the character model has multiple animated parts
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetFloat("fSpeedAnPlayerChild", fSpeedAnPlayerChild);
            }
            // anPlayer.SetFloat("Speed_f", 0f);
        }

        // ------------------------------------------------------------------------------------------------

        bInMotionLastFrame = bInMotionThisFrame;

        // ------------------------------------------------------------------------------------------------

    }

    // ------------------------------------------------------------------------------------------------

}
