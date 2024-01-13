using util.lang;

namespace types {
// Room types are hardcoded
public class RoomTypes {
    public static readonly RoomType bedroom = new("bedroom", "bed");
    public static readonly RoomType dormitory = new("dormitory", "bed");
    public static readonly RoomType hospital = new("hospital", "bed");
    public static readonly RoomType prison = new("prison", "bed");
    // TODO tavern bedroom
    public static readonly RoomType dinningRoom = new("dinning room", "table");
    public static readonly RoomType dinningHall= new("dinning hall", "table");
    public static readonly RoomType temple = new("temple", "altar");
    public static readonly RoomType workshop = new("workshop", "workbench");

    // tavern	tavern counter
    //     throne room	throne
    // shop	shop counter
    // tomb	grave, sarcofage

    public static readonly MultiValueDictionary<string, RoomType> map = new();

    static RoomTypes() {
        map.add(bedroom.buildingType, bedroom);
        map.add(dormitory.buildingType, dormitory);
        map.add(hospital.buildingType, hospital);
        map.add(prison.buildingType, prison);
        map.add(dinningRoom.buildingType, dinningRoom);
        map.add(dinningHall.buildingType, dinningHall);
        map.add(temple.buildingType, temple);
        map.add(workshop.buildingType, workshop);
    }
}

public class RoomType {
    public readonly string name;
    public readonly string buildingType;


    public RoomType(string name, string buildingType) {
        this.name = name;
        this.buildingType = buildingType;
    }
}
}