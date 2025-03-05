using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StageManager : MonoBehaviour
{
    public static readonly string[] stages = new string[]{
        "",
        "AWAKENING", // After shard is defeated, > round 12
        "DAUNTLESS", // After round 24
        "BOSS RUSH", // Round 44
        "THE ENDGAME...", // Everything after this point (round 45) the player must seek out and is not part of the game by default
        "THE RUNES", // Portal pattern puzzle?D 
        "AETHERIAL CORRUPTION", 
        "THE FINAL DUAL"
    };

    public static readonly string[] stage_2_dialoge = new string[]{
        "It appears this player has defeated an elite",
        "I assure you, you will fail sooner or later",
        "That was a close one. Your end nears",
        "",
    };
    public static readonly string[] stage_3_dialoge = new string[]{
        "How have you managed to get this far",
        "Most people have failed by now. How are you different",
        "You should be dead by now",
        "You cant keep going forever",
        "I have a special challenge for you this round.",
        "Round after round, you march closer and closer.",
        "Why are you still going? Do you want freedom? Do you want to break the loop?",
        "Its funny how you think your progress makes a difference to where you end up"
    };

    public static readonly string[] boss_rush_dialoge = new string[]{
        "You have come so far, and I've grown impatient waiting for your demise.",
        "Fine. This challenge will ensure you progress no further.",
    };

    public static readonly string[] low_health_dialogue = new string[]{
        "Getting pretty close",
        "This seems like the end",
        "Quite an overwhelming situation here",
        "Seems to be a skill issue",
        "Die?",
        "This must be it",
        "You had a good run",
        "You wanted to reach the end, but you wont see it this day.",
        "I'm saying goodbye early"
    };

    public static readonly string[] failed_game = new string[]{
        "The loop continues. Try, Try again.",
        "The loop continues, but it can be broken. Keep trying.",
        "You are getting closer. Again!",
        "So close yet so far. Nevertheless, the end is possible with another attempt.",
        "You are almost there! Keep trying!",
        "One attempt closer to victory. One attempt closer if the will to win is unchanged.",
        "There is no true beginning or end if there is no journey.",
        "The loop continues. Alright lets do this one last time.",
    };

    [SerializeField]
    private Text stage_text, dialogue_text;
    
    [SerializeField]
    private Color dialogue_color;

    [SerializeField]
    private int current_stage = 1, 
    health_dialogue_index = 0;
    public bool can_hurt_message {get; private set;}

    public static StageManager main {get; private set;}


    // Update is called once per frame
    void Awake()
    {

        if(main == null){
            main = this;
        }
        else{
            //Destroy(this);
        }

        can_hurt_message = true;
    }

    void InitStageFadeAnimation(int stage){
        stage_text.text = "STAGE " + stage.ToString() + ": " + stages[stage-1];
        current_stage = stage;
        StartCoroutine("StageAnimCoroutine");
    }

    public void HurtDialogue(){
        dialogue_text.text = low_health_dialogue[health_dialogue_index];
        if(health_dialogue_index+1  != low_health_dialogue.Length){
            health_dialogue_index++;
        }
        else{
            health_dialogue_index = 0;
        }
        StartCoroutine("DialogueCoroutine");
    }

    IEnumerator StageAnimCoroutine(){
        
        float alpha = 0;
        float font = 60;
        Color color = new Color(1, 1, 1, alpha);
        stage_text.fontSize = (int)font;
        stage_text.color = color; 

        float time = 0;
        while (time < 3){
            alpha += 0.34f * Time.deltaTime;
            color = new Color(1, 1, 1, alpha);
            stage_text.color = color;

            
            font -= 7 * Time.deltaTime;
            stage_text.fontSize = (int)font; 
            time += Time.deltaTime;  
            yield return null;
        }

        while (time < 6){
            font -= 7 * Time.deltaTime;
            stage_text.fontSize = (int)font; 
            time += Time.deltaTime;  
            yield return null;
        }

        while (time < 8){
            alpha -= 0.5f * Time.deltaTime;
            color = new Color(1, 1, 1, alpha);
            stage_text.color = color;

            
            font -= 7 * Time.deltaTime;
            stage_text.fontSize = (int)font; 
            time += Time.deltaTime;  
            yield return null;
        }

    }
    IEnumerator DialogueCoroutine(){

        can_hurt_message = false;

        float alpha = 1;
        Color color = new Color(1, 0, 0, alpha);
        stage_text.color = color; 

        dialogue_text.gameObject.SetActive(true);
        yield return new WaitForSeconds(4);

        float time = 2;
        while (time > 0){
            alpha -= 0.5f * Time.deltaTime;
            color = new Color(1, 0, 0, alpha);
            stage_text.color = color;
        
            time -= Time.deltaTime;  
            yield return null;
        }
        
        dialogue_text.gameObject.SetActive(false);

        yield return new WaitForSeconds(4);
        can_hurt_message = true;
    }
}
