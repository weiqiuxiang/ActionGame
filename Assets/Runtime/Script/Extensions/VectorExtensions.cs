using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Extensions
{
    /// <summary>
    /// Vector2、Vector3拡張
    /// </summary>
    public static class VectorExtensions
    {
        public static Vector3 SetX(this Vector3 vector, float value)
        {
            vector.x = value;
            return vector;
        }
        
        public static Vector3 SetY(this Vector3 vector, float value)
        {
            vector.y = value;
            return vector;
        }
        
        public static Vector3 SetZ(this Vector3 vector, float value)
        {
            vector.z = value;
            return vector;
        }
        
        public static Vector2 SetX(this Vector2 vector, float value)
        {
            vector.x = value;
            return vector;
        }
        
        public static Vector2 SetY(this Vector2 vector, float value)
        {
            vector.y = value;
            return vector;
        }
    }
}
