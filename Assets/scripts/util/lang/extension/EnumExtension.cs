using System;
using enums.item;

public static class EnumExtension {
    
    public static ItemTagEnum get(this ItemTagEnum source, string value) {
        ItemTagEnum result;
        if(Enum.TryParse<ItemTagEnum>(value.ToUpper(), out result)) return result;
        throw new ArgumentException("Value " + value + " not present in ItemTagEnum");
    }
}