using System;
using game.view.tilemaps;
using UnityEngine;
using util.geometry;
using util.geometry.bounds;

namespace game.view.camera {
    // stores bounds of tiles, displayed as selected
    // adds new tiles when selection is updated.
    public class SelectionState {
        public LocalMapTileUpdater updater; // for showing selection frame tiles
        public Vector3Int selectionStart;
        public TwoPointIntBounds3 bounds = new(); // actual bounds of selection
        public SelectionType selectionType;
        public bool started;

        // optimization
        private Vector3Int previousPos; // mouse position
        private TwoPointIntBounds3 previousBounds = new();
        
        public void startSelection(Vector3Int pos) {
            started = true;
            selectionStart = pos;
            previousPos = pos;
            bounds.set(pos, pos);
            previousBounds.set(pos, pos);
            updater.createSelectionTile(pos.x, pos.y, pos.z);
        }

        public void update(Vector3Int pos) {
            if (pos == previousPos || selectionType == SelectionType.SINGLE) return;
            bounds.pos2 = pos;
            if (selectionType == SelectionType.ROW) {
                if (Math.Abs(bounds.width) > Math.Abs(bounds.height)) {
                    pos.y = bounds.pos1.y;
                } else {
                    pos.x = bounds.pos1.x;
                }
            }
            bounds.pos2 = pos;
            updateView(pos);
            previousBounds.set(bounds);
            previousPos = pos;
        }

        public void reset() {
            started = false;
            selectionStart = Vector3Int.left;
            bounds.iterate((x, y, z) => updater.hideSelectionTile(x,y,z));
            bounds.set(-1, -1, -1, -1, -1, -1);
        }
        
        private void updateView(Vector3Int newPos) {
            Vector3Int delta = newPos - previousPos;
            if (delta.x != 0) {
                new ValueRangeInt().setAndNormalize(previousPos.x, newPos.x).iterate(x => {
                    previousBounds.iterateY(y => {
                        previousBounds.iterateZ(z => {
                            updateTileByBounds(x, y, z);
                        });
                    });
                });
            }
            if (delta.y != 0) {
                new ValueRangeInt().setAndNormalize(previousPos.y, newPos.y).iterate(y => {
                    previousBounds.iterateX(x => {
                        previousBounds.iterateZ(z => {
                            updateTileByBounds(x, y, z);
                        });
                    });
                });
            }
            if (delta.z != 0) {
                new ValueRangeInt().setAndNormalize(previousPos.z, newPos.z).iterate(z => {
                    previousBounds.iterateY(y => {
                        previousBounds.iterateX(x => {
                            updateTileByBounds(x, y, z);
                        });
                    });
                });
            }
        }

        private void updateTileByBounds(int x, int y, int z) {
            if (bounds.isIn(x, y, z)) {
                updater.createSelectionTile(x,y,z);
            } else {
                updater.hideSelectionTile(x, y, z);
            }
        }
    }

    public enum SelectionType {
        AREA,
        ROW,
        SINGLE
    }
}