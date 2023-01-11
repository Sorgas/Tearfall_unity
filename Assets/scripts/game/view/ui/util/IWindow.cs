namespace game.view.ui.util {

    // window is UI piece that can be closed and opened
    public interface IWindow : ICloseable {
        public string getName();
    }
}