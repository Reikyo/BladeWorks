using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    private bool bInMotion = false;
    private bool bAnimateWalkThisFrame = false;
    private bool bAnimateWalkLastFrame = false;
    private float fMetresPerSecWalk = 5f;
    private float fForceJump = 300f;
    private bool bOnSurface = true;
    private float fTimeTrgJump = 0f;
    private float fTimeTrgJumpDelay = 0.1f;
    private float fInputHorz;
    private float fInputVert;
    private Vector3 v3DirectionMove;
    private Vector3 v3DirectionLook;
    private Rigidbody rbPlayer;
    private Animator anPlayer;
    public Animator[] anPlayerChildren;
    public int iDamage = 20;
    private List<string> sListAnimatorClipNameAction = new List<string>() {"Berserker_dodge_01", "Berserker_attack_01", "Berserker_attack_02", "Berserker_attack_03"};
    private string sTrgAction;
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
    private RaycastHit rayHitSurface;

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

        bOnSurface = Physics.Raycast(transform.position + 0.2f * Vector3.up, -Vector3.up, out rayHitSurface, 0.21f);

        // ------------------------------------------------------------------------------------------------

        fInputHorz = Input.GetAxis("Horizontal");
        fInputVert = Input.GetAxis("Vertical");

        // Debug.Log(string.Format("Horz:{0} Vert:{1}", fInputHorz, fInputVert));

        if (Math.Abs(fInputHorz) + Math.Abs(fInputVert) > 0f)
        {
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

            bInMotion = true;
        }
        else
        {
            bInMotion = false;
        }

        // ------------------------------------------------------------------------------------------------

        if (    bInMotion
            &&  bOnSurface )
        {
            bAnimateWalkThisFrame = true;
            fSpeedAnPlayerChild = 1f;
        }
        else
        {
            bAnimateWalkThisFrame = false;
            fSpeedAnPlayerChild = 0f;
        }

        if (bAnimateWalkThisFrame != bAnimateWalkLastFrame)
        {
            // anPlayer.SetFloat("fSpeedAnPlayerChild", fSpeedAnPlayerChild);
            // Only needed if the character model has multiple animated parts:
            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetFloat("fSpeedAnPlayerChild", fSpeedAnPlayerChild);
            }
        }

        bAnimateWalkLastFrame = bAnimateWalkThisFrame;

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

        if (    Input.GetButtonDown("Jump") // Joystick button 1 = PS4 X button
            &&  bOnSurface
            &&  (Time.time - fTimeTrgJump >= fTimeTrgJumpDelay) )
        {
            fTimeTrgJump = Time.time;
            rbPlayer.AddForce(fForceJump * Vector3.up, ForceMode.Impulse);
        }

        // ------------------------------------------------------------------------------------------------

        if (!sListAnimatorClipNameAction.Contains(this.anPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.name))
        {
            if (anPlayer.GetBool("bTrgAction"))
            {
                return;
            }

            if (    Input.GetButtonDown("Dodge") // Joystick button 2 = PS4 circle button
                &&  bOnSurface )
            {
                sTrgAction = "trgDodge";
            }
            // if (Input.GetButtonDown("Attack1")) // Joystick button 2 = PS4 circle button
            // {
            //     sTrgAction = "trgAttack1";
            // }
            else if (Input.GetButtonDown("Attack2")) // Joystick button 3 = PS4 triangle button
            {
                sTrgAction = "trgAttack2";
            }
            else if (Input.GetButtonDown("Attack3")) // Joystick button 0 = PS4 square button
            {
                sTrgAction = "trgAttack3";
            }
            else
            {
                return;
            }

            foreach (Animator anPlayerChild in anPlayerChildren)
            {
                anPlayerChild.SetTrigger(sTrgAction);
            }

            anPlayer.SetBool("bTrgAction", true);
        }

        if (anPlayer.GetBool("bTrgAction"))
        {
            anPlayer.SetBool("bTrgAction", false);
        }

        if (this.anPlayer.GetCurrentAnimatorClipInfo(0)[0].clip.name == "Berserker_dodge_01")
        {
            return;
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
        anPlayer.SetBool("bTrgAction", false);
        bAttackInDamagePhase = false;
    }

    // ------------------------------------------------------------------------------------------------

}
