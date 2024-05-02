using System;
using System.Collections.Generic;
using game.model.component.task.order;
using types.item.recipe;
using UnityEngine;
using util.geometry.bounds;
using util.lang;

namespace types.building {
    public class BuildingType {
        public const string SLEEP_FURNITURE = "sleepFurniture";
        public const string DOOR = "door";

        public string name;
        public string title;
        public string tileset;
        public int tilesetSize = 64;
        public int tileCount = 1; // for each orientation
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
        public string passage = PassageTypes.PASSABLE.name;
        public string category;

        public List<string> components = new(); // components that should be added to building
        public MultiValueDictionary<string, Ingredient> ingredients = new();
        public int buildingExp;
        
        public BuildingOrder dummyOrder;
        
        public void init() {
            horizontalSize = new Vector2Int(size[1], size[0]);
            verticalSize = new Vector2Int(size[0], size[1]);
            horizontalSize3 = new Vector3Int(size[1], size[0], 0);
            verticalSize3 = new Vector3Int(size[0], size[1], 0);
        }
        
        public bool isSingleTile() => size[0] == 1 && size[1] == 1;
        
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

public class RawBuildingType {
    public const string SLEEP_FURNITURE = "sleepFurniture";

    public string name;
    public string title;
    public string tileset;
    public int tilesetSize;
    public int tileCount;
    public int[] size; // for N, S orientations
    public int[] positionN;
    public int[] positionS;
    public int[] positionE;
    public int[] positionW;
    public string passage;
    public int[] access; // TODO add rawbuilding type class, use Vector2Int here
    public string job; // for workbenches
    public string category;

    public string[] components;
    public string[] ingredients;
    public int buildingExp;
}
}