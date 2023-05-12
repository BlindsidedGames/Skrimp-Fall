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
        var SkrimpOwned = skrimpInterface.level.skrimpCount + oracle.saveData.player.level / 10;
        var devSkrimpOwned = skrimpInterface.level.devSkrimp;
        var totalSkrimp = SkrimpOwned + devSkrimpOwned;
        long totalSkrimpToSpawn = 0;
        switch (oracle.saveData.preferences.skrimpOnScreen)
        {
            case SkrimpOnScreen.Fifty:
                totalSkrimpToSpawn = totalSkrimp > 50 ? 50 : totalSkrimp;
                break;
            case SkrimpOnScreen.OneHundred:
                totalSkrimpToSpawn = totalSkrimp > 100 ? 100 : totalSkrimp;
                break;
            case SkrimpOnScreen.Unlimited:
                totalSkrimpToSpawn = totalSkrimp;
                break;
            case SkrimpOnScreen.MegaSkrimp:
                totalSkrimpToSpawn = 1;
                break;
            default:
                goto case SkrimpOnScreen.Fifty;
        }

        var skrimpToSpawn = SkrimpOwned < totalSkrimpToSpawn ? SkrimpOwned : totalSkrimpToSpawn;
        var devSkrimpToSpawn = skrimpToSpawn < totalSkrimpToSpawn ? totalSkrimpToSpawn - skrimpToSpawn : 0;

        skrimpInterface.level.valueMultiFromBonusSkrimp = totalSkrimp > totalSkrimpToSpawn
            ? 1f + (float)(totalSkrimp - totalSkrimpToSpawn) / totalSkrimpToSpawn
            : 1;


        if (SkrimpManager.skrimCount < skrimpToSpawn)
            for (var i = 0;
                 i < skrimpToSpawn - SkrimpManager.skrimCount;
                 i++)
            {
                var newSkrimp = Instantiate(skrimpPrefab, portal.position, quaternion.identity, transform)
                    .GetComponent<SkrimpHitThingMovePortal>();
                newSkrimp.topPortal = portal;
                newSkrimp.MovetoPortal();
                yield return 0;
            }

        if (SkrimpManager.devSkrimpCount < devSkrimpToSpawn)
            for (var i = 0; i < devSkrimpToSpawn - SkrimpManager.devSkrimpCount; i++)
            {
                var newSkrimp = Instantiate(devSkrimpPrefab, portal.position, quaternion.identity, transform)
                    .GetComponent<SkrimpHitThingMovePortal>();
                newSkrimp.topPortal = portal;
                newSkrimp.MovetoPortal();
                yield return 0;
            }

        if (SkrimpManager.skrimCount + SkrimpManager.devSkrimpCount < totalSkrimpToSpawn &&
            (SkrimpManager.skrimCount < SkrimpOwned || SkrimpManager.devSkrimpCount < skrimpInterface.level.devSkrimp))
            StartCoroutine(StaggerSpawn());
    }
}