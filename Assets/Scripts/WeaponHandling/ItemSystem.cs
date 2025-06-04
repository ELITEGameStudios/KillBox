public class ItemSystem
{
    
}

public class Inventory
{
    public static readonly int[] items;

    public static void RemoveItemFromUsage(Item item){
        items[item._itemID] --;
    }
}

public abstract class Item{

    public string _name {get; protected set;}
    public string _desc {get; protected set;}
    public int _tier {get; protected set;}
    public int _itemID {get; protected set;}

    protected void Activate(){
        if (Inventory.items[_itemID] > 0)
        {
            Function();
            RetireItem();
        }
    }

    public abstract void Function();

    public void RetireItem(){
        Inventory.RemoveItemFromUsage(this);
    }
}

public abstract class Booster : Item{

    public int _cooldown {get; protected set;}
    public int _current_cooldown {get; protected set;}
    public bool _cooldown_override {get; protected set;}
    public bool _ready {get; protected set;}

    public void Use(){
        Activate();
        StartCooldown();
    }

    protected void StartCooldown(){
        _current_cooldown = _cooldown;
        _ready = false;
    }

    public void UpdateCooldowns(){
        if (_current_cooldown <= 0){
            _ready = true;
        }
        else{
            _current_cooldown--;
        }
    }

}
