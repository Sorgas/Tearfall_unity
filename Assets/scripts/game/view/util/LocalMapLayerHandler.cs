using UnityEngine;
using UnityEngine.Tilemaps;

namespace game.view.util {
    public class LocalMapLayerHandler : MonoBehaviour {
        public Tilemap tilemap;
        public TilemapRenderer tilemapRenderer;
        public MeshRenderer planeRenderer;

        public void setVisible(bool value) {
            tilemapRenderer.enabled = value;
            planeRenderer.enabled = value;
        }
        
        public void setTile(Vector3Int position, Tile tile) => tilemap.SetTile(position, tile);

        public void setTile(Vector3Int position, Tile tile, Color color) {
            tilemap.SetTile(position, tile);
            tilemap.SetTileFlags(position, TileFlags.None);
            tilemap.SetColor(position, color);
        }
    }
}