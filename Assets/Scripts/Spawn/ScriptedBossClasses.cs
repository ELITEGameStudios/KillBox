using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedBossClasses
{

}

public struct SphericalSpawnInstruction
{
    public float angle {get; private set;}
    public float count {get; private set;}
    public float distance {get; private set;}
    public GameObject obj {get; private set;}
    
    public SphericalSpawnInstruction(float _angle, GameObject _obj, float _count = 1, float _distance = 5){
        obj = _obj;
        angle = _angle;
        count = _count;
        distance = _distance;
    }
}

public class SphericalSpawnInstructionSet
{
    public List<SphericalSpawnInstruction> instructions {get; private set;}
    public SphericalSpawnInstruction next_instruction {get; private set;}
    public float angle {get; private set;}
    public int index {get; private set;}
    public float total_time {get; private set;}
    public float step_time {get; private set;}
    public bool finished {get; private set;} = false;
    GameObject obj;

    public SphericalSpawnInstructionSet(List<SphericalSpawnInstruction> _instructions, float _time = 2){
        
        instructions = _instructions;
        index = 0;
        total_time = _time;
        step_time = total_time / instructions.Count;
        next_instruction = instructions[0];
        
        //obj = _obj;
        //angle = _angle;
    }
    public SphericalSpawnInstruction GetNext(bool advance = true){
        
        SphericalSpawnInstruction result = next_instruction;

        instructions.RemoveAt(0);
        
        if(instructions.Count == 0){
            finished = true;
        }
        else{
            next_instruction = instructions[0];
        }

        return result;
    }

}

public class InstructionSetLib{
    // Standard sets. Naming convention: 
    // c = clockwise, cc = counter clockwise
    // c/cc _ [range] _ [start_angle] _ [count] _ [distance]
    
    //public static readonly SphericalSpawnInstructionSet c_360_90_8 = new SphericalSpawnInstructionSet(
    //    new List<SphericalSpawnInstruction>(){
    //        new SphericalSpawnInstruction(_angle: 90, _count: 1, _distance: 5, _obj: )
    //    },
    //    
    //    _time: 2
    //);

}

public class SphericalSpawnInstructionSequence
{
    public List<SphericalSpawnInstructionSet> instruction_set {get; private set;}
    public SphericalSpawnInstructionSet current_instructions {get; private set;}
    public float angle {get; private set;}
    public int index {get; private set;}
    //GameObject obj;
    public bool finished {get; private set;} = false;

    public SphericalSpawnInstructionSequence(List<SphericalSpawnInstructionSet> _instruction_set){
        
        index = 0;
        instruction_set = _instruction_set;
        current_instructions = instruction_set[0];
        //obj = _obj;
        //angle = _angle;
    }

    public int UpdateInstruction(){

        if(current_instructions.finished){
            instruction_set.RemoveAt(0);

            if(instruction_set.Count == 0){
                if(!finished){
                    finished = true;
                    return 1;
                }

            }
            else{
                current_instructions = instruction_set[0];
                return 0;
            }
        }

        return -1;
    }
}
