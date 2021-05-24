using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool bInMotionLastFrame = false;
    private bool bInMotionThisFrame = false;
    private float fMetresPerSecWalk = 5f;
    private float fForceJump = 70f;
    private float fInputHorz;
    private float fInputVert;
    private Vector3 v3DirectionMove;
    private Vector3 v3DirectionLook;
    private Rigidbody rbPlayer;
    private Animator anPlayer;
    public Animator[] anPlayerChildren;
    private float fSpeedAnPlayerChild;
    private int iHealthMax = 100;
    private int iHealth;
    private bool bAlive;
    public Slider sliHealth;

    // private CharacterController ccPlayer;
    // private float jumpSpeed = 2f;
    // private float gravity = 10f;
    // private Vector3 v3DirectionMoveJump = Vector3.zero;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        // ccPlayer = GetComponent<CharacterController>();
        rbPlayer = GetComponent<Rigidbody>();
        anPlayer = GetComponent<Animator>();
        anPlayerChildren = GetComponentsInChildren<Animator>(); // n.b. This only gets the component of the first child in the tree
        iHealth = iHealthMax;
        bAlive = iHealth > 0;
        sliHealth.value = iHealth;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Attacked(10);
        }
    }

    // ------------------------------------------------------------------------------------------------

    // FixedUpdate is called once per frame
    void FixedUpdate()
    {
        fInputHorz = Input.GetAxis("Horizontal");
        fInputVert = Input.GetAxis("Vertical");

        if (Math.Abs(fInputHorz) + Math.Abs(fInputVert) > 0f)
        {
            bInMotionThisFrame = true;

            // This is good for player control from a fixed camera angle in world space:
            // v3DirectionMove = ((fInputHorz * Vector3.right) + (fInputVert * Vector3.forward)).normalized;

            // This is good for player control from a fixed camera angle in local space:
            v3DirectionMove = ((fInputHorz * transform.right) + (fInputVert * transform.forward)).normalized;

            v3DirectionLook = Vector3.RotateTowards(transform.forward, v3DirectionMove, fMetresPerSecWalk * Time.deltaTime, 0f);

            transform.position = Vector3.MoveTowards(transform.position, transform.position + v3DirectionMove, fMetresPerSecWalk * Time.deltaTime);
            transform.rotation = Quaternion.LookRotation(v3DirectionLook);

            // ccPlayer.Move(v3DirectionMove * fMetresPerSecWalk * Time.deltaTime);
        }
        else
        {
            bInMotionThisFrame = false;
        }

        // ------------------------------------------------------------------------------------------------

        if (bInMotionLastFrame != bInMotionThisFrame)
        {
            // Debug.Log("Here");
            if (bInMotionThisFrame)
            {
                fSpeedAnPlayerChild = 1f;
            }
            else
            {
                fSpeedAnPlayerChild = 0f;
            }
            // anPlayer.SetFloat("fSpeedAnPlayerChild", fSpeedAnPlayerChild);
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rbPlayer.AddForce(fForceJump * Vector3.up, ForceMode.Impulse);
        }

        if (Input.GetKeyDown(KeyCode.Return))
        {
            // anPlayer.SetBool("bAttack", true);
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetTrigger("trgAttack");
            }
            // StartCoroutine(Wait());
        }

        // if (ccPlayer.isGrounded)
        // {
        //     if (Input.GetKeyDown(KeyCode.Space))
        //     {
        //         v3DirectionMoveJump.y = jumpSpeed;
        //     }
        // }
        // else
        // {
        //     v3DirectionMoveJump.y -= gravity * Time.deltaTime;
        //     ccPlayer.Move(v3DirectionMoveJump * Time.deltaTime);
        // }

    }

    // ------------------------------------------------------------------------------------------------

    // IEnumerator Wait()
    // {
    //     yield return new WaitForSeconds(1f);
    //     foreach (Animator anPlayerChild in anPlayerChildren)
    //     {
    //         anPlayerChild.SetBool("bAttack", false);
    //     }
    // }

    // ------------------------------------------------------------------------------------------------

    public void Attacked(int iDamage)
    {
        if (iDamage <= 0)
        {
            return;
        }
        if (iHealth > iDamage)
        {
            Debug.Log("Player hit");
            iHealth -= iDamage;
            sliHealth.value = iHealth;
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetTrigger("trgAttacked");
            }
        }
        else if (iHealth > 0)
        {
            iHealth = 0;
            bAlive = false;
            sliHealth.transform.Find("Fill Area").gameObject.SetActive(false);
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetTrigger("trgKilled");
            }
            gameObject.GetComponent<PlayerController>().enabled = false; // This line disables this script!
        }
    }

    // ------------------------------------------------------------------------------------------------

}
