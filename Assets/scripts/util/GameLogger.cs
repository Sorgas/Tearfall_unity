using UnityEngine;

namespace util {
    public static class GameLogger {
        public static bool enabled = false;
        
        public static void log(string message) {
            if(enabled) Debug.Log(message);
        }
    }
}