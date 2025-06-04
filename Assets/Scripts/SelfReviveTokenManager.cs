using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SelfReviveTokenManager : MonoBehaviour, IProgressionListener, ISelfResListener, IRestartListener
{
    public static int selfResTokens {get; private set; } = 5;
    public static int selfResThisRound {get; private set; }
    public static SelfReviveTokenManager main {get; private set; }
    public static bool initialized {get; private set; }
    [SerializeField] private GameObject selfResButton;
    [SerializeField] private Text[] ownedText;
    [SerializeField] private Text RevivesAvailableText;
    [SerializeField] private UnityEvent onReviveEvent;

    public void OnGainXp(int xp)
    {}

    public void OnLevelUp()
    {
        if(ProgressionSystem.playerLevel > 25){
            selfResTokens += 1 ;
        }
        else if(ProgressionSystem.playerLevel % 5 != 0){
            selfResTokens += 1 ;
        } 
    }


    // Start is called before the first frame update
    void Awake()
    {
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        if(!initialized){
            if(SaveSystem.LoadPlayer() != null){
                PlayerData data = SaveSystem.LoadPlayer();
                // selfResTokens = data.selfReviveTokens;
            }
            else{
                // selfResTokens = 0;
            }
        }

    }

    void Update(){
        foreach (Text text in ownedText)
        {
            text.text = selfResTokens.ToString();
        }
    }

    public void UseSelfResToken(){
        selfResTokens--;
        selfResThisRound++;
        selfResButton.SetActive(false);
        Player.main.health.SetMaxHealth(Player.main.health.GetMaxHealth(), "JWBVIHEWBCV*&T^&237236fg3gv38fvr3v3v6)*&", true);
        EnemyCounter.main.DestroyAllEnemies(false);
        KillboxEventSystem.TriggerSelfResEvent();
        // GameManager.main.ContinueGameAfterAD();

        //if(res_count == 2)
        //{
        //    
        //}

    }
    public bool CanSelfRes(){
        return selfResTokens > 0 && selfResThisRound < 2;
    }

    public void OnSelfResPrompt()
    {
        if(GameplayUI.instance.GetSelfResButton() != null){
            GameplayUI.instance.GetSelfResButton().SetActive(true);
        }
        else return;
        if(selfResTokens > 1 && selfResThisRound == 0){
            RevivesAvailableText.text = "2 REVIVES AVAILABLE"; 
        }

        else if( (selfResTokens > 0 && selfResThisRound == 1) || (selfResTokens == 1 && selfResThisRound < 2) ){
            RevivesAvailableText.text = "1 REVIVE AVAILABLE"; 
        }
    }

    public void OnSelfRes()
    {
        onReviveEvent.Invoke();
    }

    public void OnRestartGame()
    {
        selfResThisRound = 0;
    }
}
