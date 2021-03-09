using UnityEngine;

namespace Assets.scripts.util.geometry {
    public class FloatBounds2 {
        public float minX;
        public float minY;
        public float maxX;
        public float maxY;

        public FloatBounds2() : this(0, 0, 0, 0) { }

        public FloatBounds2(float minX, float minY, float maxX, float maxY) {
            set(minX, minY, maxX, maxY);
        }

        public FloatBounds2 set(float minX, float minY, float maxX, float maxY) {
            this.minX = minX;
            this.minY = minY;
            this.maxX = maxX;
            this.maxY = maxY;
            return this;
        }

        public FloatBounds2 set(FloatBounds2 bounds) {
            return set(bounds.minX, bounds.minY, bounds.maxX, bounds.maxY);
        }

        public Vector3 getOutVector(Vector2 bottomLeft, Vector2 topRight) {
            Vector3 vector = new Vector3();
            if (bottomLeft.x < minX) vector.x = bottomLeft.x - minX;
            if (topRight.x > maxX) vector.x = topRight.x - maxX;
            if (bottomLeft.y < minY) vector.y = bottomLeft.y - minY;
            if (topRight.y > maxY) vector.y = topRight.y - maxY;
            return vector;
        }
        // returns vector that shows distance to target
        public Vector2 getDirectionVector(Vector2 target) {
            Vector2 vector = new Vector2();
            if (target.x < minX) vector.x = target.x - minX;
            if (target.x > maxX) vector.x = target.x - maxX;
            if (target.y < minY) vector.y = target.y - minY;
            if (target.y > maxY) vector.y = target.y - maxY;
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

        // adds vector components to bounds components, 'moving' the frame
        public void move(Vector2 vector) {
            maxX += vector.x;
            minX += vector.x;
            maxY += vector.y;
            minY += vector.y;
        }

        public override string ToString() {
            return "x:[" + minX + " " + maxX + "], y:[" + minY + " " + maxY + "]";
        }
    }
}