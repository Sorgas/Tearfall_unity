using System;
using System.Collections.Generic;
using UnityEngine;

namespace types.unit.race {
    [Serializable]
    public class CreatureAppearance {
        public string atlas;
        public int[] head; // y, tiles number male, tiles number female
        public int[] body;
        public List<Color> skintones;
    
        public void process() { }
    }
}