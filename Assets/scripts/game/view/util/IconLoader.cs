using UnityEngine;
using util.lang;

namespace game.view.util {
    public class IconLoader : Singleton<IconLoader> {

        public static Sprite get(string name) {
            return Resources.Load<Sprite>("icons/" + name);
        }
    }
}