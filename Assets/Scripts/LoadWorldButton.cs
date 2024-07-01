using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadWorldButton : MonoBehaviour {

    #region Parameters

    private string worldName;

    #endregion

    #region Getters/Setters

    public void SetWorldName(string newWorldName) {
        // SET NEW WORLD NAME
        worldName = newWorldName;

        // SET WORLD NAME TO BUTTON
        transform.GetChild(0).GetComponent<TMPro.TMP_Text>().text = newWorldName;
    }

    #endregion

    #region LoadWorld

    public void LoadWorld() {
        // LOAD WORLD
        GameManager.instance.StartGame(false, worldName);
    }

    #endregion

}
