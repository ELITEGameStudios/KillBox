using System.Collections;
using System.Xml.Serialization;
using Pathfinding;
using UnityEngine;

public class PiercerScript : MonoBehaviour
{

    [SerializeField] private GameObject beam_indicator_prefab;
    [SerializeField] private Rigidbody2D bullet;
    [SerializeField] private Transform spawn;
    [SerializeField] private float spread, range, fireRate, poolIndex, waitTime;
    [SerializeField] private int bulletsPerShot, velocity, warningFlashes, shots, currentState;
    [SerializeField] private ObjectPool objectPool;
    [SerializeField] private Vector3 spawnRot;
    [SerializeField] private bool has_audio;
    [SerializeField] private AudioSource audio;
    [SerializeField] private bool CanShoot, trigger;
    [SerializeField] private AIPath pathing;
    [SerializeField] private Color warningColor;
    [SerializeField] private int chargeupTime;
    [SerializeField] private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        objectPool = GameObject.Find("BulletPool"+poolIndex.ToString()).GetComponent<ObjectPool>();
        waitTime = Random.Range(3, 6);
        CanShoot = true;
        TransitionState(0);

        if(rb == null){
            rb = gameObject.GetComponent<Rigidbody2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {

        if(currentState == 0){
            float distanceFromPlayer = Vector2.Distance(Player.main.tf.position, transform.position);
            pathing.maxSpeed = Mathf.LerpUnclamped(-1.5f, 8, (distanceFromPlayer / 15) );
        }

        if(CanShoot && trigger){
            Shoot();
        }
        if(currentState == 0){
            Vector3 relative = transform.InverseTransformPoint(Player.main.tf.position);
            float angle = Mathf.Atan2(relative.x, relative.y) * Mathf.Rad2Deg;
            transform.Rotate(0,0, -angle);
        }
    }

    void Shoot()
    {
        for(int i = 0; i < bulletsPerShot; i++)
        {
            spawn.localEulerAngles += new Vector3(0, 0, Random.Range(-spread, spread));
            
            if(objectPool.GetPooledObject() != null){
                bullet = objectPool.GetPooledObject().GetComponent<Rigidbody2D>();
                bullet.gameObject.transform.position = spawn.position;
                bullet.gameObject.transform.rotation = spawn.rotation;

                bullet.gameObject.GetComponent<BulletDestroy>().NewTimer(range);
                bullet.gameObject.SetActive(true);


                bullet.AddForce(spawn.up * velocity);
                spawn.localEulerAngles = spawnRot;
            }
            else{
                spawn.localEulerAngles = spawnRot;
                break;
            }

            spawn.localEulerAngles = spawnRot;
        }

        if (has_audio && audio != null)
        {
            audio.Play();
        }

        CanShoot = false;
        StartCoroutine(ShootCooldown());
    }

    void TransitionState(int state){
        switch(state){
            case 0:
                StartCoroutine(MainState());
                currentState = 0;
                break;
            case 1:
                StartCoroutine(indicator());
                StartCoroutine(ShootingState());
                currentState = 1;
                break;
        }
    }

    IEnumerator ShootCooldown()
    {
        yield return new WaitForSeconds(fireRate);
        CanShoot = true;
    }

    IEnumerator indicator(){
        Debug.Log("Indicating");

        beam_indicator_prefab.SetActive(true);
        float charge = chargeupTime;
        while(charge > 0){
            
            beam_indicator_prefab.GetComponent<SpriteRenderer>().color = 
                Color.Lerp(Color.clear, warningColor, 0.5f * Mathf.Sin(2*Mathf.PI * (warningFlashes * charge - 0.25f)) + 0.5f);

            charge -= Time.deltaTime;
            yield return null;
        }
        beam_indicator_prefab.SetActive(false);
    }

    IEnumerator MainState(){
        pathing.enabled = true;
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
        
        yield return new WaitForSeconds(waitTime);
        Debug.Log("Switching to guns");
        TransitionState(1);
    }

    IEnumerator ShootingState(){
        pathing.enabled = false;
        rb.constraints = RigidbodyConstraints2D.FreezeAll;

        // Indicator should be finished around this time
        yield return new WaitForSeconds(chargeupTime);

        int currentShots = shots;
        while(currentShots > 0){
            if(CanShoot){
                Shoot();
                currentShots--;
            }
            yield return null;
        }

        Debug.Log("Switching back to idk");
        TransitionState(0);

    }
}
