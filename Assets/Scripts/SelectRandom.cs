using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using static Oracle;


public class SelectPatient : MonoBehaviour
{
    [SerializeField] private List<WeightedSpawnSO> ailmentWeights = new();
    [SerializeField] private float[] ailmentWeightsFloat;
    [SerializeField] private List<WeightedSpawnSO> groupWeights = new();
    [SerializeField] private float[] groupWeightsFloat;

    //private Patients patients => oracle.saveData.patients;
    //private Cleric cleric => oracle.saveData.cleric;

    private void Awake()
    {
        ailmentWeightsFloat = new float[ailmentWeights.Count];
        groupWeightsFloat = new float[groupWeights.Count];
    }

    private void Start()
    {
        CalculateWeights();
    }

    private void CalculateWeights()
    {
        CalculateAilmentWeights();
        CalculateGroupWeights();
    }

    private void CalculateAilmentWeights()
    {
        float TotalWeight = 0;

        for (var i = 0; i < ailmentWeights.Count; i++)
        {
            ailmentWeightsFloat[i] = ailmentWeights[i].GetWeight();
            TotalWeight += ailmentWeightsFloat[i];
        }

        for (var i = 0; i < ailmentWeightsFloat.Length; i++) ailmentWeightsFloat[i] /= TotalWeight;
    }

    private void CalculateGroupWeights()
    {
        float TotalWeight = 0;

        for (var i = 0; i < groupWeights.Count; i++)
        {
            groupWeightsFloat[i] = groupWeights[i].GetWeight();
            TotalWeight += groupWeightsFloat[i];
        }

        for (var i = 0; i < groupWeightsFloat.Length; i++) groupWeightsFloat[i] /= TotalWeight;
    }


    public void SelectPatientType()
    {

        SetStatusWeightChances();
        var value = Random.value;
        for (var i = 0; i < ailmentWeightsFloat.Length; i++)
        {
            if (value < ailmentWeightsFloat[i])
            {
                var spawn = ailmentWeights[i].obj;

                switch (spawn)
                {
                    case 1:
                    {
                        break;
                    }
                    
                    default:
                        break;
                }

                return;
            }

            value -= ailmentWeightsFloat[i];
        }
    }

    private void SelectGroupType()
    {
        var value = Random.value;
        for (var i = 0; i < groupWeightsFloat.Length; i++)
        {
            if (value < groupWeightsFloat[i])
            {
                var spawn = groupWeights[i].obj;

                switch (spawn)
                {
                    case 1:
                    {
                        break;
                    }
                    default:
                        break;
                }

                return;
            }

            value -= groupWeightsFloat[i];
        }
    }


    private void SetStatusWeightChances()
    {
        
        CalculateAilmentWeights();
    }

    private void SetGroupWeightChances(double percent)
    {
        groupWeights[0].MinWeight = 1 - percent;
        groupWeights[0].MaxWeight = 1 - percent;

        groupWeights[1].MinWeight = percent / 4;
        groupWeights[1].MaxWeight = percent / 2;

        groupWeights[2].MinWeight = percent;
        groupWeights[2].MaxWeight = percent;
        CalculateGroupWeights();
    }
}