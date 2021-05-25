using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    public GameObject goPlayer;
    private PlayerController playerController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        playerController = goPlayer.GetComponent<PlayerController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        if (    (playerController.bAttackInDamagePhase)
            &&  (collider.gameObject.CompareTag("Enemy")) )
        {
            collider.gameObject.GetComponent<EnemyController>().Attacked(playerController.iDamage);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
