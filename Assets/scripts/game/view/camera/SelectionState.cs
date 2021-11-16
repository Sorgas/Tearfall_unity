using System;
using game.model.tilemaps;
using UnityEngine;
using util.geometry;

namespace game.view.camera {

    // stores bounds of tiles, displayed as selected
    // adds new tiles when selection is updated.
    public class SelectionState {
        private Vector3Int end;
        private TwoPointIntBounds3 bounds = new TwoPointIntBounds3();
        private TwoPointIntBounds3 maxBounds = new TwoPointIntBounds3(); // cache instance
        private LocalMapTileUpdater updater;

        public void startSelection(Vector3Int pos) {
            end = pos;
            bounds.set(pos, pos);
            maxBounds.set(pos, pos);
            updater.createSelectionTile(pos.x, pos.y, pos.z);
        }

        public void update(Vector3Int pos) {
            bounds.pos2 = pos;
            maxBounds.set(bounds).extendTo(pos);
            updateView(pos);
            end = pos;
        }

        public void reset() {
            bounds.iterate((x, y, z) => updater.hideSelectionTile(x,y,z));
        }

        private void updateView(Vector3Int newEnd) {
            Vector3Int delta = newEnd - end;
            ValueRangeInt xRange;
            ValueRangeInt yRange;
            ValueRangeInt zRange;
            if (delta.x != 0) {
                xRange = Math.Sign(end.x - bounds.pos1.x) == Math.Sign(delta.x)
                    ? new ValueRangeInt().setAndNormalize(newEnd.x, end.x + Math.Sign(delta.x))
                    : new ValueRangeInt().setAndNormalize(end.x, newEnd.x - Math.Sign(delta.x));
                xRange.iterate(x => {
                    maxBounds.iterateY(y => {
                        maxBounds.iterateZ(z => {
                            if (bounds.isIn(x, y, z)) {
                                updater.createSelectionTile(x,y,z);
                            } else {
                                updater.hideSelectionTile(x, y, z);
                            }
                        });
                    });
                });
            }
            if (delta.y != 0) {
                yRange = Math.Sign(end.y - bounds.pos1.y) == Math.Sign(delta.y)
                    ? new ValueRangeInt().setAndNormalize(newEnd.y, end.y + Math.Sign(delta.y))
                    : new ValueRangeInt().setAndNormalize(end.y, newEnd.y - Math.Sign(delta.y));
            }
            if (delta.z != 0) {
                zRange = Math.Sign(end.z - bounds.pos1.z) == Math.Sign(delta.z)
                    ? new ValueRangeInt().setAndNormalize(newEnd.z, end.z + Math.Sign(delta.z))
                    : new ValueRangeInt().setAndNormalize(end.z, newEnd.z - Math.Sign(delta.z));
            }
            
        }
    }
}