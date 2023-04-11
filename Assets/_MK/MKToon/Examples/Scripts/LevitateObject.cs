﻿//////////////////////////////////////////////////////
// MK Toon Examples LevitateObject              	//
//					                                //
// Created by Michael Kremmel                       //
// www.michaelkremmel.de                            //
// Copyright © 2020 All rights reserved.            //
//////////////////////////////////////////////////////

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MK.Toon.Examples
{
    public class LevitateObject : MonoBehaviour
    {
        [SerializeField]
        private float _timeOffset = 0;
        [SerializeField]
        private float scale = 0.25f;
        private Vector3 _awakePosition = Vector3.zero;

        private void Awake()
        {
            _awakePosition = transform.position;
        }

        private void Update()
        {
            transform.position = _awakePosition + Vector3.up * Mathf.Sin(Time.time + _timeOffset) * scale;
        }
    }
}
