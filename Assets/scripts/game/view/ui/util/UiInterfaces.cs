using game.input;
using game.view.system.mouse_tool;
using UnityEngine;
using util.lang;

namespace game.view.ui.util {
public interface IHotKeyAcceptor {
    public bool accept(KeyCode key);
}

// should affect only ui GOs
public interface ICloseable {
    public void close();

    public void open();
}

public interface INamed {
    public string getName();
}

// in-game window
public abstract class GameWindow : MonoBehaviour, ICloseable, IHotKeyAcceptor, INamed {
    public virtual void close() {
        gameObject.SetActive(false);
    }

    public virtual void open() {
        gameObject.SetActive(true);
    }

    public virtual bool accept(KeyCode key) {
        if (key == KeyCode.Q) {
            WindowManager.get().closeWindow(getName());
            MouseToolManager.get().reset();
            return true;
        }
        return false;
    }

    public abstract string getName();
}

// in-game widget. See GameWidgetManager
public abstract class GameWidget : GameWindow, Initable {
    public virtual void reset() { }

    public virtual void init() { }

    public override bool accept(KeyCode key) {
        return false;
    }
}
}