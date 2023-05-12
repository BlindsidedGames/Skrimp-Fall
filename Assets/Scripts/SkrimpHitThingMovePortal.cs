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

    private float _scoreMulti = 1;

    private static readonly int Splash = Animator.StringToHash("Splash");

    private Level level => skrimpInterface.level;

    private void OnTriggerEnter2D(Collider2D col)
    {
        switch (col.tag)
        {
            case "GroundPortal":
                audioSource.PlayOneShot(defaultAudioClips[Random.Range(0, defaultAudioClips.Length)]);
                MovetoPortal();
                level.currency += level.skrimpValue * (1 + oracle.saveData.player.level / 10f) *
                                  level.valueMultiFromBonusSkrimp * _scoreMulti;
                oracle.saveData.player.experience += (1 + (int)oracle.saveData.levelSelector) *
                                                     level.valueMultiFromBonusSkrimp * _scoreMulti;
                oracle.saveData.statistics.timesSkrimpGoneThroughPortal++;
                oracle.saveData.level.levelStats.timesSkrimpGoneThroughPortal++;

                _scoreMulti = 1;
                break;
            case "x1.5":
                _scoreMulti *= 1.5f;
                break;
            case "x2":
                _scoreMulti *= 2;
                break;
            case "x3":
                _scoreMulti *= 3;
                break;
            case "x5":
                _scoreMulti *= 5;
                break;
            case "x10":
                _scoreMulti *= 10;
                break;
            case "x25":
                _scoreMulti *= 25;
                break;
            case "x50":
                _scoreMulti *= 50;
                break;
            case "div2":
                _scoreMulti /= 2;
                break;
            case "div4":
                _scoreMulti /= 4;
                break;
            case "div10":
                _scoreMulti /= 10;
                break;
        }

        if (_scoreMulti > 50) _scoreMulti = 50;
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
            _scoreMulti = .1f;
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