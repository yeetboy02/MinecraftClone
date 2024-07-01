using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    #region Singleton
    public static GameManager instance;

    void Start() {
        if (instance != null)
                return;
        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    #endregion

    #region StartGame

    public void StartGame(bool newWorld, string worldName) {
        StartCoroutine(StartGameCoroutine(newWorld, worldName));
    }

    private IEnumerator StartGameCoroutine(bool newWorld, string worldName) {
        // LOAD GAME SCENE
        var levelLoad = SceneManager.LoadSceneAsync("Game", LoadSceneMode.Single);

        // WAIT FOR SCENE TO FINISH LOADING
        while (!levelLoad.isDone) {
            yield return null;
        }

        // START GAME
        if (newWorld) {
            CreateNewWorld(worldName);
        } else {
            LoadWorld(worldName);
        }

        yield return null;
    }

    #endregion

    #region CreateNewWorld

    private void CreateNewWorld(string worldName) {
        // SET NEW WORLD NAME
        WorldHandler.instance.SetCurrWorldName(worldName);

        // SAVE NEW CREATED WORLD
        WorldHandler.instance.SaveWorld();
    }

    #endregion

    #region LoadWorld

    private void LoadWorld(string worldName) {
        // SET LOADED WORLD NAME
        WorldHandler.instance.SetCurrWorldName(worldName);

        // LOAD WORLD
        WorldHandler.instance.LoadWorld(worldName);
    }

    #endregion

    #region EndGame

    public void ExitGame() {
        // SAVE CURRENT WORLD
        WorldHandler.instance.SaveWorld();

        // EXIT GAME
        SceneManager.LoadScene("MainMenu");
    }

    public void EndGame() {
        // END GAME
        Application.Quit();
    }

    #endregion

}
