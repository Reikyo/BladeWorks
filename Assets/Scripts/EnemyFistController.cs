using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFistController : MonoBehaviour
{
    public GameObject goEnemy;
    private EnemyController enemyController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        enemyController = goEnemy.GetComponent<EnemyController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        if (    (enemyController.bAttackInDamagePhase)
            &&  (collider.gameObject.CompareTag("Player")) )
        {
            collider.gameObject.GetComponent<PlayerController>().Attacked(enemyController.iDamage);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
