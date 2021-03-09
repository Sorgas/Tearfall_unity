using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Assets.scripts.mainMenu.worldmap
{
// associates tile assets to world cell types
public class TilesHolder : MonoBehaviour {
    public Dictionary<string, TileBase> tiles = new Dictionary<string, TileBase>();
    public int qwer;

    public void Start() {
       
    }

}
}