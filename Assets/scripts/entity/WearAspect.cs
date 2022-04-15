namespace entity {
    public class WearAspect : Aspect {
        public string slot;
        public string layer;
        // TODO add additional bodyparts

        public WearAspect(string slot, string layer) {
            this.slot = slot;
            this.layer = layer;
        }
    }
}