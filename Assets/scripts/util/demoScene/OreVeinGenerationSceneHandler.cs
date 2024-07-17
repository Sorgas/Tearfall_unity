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

    private float[,] array;
    private float scale = 1f;
    private int size = 50;

    public void Start() {
        button.onClick.AddListener(() => { createVein(imageSizeSlider.value); });
        veinLengthSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinStepSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinMaxTurnSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
        veinTurnChanceSlider.onValueChanged.AddListener((value) => createVein(imageSizeSlider.value));
    }

    private void flushToImage() {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (var y = 0; y < size; y++) {
            for (var x = 0; x < size; x++) {
                texture.SetPixel(x, y, getColor((int)array[x, y]));
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0, 0));
        image.sprite = sprite;
    }

    private void createVein(float newSize) {
        size = Mathf.RoundToInt(newSize) * 50;
        array = new float[size, size];

        Vector2 position = new Vector2(size / 2, size / 2);
        Vector2 direction = new Vector3(veinStepSlider.value, 0, 0);
        direction = Quaternion.Euler(0, 0, Random.Range(0, 360)) * direction;
        buildVeinSide(position, direction);
        direction = Quaternion.Euler(0, 0, 180) * direction;
        buildVeinSide(position, direction);
        flushToImage();
    }

    private void buildVeinSide(Vector2 position, Vector2 direction) {
        float turnMax = veinMaxTurnSlider.value;
        Vector2 currentDirection = direction;
        int length = (int)veinLengthSlider.value;
        List<Vector2Int> dots = new();
        for (int i = 0; i < length; i++) {
            position += currentDirection;
            setValue((int)position.x, (int)position.y, 1);
            dots.Add(new Vector2Int((int)position.x, (int)position.y));
            if (Random.Range(0, 10) < veinTurnChanceSlider.value) {
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

    private void GenerateConnectedCells(int cx, int cy, int numberOfCells) { 
        Debug.Log(numberOfCells);
        int cellsAdded = 0;
        (int, int) pos = new(1, 2);
        List<(int, int)> frontier = new();
        frontier.Add((cx, cy));
        while (cellsAdded < numberOfCells && frontier.Count > 0) {
            var (x, y) = frontier[Random.Range(0, frontier.Count)]; // Choose a random cell from the frontier
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

    private Color getColor(int value) {
        return value switch {
            0 => Color.black,
            1 => Color.white,
            2 => Color.red,
            _ => Color.gray
        };
    }
    
    private void setValue(int x, int y, int value) {
        if (inArray(x, y)) array[x, y] = value;
    }

    private bool inArray(int x, int y) {
        return x >= 0 && x < size && y >= 0 && y < size;
    }
}
}