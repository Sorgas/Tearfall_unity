using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assets.scripts.enums.action {
    public enum ActionTargetTypeEnum : byte {
        EXACT = 0, // performer should stand in same position
        NEAR = 1, // performer should stand in adjacent tile or lower ramp
        ANY = 2 // any of above
    }
}
