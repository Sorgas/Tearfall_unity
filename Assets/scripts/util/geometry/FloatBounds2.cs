using UnityEngine;

namespace util.geometry {
    public class FloatBounds2 {
        public float minX;
        public float minY;
        public float maxX;
        public float maxY;

        public FloatBounds2() : this(0, 0, 0, 0) {}

        public FloatBounds2(float minX, float minY, float maxX, float maxY) {
            set(minX, minY, maxX, maxY);
        }

        public void set(float minX, float minY, float maxX, float maxY) {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
        }

        public Vector3 getOutVector(Vector2 bottomLeft, Vector2 topRight) {
            Vector3 vector = new Vector3();
            if (bottomLeft.x < minX) vector.x = bottomLeft.x - minX;
            if (topRight.x > maxX) vector.x = topRight.x - maxX;
            if (bottomLeft.y < minY) vector.y = bottomLeft.y - minY;
            if (topRight.y > maxY) vector.y = topRight.y - maxY;
            return vector;
        }

        public bool isIn(Vector3 vector) {
            return isIn(vector.x, vector.y);
        }

        public bool isIn(float x, float y) {
            return x >= minX && x <= maxX && y >= minY && y <= maxY;
        }

        public void extend(float value) {
            minX -= value;
            minY -= value;
            maxX += value;
            maxY += value;
        }
    }
}