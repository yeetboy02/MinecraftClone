using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WorldHandler : MonoBehaviour {

    #region Singleton

    public static WorldHandler instance;

    void Start() {
            if (instance != null)
                    return;
            instance = this;

            // INITIALIZE WORLD ARRAY
            currWorld = new GameObject[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];

            // TEMPORARY LOAD WORLD
            LoadWorld(worldName);
    }
    
    #endregion

    #region Parameters

    [SerializeField] private GameObject blockPrefab;

    private Vector3 worldSize = new Vector3(10, 10, 10);

    // TEMPORARY WORLD
    private string worldName = "World1";

    #endregion

    #region Variables

    GameObject[ , , ] currWorld;

    #endregion

    #region HandleJSON

    public string ReadJSON(FileInfo jsonFile) {
        // READ JSON FILE CONTENT AS A STRING
        return File.ReadAllText(jsonFile.FullName);
    }

    public void WriteJSON(string path, string json) {
        // WRITE JSON STRING TO FILE
        File.WriteAllText(path, json);
    }

    #endregion

    #region Methods

    public GameObject PlaceBlock(Vector3 position) {
        // CHECK IF POSITION IS WITHIN WORLD BOUNDS
        if (position.x < 0 || position.x >= worldSize.x || position.y < 0 || position.y >= worldSize.y || position.z < 0 || position.z >= worldSize.z)
            return null;

        // INSTANTIATE BLOCK
        GameObject currBlock = Instantiate(blockPrefab, position, Quaternion.identity);

        // ADD BLOCK TO WORLD BLOCK LIST
        currWorld[(int)position.x, (int)position.y, (int)position.z] = currBlock;

        // RETURN CURRENTLY PLACED BLOCK
        return currBlock;
    }

    public void BreakBlock(GameObject block) {
        // GET BLOCK POSITION
        Vector3 blockPosition = block.transform.position;

        // REMOVE BLOCK FROM WORLD BLOCK LIST
        currWorld[(int)blockPosition.x, (int)blockPosition.y, (int)blockPosition.z] = null;

        // DESTROY BLOCK
        Destroy(block);

        // TEMPORARY SAVE WORLD
        SaveWorld();
    }

    #endregion

    #region SaveWorld

    public void SaveWorld() {
        // CREATE NEW WORLD MODEL
        World world = new World();

        // CREATE SERIALIZEABLE BLOCK LIST
        world.blocks = CreateSerializedWorld(CreateWorldFromCurrWorld());

        // SERIALIZE WORLD MODEL
        string json = JsonUtility.ToJson(world);

        // SAVE WORLD TO FILE
        WriteJSON(Application.dataPath + "/Worlds/World1.json", json);
    }

    private string[ , , ] CreateWorldFromCurrWorld() {
        // CREATE NEW EMPTY WORLD ARRAY
        string[ , , ] world = new string[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];

        // FILL WORLD ARRAY WITH BLOCK STRINGS
        for (int x = 0; x < worldSize.x; x++) {
            for (int y = 0; y < worldSize.y; y++) {
                for (int z = 0; z < worldSize.z; z++) {
                    world[x, y, z] = currWorld[x, y, z] == null ? null : "Block";
                }
            }
        }

        return world;
    }

    #endregion

    #region LoadWorld

    public void LoadWorld(string worldName) {
        // READ WORLD FROM FILE
        string json = ReadJSON(new FileInfo(Application.dataPath + "/Worlds/" + worldName + ".json"));

        // DESERIALIZE WORLD MODEL
        World world = JsonUtility.FromJson<World>(json);

        // DESERIALIZE WORLD BLOCK LIST
        currWorld = DeserializeWorldList(world.blocks);
    }

    #endregion

    #region Serialization

    public List<Block> CreateSerializedWorld(string[ , , ] world) {
        // CLEAR SERIALIZED WORLD LIST
        List<Block> serializedWorld = new List<Block>();

        // CONVERT ARRAY OF GAMEOBJECTS TO LIST OF ALL BLOCKS
        for (int i = 0; i < currWorld.GetLength(0); i++) {
            for (int j = 0; j < currWorld.GetLength(1); j++) {
                for (int k = 0; k < currWorld.GetLength(2); k++) {
                    if (currWorld[i, j, k] != null) {
                        serializedWorld.Add(new Block(new Vector3(i, j, k), "Block"));
                    }
                }
            }
        }

        return serializedWorld;
    }

    public GameObject[ , , ] DeserializeWorldList(List<Block> serializedWorld) {
        // INITIALIZE WORLD ARRAY
        GameObject[ , , ] world = new GameObject[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];

        // CONVERT LIST OF BLOCKS TO ARRAY OF GAMEOBJECTS
        foreach (Block block in serializedWorld) {
            world[(int)block.position.x, (int)block.position.y, (int)block.position.z] = Instantiate(blockPrefab, block.position, Quaternion.identity);
        }

        return world;
    }

    #endregion

    #region WorldModels

    [System.Serializable]
    public class World {
        public List<Block> blocks;
    }

    [System.Serializable]
    public struct Block {
        public Vector3 position;
        public string block;

        public Block(Vector3 position, string block) {
            this.position = position;
            this.block = block;
        }
    }

    #endregion

}