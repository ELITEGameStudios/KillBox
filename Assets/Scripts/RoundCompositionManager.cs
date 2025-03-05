using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundCompositionManager : MonoBehaviour
{
    

    [SerializeField] public RoundComposition[] compositions {get; private set;} = new RoundComposition[]{
        RoundComposition.triadic,
        RoundComposition.lasers,
        RoundComposition.prismatic,
        RoundComposition.ranged,
        RoundComposition.debuff,
    };

    public RoundComposition current {get; private set;}
    public static RoundCompositionManager main {get; private set;}
    public bool hasComposition; //{get; private set;}
    [SerializeField] private int currentCompIndex; 


    void Awake(){
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

        currentCompIndex = 0;
    }

    public void AvoidComposition(){

        hasComposition = false;
    }

    public void ChangeComposition(){

        // List<RoundComposition> validCompositions = new List<RoundComposition>();

        // foreach (RoundComposition composition in compositions){
        //     if(
        //         composition.startRound <= GameManager.main.LvlCount &&
        //         (composition.endRound > GameManager.main.LvlCount || composition.endRound == -1) )
        //     {    
        //         validCompositions.Add(composition);
        //     }
        // }

        hasComposition = false;
        if(BossRoundManager.main.isBossRound){return;}

        int chance = Random.Range(0, 12);

        if(chance < compositions.Length && GameManager.main.LvlCount > 5 ){

            currentCompIndex++;
            if(currentCompIndex >= compositions.Length-1){ currentCompIndex = 0; }

            while(!hasComposition){
                if(compositions[currentCompIndex].isValid()){
                    current = compositions[currentCompIndex];
                    hasComposition = true;
                }
                else{currentCompIndex++;
                    if(currentCompIndex >= compositions.Length-1){ currentCompIndex = 0; }
                }
            }

            Debug.Log("Composition set");
        }

        // if(index < validCompositions.Count && GameManager.main.LvlCount > 5 ){

        //     currentCompIndex++;
        //     if(currentCompIndex >= validCompositions.Count){ currentCompIndex = 0; }

        //     hasComposition = true;
        //     current = compositions[currentCompIndex];
        //     // current = RoundComposition.debuff;
        //     Debug.Log("Composition set");
        // }
        // else{
        //     hasComposition = false;
        // }
    }
}
