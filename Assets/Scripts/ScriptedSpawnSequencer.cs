using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptedSpawnSequencer : MonoBehaviour
{

    [SerializeField] private SphericalSpawner spawner;
    [SerializeField] private SphericalSpawnInstructionSequence sequence;
    [SerializeField] private bool active;
    [SerializeField] private float clock, time, target_time; 


    // Start is called before the first frame update
    void BeginSequence(SphericalSpawnInstructionSequence _sequence)
    {
        sequence = _sequence;


        target_time = sequence.current_instructions.step_time;
    }

    void FixedUpdate()
    {
        if(active){
            clock += Time.fixedDeltaTime; // + music factor
        }

        if(clock >= target_time){

            if(!sequence.finished){

                spawner.SummonAtAngle(sequence.current_instructions.GetNext());
                int status = sequence.UpdateInstruction();

                switch (status){
                    case -1:

                        
                        break;
                    case 0:
                        target_time = sequence.current_instructions.step_time;
                        break;
                    case 1:
                        
                        break;

                }


                clock = 0;
            }
        }



        //Time.fixedDeltaTime;
    }
}
