using System;
using System.Collections.Generic;
using enums;
using game.model;
using game.model.localmap;
using UnityEngine;
using Random = UnityEngine.Random;

namespace generation.localgen.generators {
    public class LocalForestGenerator : LocalGenerator {
        private int maxAttempts = 20;
        private List<Vector3Int> treePositions = new();
        private LocalMap map;
         
        public override void generate() {
            map = GameModel.localMap;
            int treesNumber = config.areaSize * config.areaSize / 125 * config.forestationLevel;
            treesNumber += Random.Range(-1, 1) * 20;
            for (int i = 0; i < treesNumber; i++) {
                Vector3Int treePosition = findPlaceForTree();
                if(treePosition.x >= 0) createTree(treePosition);
            }
        }

        private Vector3Int findPlaceForTree() {
            for (int i = 0; i < maxAttempts; i++) {
                int x = Random.Range(0, map.bounds.maxX);
                int y = Random.Range(0, map.bounds.maxY);
                int z = findZ(x, y);
                if (z >= 0 && checkTreeOverlap(x, y)) return new Vector3Int(x, y, z);
            }
            return Vector3Int.left;
        }

        private int findZ(int x, int y) {
            for (int z = map.bounds.maxZ; z > 0; z--) {
                if (map.blockType.get(x, y, z) == BlockTypeEnum.FLOOR.CODE) {
                    return z;
                }
            }
            return -1;
        }

        private bool checkTreeOverlap(int x, int y) {
            foreach (Vector3Int treePosition in treePositions) {
                if (Math.Abs(treePosition.x - x) < 3 || Math.Abs(treePosition.y - y) < 3) {
                    return false;
                }
            }
            return true;
        }
        
        private void createTree(Vector3Int treePosition) {
            
        }
        
        public override string getMessage() {
            return "Adding forests...";
        }
    }
}