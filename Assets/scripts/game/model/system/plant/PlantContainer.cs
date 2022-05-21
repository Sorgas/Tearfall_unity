using System.Collections.Generic;
using game.model.component;
using game.model.component.plant;
using Leopotam.Ecs;
using UnityEngine;
using util.lang.extension;

namespace game.model.system.plant {
    // TODO handle multi-tile trees
    public class PlantContainer : ModelComponent {
        private Dictionary<Vector3Int, PlantBlock> plantBlocks = new();
        private Dictionary<Vector3Int, EcsEntity> plants = new();

        //    private PlantProductGenerator plantProductGenerator;
        //    private LocalMap localMap;
        //    private final Position cachePosition;

        public void addPlant(EcsEntity plant, Vector3Int position) {
            plants.Add(position, plant);
            plantBlocks.Add(position, plant.take<PlantComponent>().block);
            plant.Replace(new PositionComponent { position = position });
        }

        public EcsEntity getPlant(Vector3Int position) {
            return plants.ContainsKey(position) ? plants[position] : EcsEntity.Null;
        }

        public EcsEntity getPlant(int x, int y, int z) => getPlant(new Vector3Int(x, y, z));

        public PlantBlock getBlock(Vector3Int position) {
            return plantBlocks.ContainsKey(position) ? plantBlocks[position] : null;
        }

        public void removePlant(EcsEntity plant, bool leaveProduct) {
            plants.Remove(plant.pos());
            plantBlocks.Remove(plant.pos());
            plant.Replace(new PlantRemoveComponent{leaveProduct = leaveProduct});
        }
        
        //public PlantContainer() {
        //        plantBlocks = new HashMap<>();
        //        plantProductGenerator = new PlantProductGenerator();
        //        addSystem(new PlantSeedSystem());
        //        addSystem(new PlantGrowthSystem());
        //        cachePosition = new Position();
        //    }

        //    public void add(AbstractPlant plant, Position position) {
        //        if (plant.type.isTree) addTree((Tree)plant, position);
        //        else if (plant.type.isPlant) addPlant((Plant)plant, position);
        //    }

        //    private void addPlant(Plant plant, Position position) {
        //        plant.position = position;
        //        if (addBlock(plant.getBlock(), position)) objects.add(plant);
        //    }

        //    //TODO checking space for placing
        //    //TODO merging overlaps with other trees.
        //    private void addTree(Tree tree, Position position) {
        //        objects.add(tree);
        //        tree.setPosition(position);
        //        Position vector = tree.getArrayStartPosition();
        //        PlantBlock[][][] treeParts = tree.getBlocks();
        //        for (int x = 0; x < treeParts.length; x++) {
        //            for (int y = 0; y < treeParts[x].length; y++) {
        //                for (int z = 0; z < treeParts[x][y].length; z++) {
        //                    if (treeParts[x][y][z] == null) continue;
        //                    Position onMapPosition = Position.add(vector, x, y, z);
        //                    if (!localMap().inMap(onMapPosition)) {
        //                        treeParts[x][y][z] = null; // remove block that is out of map
        //                    } else {
        //                        addBlock(treeParts[x][y][z], onMapPosition);
        //                    }
        //                }
        //            }
        //        }
        //    }

        //    private boolean addBlock(PlantBlock block, Position position) {
        //        block.position = position;
        //        if (plantBlocks.containsKey(position))
        //            return Logger.PLANTS.logDebug(block.plant + " is blocked by " + plantBlocks.get(position).plant, false);
        //        plantBlocks.put(position, block);
        //        localMap().updatePassage(position);
        //        return true;
        //    }

        //    public void remove(AbstractPlant plant, boolean leaveProduct) {
        //        if (plant != null && objects.remove(plant)) removePlantBlocks(plant, leaveProduct);
        //    }

        //    public void removePlantBlocks(AbstractPlant plant, boolean leaveProduct) {
        //        if (plant instanceof Tree) { // remove all tree blocks
        //            Tree tree = (Tree)plant;
        //            PlantBlock[][][] treeParts = tree.getBlocks();
        //            for (int x = 0; x < treeParts.length; x++) {
        //                for (int y = 0; y < treeParts[x].length; y++) {
        //                    for (int z = 0; z < treeParts[x][y].length; z++) {
        //                        if (treeParts[x][y][z] != null) removeBlock(treeParts[x][y][z], leaveProduct);
        //                    }
        //                }
        //            }
        //        } else if (plant instanceof Plant) { // remove plant block
        //            removeBlock(((Plant)plant).getBlock(), leaveProduct);
        //        }
        //    }

        //    private void removeBlock(PlantBlock block, boolean leaveProduct) {
        //        if (block == null) return;
        //        if (plantBlocks.get(block.position) != block) {
        //            Logger.PLANTS.logError("Plant block with position " + block.position + " not stored in its position.");
        //        } else {
        //            plantBlocks.remove(block.position);
        //            if (leaveProduct) leavePlantProduct(block);
        //            localMap().updatePassage(block.position);
        //        }
        //    }

        //    public void removeBlock(int x, int y, int z, boolean leaveProduct) {
        //        removeBlock(cachePosition.set(x, y, z), leaveProduct);
        //    }

        //    public void removeBlock(Position position, boolean leaveProduct) {
        //        PlantBlock block = plantBlocks.get(position);
        //        if (block == null) return;
        //        removeBlock(block, false);
        //        if (block.plant instanceof Tree) checkTreeDestruction((Tree)block.plant); // destroy other tree parts
        //        localMap().updatePassage(block.position);
        //    }

        //    private void leavePlantProduct(PlantBlock block) {
        //        ItemContainer itemContainer = GameMvc.model().get(ItemContainer.class);
        //    plantProductGenerator.generateCutProduct(block).forEach(item -> {
        //        itemContainer.addItem(item);
        //        itemContainer.onMapItemsSystem.addItemToMap(item, block.position);
        //    });
        //}

        //public void checkTreeDestruction(Tree tree) {
        //    //TODO check if all tree block are connected to trunk
        //    //TODO trees should have underground roots, and stay while enough root blocks are in the soil
        //}

        //public PlantBlock getPlantBlock(Position position) {
        //    return plantBlocks.get(position);
        //}

        //public boolean isPlantBlockExists(Position position) {
        //    return plantBlocks.containsKey(position);
        //}

        public bool isPlantBlockPassable(Vector3Int position) {
            return !plantBlocks.ContainsKey(position) || plantBlocks[position].isPassable();
        }

        public bool isPlantBlockPassable(int x, int y, int z) => isPlantBlockPassable(new Vector3Int(x, y, z));

        //public AbstractPlant getPlantInPosition(Position position) {
        //    return plantBlocks.containsKey(position) ? plantBlocks.get(position).plant : null;
        //}

        //public void removePlant(Position position) {
        //    Optional.ofNullable(plantBlocks.get(position))
        //            .ifPresent(block-> {
        //        plantBlocks.remove(position);
        //        // TODO check tree damage
        //    });
        //}

        //private LocalMap localMap() {
        //    return localMap == null ? localMap = GameMvc.model().get(LocalMap.class) : localMap;
        //}
    }
}
