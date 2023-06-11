using System;
using System.Collections.Generic;
using UnityEngine;

namespace types.building {
    [Serializable]
    public class BuildingType {
        public const string SLEEP_FURNITURE = "sleepFurniture";

        public string name;
        public string title;
        public string tileset;
        public int[] size; // for N, S orientations
        public Vector2Int horizontalSize;
        public Vector2Int verticalSize;
        public int[] positionN;
        public int[] positionS;
        public int[] positionE;
        public int[] positionW;
        public int[] access; // TODO add rawbuilding type class, use Vector2Int here
        public string job; // for workbenches
        public string passage;
        public string category;

        public string[] materials; // ways to build this building (raw)
        public BuildingVariant[] variants; // ways to build this building
        
        public string[] rawComponents;
        public List<string> components = new(); // components that should be added to building

        public void init() {
            horizontalSize = new Vector2Int(size[1], size[0]);
            verticalSize = new Vector2Int(size[0], size[1]);
        }
        
        public bool isSingleTile() => size[0] == 1 && size[1] == 1;

        public BuildingVariant selectVariant(string itemType) {
            for (var i = 0; i < variants.Length; i++) {
                if (variants[i].itemType.Equals(itemType)) {
                    return variants[i];
                }
            }
            return null;
        }

        public Vector2Int getSizeByOrientation(Orientations orientation) => OrientationUtil.isHorizontal(orientation) ? horizontalSize : verticalSize;
    }
}