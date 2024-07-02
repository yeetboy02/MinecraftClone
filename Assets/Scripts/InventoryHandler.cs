using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryHandler : MonoBehaviour {

    #region Parameters

    private int currSelectedBlock = 0;

    #endregion

    #region Setup

    void Start () {
        ApplySelection();
    }

    #endregion

    #region Methods

    public void SelectBlock(int blockType) {
        // SET CURRENT SELECTED BLOCK
        currSelectedBlock = blockType;

        ApplySelection();
    }

    private void ApplySelection() {
        // DESELECT ALL BLOCKS
        foreach (Transform child in transform) {
            child.gameObject.GetComponent<Outline>().enabled = false;
        }

        // SELECT BLOCK
        transform.GetChild(currSelectedBlock).gameObject.GetComponent<Outline>().enabled = true;
    }

    #endregion

}
