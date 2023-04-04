using System.Collections.Generic;
using generation.worldgen.generators.deity;
using UnityEngine;
using util.input;
using util.lang;

namespace types.deity {
    // loads deity aspects configs from jsons
    public class DeityAspectMap : Singleton<DeityAspectMap> {
        private const string ASPECT_PATH = "data/deity/deity_aspects";
        private Dictionary<string, DeityAspect> map = new();

        public DeityAspectMap() {
            loadAspects();
        }

        private void loadAspects() {
            log("loading recipe lists");
            TextAsset file = Resources.Load<TextAsset>(ASPECT_PATH);
            int count = 0;
            DeityAspect[] aspects = JsonArrayReader.readArray<DeityAspect>(file.text);
            foreach (DeityAspect aspect in aspects) {
                map.Add(aspect.name, aspect);
                count++;
            }
            log("loaded " + count + " from " + file.name);
        }

        public static DeityAspect get(string name) => get().map[name];
        
        private void log(string message) {
            Debug.Log("[BuildingTypeMap]: " + message);
        }
    }
}