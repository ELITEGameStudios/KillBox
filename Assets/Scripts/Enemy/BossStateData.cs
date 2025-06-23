
using Pathfinding;
using UnityEngine;

[System.Serializable]
public abstract class BossStateData
{
    protected BossBase host;
    public BossBase GetBase() { return host; }

    // Ported variables from bossBase
    public string name;
    public Sprite displaySprite;
    public Color displayColor;
    public AIPath movement_script;
    public Rigidbody2D rb_self;
    public Collider2D self_collider;
    public EnemyHealth health;
    public Animator animator;
    public float normalizedHealth { get { return (float)health.CurrentHealth / health.maxHealth; } }

    public bool finished;

    public BossStateData(BossBase bossBase) // Always include super(bossBase) in any child class constructors
    {
        host = bossBase;

        // Ported variables from bossBase
        name = bossBase.name;
        displaySprite = bossBase.displaySprite;
        displayColor = bossBase.displayColor;
        movement_script = bossBase.movement_script;
        rb_self = bossBase.rb_self;
        self_collider = bossBase.self_collider;
        health = bossBase.health;
        animator = bossBase.animator;

        OnReset();
    }


    public virtual void OnReset() // Called When the state object is first created and when resetting the state to be used again. Put all reset code here
    {
        finished = false;
    }

    public abstract void Start(); // Called When the state object becomes active

    public abstract void Update(); // Called every frame while the object is active

    public virtual void FixedUpdate() {} // Called every physics frame while the object is active

    public virtual void End(bool interrupted = false) // Called once the state declares it is finished its task
    {
        finished = true;
    }
}