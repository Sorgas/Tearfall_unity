
using Assets.scripts.types.item.recipe;

public class CraftingOrder {
    public string name;
    public Recipe recipe;
    public CraftingOrderStatus status; 

    public enum CraftingOrderStatus {
        PERFORMING,
        WAITING,
        PAUSED,
        PAUSED_PROBLEM
    }
}