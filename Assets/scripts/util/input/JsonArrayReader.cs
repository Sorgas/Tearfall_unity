using UnityEngine;

namespace util.input {
    public class JsonArrayReader {

        public static T[] readArray<T>(string json) {
            string newJson = "{ \"array\": " + json + "}";
            JsonArrayWrapper<T> wrapper = JsonUtility.FromJson<JsonArrayWrapper<T>> (newJson);
            return wrapper.array;
        }

        public class JsonArrayWrapper<K> {
            public K[] array;
        }
    }
}