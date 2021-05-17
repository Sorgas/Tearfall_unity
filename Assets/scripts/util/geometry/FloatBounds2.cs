using System;
using UnityEngine;

namespace Assets.scripts.util.geometry {
    // TODO Vector2.x = value doesn't work!
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

        public void putIn(IntVector3 vector) {
            vector.x = (int)Math.Max(minX, Math.Min(maxX, vector.x));
            vector.y = (int)Math.Max(minY, Math.Min(maxY, vector.y));
        }

        // vector from outside position to nearest border
        public Vector2 getInVector(Vector2 position) {
            Vector2 vector = new Vector2();
            if (position.x < minX) vector.x = minX - position.x;
            if (position.x > maxX) vector.x = maxX - position.x;
            if (position.y < minY) vector.y = minY - position.y;
            if (position.y > maxY) vector.y = maxY - position.y;
            return vector;
        }

        // vector from border to outside position
        public Vector2 getDirectionVector(Vector2 position) {
            Vector2 vector = new Vector2();
            if (position.x < minX) vector.x = position.x - minX;
            if (position.x > maxX) vector.x = position.x - maxX;
            if (position.y < minY) vector.y = position.y - minY;
            if (position.y > maxY) vector.y = position.y - maxY;
            return vector;
        }

       public Vector3 putInto(Vector3 vector) {
            vector.x = Math.Min(maxX, Math.Max(minX, vector.x));
            vector.y = Math.Min(maxY, Math.Max(minY, vector.y));
            return vector;
        }

        public bool isIn(Vector3 vector) => isIn(vector.x, vector.y);

        public bool isIn(IntVector3 vector) => isIn(vector.x, vector.y);

        public bool isIn(float x, float y) {
            return x >= minX && x <= maxX && y >= minY && y <= maxY;
        }

        public void extend(float value) {
            extendX(value);
            extendY(value);
        }

        public void extendX(float value) {
            minX -= value;
            maxX += value;
        }

        public void extendY(float value) {
            minY -= value;
            maxY += value;
        }

        // adds vector components to bounds components, 'moving' the frame
        public void move(Vector2 vector) => move(vector.x, vector.y);

        public void move(float x, float y) {
            maxX += x;
            minX += x;
            maxY += y;
            minY += y;
        }

        public override string ToString() {
            return "x:[" + minX + " " + maxX + "], y:[" + minY + " " + maxY + "]";
        }
    }
}