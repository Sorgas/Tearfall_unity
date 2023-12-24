using UnityEngine;
using util;

namespace game.model.localmap {
// stores tiles that are considered indoors. All other tiles are outdoors.
public class IndoorMap : UtilByteArray {
    
    public IndoorMap(int xSize, int ySize, int zSize) : base(xSize, ySize, zSize) { }
    
    public IndoorMap(Vector3Int size) : base(size) { }
}
}