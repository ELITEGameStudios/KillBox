using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour
{
    public LvlPrefResetter prefResetter;
    public void SceneLoader(int SceneIndex)
    {
        SceneManager.LoadScene(SceneIndex);
    }

    public void AppQuitter()
    {
        prefResetter.ResetAll();
        Application.Quit();
        Debug.Log("Quit Successful!");
    }
}
