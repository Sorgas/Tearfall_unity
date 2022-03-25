using System;
using System.Collections.Generic;
using System.Linq;
using enums.unit.body;
using enums.unit.body.raw;
using Newtonsoft.Json;
using UnityEngine;
using util.input;
using util.lang;

namespace enums.unit {
    // stores types of creatures loaded from jsons
    public class CreatureTypeMap : Singleton<CreatureTypeMap>{
        public readonly Dictionary<string, CreatureType> creatureTypes = new Dictionary<string, CreatureType>();
        public readonly Dictionary<string, BodyTemplate> templates = new Dictionary<string, BodyTemplate>();

        public CreatureTypeMap() {
            loadTemplates();
            loadCreatures();
        }
        
        private void loadTemplates() {
            Debug.Log("loading body templates");
            TextAsset file = Resources.Load<TextAsset>("data/creatures/body_templates");
            BodyTemplateProcessor templateProcessor = new BodyTemplateProcessor();
            RawBodyTemplate[] rawTemplates = JsonConvert.DeserializeObject<RawBodyTemplate[]>(file.text);
            rawTemplates.Select(template => templateProcessor.process(template)).ToList().ForEach(template => templates.Add(template.name, template));
        }

        private void loadCreatures() {
            Debug.Log("loading creature types");
            TextAsset file = Resources.Load<TextAsset>("data/creatures/creatures");
            CreatureTypeProcessor typeProcessor = new CreatureTypeProcessor(this);
            List<RawCreatureType> types = JsonConvert.DeserializeObject<List<RawCreatureType>>(file.text);
            types.Select(type => typeProcessor.process(type)).ToList().ForEach(type => creatureTypes.Add(type.name, type));
        }
    
        public static CreatureType getType(String specimen) {
            return get().creatureTypes[specimen];
        }
    }
}