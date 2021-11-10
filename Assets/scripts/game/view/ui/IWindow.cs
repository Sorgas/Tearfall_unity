namespace game.view.ui {

    // window is UI piece that can be closed and opened
    public interface IWindow {
        public void close();

        public void open();

        public string getName();
    }
}