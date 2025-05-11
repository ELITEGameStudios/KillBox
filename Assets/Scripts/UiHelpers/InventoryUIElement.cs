using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Pathfinding.Serialization;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class InventoryUIElement : MonoBehaviour, IRestartListener
{
    [SerializeField]
    private string weapon_key;

    [SerializeField]
    private WeaponItem item {
        get {
            try {
                return WeaponItemList.Instance.GetItem(weapon_key);
            }
            catch{
                return null;
            }
        }
    }

    [SerializeField]
    private Image overlay, main_img, gun_img;

    [SerializeField] private bool assigned, is_main, selected, initialized;

    [SerializeField]
    private int tier, type;

    [SerializeField]
    private Animator animator;
    [SerializeField] private Vector3 initScale;

    [SerializeField] private float timer;
    [SerializeField] private Color cooldown_color;
    [SerializeField] private Button mainButton;
    [SerializeField] private float dimLevel = 1;
    [SerializeField] private int mainButtonChildNum = 0;
    [SerializeField] private ButtonStatus state;
    // public bool selected {get {return !(state == ButtonStatus.SELECTED);}}

    public enum ButtonStatus{
        UNPURCHASABLE,
        PURCHASABLE,
        SELECTED,
        OWNED
    }

    void Awake()
    {
        weapon_key = name;
        if(!assigned){
            overlay = transform.GetChild(0).GetComponent<Image>();
            main_img = transform.GetChild(mainButtonChildNum).GetComponent<Image>();
            gun_img = main_img.transform.GetChild(0).GetComponent<Image>();
        }

        if(type == 0){
            mainButton = transform.GetChild(mainButtonChildNum).GetComponent<Button>();
            mainButton.gameObject.AddComponent<InventoryUiButtonHelper>();
            mainButton.gameObject.GetComponent<InventoryUiButtonHelper>().host = this;
        }

        initScale = gun_img.transform.localScale;
    }
    void Start()
    {
        if(!initialized){InitializeUIElement();}
        InventoryUIManager.Instance.AddMainButton(this);
    }

    public void InitializeUIElement(){
        // item = WeaponItemList.Instance.GetItem(weapon_key);
        if(item != null && !initialized){
            tier = item.tier;

            //Sprite new_sprite = Sprite.Create(item.graphic, new Vector2(0.5f, 0.5f));
            if(type != 0){

            }
            else{
                gun_img.sprite = item.graphic;
                ColorBlock colourBlock = mainButton.colors;
                colourBlock.normalColor = InventoryUIManager.Instance.tier_colors[tier];
                colourBlock.highlightedColor= Color.white;
                colourBlock.selectedColor= Color.white;
                mainButton.colors = colourBlock;
                // main_img.color = InventoryUIManager.Instance.tier_colors[tier]; 
                main_img.color = Color.clear;
            }

            initialized = true;
            Debug.Log("Hehehehaw");
        }

        if(is_main){
            if(type == 1 && InventoryUIManager.Instance.primary_element == null){
                InventoryUIManager.Instance.primary_element = this;

                // mainButton.transition.ColorTint.NormalColor = InventoryUIManager.Instance.tier_colors[0];
                
                
                main_img.color = Color.white;
                gun_img.color = InventoryUIManager.Instance.tier_colors[0];
            }
        
            else if(type == 2 && InventoryUIManager.Instance.secondary_element == null){
                InventoryUIManager.Instance.secondary_element = this;
            }

            else if(type == 3 && InventoryUIManager.Instance.dual_element == null){
                InventoryUIManager.Instance.dual_element = this;
            }
        }
    }

    // Start is called before the first frame update
    public void OnOwnedCheck(){

        if(type != 0){
            return;
        }

        // main_img.color = InventoryUIManager.Instance.tier_colors[tier];
        // main_img.color = Color.clear;
        // gun_img.color = InventoryUIManager.Instance.tier_colors[tier];
        

        // if(item.owned){
        //     overlay.color = Color.clear;
        //     //main_img.color = InventoryUIManager.Instance.tier_colors[tier];
        //     //gun_img.color = InventoryUIManager.Instance.tier_colors[tier];
        // }
        // else{

        //         if(!item.Compare(GameManager.main.ScoreCount)){
        //             overlay.color = InventoryUIManager.Instance.dim_shade;
                    
        //         }
        //         else{
        //             overlay.color = InventoryUIManager.Instance.purchasable_shade;
        //         }
        // }
    }

    public void OnTargetCheck(){

        if(type != 0){
            return;
        }
        
        if(InventoryUIManager.Instance.target_key == weapon_key){

            main_img.color = Color.white;
            gun_img.color = Color.white;
            // overlay.color = Color.clear;
        }
        //else{
        //    overlay.color = InventoryUIManager.Instance.dim_shade;
        //}
    }

    public void EquipDisplay(){
        if(type == 0){
            return;
        }

        switch(type){
            case 1:
            {
                main_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.primary_weapon.tier];
                gun_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.primary_weapon.tier];
                gun_img.sprite = GunHandler.Instance.primary_weapon.graphic;
                break;
            }
            case 2:
            {
                if(GunHandler.Instance.backup_weapon != null){
                    main_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.backup_weapon.tier];
                    gun_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.backup_weapon.tier];
                    gun_img.sprite = GunHandler.Instance.backup_weapon.graphic;
                }
                else{
                    main_img.color = Color.white;
                    gun_img.color = Color.white;
                    gun_img.sprite = null;
                    gun_img.color= Color.clear;

                }

                break;
            }
            case 3:
            {
                if(GunHandler.Instance.dual_weapon != null){
                    main_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.dual_weapon.tier];
                    gun_img.color = InventoryUIManager.Instance.tier_colors[GunHandler.Instance.dual_weapon.tier];
                    gun_img.sprite = GunHandler.Instance.dual_weapon.graphic;
                }
                else if(UpgradesManager.Instance.current_levels[4] != 1){
                    main_img.color = Color.black;
                    gun_img.color = Color.black;

                }
                else{
                    main_img.color = Color.white;
                    gun_img.color = Color.white;
                }
                break;
            }
        }

        if(animator != null){
            if(type == 1 && GunHandler.Instance.current_is_primary){
                animator.Play("Selected");
            }
            else if(type == 2 && !GunHandler.Instance.current_is_primary){
                animator.Play("Selected");
            }
        }
    }

    bool CoolingDownCheck(){
        switch (type){
            case 1:
                if(GunHandler.Instance.primary_cooldown.cooling_down){ return true; }
                break;
            case 2:
                if(GunHandler.Instance.secondary_cooldown.cooling_down){ return true; }
                break;
        }

        return false;
    }
    public void ShootAnimation(){
        animator.Play("Shoot");
    }
    public void StartCooldown(){
        StartCoroutine("CooldownAnim");
    }
    public void Flip(){
        animator.Play("Selected");
    }

    void Update(){
        if(InventoryUIManager.Instance.target_key == weapon_key && type == 0){ // Should be selected here??
            Debug.Log("Hi! I should be selected...");
            bool purchasable = item.Compare(GameManager.main.ScoreCount);
            gun_img.transform.localScale = Vector3.Lerp(initScale, initScale + new Vector3(0.2f, 0.2f, 0.2f),CommonFunctions.SineEase(timer/ (purchasable ? 0.5f : 1)));
            timer += Time.deltaTime;

            // maybe delete
            main_img.color = Color.white;
            gun_img.color = Color.white;
            // overlay.color = Color.clear;
        }
        else{
            gun_img.transform.localScale = initScale;

            if(type == 0){
            // overlay.color = Color.clear;

                // if(selected){
                //     main_img.color = Color.white;
                //     gun_img.color = Color.white;
                // }
                // else{
                    main_img.color = Color.clear;

                    if(item != null){

                        if(item.owned){ // The player owns this weapon
                            gun_img.color = InventoryUIManager.Instance.tier_colors[tier];
                            overlay.color = Color.Lerp( Color.clear, InventoryUIManager.Instance.tier_colors[tier], 0.5f);
                            // overlay.color = Color.clear;
                        }
                        else{
                            gun_img.color = Color.Lerp(Color.black, InventoryUIManager.Instance.tier_colors[tier], dimLevel);
                                
                            if(item.Compare(GameManager.main.ScoreCount)){
                                // The player can buy this weapon
                                main_img.color = Color.Lerp(Color.clear, InventoryUIManager.Instance.purchasable_shade, CommonFunctions.SineNormalize(Time.timeSinceLevelLoad, 2));
                            }
                        }
                    }
                // }
            }
        }


    }


    // Update is called once per frame
    public void SetTargetKey()
    {
        timer = 0;
        InventoryUIManager.Instance.OnSetTargetKey(weapon_key);   
        // state = ButtonStatus.SELECTED;
    }

    IEnumerator CooldownAnim(){
        while (CoolingDownCheck()){
            if(gun_img.color == cooldown_color){gun_img.color = Color.white;}
            else{gun_img.color = cooldown_color;}

            yield return new WaitForSeconds(0.1f);
        }
        EquipDisplay();

    }

    public void OnRestartGame()
    {
        if(!InventoryUIManager.Instance.primary_element == this)
        {
            if(type==3){

                main_img.color = Color.black;
                gun_img.sprite = WeaponItemList.Instance.GetItem("Light AR").graphic;
                gun_img.color= Color.black;
            }
            else{
                main_img.color = Color.white;
                gun_img.color = Color.white;
            }
        }
        else
        {
            main_img.color = InventoryUIManager.Instance.tier_colors[0];
            gun_img.color = InventoryUIManager.Instance.tier_colors[0];
        }
    }

    public void OnSelect(){
        // if(baseEventData.selectedObject.GetComponent<Button>() == mainButton){
            overlay.color = Color.clear;
            main_img.color = Color.white;
            gun_img.color = Color.white;
            selected = true;
            Debug.Log("g");

        // }
    }

    public void OnDeselect(){
        if(InventoryUIManager.Instance.target_key == weapon_key){
            main_img.color = InventoryUIManager.Instance.tier_colors[tier];
            ColorBlock colourBlock = mainButton.colors;
            colourBlock.normalColor = Color.black;
            colourBlock.highlightedColor= Color.white;
            colourBlock.selectedColor= Color.white;
        }
        else{
            main_img.color = InventoryUIManager.Instance.tier_colors[tier];
            ColorBlock colourBlock = mainButton.colors;
            colourBlock.normalColor = InventoryUIManager.Instance.tier_colors[tier];
            colourBlock.highlightedColor= Color.white;
            colourBlock.selectedColor= Color.white;
        }

        overlay.color = InventoryUIManager.Instance.dim_shade;
        selected = false;
        
    }
}
