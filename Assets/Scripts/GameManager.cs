using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    private AudioSource sfxsrcGameManager;
    public AudioClip sfxclpPlayerDodge;
    public AudioClip sfxclpPlayerAttackSwipe1; // DM-CGS-46
    public AudioClip sfxclpPlayerAttackSwipe2; // DM-CGS-47
    public AudioClip sfxclpPlayerAttackDamage1;
    public AudioClip sfxclpPlayerAttackDamage2;
    public AudioClip sfxclpEnemyAttackSwipe1;
    public AudioClip sfxclpEnemyAttackSwipe2;
    public AudioClip sfxclpEnemyAttackDamage1; // DM-CGS-48
    public AudioClip sfxclpEnemyAttackDamage2; // DM-CGS-49
    public AudioClip sfxclpPowerUp; // DM-CGS-28

    // ------------------------------------------------------------------------------------------------

    // Start is called before the first frame update
    void Start()
    {
        sfxsrcGameManager = GetComponent<AudioSource>();
    }

    // ------------------------------------------------------------------------------------------------

    // Update is called once per frame
    void Update()
    {

    }

    // ------------------------------------------------------------------------------------------------

    // public void VfxclpPlay(string sVfxclpName, Vector3 v3Position)
    // {
    //     switch(sVfxclpName)
    //     {
    //         case "vfxclpWallDestructible": vfxclpName = vfxclpWallDestructible; break;
    //     }
    //     GameObject go = Instantiate(vfxclpName, v3Position, vfxclpName.transform.rotation);
    //     StartCoroutine(WaitUntilDestroy(go, 10f));
    // }

    // ------------------------------------------------------------------------------------------------

    public void SfxclpPlay(string sSfxclpName)
    {
        switch(sSfxclpName)
        {
            case "sfxclpPlayerDodge": sfxsrcGameManager.PlayOneShot(sfxclpPlayerDodge); break;
            case "sfxclpPlayerAttackSwipe1": sfxsrcGameManager.PlayOneShot(sfxclpPlayerAttackSwipe1); break;
            case "sfxclpPlayerAttackSwipe2": sfxsrcGameManager.PlayOneShot(sfxclpPlayerAttackSwipe2); break;
            case "sfxclpPlayerAttackDamage1": sfxsrcGameManager.PlayOneShot(sfxclpPlayerAttackDamage1); break;
            case "sfxclpPlayerAttackDamage2": sfxsrcGameManager.PlayOneShot(sfxclpPlayerAttackDamage2); break;
            case "sfxclpEnemyAttackSwipe1": sfxsrcGameManager.PlayOneShot(sfxclpEnemyAttackSwipe1); break;
            case "sfxclpEnemyAttackSwipe2": sfxsrcGameManager.PlayOneShot(sfxclpEnemyAttackSwipe2); break;
            case "sfxclpEnemyAttackDamage1": sfxsrcGameManager.PlayOneShot(sfxclpEnemyAttackDamage1); break;
            case "sfxclpEnemyAttackDamage2": sfxsrcGameManager.PlayOneShot(sfxclpEnemyAttackDamage2); break;
            case "sfxclpPowerUp": sfxsrcGameManager.PlayOneShot(sfxclpPowerUp); break;
        }
    }

    // ------------------------------------------------------------------------------------------------

    IEnumerator WaitUntilDestroy(GameObject go, float fTimeWait)
    {
        yield return new WaitForSeconds(fTimeWait);
        Destroy(go);
    }

    // ------------------------------------------------------------------------------------------------

}
