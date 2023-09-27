using System;
using System.Collections.Generic;
using UnityEngine;
using util.geometry.bounds;

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
        public Vector3Int horizontalSize3;
        public Vector3Int verticalSize3;
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
            horizontalSize3 = new Vector3Int(size[1], size[0], 0);
            verticalSize3 = new Vector3Int(size[0], size[1], 0);
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
        public Vector3Int getSize3ByOrientation(Orientations orientation) => OrientationUtil.isHorizontal(orientation) ? horizontalSize3 : verticalSize3;
        public IntBounds3 getBounds(Vector3Int position, Orientations orientation) => IntBounds3.byStartAndSize(position, getSize3ByOrientation(orientation));

        public Vector3Int getAccessByPositionAndOrientation(Vector3Int position, Orientations orientation) {
            return position + getAccessOffsetByRotation(orientation);
        }

        public Vector3Int getAccessOffsetByRotation(Orientations orientation) {
            return orientation switch {
                Orientations.N => new Vector3Int(access[0], access[1], 0),
                Orientations.E => new Vector3Int(access[1], size[0] - 1 - access[0], 0),
                Orientations.S => new Vector3Int(size[0] - 1 - access[0], size[1] - 1 - access[1], 0),
                Orientations.W => new Vector3Int(size[1] - 1 - access[1], access[0], 0),
                _ => throw new ArgumentOutOfRangeException()
            };
        }
    }
}