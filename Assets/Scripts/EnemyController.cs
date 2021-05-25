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
    private float fFractionThroughAttackClip;
    private float fFractionThroughAttackClipDamagePhaseStart = 0.50f;
    private float fFractionThroughAttackClipDamagePhaseEnd = 0.75f;
    public bool bAttackInDamagePhase = false;
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
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Attacked(10);
        }

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

        navEnemy.destination = goPlayer.transform.position;

        if (navEnemy.remainingDistance > navEnemy.stoppingDistance)
        {
            if (!anEnemy.GetBool("bMotionWalk"))
            {
                anEnemy.SetBool("bMotionWalk", true);
            }
            return;
        }

        if (this.anEnemy.GetCurrentAnimatorClipInfo(0)[0].clip.name != "Attack01")
        {
            if (!anEnemy.GetBool("bTrgAttack"))
            {
                if (anEnemy.GetBool("bMotionWalk"))
                {
                    anEnemy.SetBool("bMotionWalk", false);
                }
                transform.LookAt(
                    goPlayer.transform.position
                    - 0.5f * transform.right
                );
                anEnemy.SetTrigger("trgAttack");
                anEnemy.SetBool("bTrgAttack", true);
            }
            return;
        }

        if (anEnemy.GetBool("bTrgAttack"))
        {
            anEnemy.SetBool("bTrgAttack", false);
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
            gameObject.GetComponent<EnemyController>().enabled = false; // This line disables this script!
        }
    }

    // ------------------------------------------------------------------------------------------------

}
