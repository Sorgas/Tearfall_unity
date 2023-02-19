using System.Collections.Generic;
using generation.zone;
using util.lang.extension;
using static generation.zone.StockpileConfigItemStatus;

namespace game.view.ui.stockpileMenu {
    public class StockpileConfigItem {
        public string name;
        public int id; // for materials
        public Dictionary<string, StockpileConfigItem> children;
        public StockpileMenuLevel level;
        public StockpileConfigItemStatus status;

        public StockpileConfigItem(string name, StockpileMenuLevel level, int id = -1) {
            this.name = name;
            this.id = id;
            this.level = level;
            children = new();
        }

        public StockpileConfigItem clone() {
            StockpileConfigItem clone = new(name, level);
            foreach (KeyValuePair<string, StockpileConfigItem> pair in children) {
                clone.children.Add(pair.Key, pair.Value.clone());
            }
            return clone;
        }

        // recursively sets new status to current and children items
        public void setStatus(StockpileConfigItemStatus newStatus) {
            status = newStatus;
            if (children != null) children.Values.ForEach(child => child.setStatus(newStatus));
        }

        // recursively update statuses from children to parents
        public void updateStatusByChildren() {
            children.Values.ForEach(child => child.updateStatusByChildren());
            status = getStatusByChildren();
        }

        // combines statuses of all children. All enabled -> enabled, all disabled -> disabled, mixed otherwise.
        public StockpileConfigItemStatus getStatusByChildren() {
            if (children.Count == 0) return status;
            bool hasEnabled = false;
            bool hasDisabled = false;
            foreach (StockpileConfigItem child in children.Values) {
                switch (child.status) {
                    case MIXED:
                        return MIXED;
                    case ENABLED:
                        hasEnabled = true;
                        break;
                    case DISABLED:
                        hasDisabled = true;
                        break;
                }
                if (hasEnabled && hasDisabled) return MIXED;
            }
            return hasEnabled ? ENABLED : DISABLED;
        }
    }
}