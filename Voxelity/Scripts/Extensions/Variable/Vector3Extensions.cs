using UnityEngine;

namespace Voxelity.Extensions
{
    /// <summary>
    /// Vector3 extensions.
    /// </summary>
    public static class Vector3Extensions
    {
        public static Vector3 WithX(this Vector3 self, float x) => new Vector3(x, self.y, self.z);
        public static Vector3 WithY(this Vector3 self, float y) => new Vector3(self.x, y, self.z);
        public static Vector3 WithZ(this Vector3 self, float z) => new Vector3(self.x, self.y, z);
        public static Vector3 With(this Vector3 self, float? x = null, float? y = null, float? z = null) => new Vector3(x ?? self.x, y ?? self.y, z ?? self.z);
        public static Vector3 Flat(this Vector3 self, float yValue = 0) => new Vector3(self.x, yValue, self.z);
        public static Vector3Int ToVector3Int(this Vector3 self) => new Vector3Int((int)self.x, (int)self.y, (int)self.z);
        public static Vector2 ToV2(this Vector3 self) => new Vector2(self.x, self.y);
        public static Vector3 DirectionTo(this Vector3 source, Vector3 destination) => Vector3.Normalize(destination - source);
        public static string Vector3ToString(this Vector3 self) => $"{self.x}~{self.y}~{self.z}";

        public static Vector3 Divide(this Vector3 source, Vector3 dividedWith) => new Vector3(source.x / dividedWith.x,
            source.y / dividedWith.y, source.z / dividedWith.z);

        public static Vector3 half => new Vector3(0.5f, 0.5f, 0.5f);
    }
}