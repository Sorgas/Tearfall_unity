using System.Collections.Generic;
using game.view.ui.util;
using UnityEngine;
using util.lang;

namespace game.input {
    // widget is ui piece that is always visible
    // widgets hot key maps should not intersect with each other and camera controls 
    // passes input to widgets
    public class WidgetManager : Singleton<WidgetManager>, IHotKeyAcceptor {
        public HashSet<IHotKeyAcceptor> widgets = new();
        
        public bool accept(KeyCode key) {
            foreach (var hotKeyAcceptor in widgets) {
                if (hotKeyAcceptor.accept(key)) return true;
            }
            return false;
        }

        public void addWidget(IHotKeyAcceptor widget) {
            widgets.Add(widget);
        }
    }
}