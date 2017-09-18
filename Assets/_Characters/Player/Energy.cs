﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG.Characters
{
    public class Energy : MonoBehaviour
    {
        [SerializeField] RawImage energyBarImage = null;
        [SerializeField] float maxEnergyPoints = 100f;
        [SerializeField] float regenPointsPerSecond = 1f;

        CameraUI.CameraRaycaster cameraRaycaster;
        float currentEnergyPoints;
        
        // Use this for initialization
        void Start()
        {
            currentEnergyPoints = maxEnergyPoints;
        }

        void Update()
        {
            if (currentEnergyPoints < maxEnergyPoints)
            {
                AddEnergyPoint();
                UpdateEnergyBar();
            }
        }

        private void AddEnergyPoint()
        {
            var pointsToAdd = regenPointsPerSecond * Time.deltaTime;  // multiply Time.deltaTime to make things happen per second instead of per frame.
            currentEnergyPoints = Mathf.Clamp(currentEnergyPoints + pointsToAdd, 0, maxEnergyPoints); // if you don't clamp it it may go above MAX if regends too fast.
        }

        public bool IsEnergyAvailable(float amount)
        {
            return amount <= currentEnergyPoints;
        }

        public void ConsumeEnergy(float amount)
        {
            float newEnergyPoints = currentEnergyPoints - amount;
            currentEnergyPoints = Mathf.Clamp(newEnergyPoints, 0f, maxEnergyPoints);
            UpdateEnergyBar();
        }

        private void UpdateEnergyBar()
        {
            // TODO remove magic numbers
            float xValue = -(EnergyAsPercentage() / 2f) - 0.5f;
            energyBarImage.uvRect = new Rect(xValue, 0f, 0.5f, 1f);
        }

        float EnergyAsPercentage()
        {
                return currentEnergyPoints / maxEnergyPoints;
        }
    }
}
