using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Pathfinding;
public class PlayerDamage : MonoBehaviour
{
    public Rigidbody2D rb;
    public EnemyHealth healthScript;
    public GameObject ClosestExplosion, GetSelf, DmgTxt, DmgTxtPrefab, UIcanvas;
    public float DistanceFromGrenade, hit_txt_size, incomingForceMod = 1;
    public int ExplosionDamage;
    public bool ultra_immune, custom_hit_txt_size, kbImmune;
    public Text HitTxt;
    public UltraManager ultraManager;
    public ObjectPool objectPool;
    AIPath ai;
    [SerializeField] List<CustomForce> customForces;

    void Awake()
    {
        if (customForces == null) customForces = new();
        if (ai == null) ai = gameObject.GetComponent<AIPath>();
        if (!kbImmune && ai != null) { ai.canMove = false; }
    }

    void Start()
    {
        UIcanvas = GameObject.FindWithTag("WorldCanvas");
        objectPool = GameObject.Find("BulletPool2").GetComponent<ObjectPool>();
        healthScript = gameObject.GetComponent<EnemyHealth>();
    }
    void Update()
    {
        ClosestExplosion = GameObject.FindWithTag("Explosion");
        if (ClosestExplosion != null)
        {
            DistanceFromGrenade = Vector3.Distance(ClosestExplosion.transform.position, GetSelf.transform.position);
            if (DistanceFromGrenade <= 3)
                healthScript.TakeDmg(ExplosionDamage);
        }
    }

    public void AddCustomForce(CustomForce force){
        customForces.Add(force);
    }

    Vector2 UpdateForces() {
        Vector2 netCustomForces = Vector2.zero;

        foreach (CustomForce force in customForces) { // Updating and retrieving forces
            force.Update();
            netCustomForces += force.force;
        }
        
        for (int i = customForces.Count-1; i >= 0; i--){ // Clearing finished forces
            if(customForces[i].finished){ customForces.RemoveAt(i); }
        }

        return netCustomForces * Time.fixedDeltaTime;
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        if (!kbImmune)
        {
            ///     ai.canMove = false;
            if (ai != null && ai.enabled)
            {
                Vector3 nextPosition;
                Quaternion nextRotation;
                // Calculate how the AI wants to move
                ai.MovementUpdate(Time.deltaTime, out nextPosition, out nextRotation);
                

                // Add forces
                nextPosition += (Vector3)UpdateForces();

                // Actually move the AI
                ai.FinalizeMovement(nextPosition, nextRotation);
            }
            else
            {
                // rb.MovePosition(rb.position + rb.velocity + UpdateForces());
            }
        }
    }


    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletClass bulletScript = collision.gameObject.GetComponent<BulletClass>();
            healthScript.TakeDmg(bulletScript.damage);

            AddCustomForce(
                new CustomForce(
                    (transform.position - collision.transform.position).normalized * bulletScript.knockbackForce * incomingForceMod,
                    bulletScript.knockbackTime
                )
            );
            
            //poolManager.GetDmgFromPool(gameObject, 16);
            //if(DmgTxt == null){
            DmgTxt = objectPool.GetPooledObject();

            if(DmgTxt != null){
                DmgTxt.transform.position = transform.position;
                    //}
                //else
                //{
                    //DmgTxt.GetComponent<BulletDestroy>().StartRangeCall();
                DmgTxt.transform.SetParent(UIcanvas.transform);
                DmgTxt.transform.localEulerAngles = new Vector3(0, 0, 0);
                HitTxt = DmgTxt.GetComponent<Text>();
                HitTxt.text = bulletScript.damage.ToString();
                if (custom_hit_txt_size)
                {
                    DmgTxt.transform.localScale = new Vector3(hit_txt_size, hit_txt_size, hit_txt_size);
                }
                DmgTxt.SetActive(true);
                DmgTxt.GetComponent<BulletDestroy>().RestartTimer();
            }
            collision.gameObject.GetComponent<BulletDestroy>().ResetRangeCallWhenHit();
            //}
        }


        if (collision.gameObject.CompareTag("Player"))
        {
            ultraManager = collision.gameObject.GetComponent<UltraManager>();
            if (ultraManager.IsUltra && !ultra_immune)
                healthScript.Die();
        }
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Bullet"))
        {
            BulletClass bulletScript = collider.gameObject.GetComponent<BulletClass>();
            healthScript.TakeDmg(bulletScript.damage);

            AddCustomForce(
                new CustomForce(
                    // (transform.position - collider.transform.position) * bulletScript.knockbackForce,
                    collider.attachedRigidbody.velocity.normalized * bulletScript.knockbackForce * incomingForceMod,
                    bulletScript.knockbackTime
                )
            );

            //poolManager.GetDmgFromPool(gameObject, 16);
            //if(DmgTxt == null){
            DmgTxt = objectPool.GetPooledObject();
            DmgTxt.transform.position = transform.position;
            //}
            //else
            //{
            //DmgTxt.GetComponent<BulletDestroy>().StartRangeCall();
            DmgTxt.transform.parent = UIcanvas.transform;
            DmgTxt.transform.localEulerAngles = new Vector3(0, 0, 0);
            HitTxt = DmgTxt.GetComponent<Text>();
            HitTxt.text = bulletScript.damage.ToString();
            DmgTxt.SetActive(true);
            DmgTxt.GetComponent<BulletDestroy>().RestartTimer();

            if(!bulletScript.isTrigger){
                collider.gameObject.GetComponent<BulletDestroy>().ResetRangeCallWhenHit();
            }
            //}
        }


        if (collider.gameObject.CompareTag("Player"))
        {
            ultraManager = collider.gameObject.GetComponent<UltraManager>();
            if (ultraManager.IsUltra && !ultra_immune)
                healthScript.Die();
        }
    }

}
