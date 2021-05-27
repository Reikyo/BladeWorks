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
    public int iDamage = 20;
    private float fFractionThroughAttackClip;
    private float fFractionThroughAttackClipDamagePhaseStart = 0.30f;
    private float fFractionThroughAttackClipDamagePhaseEnd = 0.60f;
    public bool bAttackInDamagePhase = false;
    private float fSpeedAnPlayerChild;
    private int iHealthMax = 100;
    public int iHealth;
    public bool bAlive;
    public Slider sliHealth;
    private Camera camMainCamera;

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
        camMainCamera = GameObject.Find("Main Camera").GetComponent<Camera>();

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

    // FixedUpdate may be called more or less than once per frame, depending on the frame rate
    void FixedUpdate()
    {

        // ------------------------------------------------------------------------------------------------

        fInputHorz = Input.GetAxis("Horizontal");
        fInputVert = Input.GetAxis("Vertical");

        // Debug.Log(string.Format("Horz:{0} Vert:{1}", fInputHorz, fInputVert));

        if (Math.Abs(fInputHorz) + Math.Abs(fInputVert) > 0f)
        {
            bInMotionThisFrame = true;

            // This is good for player control from a fixed camera angle in world space:
            // v3DirectionMove = ((fInputHorz * Vector3.right) + (fInputVert * Vector3.forward)).normalized;

            // This is good for player control from a fixed camera angle in local space:
            // v3DirectionMove = ((fInputHorz * transform.right) + (fInputVert * transform.forward)).normalized;

            // This is good for player control from a variable camera angle in local space. Motion is wrt to
            // the camera, not the player, but the player look direction is still wrt to the player, not the camera:
            v3DirectionMove = ((fInputHorz * camMainCamera.transform.right) + (fInputVert * camMainCamera.transform.forward)).normalized;

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

        // if (Input.GetKeyDown(KeyCode.Space))
        if (Input.GetButtonDown("Jump"))
        {
            rbPlayer.AddForce(fForceJump * Vector3.up, ForceMode.Impulse);
        }

        // ------------------------------------------------------------------------------------------------

        if (this.anPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Berserker_attack_03")
        {
            // if (    (Input.GetKeyDown(KeyCode.Return))
            if (    (Input.GetButtonDown("Attack1"))
                &&  (!anPlayer.GetBool("bTrgAttack")) )
            {
                foreach (Animator anPlayerChild in anPlayerChildren)
                {
                    anPlayerChild.SetTrigger("trgAttack");
                }
                anPlayer.SetBool("bTrgAttack", true);
            }
            return;
        }

        if (anPlayer.GetBool("bTrgAttack"))
        {
            anPlayer.SetBool("bTrgAttack", false);
        }

        fFractionThroughAttackClip =
            this.anPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime
            - (float)Math.Truncate(this.anPlayer.GetCurrentAnimatorStateInfo(0).normalizedTime);

        if (    (fFractionThroughAttackClip >= fFractionThroughAttackClipDamagePhaseStart)
            &&  (fFractionThroughAttackClip <= fFractionThroughAttackClipDamagePhaseEnd) )
        {
            if (!bAttackInDamagePhase)
            {
                bAttackInDamagePhase = true;
            }
            return;
        }

        if (bAttackInDamagePhase)
        {
            bAttackInDamagePhase = false;
        }

        // ------------------------------------------------------------------------------------------------

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
        if ((iHealth == 0) || (iDamage <= 0))
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
        else
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
        anPlayer.SetBool("bTrgAttack", false);
        bAttackInDamagePhase = false;
    }

    // ------------------------------------------------------------------------------------------------

}
