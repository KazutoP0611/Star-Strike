using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class BoosterView : MonoBehaviour
{
    [SerializeField] private RectTransform boosterGroup;
    [SerializeField] private GameObject boosterIndicatorUIPrefab;

    private List<BoosterIndicator> boosterIndicators;

    public void Intialize(int maxBoost)
    {
        boosterIndicators = new List<BoosterIndicator>();

        CreateBoosterIndicators(maxBoost);
    }

    private void CreateBoosterIndicators(int maxBoost)
    {
        for (int i = 0; i < maxBoost; i++)
        {
            GameObject boosterObject = Instantiate(boosterIndicatorUIPrefab, boosterGroup);
            BoosterIndicator boosterIndicatorTemp = boosterObject.GetComponent<BoosterIndicator>();
            boosterIndicators.Add(boosterIndicatorTemp);
        }

        SetBoostIndicators(maxBoost);
    }

    public void SetBoostIndicators(int currentBoost)
    {
        for (int i = 0; i < boosterIndicators.Count(); i++)
        {
            boosterIndicators[i].SetBoosterSprite(i < currentBoost);
        }
    }
}
