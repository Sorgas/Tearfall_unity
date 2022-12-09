using System.Collections.Generic;
using UnityEngine;
using util.input;
using util.lang;

public class SubstrateTypeMap : Singleton<SubstrateTypeMap> {
    private Dictionary<int, SubstrateType> map = new();

    public SubstrateTypeMap() {
        loadAll();
    }


    private void loadAll() {
        string logMessage = "loading substrates";
        map.Clear();
        TextAsset[] files = Resources.LoadAll<TextAsset>("data/substrates");
        foreach (TextAsset file in files) {
            int count = 0;
            SubstrateType[] raws = JsonArrayReader.readArray<SubstrateType>(file.text);
            if (raws == null) continue;
            foreach (SubstrateType raw in raws) {
                map.Add(raw.id, raw);
                count++;
            }
            logMessage += "loaded " + count + " from " + file.name + "\n";
        }
        Debug.Log(logMessage);
    }

    public SubstrateType get(int id) => map[id];

    public IEnumerable<SubstrateType> all() => map.Values;
}