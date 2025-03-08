using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLoader : MonoBehaviour
{
    [SerializeField]
    private Slider slider;
    private float timer = 5.5f, elapsed = 0, progress, time_value, init_time, post_load_time = 0.75f;
    private bool loading = false, fading_out = false;
    [SerializeField] private string sceneName;

    [SerializeField]
    private Animator anim;

    [SerializeField]
    private Scene loading_scene;

    // Start is called before the first frame update
    void Start()
    {
        init_time = timer;
        loading_scene = SceneManager.GetActiveScene();
        if(loading_scene.name == ""){
            SceneManager.LoadScene(1);
        }
        //StartCoroutine(LoadYourAsyncScene());
    }


    void Update(){

        Debug.Log("Active Scene is '" + loading_scene.name + "'.");

        if(timer <= 0){
            if(!loading){
                StartCoroutine(LoadYourAsyncScene());
                loading = true;
            }
            
        }
        else{
            timer -= Time.deltaTime;
            elapsed += Time.deltaTime;

            time_value = 50 * elapsed/init_time;
            if(time_value >= 50){
                time_value = 50;
            }

            slider.value = time_value;

        }
    }


    //async Task LoadGame()
    //{
    //    await Task.Run(() => {
//
    //        SceneManager.LoadSceneAsync("Menus");
    //        
    //    });
//
    //    slider.value = 100;
//
//
    //}

    IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        // AsyncOperation loader = SceneManager.LoadSceneAsync("Menus");
        AsyncOperation loader = SceneManager.LoadSceneAsync(sceneName);
        loader.allowSceneActivation = false;

        // Wait until the asynchronous scene fully loads
        while (!loader.isDone)
        {
            slider.value = 50 + (int)((loader.progress/0.9) * 100 / 2);
            yield return null;

            if(loader.progress >= 0.9f){

                if(!fading_out){
                    anim.Play("main");
                    fading_out = true;
                }
                post_load_time -= Time.deltaTime;

                if(post_load_time <= 0){
                    loader.allowSceneActivation = true;
                }
            }
        }

        //SceneManager.SetActiveScene(loading_scene);
    }
}
