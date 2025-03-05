using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipmentGeneralManager : MonoBehaviour
{
    GameManager manager;

    [SerializeField] private OverdriveManager over;
    [SerializeField] private UltraManager ultra;
    [SerializeField] private HotshotEquipManager hotshot;
    [SerializeField] private EAI_equipment_manager minions;
    // WOAH SHIELD EQUIPMENT ITS AN EXTRA HP BAR (FORTBITE) TAKES HITS INSTEAD OF DAMAGE LOL SKILL ISSUE IF YOU USE THIS NGL


    private int index;
    // Start is called before the first frame update
    void Start()
    {
        manager = GameObject.Find("Manager").GetComponent<GameManager>();
        
    }

    // Update is called once per frame
    void Update()
    {
        index = manager.equipment_index;
    }

    public void StopAllEquipments(){
        switch (index){
            case 0:{

                over.DeactivateButton();
                break;
            }
            case 1:{
                
                ultra.DeactivateButton();
                break;
            }
            case 2:{

                hotshot.DeactivateButton();
                break;
            }
            case 3:{
                
                minions.DeactivateButton();
                break;
            }
        }
    }

    public void GamemodeStart(){
        switch (index){
            case 0:{

                over.GamemodeStart();
                break;
            }
            case 1:{
                
                ultra.GamemodeStart();
                break;
            }
            case 2:{

                hotshot.GamemodeStart();
                break;
            }
            case 3:{
                
                minions.GamemodeStart();
                break;
            }
        }
    }
}
