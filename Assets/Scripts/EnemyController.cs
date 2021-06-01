using System;
﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{
    private float fMetresPerSecWalk = 2f;
    private int iHealthMax = 100;
    public int iHealth;
    public bool bAlive;
    public Slider sliHealth;
    private Animator anEnemy;
    public int iDamage = 10;
    private List<string> sListAnimatorClipNameAction = new List<string>() {"Attack01", "Attack02"};
    private string sTrgAction;
    private bool bMirrorAction;
    private float fFractionThroughAttackClip;
    private float fFractionThroughAttackClipDamagePhaseStart;
    private float fFractionThroughAttackClipDamagePhaseEnd;
    public bool bAttackInDamagePhase = false;
    private float fLookAtOffsetAttack;
    private NavMeshAgent navEnemy;
    private GameObject goPlayer;
    private PlayerController playerController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        anEnemy = GetComponent<Animator>();
        navEnemy = GetComponent<NavMeshAgent>();
        goPlayer = GameObject.FindWithTag("Player");
        playerController = goPlayer.GetComponent<PlayerController>();

        iHealth = iHealthMax;
        bAlive = iHealth > 0;
        sliHealth.value = iHealth;
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    // FixedUpdate may be called more or less than once per frame, depending on the frame rate
    // void FixedUpdate()
    {

        // ------------------------------------------------------------------------------------------------

        if (Input.GetKeyDown(KeyCode.E))
        {
            Attacked(10);
        }

        // ------------------------------------------------------------------------------------------------

        if (!playerController.bAlive)
        {
            if (navEnemy.enabled)
            {
                navEnemy.enabled = false;
            }
            if (anEnemy.GetBool("bMotionWalk"))
            {
                anEnemy.SetBool("bMotionWalk", false);
            }
            if (!anEnemy.GetBool("bTrgVictory"))
            {
                anEnemy.SetTrigger("trgVictory");
                anEnemy.SetBool("bTrgVictory", true);
            }
            return;
        }

        // ------------------------------------------------------------------------------------------------

        navEnemy.destination = goPlayer.transform.position;

        // This commented line should work, and has been seen to work, but for some reason it doesn't
        // always work, hence the manual calculation ...
        // if (navEnemy.remainingDistance > navEnemy.stoppingDistance)
        if (Math.Pow(
                    Math.Pow(transform.position.x - navEnemy.destination.x, 2f)
                +   Math.Pow(transform.position.z - navEnemy.destination.z, 2f),
                0.5f
            ) > navEnemy.stoppingDistance)
        {
            if (!anEnemy.GetBool("bMotionWalk"))
            {
                anEnemy.SetBool("bMotionWalk", true);
            }
            return;
        }

        // ------------------------------------------------------------------------------------------------

        if (!sListAnimatorClipNameAction.Contains(this.anEnemy.GetCurrentAnimatorClipInfo(0)[0].clip.name))
        {
            if (anEnemy.GetBool("bTrgAction"))
            {
                return;
            }

            if (anEnemy.GetBool("bMotionWalk"))
            {
                anEnemy.SetBool("bMotionWalk", false);
            }

            bool bMirrorAction = (UnityEngine.Random.Range(0,2) == 0);

            if (UnityEngine.Random.Range(0,2) == 0)
            {
                sTrgAction = "trgAttack1";
                if (!bMirrorAction)
                {
                    fLookAtOffsetAttack = -0.5f;
                    // Approximate fractional collision time is 0.64
                    fFractionThroughAttackClipDamagePhaseStart = 0.54f;
                    fFractionThroughAttackClipDamagePhaseEnd = 0.74f;
                }
                else
                {
                    fLookAtOffsetAttack = 0.5f;
                    // Approximate fractional collision time is 0.11
                    fFractionThroughAttackClipDamagePhaseStart = 0.01f;
                    fFractionThroughAttackClipDamagePhaseEnd = 0.21f;
                }
            }
            else
            {
                sTrgAction = "trgAttack2";
                if (!bMirrorAction)
                {
                    fLookAtOffsetAttack = 1f;
                    // Approximate fractional collision time is 0.45
                    fFractionThroughAttackClipDamagePhaseStart = 0.35f;
                    fFractionThroughAttackClipDamagePhaseEnd = 0.55f;
                }
                else
                {
                    fLookAtOffsetAttack = -1f;
                    // Approximate fractional collision time is 0.95
                    fFractionThroughAttackClipDamagePhaseStart = 0.85f;
                    fFractionThroughAttackClipDamagePhaseEnd = 1.00f;
                }
            }

            // bMirrorAction = true;
            // sTrgAction = "trgAttack2";
            // fLookAtOffsetAttack = -1f;
            // // Approximate fractional collision time is 0.95
            // fFractionThroughAttackClipDamagePhaseStart = 0.85f;
            // fFractionThroughAttackClipDamagePhaseEnd = 1.00f;

            transform.LookAt(
                goPlayer.transform.position
                + fLookAtOffsetAttack * transform.right
            );

            anEnemy.SetTrigger(sTrgAction);
            anEnemy.SetBool("bTrgAction", true);
            anEnemy.SetBool("bMirrorAction", bMirrorAction);
            return;
        }

        if (anEnemy.GetBool("bTrgAction"))
        {
            anEnemy.SetBool("bTrgAction", false);
        }

        fFractionThroughAttackClip =
            this.anEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime
            - (float)Math.Truncate(this.anEnemy.GetCurrentAnimatorStateInfo(0).normalizedTime);

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

    public void Attacked(int iDamage)
    {
        if ((iHealth == 0) || (iDamage <= 0))
        {
            return;
        }
        if (iHealth > iDamage)
        {
            Debug.Log("Enemy hit");
            iHealth -= iDamage;
            sliHealth.value = iHealth;
            anEnemy.SetTrigger("trgAttacked");
        }
        else
        {
            iHealth = 0;
            bAlive = false;
            sliHealth.transform.Find("Fill Area").gameObject.SetActive(false);
            anEnemy.SetTrigger("trgKilled");
            gameObject.GetComponent<EnemyController>().enabled = false; // This line disables this script!
        }
        anEnemy.SetBool("bTrgAction", false);
        bAttackInDamagePhase = false;
    }

    // ------------------------------------------------------------------------------------------------

}
