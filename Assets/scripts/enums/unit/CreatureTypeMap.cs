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
        public readonly Dictionary<string, CreatureType> creatureTypes = new Dictionary<string, CreatureType>();
        public readonly Dictionary<string, BodyTemplate> templates = new Dictionary<string, BodyTemplate>();

        public CreatureTypeMap() {
            loadTemplates();
            loadCreatures();
        }
        
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