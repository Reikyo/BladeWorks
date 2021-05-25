using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFistController : MonoBehaviour
{
    private GameObject goPlayer;
    public GameObject goEnemy;
    private PlayerController playerController;
    private EnemyController enemyController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");
        playerController = goPlayer.GetComponent<PlayerController>();
        enemyController = goEnemy.GetComponent<EnemyController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        if (    (enemyController.bAttackInDamagePhase)
            &&  (collider.gameObject.CompareTag("Player")) )
        {
            playerController.Attacked(10);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
