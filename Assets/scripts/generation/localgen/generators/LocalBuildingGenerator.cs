using game.model.component;
using game.model.component.building;
using game.model.component.task.order;
using game.model.localmap;
using generation.item;
using Leopotam.Ecs;
using types;
using types.building;
using types.material;
using UnityEngine;
using util.lang.extension;

namespace generation.localgen.generators {
    public class LocalBuildingGenerator : LocalGenerator {
        private int placingAttemps = 20;

        public LocalBuildingGenerator(LocalMapGenerator generator) : base(generator) {}

        public override void generate() {
            Debug.Log("generating buildings ");
            foreach (var entry in container.buildingsToAdd) {
                BuildingType type = BuildingTypeMap.get(entry.Key);
                Vector3Int position = getPositionForBuilding(type);
                Debug.Log("position for " + type.name + " found: " + position);
                if(position.z >= 0) createBuilding(type, position, entry.Value);
            }
        }

        public void createBuilding(BuildingType type, Vector3Int position, string material) {
            BuildingOrder order = new("log", MaterialMap.get().material(material).id, 1, position);
            order.type = type;
            order.orientation = Orientations.N;
            EcsEntity building = container.model.buildingContainer.createBuilding(order);
            storeItems(building, type.name);
        }

        public void storeItems(EcsEntity building, string typeName) {
            if (container.itemsToStore.ContainsKey(typeName)) {
                foreach (var row in container.itemsToStore[typeName]) {
                    string[] args = row.Split("/");
                    ItemGenerator generator = new ItemGenerator();
                    ItemContainerComponent component = building.take<ItemContainerComponent>();
                    for (int i = 0; i < 5; i++) {
                        EcsEntity item = generator.generateItem(args[0], args[1], container.model.createEntity());
                        component.items.Add(item);
                        container.model.itemContainer.stored.addItemToContainer(item, building);
                    }
                }
            }
        }
        
        private Vector3Int getPositionForBuilding(BuildingType type) {
            LocalMap map = container.map;
            int x = map.bounds.maxX / 2;
            int y = map.bounds.maxY / 2;
            for (var i = 0; i < placingAttemps; i++) {
                Vector3Int position = checkXyPosition(x, y);
                if (position.z >= 0) {
                    if (validatePosition(position, type)) return position;
                }
                x++;
            }
            return new Vector3Int(0, 0, -1);
        }

        private Vector3Int checkXyPosition(int x, int y) {
            LocalMap map = container.map;
            Vector3Int position = new(x, y, 0);
            for (var z = map.bounds.maxZ; z >= map.bounds.minZ; z--) {
                if (map.blockType.getEnumValue(x, y, z) == BlockTypes.FLOOR) return new Vector3Int(x, y, z);
            }
            return new Vector3Int(x, y, -1);
        }

        private bool validatePosition(Vector3Int position, BuildingType type) {
            LocalMap map = container.map;
            if(type.access != null) {
                if (map.blockType.getEnumValue(position.x + type.access[0], position.y + type.access[1], position.z) != BlockTypes.FLOOR) return false;
            }
            for (var x = 0; x < type.size[0]; x++) {
                for (var y = 0; y < type.size[1]; y++) {
                    if (map.blockType.getEnumValue(position.x + x, position.y + y, position.z) != BlockTypes.FLOOR) return false;
                }
            }
            return true;
        }

        public override string getMessage() {
            return "Placing buildings";
        }
    }
}