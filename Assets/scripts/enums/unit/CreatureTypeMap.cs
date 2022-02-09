using System;
using System.Collections.Generic;
using System.Linq;
using enums.material;
using enums.unit.body;
using enums.unit.body.raw;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.unit {
    // stores types of creatures loaded from jsons
    public class CreatureTypeMap : Singleton<CreatureTypeMap>{
        private Dictionary<string, CreatureType> creatureTypes = new Dictionary<string, CreatureType>();
        private Dictionary<string, BodyTemplate> templates = new Dictionary<string, BodyTemplate>();

        public CreatureTypeMap() {
            loadTemplates();
            loadCreatures();
        }

        private void loadFiles() {
            Debug.Log("loading creatures");
            map.Clear();
            TextAsset[] files = Resources.LoadAll<TextAsset>("data/creatures");
            foreach (TextAsset file in files) {
                int count = 0;
                RawMaterial[] materials = JsonArrayReader.readArray<RawMaterial>(file.text);
                if (materials == null) continue;
                foreach (RawMaterial raw in materials) {
                    Material_ material = new Material_(raw);
                    map.Add(material.name, material);
                    idMap.Add(material.id, material);
                    count++;
                }
                Debug.Log("loaded " + count + " from " + file.name);
            }
        }

        public Material_ material(int id) {
            // Debug.Log(id + " " + idMap.Count + " " + map.Count);
            return idMap[id];
        }

        public Material_ material(string name) {
            return map[name];
        }

        public int id(string name) {
            return material(name).id;
        }

        public List<Material_> getByTag(string tag) {
            return map.Values
                .Select(material => material)
                .Where(material => material.tags.Contains(tag))
                .ToList();
        }

        public List<Material_> all => map.Values.ToList();

        private void loadTemplates() {
            // Logger.LOADING.logDebug("loading body templates");
            TextAsset file = Resources.Load<TextAsset>("data/creatures/body_templates.json");

            BodyTemplateProcessor templateProcessor = new BodyTemplateProcessor();
            RawBodyTemplate[] rawTemplates = JsonArrayReader.readArray<RawBodyTemplate>(file.text);
            rawTemplates.Select(template => templateProcessor.process(template)).ToList().ForEach(template => templates.Add(template.name, template));
        }

        private void loadCreatures() {
            // Logger.LOADING.logDebug("loading creature types");
            TextAsset file = Resources.Load<TextAsset>("data/creatures/creatures.json");
            CreatureTypeProcessor typeProcessor = new CreatureTypeProcessor(this);
            RawCreatureType[] rawTypes = JsonArrayReader.readArray<RawCreatureType>(file.text);
            rawTypes.Select(type => typeProcessor.process(type)).ToList().ForEach(type => creatureTypes.Add(type.name, type));
        }
    
        public static CreatureType getType(String specimen) {
            return get().creatureTypes[specimen];
        }
    }
}