using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSwordController : MonoBehaviour
{
    private GameManager gameManager;
    public GameObject goPlayer;
    private PlayerController playerController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        playerController = goPlayer.GetComponent<PlayerController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        if (    (playerController.bAttackInDamagePhase)
            &&  (collider.gameObject.CompareTag("Enemy")) )
        {
            if (UnityEngine.Random.Range(0,2) == 0)
            {
                gameManager.SfxclpPlay("sfxclpPlayerAttackDamage1");
            }
            else
            {
                gameManager.SfxclpPlay("sfxclpPlayerAttackDamage2");
            }
            collider.gameObject.GetComponent<EnemyController>().Attacked(playerController.iDamage);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
