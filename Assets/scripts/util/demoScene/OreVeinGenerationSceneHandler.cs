using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

namespace util.demoScene {
public class OreVeinGenerationSceneHandler : MonoBehaviour {
    public Button button;
    public Image image;
    public Slider imageSizeSlider;

    public Slider veinLengthSlider;
    public Slider veinStepSlider;
    public Slider veinMaxTurnSlider;
    public Slider veinTurnChanceSlider;

    private float[,] baseArray;
    private float[,] modArray;
    private float[,] resultArray;
    private float scale = 1f;
    private int size = 100;

    public void Start() {
        button.onClick.AddListener(() => { createVein(imageSizeSlider.value); });
        veinLengthSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinStepSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinMaxTurnSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinTurnChanceSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
    }

    private void flushToImage(float min, float max) {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (var y = 0; y < size; y++) {
            for (var x = 0; x < size; x++) {
                texture.SetPixel(x, y, getColor((int)resultArray[x, y]));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0, 0));
        image.sprite = sprite;
    }

    private void createVein(float newSize) {
        size = Mathf.RoundToInt(newSize) * 100;
        // scale = newScale;
        baseArray = new float[size, size];
        modArray = new float[size, size];
        resultArray = new float[size, size];

        Vector2 position = new Vector2(size / 2, size / 2);
        Vector2 direction = new Vector3(veinStepSlider.value, 0, 0);
        direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * direction;
        buildVeinSide(position, direction);
        direction = Quaternion.Euler(0, 0, 180) * direction;
        buildVeinSide(position, direction);
        flushToImage(0, 2);
    }

    private void buildVeinSide(Vector2 position, Vector2 direction) {
        float turnMax = veinMaxTurnSlider.value;
        Vector2 currentDirection = direction;
        int length = (int)veinLengthSlider.value;
        List<Vector2Int> dots = new();
        for (int i = 0; i < length; i++) {
            // addSpot();
            position += currentDirection;
            resultArray[(int)position.x, (int)position.y] = 1;
            dots.Add(new Vector2Int((int)position.x, (int)position.y));
            if (Random.Range(0, 10) < veinTurnChanceSlider.value) {
                currentDirection = Quaternion.Euler(0, 0, Random.Range(-turnMax, turnMax)) * currentDirection;
            }
        }
        for (var i = 0; i < dots.Count; i++) {
            Vector2Int dot = dots[i];
            GenerateConnectedCells(dot.x, dot.y, 1 + (length - i) * 4 / length);
        }
    }

    private void addSpot(int cx, int cy, int radius) {
        int sqrRadius = radius * radius;
        Vector2Int center = new Vector2Int(cx, cy);
        // for (int i = 0; i < 5; i++) {
        //     // Vector2 newPosition = new Vector2(cx + Random.Range(-1, 2), cy + Random.Range(-1, 2));
        //     int nx = cx + Random.Range(-1, 2);
        //     int ny = cy + Random.Range(-1, 2);
        //     if (nx >= 0 && nx < size && ny >= 0 && ny < size) {
        //             resultArray[nx,ny] = 1;
        //     }
        // }
        resultArray[cx, cy] = 1;
        // for (var y = cy - radius - 1; y < cy + radius + 1; y++) {
        //     for (var x = cx - radius - 1; x < cx + radius + 1; x++) {
        //         if (x >= 0 && x < size && y >= 0 && y < size) {
        //             Vector2Int point = new Vector2Int(x, y);
        //             if ((point - center).magnitude < radius) {
        //                 resultArray[x,y] = 1;
        //             }
        //         }
        //     }
        // }
        // Vector2Int point = new Vector2Int(x, y);
        // if ((point - center).magnitude < radius) {
        // }
    }

    public void GenerateConnectedCells(int cx, int cy, int numberOfCells) { 
        Debug.Log(numberOfCells);
        int cellsAdded = 0;
        (int, int) pos = new(1, 2);
        List<(int, int)> frontier = new();
        frontier.Add((cx, cy));
        while (cellsAdded < numberOfCells && frontier.Count > 0) {
            var (x, y) = frontier[Random.Range(0, frontier.Count)]; // Choose a random cell from the frontier
            // var (x, y) = frontier[0]; // Choose a random cell from the frontier
            if (resultArray[x, y] == 0) {
                resultArray[x, y] = 2;
                cellsAdded++;
            }
            frontier.Remove((x, y));
            List<(int, int)> neighbors = GetNeighbors(x, y);

            foreach (var (nx, ny) in neighbors) {
                if (resultArray[nx, ny] == 0 && !frontier.Contains((nx, ny))) {
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

    private Color getColor(int value) {
        return value switch {
            0 => Color.black,
            1 => Color.white,
            2 => Color.red,
            _ => Color.gray
        };
    }
}
}