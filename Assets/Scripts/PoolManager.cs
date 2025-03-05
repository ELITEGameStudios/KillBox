using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    public GameObject[][] Pools;

    void Start(){
        Pools = new GameObject[16][];
    }

    public void PoolObject(GameObject obj, int index){
        if(Pools[index] == null){
            Pools[index] = new GameObject[] {obj, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, null, };
            Debug.Log("Successfully Pooled, and created New Index!");
        }
        else{
            for(int i = 0; i <= Pools[index].Length; i++){
                if(Pools[index][i] != null){
                    Debug.Log("passed slot "+ i);
                    continue;
                }
                else
                    Pools[index][i] = obj;
                    Debug.Log("Successfully Pooled! Pool size of Pools[" + index + "] is" + Pools[index].Length);
                    break;
            }
        }
    }
    public void destroyObj(GameObject obj){
        obj.GetComponent<BulletDestroy>().ResetRangeCall();
        obj.SetActive(false);
        Debug.Log("Pooled object is false.");
    }

    public void GetFromPool(shooterScript2D script, int index){
        if(Pools[index] != null){
            for(int i = 0; i < Pools[index].Length; i++){
                if (Pools[index][i] != null){
                    script.clone = Pools[index][i].GetComponent<Rigidbody2D>();
                    script.clone.gameObject.SetActive(true);
                    Pools[index][i] = null;
                }
                else{
                    if( i++ == Pools[index].Length){
                        script.clone = null;
                        break;
                    }
                }
            }
        }
        else
            script.clone = null;
    }

        public void GetFromAIPool(AIShooterScript script, int index){
        if(Pools[index] != null){
            for(int i = 0; i < Pools[index].Length; i++){
                if (Pools[index][i] != null){
                    script.clone = Pools[index][i].GetComponent<Rigidbody2D>();
                    script.clone.gameObject.SetActive(true);
                    Pools[index][i] = null;
                }
                else{
                    if( i++ == Pools[index].Length){
                        script.clone = null;
                        break;
                    }
                }
            }
        }
        else
            script.clone = null;
        
    }
    public void GetFlashFromPool(GameObject obj, int index){
        
        shooterScript2D script = obj.GetComponent<shooterScript2D>();
        if(Pools[index] != null){
            for(int i = 0; i < Pools[index].Length; i++){
                if (Pools[index][i] != null){
                    script.GraphicClone = Pools[index][i];
                    script.GraphicClone.SetActive(true);
                    Pools[index][i] = null;
                }
                else{
                    if( i++ == Pools[index].Length){
                        script.GraphicClone = null;
                        break;
                    }
                }
            }
        }
        else
            script.clone = null;
    }

        public void GetDmgFromPool(GameObject obj, int index){
        
        PlayerDamage script = obj.GetComponent<PlayerDamage>();
        if(Pools[index] != null){
            for(int i = 0; i < Pools[index].Length; i++){
                if (Pools[index][i] != null){
                    script.DmgTxt = Pools[index][i];
                    script.DmgTxt.SetActive(true);
                    Pools[index][i] = null;
                }
                else{
                    if( i++ == Pools[index].Length){
                        script.DmgTxt = null;
                        break;
                    }
                }
            }
        }
        else
            script.DmgTxt = null;
    }
}
