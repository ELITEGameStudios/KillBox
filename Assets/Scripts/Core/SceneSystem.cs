using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSystem : MonoBehaviour
{
    [SerializeField] private List<Scene> scenes;
    [SerializeField] private Scene activeScene; 
    [SerializeField] private string flexibleLoadingSceneName, gameSceneName, gameUIName, menusSceneName; 
    [SerializeField] private int flexibleTransitionTime = 2; 
    public static SceneSystem Instance { get; private set; }
    [SerializeField] private string[] mapSceneNames; 
    [SerializeField] private Scene loadingScene;
    [SerializeField] private Scene menusScene;
    [SerializeField] private Scene gameScene;
    [SerializeField] private Scene gameUI;
    [SerializeField] private Scene currentMapScene;
    void Awake(){
        if(Instance == null) {Instance = this;}
        else if(Instance != this) {Destroy(this);}
        
        DontDestroyOnLoad(gameObject);       
    }

    void UpdateSceneData(){

        activeScene = SceneManager.GetActiveScene();
        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }

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
        yield return StartCoroutine(LoadAdditiveCoroutine(flexibleLoadingSceneName));
        loadingScene = SceneManager.GetSceneByName(flexibleLoadingSceneName);
        menusScene = SceneManager.GetSceneByName(menusSceneName);

        yield return new WaitForSecondsRealtime(flexibleTransitionTime);
        
        yield return StartCoroutine(LoadAdditiveCoroutine(gameSceneName)); 
        yield return StartCoroutine(LoadAdditiveCoroutine(gameUIName)); 
        yield return StartCoroutine(LoadAdditiveCoroutine(mapSceneNames[0])); 

        currentMapScene = SceneManager.GetSceneByName(mapSceneNames[0]);
        gameScene = SceneManager.GetSceneByName(gameSceneName);
        gameUI = SceneManager.GetSceneByName(gameUIName);
    }
}
