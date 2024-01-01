using UnityEngine;

namespace util {
public class UtilUshortArray {
    private ushort[,,] array;
    Vector3Int size;

    public UtilUshortArray(int xSize, int ySize, int zSize) {
        size = new Vector3Int(xSize, ySize, zSize);
        array = new ushort[xSize, ySize, zSize];
    }

    public UtilUshortArray(Vector3Int size) : this(size.x, size.y, size.z) { }

    public ushort get(int x, int y, int z) => array[x, y, z];

    public ushort get(Vector3Int position) => get(position.x, position.y, position.z);

    public void set(int x, int y, int z, ushort value) => setImpl(x, y, z, value);

    public void set(Vector3Int position, ushort value) => set(position.x, position.y, position.z, value);

    protected virtual void changeImpl(int x, int y, int z, ushort delta) {
        array[x, y, z] += delta;
    }

    public bool withinBounds(int x, int y, int z) => x >= 0 && y >= 0 && z >= 0 && x < size.x && y < size.y && z < size.z;

    protected virtual void setImpl(int x, int y, int z, ushort value) {
        array[x, y, z] = value;
    }
}
}