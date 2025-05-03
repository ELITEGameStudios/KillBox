using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIShooterScript : MonoBehaviour
{
    public GameObject bullet;
    public Transform playerTf, Spawn, Enemytf;
    public float Velocity, spread;
    public Rigidbody2D bulletRb, clone;
    public Vector3 SpawnRot;
    public bool CanShoot, sees_player, FAS, boss, spray, has_audio;
    public int BulletsPerShot, poolIndex, damage_boss_field;
    public float FR, AimProxim;
    public Camera cam;
    public PrefabToGameObjectHostile GetPrefabToGameObject;
    public ObjectPool objectPool;

    public AudioSource audio;

    [SerializeField]
    private float range;

    Vector2 mousePos;

    // Start is called before the first frame update
    void Start()
    {
        CanShoot = true;
        SpawnRot = Spawn.localEulerAngles;
        objectPool = GameObject.Find("BulletPool"+poolIndex.ToString()).GetComponent<ObjectPool>();

        if(range == 0){
            range = 0.5f;
        }
        
    }
    // Update is called once per frame
    void FixedUpdate()
    {

        Enemytf = GameObject.FindWithTag("Player").transform;

        LayerMask mask = LayerMask.GetMask("Player");

        RaycastHit2D hit = Physics2D.Raycast(playerTf.position, playerTf.up, Mathf.Infinity, mask);

        Debug.DrawRay(playerTf.position, playerTf.up, Color.white);

        if(hit.collider != null && hit.collider.gameObject.tag == "Player") { sees_player = true; }
        else{ sees_player = false; }

        if (Vector3.Distance(playerTf.position, Enemytf.position) > AimProxim && CanShoot)
            FAS = true;
        else
            FAS = false;
    }
    void Update()
    {

        if (FAS && CanShoot && sees_player)
            Shoot();
        else if (CanShoot && spray)
            Shoot();
    }

    void Shoot()
    {
        for(int i = 0; i < BulletsPerShot; i++)
        {
            Spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            if(objectPool.GetPooledObject() != null){
                clone = objectPool.GetPooledObject().GetComponent<Rigidbody2D>();
                clone.gameObject.transform.position = Spawn.position;
                clone.gameObject.transform.rotation = Spawn.rotation;

                clone.gameObject.GetComponent<BulletDestroy>().NewTimer(range);
                clone.gameObject.SetActive(true);

                //for boss
                if(boss){
                    clone.gameObject.GetComponent<EnemyDamage>().damage = damage_boss_field;
                }

                clone.AddForce(Spawn.up * Velocity);
                Spawn.localEulerAngles = SpawnRot;
            }
            else{
                Spawn.localEulerAngles = SpawnRot;
                break;
            }

            Spawn.localEulerAngles = SpawnRot;
        }

        if (has_audio && audio != null)
        {
            audio.Play();
        }

        CanShoot = false;
        StartCoroutine(ShootCooldown());
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(FR);
        CanShoot = true;
    }
}
