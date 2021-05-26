using UnityEngine;

namespace Tearfall_unity.Assets.scripts.util.input {
    public class JsonArrayReader {

        public static T[] readArray<T>(string json) {
            string newJson = "{ \"array\": " + json + "}";
                Debug.Log(json);
            JsonArrayWrapper<T> wrapper = JsonUtility.FromJson<JsonArrayWrapper<T>> (newJson);
            return wrapper.array;
        }

        public class JsonArrayWrapper<K> {
            public K[] array;
        }
    }
}