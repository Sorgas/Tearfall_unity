using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Leopotam.Ecs;

namespace util.item {
    public abstract class ItemSelector {
        /**
         * Checks if collection contains appropriate item.
         */
        public bool checkItems(List<EcsEntity> items) {
            return items.Count(item => checkItem(item)) > 0;
        }

        /**
         * Selects sublist of appropriate items.
         * If selector should select multiple items, but not all can be selected, this should return empty list.
         */
        public List<EcsEntity> selectItems(IEnumerable<EcsEntity> items) {
            return items.Where(item => checkItem(item)).ToList();
        }

        /**
         * Checks if given item is appropriate.
         */
        public abstract bool checkItem(EcsEntity item);
    }
}