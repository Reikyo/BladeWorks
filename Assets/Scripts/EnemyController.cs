using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyController : MonoBehaviour
{

    private float fMetresPerSecWalk = 2f;
    public float fHealth;
    private float fHealthMax = 100f;
    public Slider sliHealth;

    private UnityEngine.AI.NavMeshAgent navEnemy;
    private GameObject goPlayer;

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        fHealth = fHealthMax;
        sliHealth.value = fHealth;

        navEnemy = GetComponent<NavMeshAgent>();
        goPlayer = GameObject.FindWithTag("Player");
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("Enemy hit");
            fHealth -= 10f;
            if (fHealth >= sliHealth.minValue)
            {
                sliHealth.value = fHealth;
            }
            if (fHealth <= 0f)
            {
                fHealth = 0f;
                sliHealth.transform.Find("Fill Area").gameObject.SetActive(false);
            }
        }
        navEnemy.destination = goPlayer.transform.position;
    }

    // ------------------------------------------------------------------------------------------------

}
