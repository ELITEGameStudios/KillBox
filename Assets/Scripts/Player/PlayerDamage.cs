using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerDamage : MonoBehaviour
{
    public Rigidbody2D rb;
    public EnemyHealth healthScript;
    public GameObject ClosestExplosion, GetSelf, DmgTxt, DmgTxtPrefab, UIcanvas;
    public float DistanceFromGrenade, hit_txt_size;
    public int ExplosionDamage;
    public bool ultra_immune, custom_hit_txt_size;
    public Text HitTxt;
    public UltraManager ultraManager;
    public ObjectPool objectPool;

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

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Bullet"))
        {
            BulletClass bulletScript = collision.gameObject.GetComponent<BulletClass>();
            healthScript.TakeDmg(bulletScript.damage);
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
