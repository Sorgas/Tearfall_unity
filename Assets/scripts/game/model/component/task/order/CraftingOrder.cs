
using Assets.scripts.types.item.recipe;

public class CraftingOrder {
    public string name;
    public Recipe recipe;
    public CraftingOrderStatus status;
    public bool repeated;
    public int quantity;

    public CraftingOrder(Recipe recipe) {
        this.name = recipe.title;
        this.recipe = recipe;
        this.status = CraftingOrderStatus.WAITING;
    }

    public enum CraftingOrderStatus {
        PERFORMING,
        WAITING,
        PAUSED,
        PAUSED_PROBLEM
    }
}