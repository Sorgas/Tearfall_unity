using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.scripts.util.geometry
{
public class IntBounds2 {
    public int minX;
    public int minY;
    public int maxX;
    public int maxY;
    public int width;
    public int height;

    public IntBounds2() : this(0, 0, 0, 0) {
    }

    public IntBounds2(int minX, int minY, int maxX, int maxY) {
        set(minX, minY, maxX, maxY);
    }

    public IntBounds2(IntVector3 start, Vector2 size) : this(start, size.x, size.y) {
    }

    public IntBounds2(IntVector3 start, float width, float height) : this(start, (int)Math.Round(width),
        (int)Math.Round(height)) {
    }

    public IntBounds2(IntVector3 start, int width, int height) : this(start.x, start.y, start.x + width - 1,
        start.y + height - 1) {
    }

    public IntBounds2(int width, int height) : this(0, 0, width - 1, height - 1) {
    }

    public IntBounds2(IntBounds2 bounds) {
        set(bounds);
    }

    public bool isIn(IntVector3 vector) {
        return isIn(vector.x, vector.y);
    }

    public bool isIn(Vector2 vector) {
        return isIn(vector.x, vector.y);
    }

    public bool isIn(float x, float y) {
        return x >= minX && x <= maxX && y >= minY && y <= maxY;
    }

    public IntBounds2 set(int minX, int minY, int maxX, int maxY) {
        this.minX = Math.Min(minX, maxX);
        this.maxX = Math.Max(minX, maxX);
        this.minY = Math.Min(minY, maxY);
        this.maxY = Math.Max(minY, maxY);
        width = maxX - minX;
        height = maxY - minY;
        return this;
    }

    public IntBounds2 set(IntBounds2 bounds) {
        set(bounds.minX, bounds.minY, bounds.maxX, bounds.maxY);
        return this;
    }

    public void clamp(int minX, int minY, int maxX, int maxY) {
        this.minX = Math.Max(this.minX, minX);
        this.minY = Math.Max(this.minY, minY);
        this.maxX = Math.Min(this.maxX, maxX);
        this.maxY = Math.Min(this.maxY, maxY);
    }

    public IntBounds2 extend(int value) {
        minX -= value;
        minY -= value;
        maxX += value;
        maxY += value;
        return this;
    }

    public void iterate(Action<int, int> action) {
        for (int x = minX; x <= maxX; x++) {
            for (int y = maxY; y >= minY; y--) {
                action.Invoke(x, y);
            }
        }
    }

    public void extendTo(Vector2 vector) {
        extendTo((int)Math.Round(vector.x), (int)Math.Round(vector.y));
    }

    public void extendTo(int x, int y) {
        if (x > 0) maxX += x;
        if (x < 0) minX += x;
        if (y > 0) maxY += y;
        if (y < 0) minY += y;
    }

    public List<IntVector2> list() {
        List<IntVector2> list = new List<IntVector2>();
        for (int x = minX; x <= maxX; x++) {
            for (int y = minY; y <= maxY; y++) {
                list.Add(new IntVector2(x, y));
            }
        }
        return list;
    }

    public List<IntVector2> listBorders() {
        List<IntVector2> list = new List<IntVector2>();
        for (int x = minX; x <= maxX; x++) {
            list.Add(new IntVector2(x, minY));
            list.Add(new IntVector2(x, maxY));
        }
        for (int y = minY + 1; y < maxY; y++) {
            list.Add(new IntVector2(minX, y));
            list.Add(new IntVector2(maxX, y));
        }
        return list;
    }

    public bool isCorner(int x, int y) {
        return (x == minX || x == maxX) && (y == minY || y == maxY);
    }

    public Vector3 putInto(Vector3 vector) {
        vector.x = Math.Min(maxX, Math.Max(minX, vector.x));
        vector.y = Math.Min(maxY, Math.Max(minY, vector.y));
        return vector;
    }

    public String toString() {
        return "Int3dBounds{" + " " + minX + " " + minY + " " + maxX + " " + maxY + '}';
    }
}
}