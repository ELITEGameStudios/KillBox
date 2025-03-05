using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundComposition
{
    public string[] tags {get; private set;}
    public int[] limits {get; private set;}
    public int startRound {get; private set;}
    public int endRound {get; private set;}
  
    // public static RoundComposition normal {get; private set;} = new RoundComposition(
    //     new string[]{
    //         "Piercer",
    //         "Slow Field",
    //         "Fire Dot",
    //         "Fire Triad"
    //     } );

    public static RoundComposition triadic {get; private set;} = new RoundComposition(
        new string[]{
            "Mini Triad",
            "Triad",
            "Speedy Triad",
            "Endgame Triad",
            "Fire Triad"
        } );

    public static RoundComposition lasers {get; private set;} = new RoundComposition(
        new string[]{
            "Half Polaroid",
            "Polaroid",
            "Dart"
        });

    public static RoundComposition prismatic {get; private set;} = new RoundComposition(
        new string[]{
            "Half Polaroid",
            "Polaroid",
            "Ring",
            "Piercer"
        }, 25, 35);
    public static RoundComposition ranged {get; private set;} = new RoundComposition(
        new string[]{
            "Shooter",
            "Blue Shooter",
            "Prestige Shooter",
            "Piercer",
            "Slow Field",
            "Dart"
        }, 10);

    public static RoundComposition debuff {get; private set;} = new RoundComposition(
        new string[]{
            "Slow Field",
            "Camo Dot",
            "Fire Dot",
            "Fire Triad"
        }, 15 );

    public RoundComposition(string[] tags){
        this.tags = tags;
        startRound = 5;
        endRound = -1;
    }

    public RoundComposition(string[] tags, int startRound){
        this.tags = tags;
        if(startRound >= 5){ this.startRound = startRound; }
        else{ this.startRound = 5; }
        endRound = -1;
    }

    public RoundComposition(string[] tags, int startRound, int endRound){
        this.tags = tags;
        if(startRound >= 5){ this.startRound = startRound; }
        else{ this.startRound = 5; }

        if(endRound >= 10){ this.endRound = endRound; }
        else{ this.endRound = -1; }
    }

    public bool isValid(){
        return 
            (startRound <= GameManager.main.LvlCount) &&
            (endRound > GameManager.main.LvlCount || endRound == -1);
    }
}
