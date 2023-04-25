using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using static SkrimpInterface;
using static Oracle;

public class SkrimpHitThingMovePortal : MonoBehaviour
{
    [SerializeField] public Transform topPortal;
    [SerializeField] private GameObject rainDrop;
    [SerializeField] private SkrimpManager skrimpManager;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] defaultAudioClips;
    [SerializeField] private AudioClip[] splatAudioClips;

    private static readonly int Splash = Animator.StringToHash("Splash");

    private Level level => skrimpInterface.level;

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("GroundPortal"))
        {
            audioSource.PlayOneShot(defaultAudioClips[Random.Range(0, defaultAudioClips.Length)]);
            MovetoPortal();
            level.currency += level.skrimpValue * (1 + oracle.saveData.player.level / 10f);
            oracle.saveData.player.experience += 1 + (int)oracle.saveData.levelSelector;
            oracle.saveData.statistics.timesSkrimpGoneThroughPortal++;
            oracle.saveData.level.levelStats.timesSkrimpGoneThroughPortal++;
        }
    }

    public void MovetoPortal()
    {
        StopAllCoroutines();
        var topPortalPosition = topPortal.position;
        topPortalPosition.x = Random.Range(-2f, 2f);
        topPortal.position = topPortalPosition;
        transform.position = topPortalPosition;
    }

    private void OnCollisionEnter2D(Collision2D col)
    {
        if (!skrimpManager.devSkrimp)
            if (rainDrop.activeSelf && !col.gameObject.CompareTag("Player"))
                if (oracle.saveData.preferences.skinSelection == SkinSelection.Rain)
                    rainDrop.GetComponent<Animator>().SetTrigger(Splash);
        if (col.gameObject.CompareTag("Floor"))
        {
            var topPortalPosition = topPortal.position;
            topPortalPosition.x = Random.Range(-2f, 2f);
            topPortal.position = topPortalPosition;
            transform.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            StopAllCoroutines();
            StartCoroutine(PauseSkrimp(topPortalPosition));
            oracle.saveData.level.levelStats.timesSkrimpHitGround++;
            oracle.saveData.statistics.timesSkrimpHitGround++;
            audioSource.PlayOneShot(splatAudioClips[Random.Range(0, splatAudioClips.Length)]);
        }
    }

    private IEnumerator PauseSkrimp(Vector3 pos)
    {
        yield return new WaitForSeconds(5);
        transform.position = pos;
    }
}