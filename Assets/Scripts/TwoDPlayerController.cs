using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TwoDPlayerController : MonoBehaviour, IShopUIEventListener
{
    [SerializeField] private Transform gunTransform, dualGunTransform;
    public float speed = 5f, slow_mod = 1f,  angle, deadzone, pointBlankRange;

    [SerializeField]
    private float dashTimer, dashCooldownTimer, dashMod, dashCooldown, dashDuration;
    public Rigidbody2D rb;
    Vector2 movement, mousePos, PcMove, dashVector;
    public Vector2 rotation_value;
    public Camera cam;
    public Joystick MoveJoystick, RotJoystick;
    public bool mobile, dashing, canDash;
    [SerializeField] private bool canMove;
    [SerializeField] private GameObject dashParticleObject;

    [SerializeField] private TriggerColliderTracker colliders;
    [SerializeField] private AudioSource audio;
    [SerializeField] private Vector3 test;
    private bool rotating = false;

    InputManager controls;

    Vector3 particleAngleVector;
    public float GetDashCooldownTimer(){return dashCooldownTimer;}

    void Awake()
    {
        
        controls = new InputManager();

        controls.Gameplay.move.performed += ctx => movement = ctx.ReadValue<Vector2>();
        controls.Gameplay.move.canceled += ctx => movement = Vector2.zero;

        controls.Gameplay.shoot.performed += ctx => rotation_value = ctx.ReadValue<Vector2>();
        controls.Gameplay.shoot.canceled += ctx => rotation_value = Vector2.zero;
        canDash = true;
        dashParticleObject.SetActive(true);
    }

    void OnEnable()
    {
        controls.Gameplay.Enable();
    }
    void OnDisable()
    {
        controls.Gameplay.Disable();
    }

    // Update is called once per frame
    void Update()
    {

        // rotation_value.Normalize();
        // movement.Normalize();

        if(mobile)
        {
            movement.x = Mathf.Clamp(MoveJoystick.Horizontal, -1, 1);
            movement.y = Mathf.Clamp(MoveJoystick.Vertical, -1, 1);
        }
        else if(!DetectInputDevice.main.isController)
        {
            movement.x = Input.GetAxis("Horizontal") * 0.85f;
            movement.y = Input.GetAxis("Vertical") * 0.85f;
            // movement.x = Input.GetAxisRaw("Horizontal");
            // movement.y = Input.GetAxisRaw("Vertical");
        }



        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void Dash(){
        dashTimer = dashDuration;
        dashCooldownTimer = dashCooldown;
        dashVector = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        dashVector.Normalize();
        audio.Play();
        particleAngleVector = dashVector;
        
        canDash = false;
        dashing = true;
        dashParticleObject.transform.eulerAngles = new Vector3(-90, 0, Vector2.SignedAngle(Vector2.up, particleAngleVector));
        // dashParticleObject.SetActive(true);
        dashParticleObject.GetComponent<ParticleSystem>().Play();
        KillboxEventSystem.TriggerDashEvent();
    }

    void CheckDashTimers(){
        if(dashTimer > 0){
            dashTimer -= Time.fixedDeltaTime;
            dashParticleObject.transform.eulerAngles = new Vector3(-Vector2.SignedAngle(Vector2.up, particleAngleVector)-90, 90, -90);
        }
        else if(dashTimer <= 0 && dashing){
            dashing = false;
            dashVector = Vector2.zero;
            dashParticleObject.GetComponent<ParticleSystem>().Stop();
        }

        if(dashCooldownTimer > 0){
            dashCooldownTimer -= Time.fixedDeltaTime;
        }
        else if(dashCooldownTimer <= 0 && !canDash){
            canDash = true;
        }
    }

    void FixedUpdate()
    {
        // rotation_value.Normalize();
        // movement.Normalize();
        
        if( canDash && canMove && (Input.GetKeyDown(CustomKeybinds.main.Shoot2) || CustomKeybinds.main.ControllerDash())  ){ Dash(); }
        CheckDashTimers();

        if(!canMove){
            return;
        }

        if(movement.x + movement.y != 0){
            if(!dashing)
                rb.velocity =  ( ( movement * speed  * slow_mod ) + ( dashVector * dashMod ) ) * Time.fixedDeltaTime * 0.8f ;
            else
                rb.velocity =  dashVector * (speed + dashMod)  * Time.fixedDeltaTime * 0.8f;
            //rb.MovePosition(rb.position + (movement * Speed * slow_mod * Time.fixedDeltaTime) * 0.8f);
            rb.MovePosition(rb.position + rb.velocity);
        }
        else{
            if(!dashing)
                rb.velocity =  ( ( movement * speed  * slow_mod ) + ( dashVector * dashMod ) ) * Time.fixedDeltaTime;
            else
                rb.velocity =  dashVector * (speed + dashMod)  * Time.fixedDeltaTime;
            //rb.MovePosition(rb.position + movement * Speed * slow_mod * Time.fixedDeltaTime);
            rb.MovePosition(rb.position + rb.velocity);
        }

        if(mobile)
        {
            float RotX = RotJoystick.Horizontal;
            float RotY = RotJoystick.Vertical;

            if (RotX == 0 && RotY == 0)
            {
                RotX = MoveJoystick.Horizontal;
                RotY = MoveJoystick.Vertical;

                if (RotX != 0 || RotY != 0)
                {
                    angle = Mathf.Atan2(RotX, RotY) * Mathf.Rad2Deg;
                    rb.rotation = angle * -1;

                }

                rotating = false;
            }
            else
            {
                angle = Mathf.Atan2(RotX, RotY) * Mathf.Rad2Deg;
                rb.rotation = angle * -1;
                rotating = true;
            }
        }
        else if(!DetectInputDevice.main.isController){

            Vector2 direction = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)transform.position;
            Vector2 gunDirection = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector2)gunTransform.position;
            Vector2 dualGunDirection = gunDirection + (Vector2)gunTransform.position - (Vector2)dualGunTransform.position;
            
            angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            rb.rotation = angle - 90;
            // rb.rotation = angle;

            // gunTransform.localPosition = new Vector2(gunTransform.position.x, Mathf.Lerp(0.3f, 0.613f, direction.magnitude / pointBlankRange));
            
            if(direction.magnitude <= pointBlankRange){
                gunDirection = direction.normalized * pointBlankRange + (Vector2)transform.position - (Vector2)gunTransform.position;
                dualGunDirection = direction.normalized * pointBlankRange + (Vector2)transform.position - (Vector2)dualGunTransform.position;
            }

            // Debug.Log(gunDirection + " --- " + gunDirection.magnitude);
            gunTransform.rotation = Quaternion.LookRotation(-gunTransform.forward, gunDirection);
            dualGunTransform.rotation = Quaternion.LookRotation(-dualGunTransform.forward, dualGunDirection);

            // gunTransform.localEulerAngles = new Vector3(gunTransform.localEulerAngles.x, 0, gunTransform.localEulerAngles.z);
            // dualGunTransform.localEulerAngles = new Vector3(dualGunTransform.localEulerAngles.x, 0, dualGunTransform.localEulerAngles.z);
        }
        else{

            //Controller rotation inputs

            // float RotX = Mathf.Sin(0.5f * Mathf.PI * rotation_value.x);
            // float RotY = Mathf.Sin(0.5f * Mathf.PI * rotation_value.y);
            float RotX = rotation_value.x;
            float RotY = rotation_value.y;
            // float RotX = Input.GetAxis("JoystickFireX");
            // float RotY = Input.GetAxis("JoystickFireY");

            if (RotX < deadzone && RotY < deadzone && RotX > -deadzone && RotY > -deadzone)
            {
                RotX = movement.x;
                RotY = movement.y;
                rotating = false;

                if (RotX != 0 && RotY != 0)//(RotX > -deadzone || RotY > -deadzone || RotX < deadzone || RotY < deadzone)
                {
                    angle = Mathf.Atan2(RotX, RotY) * Mathf.Rad2Deg;
                    rb.rotation = angle * -1;

                }
            }
            else
            {
                angle = Mathf.Atan2(RotX, RotY) * Mathf.Rad2Deg;
                rb.rotation = angle * -1;

                rotating = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D col)
    {
        var collider_list = colliders.GetColliders();
        int count = 0;

        for(int i = 0; i < collider_list.Count; i++)
        {
            if(collider_list[i].tag != null && collider_list[i].tag == "slow_field")
            {
                count++;
            } 
        }

        if(count > 0){
            slow_mod = 0.9f - (count * 0.15f);
            if(count >= 3){
                slow_mod = 0.42f; 
            }
        }
        else{
            slow_mod = 1;
        }
    }

    void OnTriggerExit2D(Collider2D col)
    {
        if(col.tag == "slow_field")
        {
            var collider_list = colliders.GetColliders();
            int count = 0;

            for(int i = 0; i < collider_list.Count; i++)
            {
                if(collider_list[i].tag != null && collider_list[i].tag == "slow_field")
                {
                    count++;
                } 
            }

            if(count > 0){
                slow_mod = 0.95f - (count * 0.1f);
                if(count >= 4){
                    slow_mod = 0.6f; 
                }
            }
            else{
                slow_mod = 1;
            }
        }
    }

    public void OnOpenShop(int shopId)
    {
        canMove = false;
    }

    public void OnCloseShop()
    {
        canMove = true;
    }

    public void OnSetNewWeapon(WeaponItem weaponItem, int slot)
    {
        
    }

    public void SetCanMove(bool move){
        canMove = move;
    }


    public bool Rotating
    {
        get { return rotating; }
    }
}
