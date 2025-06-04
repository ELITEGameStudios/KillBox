using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    public int selectedDifficulty;
    public static MainMenuManager instance {get; private set;}



    [SerializeField] private MenuUI settingsMenu, mainMenu, splashScreen, pauseMenu, leaderboardMenu, myStuffMenu, previewMenu, volumeMenu, qualityMenu, keybindMenu, playPortalMenu;
    [SerializeField] private List<MenuUI> menuList;
    [SerializeField] private Graphic[] splashImages;
    [SerializeField] private bool inSplash, paused, switchingMenus;

    [SerializeField] private float splashScreenFadeTime, splashScreenHoldTimeMin;
    [SerializeField] private List<Scene> scenes;
    [SerializeField] private Scene activeScene; 

    [SerializeField] private GameObject menusComponents; 
    [SerializeField] private GameObject[] menuCameras; 
    [SerializeField] private MenuState state, lastState; 
    private bool InMenu {get { return state != MenuState.NONE ;} }


    public enum MenuState{
        MAIN,
        PLAY_PORTAL,
        SETTINGS,
        VOLUME,
        QUALITY,
        KEYBINDS,
        MYSTUFF,
        LEADERBOARDS,
        EQUIPMENT_PREVIEW,
        PAUSED,
        SPLASH,
        NONE
    }



    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null) {instance = this;}
        else if(instance != this) {Destroy(this);}

        state = MenuState.NONE;
        
        DontDestroyOnLoad(gameObject);
        menuList = new(){mainMenu, playPortalMenu, settingsMenu, volumeMenu, qualityMenu, keybindMenu, myStuffMenu, leaderboardMenu, previewMenu, pauseMenu, splashScreen};
        foreach (MenuUI menu in menuList) {menu.Initialize();}
    }

    void Start(){

        for (int i = 0; i < SceneManager.sceneCount; i++) { scenes.Add(SceneManager.GetSceneAt(i)); }
        activeScene = SceneManager.GetActiveScene();
        OpenMenuViaState(MenuState.SPLASH);
        // activeScene.isSubScene = true;
    }

    void Update(){
        // if(CustomKeybinds.main.){
        //     OpenMenuViaState( InMenu ? MenuState.NONE : MenuState.PAUSED);
        // }

        // if(Time.timeScale != 1.0f || Time.timeScale != 0.0f){
        //     Time.timeScale = Mathf.Clamp(
        //         Time.timeScale + Time.unscaledDeltaTime * (InMenu ? -3 : 3),
        //         0.0f,
        //         1.0f
        //     );
        // }
    }

    public void OpenMenu(){ OpenMenuViaState(MenuState.MAIN, crossFade: true); }
    public void OpenSettings(){ OpenMenuViaState(MenuState.SETTINGS, crossFade: true); }
    public void OpenLeaderboards(){ OpenMenuViaState(MenuState.LEADERBOARDS); }
    public void OpenPlayPortal(){ OpenMenuViaState(MenuState.PLAY_PORTAL, crossFade: true); }
    public void OpenMyStuff(){ OpenMenuViaState(MenuState.MYSTUFF); }
    public void Return(){ OpenMenuViaState(lastState); }
    public void Resume(){ OpenMenuViaState(MenuState.NONE); }

    public void TriggerGameStart(bool freeplay = false){ KillBox.StartNewGame(selectedDifficulty, freeplay); }
    public void OnGameSceneLoad(){ 
        OpenMenuViaState(MenuState.NONE, false); 
        InstantSwitch();
        menusComponents.SetActive(false);
        foreach(GameObject cam in menuCameras){cam.SetActive(false);}
    }

    public void End(){
        instance = null;
        Destroy(gameObject);
    }
    
    
    public void OpenMenuViaState(MenuState newState, bool doCoroutine = true, bool crossFade = false, float customFadeOut = -1){
        if(switchingMenus) {return;}
        lastState = state;
        state = newState;
        if(doCoroutine){
            StartCoroutine(SwitchMenuCoroutine(crossFade, customFadeOut));
        }

        // try{
        //     for (int i = 0; i < menuList.Count; i++) { menuList[i].SetActive((int)newState == i); }
        // }
        // catch{
        //     // must be a none state
        //     Debug.Log("No longer in a menu");
        // }
        
    }

    void InstantSwitch(){

        MenuUI newMenu = null;
        MenuUI oldMenu = null;

        for (int i = 0; i < menuList.Count; i++){
            if(i == (int)state){ newMenu = menuList[i]; }
            else if(i == (int)lastState){ oldMenu = menuList[i]; }
        }  

        if(oldMenu != null){
            oldMenu.SwitchActive(false, 0);
            oldMenu.gameObject.SetActive(false);
        }
        if(newMenu != null){
            newMenu.SwitchActive(true, 0);
            newMenu.gameObject.SetActive(true);
        }
        switchingMenus = false;
    }

    IEnumerator SwitchMenuCoroutine(bool crossFade, float customFadeOut = -1){
        bool hasCustomFadeOut = customFadeOut >= 0;

        float fadeOut = 0;
        float fadeIn = 0;

        switchingMenus = true;

        MenuUI newMenu = null;
        MenuUI oldMenu = null;

        for (int i = 0; i < menuList.Count; i++){
            if(i == (int)state){ 
                newMenu = menuList[i];
                fadeIn = newMenu.fadeTime;
            }
            else if(i == (int)lastState){ 
                oldMenu = menuList[i]; 
                fadeOut = hasCustomFadeOut ? customFadeOut : oldMenu.fadeTime;
            }
        }  


        if(oldMenu != null){
            if(crossFade && newMenu != null){
                
                newMenu.gameObject.SetActive(true);
                
                newMenu.SwitchActive(true);
                oldMenu.SwitchActive(false, fadeOut);
                
                yield return new WaitForSecondsRealtime(fadeOut > fadeIn ? 
                    fadeOut : 
                    fadeIn
                );
                
                oldMenu.gameObject.SetActive(false);
                switchingMenus = false;
                yield break;
            }

            oldMenu.SwitchActive(false, fadeOut);
            yield return new WaitForSecondsRealtime(fadeOut);
            oldMenu.gameObject.SetActive(false);

        }
        if(newMenu != null){
            newMenu.SwitchActive(true);
            newMenu.gameObject.SetActive(true);
            yield return new WaitForSecondsRealtime(fadeIn);
        }
        switchingMenus = false;
    }

    public void SelectDifficulty(int difficulty){
        selectedDifficulty = difficulty;
    }

    // IEnumerator SwitchToMainScene(){
    //     AsyncOperation operation = SceneManager.LoadSceneAsync(1);
    //     while (!operation.isDone){
    //         yield return null;
    //     }
    // }

    // IEnumerator SplashScreenCoroutine(){
    //     Debug.Log("Started");
    //     yield return new WaitForSecondsRealtime(1);
        
    //     OpenMenuViaState(MenuState.SPLASH, false);
    //     yield return StartCoroutine(SwitchMenuCoroutine(false));

    //     Debug.Log("Finished first phase");

    //     yield return StartCoroutine(SwitchToMainScene()); // Load main sccene in background
    //     yield return new WaitForSecondsRealtime(splashScreenHoldTimeMin);
        
    //     Debug.Log("Loaded scene");
    //     OpenMenuViaState(MenuState.NONE);
    //     yield return StartCoroutine(SwitchMenuCoroutine(false, 2));
    //     yield return new WaitForSecondsRealtime(3);
    //     OpenMenuViaState(MenuState.MAIN);
        
    //     Debug.Log("Done");
    // }
}
