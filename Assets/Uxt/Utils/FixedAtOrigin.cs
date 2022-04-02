﻿using UnityEngine;

namespace Uxt.Utils
{
    [ExecuteAlways]
    public class FixedAtOrigin : MonoBehaviour
    {
        private void Update()
        {
            if (!Application.isPlaying)
            {
                transform.localPosition = Vector3.zero;
            }
        }
    }
}