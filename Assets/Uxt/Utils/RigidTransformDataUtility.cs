﻿using UnityEngine;

namespace Uxt.Utils
{
    public static class RigidTransformDataUtility
    {
        public static RigidTransformData ExtractRigidTransformData(this Transform transform)
        {
            return new RigidTransformData(transform.position, transform.rotation);
        }
    }
}