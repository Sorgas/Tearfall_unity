using game.model;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;
using util.geometry;

namespace mainMenu.worldmap {
    public class WorldmapController : MonoBehaviour {
        // external objects
        public RectTransform mask;
        public RectTransform image;
        public TileBase[] tileBases;
        public Text cellDetailsText;

        // prefab components
        public Transform pointer;
        public Tilemap tilemap;
        public Camera _camera;

        public int worldSize;
        private int tileSize = 32;
        private ScrollableCameraController controller;
        private WorldmapPointerController pointerController;
        private WorldMap worldMap;
        private IntVector2 cacheVector = new IntVector2();

        public void drawWorld(WorldMap worldMap) {
            clear();
            this.worldMap = worldMap;
            worldSize = worldMap.size;
            pointer.gameObject.SetActive(true);
            Vector3 bounds = new Vector3(worldSize * tileSize, worldSize * tileSize, 0);

            pointerController = new WorldmapPointerController(worldSize, pointer);
            controller = new ScrollableCameraController(mask.rect, image, _camera, worldSize, pointerController);
            
            Vector3Int cachePosition = new Vector3Int();
            for (int x = 0; x < worldSize; x++) {
                for (int y = 0; y < worldSize; y++) {
                    cachePosition.Set(x, y, 0);
                    tilemap.SetTile(cachePosition, tileBases[Random.Range(0, tileBases.Length - 1)]);
                }
            }
            updateHintText();
        }

        public void clear() {
            tilemap.ClearAllTiles();
            pointer.gameObject.SetActive(false);
        }

        private void Update() {
            if (controller != null) controller.handleInput();
            if (pointerController != null) {
                pointerController.update();
                if (pointerController.pointerMoved) {
                    updateHintText();
                }
            }
        }

        private void updateHintText() {
            cacheVector.set(pointerController.pointer.localPosition);
            cellDetailsText.text = cacheVector + " " + worldMap.elevation[cacheVector.x, cacheVector.y];
        }
    }
}