using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameMenu : MonoBehaviour {

    #region Parameters

    [SerializeField] private GameObject inGameUI;

    [SerializeField] private PlayerInput playerInput;

    [SerializeField] private PlayerInput cameraInput;

    #endregion
    
    #region Variables

    private bool isMenuOpen = false;

    #endregion

    #region Setup

    void Start() {
        // HIDE CURSOR AT THE START OF THE GAME
        Cursor.visible = false;

        // HIDE MENU AT THE START OF THE GAME
        isMenuOpen = false;
    }

    #endregion


    #region Menu

    void Update() {
        // TOGGLE MENU
        gameObject.transform.GetChild(0).gameObject.SetActive(isMenuOpen);

        // SHOW/HIDE IN-GAME UI
        inGameUI.SetActive(!isMenuOpen);

        // DISABLE PLAYER INPUT
        playerInput.enabled = !isMenuOpen;

        // DISABLE CAMERA INPUT
        cameraInput.enabled = !isMenuOpen;
    }

    public void OnToggleMenu() {
        // TOGGLE MENU
        isMenuOpen = !isMenuOpen;

        // SHOW/HIDE CURSOR
        Cursor.visible = isMenuOpen;
    }

    #endregion

}
