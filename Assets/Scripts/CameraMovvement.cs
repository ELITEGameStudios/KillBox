using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovvement : MonoBehaviour
{
    [SerializeField]
    private Transform camera_tf, player_tf;

    [SerializeField]
    private float normal_zoom, zoom_strength;

    [SerializeField]
    private Camera cam;

    [SerializeField]
    private float smoothing, zoom_smoothing;
    
    public static CameraMovvement main {get; private set;}

    void Awake(){
        if(main == null){
            main = this;
        }
        else if(main != this){
            Destroy(this);
        }

    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector2 player2Dpos = new Vector2(player_tf.position.x, player_tf.position.y);
        Vector2 camera2Dpos = new Vector2(camera_tf.position.x, camera_tf.position.y);
        Vector2 target_pos;

        // if(BossRoundManager.main.isBossRound){
        if(false){
            target_pos = Vector2.Lerp(camera2Dpos, player2Dpos / 3 , smoothing * Time.deltaTime);
        }
        else{
            target_pos = Vector2.Lerp(camera2Dpos, player2Dpos, smoothing * Time.deltaTime);
        }
       
        camera_tf.position = new Vector3(target_pos.x, target_pos.y, camera_tf.position.z);

        float current_zoom = cam.orthographicSize;
        float target_zoom;

        // if(BossRoundManager.main.isBossRound){
        if(false){
            target_zoom = normal_zoom + (Vector2.Distance(player2Dpos, camera2Dpos) * zoom_strength/2);
        }
        else{
            target_zoom = normal_zoom + (Vector2.Distance(player2Dpos, camera2Dpos) * zoom_strength);
        }
       
        

        float zoom_diff = target_zoom - current_zoom;
        float current_zoom_diff = target_zoom - current_zoom;

        cam.orthographicSize = current_zoom + (zoom_diff * (1 / zoom_smoothing));
    }

    //7
    //
    //7 + 2 * 1 = 9
    //0.01 * 100 = 1   :    1 / 10
    //8
    //
    //9 - 7 = 2
    //
    //9 - 8 = 1
    //
    //
    //
    //17 = 8 + 2 * 1

}
