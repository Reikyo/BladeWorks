using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFistController : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject goEnemy;
    private EnemyController enemyController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        enemyController = goEnemy.GetComponent<EnemyController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        // if (collider.gameObject.CompareTag("Player"))
        // {
        //     Debug.Log(enemyController.sTrgAction);
        //     Debug.Log(enemyController.fFractionThroughAttackClip);
        //     Debug.Log("");
        // }
        if (    (enemyController.bAttackInDamagePhase)
            &&  (collider.gameObject.CompareTag("Player")) )
        {
            if (UnityEngine.Random.Range(0,2) == 0)
            {
                gameManager.SfxclpPlay("sfxclpEnemyAttackDamage1");
            }
            else
            {
                gameManager.SfxclpPlay("sfxclpEnemyAttackDamage2");
            }
            collider.gameObject.GetComponent<PlayerController>().Attacked(enemyController.iDamage);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
