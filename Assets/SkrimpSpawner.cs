using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using static SkrimpInterface;
using static Oracle;

public class SkrimpSpawner : MonoBehaviour
{
    public Transform portal;
    public Transform skrimpPrefab;
    public Transform devSkrimpPrefab;

    private void Start()
    {
        SpawnSkrimps();
    }

    private void OnEnable()
    {
        skrimpInterface.UpdateSkrimps += SpawnSkrimps;
    }

    private void OnDisable()
    {
        skrimpInterface.UpdateSkrimps -= SpawnSkrimps;
    }

    private void SpawnSkrimps()
    {
        StartCoroutine(StaggerSpawn());
    }

    private IEnumerator StaggerSpawn()
    {
        if (SkrimpManager.skrimCount < skrimpInterface.level.skrimpCount + oracle.saveData.player.level / 10)
            for (var i = 0;
                 i < skrimpInterface.level.skrimpCount + oracle.saveData.player.level / 10 - SkrimpManager.skrimCount;
                 i++)
            {
                var newSkrimp = Instantiate(skrimpPrefab, portal.position, quaternion.identity, transform)
                    .GetComponent<SkrimpHitThingMovePortal>();
                newSkrimp.topPortal = portal;
                newSkrimp.MovetoPortal();
                yield return 0;
            }

        if (SkrimpManager.devSkrimpCount < skrimpInterface.level.devSkrimp)
            for (var i = 0; i < skrimpInterface.level.devSkrimp - SkrimpManager.devSkrimpCount; i++)
            {
                var newSkrimp = Instantiate(devSkrimpPrefab, portal.position, quaternion.identity, transform)
                    .GetComponent<SkrimpHitThingMovePortal>();
                newSkrimp.topPortal = portal;
                newSkrimp.MovetoPortal();
                yield return 0;
            }

        if (SkrimpManager.skrimCount < skrimpInterface.level.skrimpCount + oracle.saveData.player.level / 10 ||
            SkrimpManager.devSkrimpCount < skrimpInterface.level.devSkrimp) StartCoroutine(StaggerSpawn());
    }
}