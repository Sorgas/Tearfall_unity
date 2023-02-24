using System;

namespace util {
    public class GameException : Exception {
        public GameException(string message) : base(message) { }
    }
}