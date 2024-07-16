using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace util.demoScene {
public class OreVeinGeneratonSceneHandler : MonoBehaviour {
    public Button button;
    public Image image;
    public Slider imageSizeSlider;
    public Slider noiseScaleSlider;
    public Slider minSlider;
    public Slider maxSlider;
    public Slider modSlider;
    
    private float[,] baseArray;
    private float[,] modArray;
    private float[,] resultArray;
    private float scale = 1f;
    private int size = 100;

    public void Start() {
        // button.onClick.AddListener(() => { createNoise(imageSizeSlider.value, noiseScaleSlider.value); });
        // imageSizeSlider.onValueChanged.AddListener(value => createNoise(value, noiseScaleSlider.value));
        // noiseScaleSlider.onValueChanged.AddListener(value => createNoise(imageSizeSlider.value, value));
        // maxSlider.onValueChanged.AddListener(value => flushToImage(minSlider.value, value));
        // minSlider.onValueChanged.AddListener(value => flushToImage(value, maxSlider.value));
        // modSlider.onValueChanged.AddListener(value => applyMods(value));
        // button.onClick.AddListener(() => { createVein(imageSizeSlider.value); });
    }

    private void flushToImage(float min, float max) {
        var texture = new Texture2D(size, size, TextureFormat.RGBA32, false);
        for (var y = 0; y < size; y++) {
            for (var x = 0; x < size; x++) {
                // float value = array[x, y];
                // Debug.Log($"{x} {y} {value}");
                // texture.SetPixel(x, y, new Color(value, value, value, 1f));
                if (resultArray[x, y] > min && resultArray[x, y] < max) {
                    texture.SetPixel(x, y, Color.white);
                } else {
                    texture.SetPixel(x, y, Color.black);
                }
            }
        }
        texture.filterMode = FilterMode.Point;
        texture.Apply();
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, size, size), new Vector2(0, 0));
        image.sprite = sprite;
    }

    private void createNoise(float newSize, float newScale) {
        size = Mathf.RoundToInt(newSize) * 100;
        scale = newScale;
        baseArray = new float[size, size];
        modArray = new float[size, size];
        resultArray = new float[size, size];
        for (var y = 0; y < size; y++) {
            for (var x = 0; x < size; x++) {
                baseArray[x, y] = Mathf.PerlinNoise(x * scale / 100, y * scale / 100) 
                                   // + Mathf.PerlinNoise(x * scale * 1.3f / 100, y * scale * 1.3f / 100)) / 2f
                    ;
                modArray[x, y] = Mathf.PerlinNoise((x + size) * scale / 100, y * scale / 100);
            }
        }
        // applyMods(modSlider.value);
    }

    private void applyMods(float value) {
        for (var y = 0; y < size; y++) {
            for (var x = 0; x < size; x++) {
                if (modArray[x, y] < value) {
                    resultArray[x, y] = 0;
                } else {
                    resultArray[x, y] = baseArray[x, y];
                }
            }
        }
        flushToImage(minSlider.value, maxSlider.value);
    }
}
}