using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Linq;

public class WorldHandler : MonoBehaviour {

    #region Singleton

    public static WorldHandler instance;

    void Start() {
            if (instance != null)
                    return;
            instance = this;

            DontDestroyOnLoad(gameObject);

            // INITIALIZE WORLD ARRAY
            currWorld = new GameObject[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];
    }
    
    #endregion

    #region BlockTypes

    public enum BlockType {
        Stone,
        Dirt,
        Grass,
        Planks,
        Iron,
        Gold,
        Coal,
        Netherrack,
        Bricks,
        Air
    }

    #endregion

    #region Parameters

    [SerializeField] private GameObject blockPrefab;

    private Vector3 worldSize = new Vector3(50, 50, 50);

    private string worldName ;

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

    #region Getters/Setters

    public List<string> GetWorldNames() {
        // GET ALL WORLD FILE NAMES
        List<string> worldNames = new List<string>();

        // GET ALL FILES IN WORLD DIRECTORY
        DirectoryInfo dir = new DirectoryInfo(Application.dataPath + "/Worlds/");
        FileInfo[] info = dir.GetFiles("*.json");

        // ADD ALL FILE NAMES TO LIST WITHOUT FILE EXTENSION
        foreach (FileInfo f in info) {
            worldNames.Add(f.Name.Replace(".json", ""));
        }

        return worldNames;
    }

    public BlockType GetBlockTypeAtPosition(Vector3 position) {
        // CHECK IF POSITION IS WITHIN WORLD BOUNDS AND IF BLOCK EXISTS
        if (position.x < 0 || position.x >= worldSize.x || position.y < 0 || position.y >= worldSize.y || position.z < 0 || position.z >= worldSize.z || currWorld[(int)position.x, (int)position.y, (int)position.z] == null)
            return BlockType.Air;

        // RETURN TYPE OF BLOCK AT INPUT POSITION
       return currWorld[(int)position.x, (int)position.y, (int)position.z].GetComponent<Block>().GetBlockType();
    }

    #endregion

    #region Methods

    public void SetCurrWorldName(string worldName) {
        this.worldName = worldName;
    }

    public void PlaceBlock(Vector3 position, BlockType blockType) {
        // CHECK IF POSITION IS WITHIN WORLD BOUNDS
        if (position.x < 0 || position.x >= worldSize.x || position.y < 0 || position.y >= worldSize.y || position.z < 0 || position.z >= worldSize.z)
            return;

        // INSTANTIATE BLOCK
        GameObject currBlock = Instantiate(blockPrefab, position, Quaternion.identity);

        // SET BLOCK TYPE
        currBlock.GetComponent<Block>().SetBlockType(blockType);

        // ADD BLOCK TO WORLD BLOCK LIST
        currWorld[(int)position.x, (int)position.y, (int)position.z] = currBlock;
    }

    public void BreakBlock(GameObject block) {
        // GET BLOCK POSITION
        Vector3 blockPosition = block.transform.position;

        // REMOVE BLOCK FROM WORLD BLOCK LIST
        currWorld[(int)blockPosition.x, (int)blockPosition.y, (int)blockPosition.z] = null;

        // DESTROY BLOCK
        Destroy(block);
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
        WriteJSON(Application.dataPath + "/Worlds/" + worldName + ".json", json);
    }

    private string[ , , ] CreateWorldFromCurrWorld() {
        // CREATE NEW EMPTY WORLD ARRAY
        string[ , , ] world = new string[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];

        // FILL WORLD ARRAY WITH BLOCK STRINGS
        for (int x = 0; x < worldSize.x; x++) {
            for (int y = 0; y < worldSize.y; y++) {
                for (int z = 0; z < worldSize.z; z++) {
                    world[x, y, z] = currWorld[x, y, z] == null ? null : GetBlockTypeAtPosition(new Vector3(x, y, z)).ToString();
                }
            }
        }

        return world;
    }


    #endregion

    #region LoadWorld

    public void LoadWorld(string worldName) {
        // CLEAR CURRENT WORLD
        ClearWorld();

        // READ WORLD FROM FILE
        string json = ReadJSON(new FileInfo(Application.dataPath + "/Worlds/" + worldName + ".json"));

        // DESERIALIZE WORLD MODEL
        World world = JsonUtility.FromJson<World>(json);

        // DESERIALIZE WORLD BLOCK LIST
        DeserializeWorldList(world.blocks);
    }

    private void ClearWorld() {
        // CLEAR WORLD
        for (int i = 0; i < currWorld.GetLength(0); i++) {
            for (int j = 0; j < currWorld.GetLength(1); j++) {
                for (int k = 0; k < currWorld.GetLength(2); k++) {
                    if (currWorld[i, j, k] != null) {
                        Destroy(currWorld[i, j, k]);
                    }
                }
            }
        }

        // RESET WORLD ARRAY
        currWorld = new GameObject[(int)worldSize.x, (int)worldSize.y, (int)worldSize.z];
    }

    #endregion

    #region CreateNewWorld

    public void CreateNewWorldFromTemplate() {
        // PLACE 3 LAYERS OF STONE BLOCKS
        for (int i = 0; i < worldSize.x; i++) {
            for (int j = 0; j < 3; j++) {
                for (int k = 0; k < worldSize.z; k++) {
                    PlaceBlock(new Vector3(i, j, k), BlockType.Stone);
                }
            }
        }

        // PLACE 2 LAYERS OF DIRT BLOCKS
        for (int i = 0; i < worldSize.x; i++) {
            for (int j = 3; j < 5; j++) {
                for (int k = 0; k < worldSize.z; k++) {
                    PlaceBlock(new Vector3(i, j, k), BlockType.Dirt);
                }
            }
        }

        // PLACE 1 LAYER OF GRASS BLOCKS
        for (int i = 0; i < worldSize.x; i++) {
            for (int j = 5; j < 6; j++) {
                for (int k = 0; k < worldSize.z; k++) {
                    PlaceBlock(new Vector3(i, j, k), BlockType.Grass);
                }
            }
        }
    }

    #endregion

    #region Serialization

    public List<SerializedBlock> CreateSerializedWorld(string[ , , ] world) {
        // CLEAR SERIALIZED WORLD LIST
        List<SerializedBlock> serializedWorld = new List<SerializedBlock>();

        // CONSTRUCT LINQ QUERY
        IEnumerable<GameObject> blockQuery = from currBlocks in currWorld.Cast<GameObject>() where currBlocks != null select currBlocks;

        // CREATE LIST OF ALL BLOCKS USING LINQ QUERY
        foreach (GameObject block in blockQuery) {
            serializedWorld.Add(new SerializedBlock(block.transform.position, block.GetComponent<Block>().GetBlockType().ToString()));
        }

        return serializedWorld;
    }

    public void DeserializeWorldList(List<SerializedBlock> serializedWorld) {
        // CONVERT LIST OF BLOCKS TO ARRAY OF GAMEOBJECTS
        foreach (SerializedBlock block in serializedWorld) {
            PlaceBlock(block.position, (BlockType)Enum.Parse(typeof(BlockType), block.blockType));
        }
    }

    #endregion

    #region WorldModels

    [System.Serializable]
    public class World {
        public List<SerializedBlock> blocks;
    }

    [System.Serializable]
    public struct SerializedBlock {
        public Vector3 position;
        public string blockType;

        public SerializedBlock(Vector3 position, string blockType) {
            this.position = position;
            this.blockType = blockType;
        }
    }

    #endregion

}