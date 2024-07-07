using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace game.view.util {
    public class LocalMapLayerHandler : MonoBehaviour {
        public Tilemap tilemap;
        public TilemapRenderer tilemapRenderer;
        public bool locked;
        private List<TileChangeData> tilesToSet = new();

        public void setVisible(bool value) {
            tilemapRenderer.enabled = value;
        }

        public void setTile(Vector3Int position, Tile tile) => setTile(position, tile, tile != null ? tile.color : Color.white);

        public void setTile(Vector3Int position, Tile tile, Color color) {
            if (locked) {
                tilesToSet.Add(new TileChangeData(position, tile, color, Matrix4x4.identity));
            } else {
                tilemap.SetTile(position, tile);
                tilemap.SetTileFlags(position, TileFlags.None);
                tilemap.SetColor(position, color);
            }
        }
        
        public void setLock() {
            locked = true;
        }

        public void unlock() {
            if (!locked) return;
            tilemap.SetTiles(tilesToSet.ToArray(), true);
            tilesToSet.Clear();
            locked = false;
        }
    }
}