using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;

public class MainMenuHandler : MonoBehaviour {

    #region Parameters

    [SerializeField] private GameObject startScreen;
    [SerializeField] private GameObject loadWorldScreen;
    [SerializeField] private GameObject createWorldScreen;

    [SerializeField] private TMP_Text worldNameErrorText;

    [SerializeField] private TMP_Text worldNameInputField;

    [SerializeField] private Button loadWorldButton;

    #endregion

    #region Variables

    private string currNewWorldName = "";

    private string currWorldNameError = "";

    Regex worldNameRegex = new Regex("[^a-zA-Z0-9]");

    #endregion

    #region Setup

    void Start() {
        // SET START SCREEN ACTIVE
        StartScreen();
    }

    #endregion

    #region MenuStates

    public enum MenuState {
        StartScreen,
        LoadWorld,
        CreateWorld
    }

    private MenuState currMenuState = MenuState.StartScreen;

    public void StartScreen() {
        // SET MENU STATE TO START SCREEN
        currMenuState = MenuState.StartScreen;

        // SET START SCREEN ACTIVE
        startScreen.SetActive(true);
        loadWorldScreen.SetActive(false);
        createWorldScreen.SetActive(false);
    }

    public void LoadWorldScreen() {
        // SET MENU STATE TO LOAD WORLD SCREEN
        currMenuState = MenuState.LoadWorld;

        // SET LOAD WORLD SCREEN ACTIVE
        startScreen.SetActive(false);
        loadWorldScreen.SetActive(true);
        createWorldScreen.SetActive(false);

        // DISPLAY WORLDS
        DisplayWorlds();
    }

    public void CreateWorldScreen() {
        // SET MENU STATE TO CREATE WORLD SCREEN
        currMenuState = MenuState.CreateWorld;

        // SET CREATE WORLD SCREEN ACTIVE
        startScreen.SetActive(false);
        loadWorldScreen.SetActive(false);
        createWorldScreen.SetActive(true);
    }

    #endregion

    #region LoadWorld

    private void DisplayWorlds() {
        // GET WORLD NAMES
        List<string> worldNames = WorldHandler.instance.GetWorldNames();

        // GET WORLD BUTTONS
        LoadWorldButton[] worldButtons = loadWorldScreen.GetComponentsInChildren<LoadWorldButton>();

        // CLEAR OUT OLD BUTTONS
        foreach(LoadWorldButton button in worldButtons) {
            Destroy(button.gameObject);
        }

        // CREATE NEW BUTTONS
        foreach(string worldName in worldNames) {
            // CREATE NEW BUTTON
            Button newButton = Instantiate(loadWorldButton, loadWorldScreen.transform.position, Quaternion.identity);
            newButton.transform.SetParent(loadWorldScreen.transform);

            // SET BUTTON WORLD NAME
            newButton.GetComponent<LoadWorldButton>().SetWorldName(worldName);
        }
    }

    #endregion

    #region CreateWorld

    public void OnInputValueChange(string value) {

        // SET CURRENT NEW WORLD NAME
        currNewWorldName = value;
    }
    public void OnCreateNewWorld() {
        // CHECK IF WORLD NAME IS VALID
        if (!CheckCurrWorldName()) return;

        // START GAME
        GameManager.instance.StartGame(true, currNewWorldName);
    }

    private bool CheckCurrWorldName() {
        // CHECK IF TOO SHORT
        if (currNewWorldName.Length < 1) {
            // SET ERROR TEXT
            currWorldNameError = "World name must be at least 1 character long";
        }
        // CHECK IF TOO LONG
        else if (currNewWorldName.Length > 24) {
            // SET ERROR TEXT
            currWorldNameError = "World name must be at most 20 characters long";
        }
        // CHECK IF WORLD NAME ALREADY EXISTS
        else if (WorldHandler.instance.GetWorldNames().Contains(currNewWorldName)) {
            // SET ERROR TEXT
            currWorldNameError = "World name already exists";
        }
        // CHECK IF CONTAINS INVALID CHARACTERS
        else if (worldNameRegex.IsMatch(currNewWorldName)) {
            // SET ERROR TEXT
            currWorldNameError = "World name can only contain letters and numbers";
        }
        else {
            // RESET ERROR TEXT
            currWorldNameError = "";
        }


        // SET ERROR TEXT
        worldNameErrorText.SetText(currWorldNameError);

        // RETURN IF ANY ERROR WAS FOUND
        return (currWorldNameError == "");
    }

    #endregion

}
