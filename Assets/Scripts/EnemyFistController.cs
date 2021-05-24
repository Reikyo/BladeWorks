using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFistController : MonoBehaviour
{
    private GameObject goPlayer;
    private PlayerController playerController;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        goPlayer = GameObject.FindWithTag("Player");
        playerController = goPlayer.GetComponent<PlayerController>();
    }

    // ------------------------------------------------------------------------------------------------

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            playerController.Attacked(10);
        }
    }

    // ------------------------------------------------------------------------------------------------

}
