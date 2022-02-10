using UnityEngine;

namespace Unstable.Utils
{
    public static class VectorUtils
    {
        public static Vector3 ConvertXy2Xz(this Vector2 vector2)
        {
            return new Vector3(vector2.x, 0.0f, vector2.y);
        }

        public static Vector2 ConvertXz2Xy(this Vector3 vector3)
        {
            return new Vector2(vector3.x, vector3.z);
        }
    }
}