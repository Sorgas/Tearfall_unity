using System.Collections.Generic;
using UnityEngine;

public class SubstrateMap {
    public readonly List<SubstrateCell> cells = new();
}

public class SubstrateCell {
    public Vector3Int position;
    public int type;
}