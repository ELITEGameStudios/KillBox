using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    [SerializeField] private List<Scene> scenes;
    [SerializeField] private Scene activeScene; 
    [SerializeField] private string flexibleLoadingSceneName, gameSceneName, gameUIName, menusSceneName; 
    [SerializeField] private float flexibleTransitionTime = 1.5f; 
    [SerializeField] private string[] mapSceneNames; 
    [SerializeField] private Scene loadingScene;
    [SerializeField] private Scene menusScene;
    [SerializeField] private Scene gameScene;
    [SerializeField] private Scene gameUI;
    [SerializeField] private Scene currentMapScene;

    public static SceneSystem Instance { get; private set; }
    
    void Awake(){
        if(Instance == null) {Instance = this;}
        else if(Instance != this) {Destroy(this);}
        
        DontDestroyOnLoad(gameObject);       
    }

    public void UpdateSceneData(){
        
        scenes.Clear();
        activeScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }
        
        DetectMenusScene();
    }

    public void LoadAndUnloadOperation(string loadSceneName, string unloadSceneName){
        StartCoroutine(LoadAdditiveCoroutine(loadSceneName));

        for (int i = 0; i < scenes.Count; i++)
        {
            if(scenes[i].name == unloadSceneName){
                StartCoroutine(UnloadSceneCoroutine(scenes[i]));
                scenes.RemoveAt(i);
            }
        }
    }

    public void AddScene(string sceneName){
        StartCoroutine(LoadAdditiveCoroutine(sceneName));
    }

    public void UnloadScene(string sceneName){
        
        for (int i = 0; i < scenes.Count; i++)
        {
            if(scenes[i].name == sceneName){
                StartCoroutine(UnloadSceneCoroutine(scenes[i]));
                scenes.RemoveAt(i);
            }
        }
    }

    public void LoadGameScenes(){
        StartCoroutine(GameInitializationCoroutine());
    }

    public void DetectMenusScene(){
        menusScene = SceneManager.GetSceneByName(menusSceneName);
    }

    IEnumerator LoadAdditiveCoroutine(string sceneName){
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
        while (!operation.isDone){
            yield return null;
        }
        
        UpdateSceneData();
    }

    IEnumerator UnloadSceneCoroutine(Scene scene){
        AsyncOperation operation = SceneManager.UnloadSceneAsync(scene);
        while (!operation.isDone){
            yield return null;
        }

        UpdateSceneData();
    }

    IEnumerator GameInitializationCoroutine(){
        // yield return StartCoroutine(LoadAdditiveCoroutine(flexibleLoadingSceneName));
        // loadingScene = SceneManager.GetSceneByName(flexibleLoadingSceneName);
        DetectMenusScene();
        FadeManager.instance.SetTarget(true, flexibleTransitionTime);
        yield return new WaitForSecondsRealtime(flexibleTransitionTime);
        
        yield return StartCoroutine(LoadAdditiveCoroutine(mapSceneNames[0]));
        yield return StartCoroutine(LoadAdditiveCoroutine(gameSceneName)); 
        yield return StartCoroutine(LoadAdditiveCoroutine(gameUIName)); 

        MainMenuManager.instance.OnGameSceneLoad(); 
        FadeManager.instance.SetTarget(false, flexibleTransitionTime);

        currentMapScene = SceneManager.GetSceneByName(mapSceneNames[0]);
        gameScene = SceneManager.GetSceneByName(gameSceneName);
        gameUI = SceneManager.GetSceneByName(gameUIName);
    }
}
