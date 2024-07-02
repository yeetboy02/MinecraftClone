using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Block : MonoBehaviour {

    #region Parameters

    private WorldHandler.BlockType blockType;

    [SerializeField] private Material dirt, bricks, coal, iron, gold, netherrack, planks, stone, grass;

    #endregion

    #region Getters/Setters

    public WorldHandler.BlockType GetBlockType() {
        // GET BLOCK TYPE
        return blockType;
    }

    public void SetBlockType(WorldHandler.BlockType type) {
        // SET BLOCK TYPE
        blockType = type;

        // SET BLOCK COLOR
        switch (blockType) {
            case WorldHandler.BlockType.Stone:
                GetComponent<MeshRenderer>().material = stone;
                break;
            case WorldHandler.BlockType.Dirt:
                GetComponent<MeshRenderer>().material = dirt;
                break;
            case WorldHandler.BlockType.Grass:
                GetComponent<MeshRenderer>().material = grass;
                break;
            case WorldHandler.BlockType.Planks:
                GetComponent<MeshRenderer>().material = planks;
                break;
            case WorldHandler.BlockType.Iron:
                GetComponent<MeshRenderer>().material = iron;
                break;
            case WorldHandler.BlockType.Gold:
                GetComponent<MeshRenderer>().material = gold;
                break;
            case WorldHandler.BlockType.Coal:
                GetComponent<MeshRenderer>().material = coal;
                break;
            case WorldHandler.BlockType.Netherrack:
                GetComponent<MeshRenderer>().material = netherrack;
                break;
            case WorldHandler.BlockType.Bricks:
                GetComponent<MeshRenderer>().material = bricks;
                break;
        }
    }

    #endregion
}