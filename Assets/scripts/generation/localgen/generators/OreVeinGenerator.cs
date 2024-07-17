using System.Collections.Generic;
using UnityEngine;

namespace generation.localgen.generators {
// generates pattern of one ore vein to 50x50 array
public class OreVeinGenerator {
    private int[,] array;
    private float scale = 1f;
    private int size = 100;

    public void createVein(float newSize, int[,] array) {
        size = Mathf.RoundToInt(newSize);
        this.array = array;

        Vector2 position = new Vector2(size / 2, size / 2);
        Vector2 direction = new Vector3(1, 0, 0);
        direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * direction;
        buildVeinSide(position, direction);
        direction = Quaternion.Euler(0, 0, 180) * direction;
        buildVeinSide(position, direction);
    }

    private void buildVeinSide(Vector2 position, Vector2 direction) {
        float turnMax = 65;
        Vector2 currentDirection = direction;
        int length = 30;
        List<Vector2Int> dots = new();
        for (int i = 0; i < length; i++) {
            position += currentDirection;
            setValue((int)position.x, (int)position.y, 1);
            dots.Add(new Vector2Int((int)position.x, (int)position.y));
            if (Random.Range(0, 10) < 3) {
                currentDirection = Quaternion.Euler(0, 0, Random.Range(-turnMax, turnMax)) * currentDirection;
            }
        }
        for (var i = 0; i < dots.Count; i++) {
            Vector2Int dot = dots[i];
            if (inArray(dot.x, dot.y)) {
                GenerateConnectedCells(dot.x, dot.y, 1 + (length - i) * 4 / length);
            }
        }
    }
    
    public void GenerateConnectedCells(int cx, int cy, int numberOfCells) { 
        int cellsAdded = 0;
        (int, int) pos = new(1, 2);
        List<(int, int)> frontier = new();
        frontier.Add((cx, cy));
        while (cellsAdded < numberOfCells && frontier.Count > 0) {
            var (x, y) = frontier[Random.Range(0, frontier.Count)]; // Choose a random cell from the frontier
            // var (x, y) = frontier[0]; // Choose a random cell from the frontier
            if (array[x, y] == 0) {
                array[x, y] = 2;
                cellsAdded++;
            }
            frontier.Remove((x, y));
            List<(int, int)> neighbors = GetNeighbors(x, y);

            foreach (var (nx, ny) in neighbors) {
                if (array[nx, ny] == 0 && !frontier.Contains((nx, ny))) {
                    frontier.Add((nx, ny));
                }
            }
        }
    }

    private List<(int, int)> GetNeighbors(int x, int y) {
        List<(int, int)> neighbors = new();
        if (x > 0) neighbors.Add((x - 1, y));
        if (x < size - 1) neighbors.Add((x + 1, y));
        if (y > 0) neighbors.Add((x, y - 1));
        if (y < size - 1) neighbors.Add((x, y + 1));
        return neighbors;
    }
    
    private void setValue(int x, int y, int value) {
        if (inArray(x, y)) array[x, y] = value;
    }

    private bool inArray(int x, int y) {
        return x >= 0 && x < size && y >= 0 && y < size;
    }
}
}