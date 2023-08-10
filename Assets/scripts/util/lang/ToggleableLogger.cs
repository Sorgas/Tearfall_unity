using UnityEngine;

namespace util.lang {
public class ToggleableLogger {
    public bool enabled = false;
    public string name;
    protected string logMessage = "";

    public ToggleableLogger(string name) {
        this.name = name;
    }

    public void log(string message) {
        if(enabled) Debug.Log($"[{name}]: {message}");
    }

    public void logWarn(string message) {
        Debug.LogWarning($"[{name}]: {message}");
    }
    
    public void logError(string message) {
        Debug.LogError($"[{name}]: {message}");
    }
    
    public void logAdd(string message) {
        if(enabled) logMessage += $"{message} \n";
    }
    
    public void flushLog() {
        if(enabled) Debug.Log(logMessage);
        logMessage = "";
    }
}
}