using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VirbelaTest
{
    /// <summary>
    /// Main class that handles distance detection between player and other objects.
    /// </summary>
    public class Manager : MonoBehaviour
    {
        [SerializeField] private Color itemClosestColor;
        [SerializeField] private Color itemDefaultColor;
        [SerializeField] private Color botClosestColor;
        [SerializeField] private Color botDefaultColor;

        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private GameObject itemPrefab;
        [SerializeField] private GameObject botPrefab;
        [SerializeField] private string fileName = "save.dat";
        [SerializeField] private bool loadFromFileAtStart;
        
        private List<Item> itemListRef;
        private List<Bot> botListRef;
        private Player playerRef;

        /// <summary>
        /// Accessor for <c>itemClosestColor</c>.
        /// </summary>
        public Color ItemClosestColor => itemClosestColor;
        
        /// <summary>
        /// Accessor for <c>itemDefaultColor</c>.
        /// </summary>
        public Color ItemDefaultColor => itemDefaultColor;
        
        /// <summary>
        /// Accessor for <c>botClosestColor</c>.
        /// </summary>
        public Color BotClosestColor => botClosestColor;
       
        /// <summary>
        /// Accessor for <c>botDefaultColor</c>.
        /// </summary>
        public Color BotDefaultColor => botDefaultColor;
        
        /// <summary>
        /// Registers the player to the manager class.
        /// </summary>
        /// <param name="newPlayer">Player instance being registered.</param>
        public void RegisterPlayer(Player newPlayer)
        {
            playerRef = newPlayer;
        }

        /// <summary>
        /// Registers an Item instance to the manager class.
        /// </summary>
        /// <param name="newItem">Item instance being registered.</param>
        public void RegisterItem(Item newItem)
        {
            itemListRef.Add(newItem);

            FindClosestItemToPlayer();
        }
        
        /// <summary>
        /// Registers a Bot instance to the manager class.
        /// </summary>
        /// <param name="newBot">Bot instance being registered.</param>
        public void RegisterBot(Bot newBot)
        {
            botListRef.Add(newBot);

            FindClosestBotToPlayer();
        }

        /// <summary>
        /// Removes a Movable instance from the manager's managed objects.
        /// </summary>
        /// <param name="deletedMovable">Movable instance being removed.</param>
        public void UnregisterMovable(MovableObject deletedMovable)
        {
            if (deletedMovable is Item)
            {
                itemListRef.Remove((Item)deletedMovable);
                
                //ensure no errors thrown on exit
                if (playerRef == null)
                {
                    return;
                }
                
                FindClosestItemToPlayer();
            }
            else if (deletedMovable is Bot)
            {
                botListRef.Remove((Bot)deletedMovable);
                
                //ensure no errors thrown on exit
                if (playerRef == null)
                {
                    return;
                }
                
                FindClosestBotToPlayer();
            }
        }

        /// <summary>
        /// Invoked to notify manager that a Movable has moved.
        /// </summary>
        /// <param name="moveObj">Movable instance that moved.</param>
        public void ReportMovableMoved(MovableObject moveObj)
        {
            if (moveObj is Player)
            {
                ReportPlayerMoved();
            }
            else if (moveObj is Item)
            {
                ReportItemMoved((Item)moveObj);
            }
            else if (moveObj is Bot)
            {
                ReportBotMoved((Bot)moveObj);
            }
        }
        
        /// <summary>
        /// Called when a Bot moves and reacts to said movement as needed.
        /// </summary>
        /// <param name="reporter">Bot instance that moved.</param>
        private void ReportBotMoved(Bot reporter)
        {
            FindClosestBotToPlayer();
        }

        /// <summary>
        /// Called when an Item moves and reacts to said movement as needed.
        /// </summary>
        /// <param name="reporter">Item instance that moved.</param>
        private void ReportItemMoved(Item reporter)
        {
            FindClosestItemToPlayer();
        }

        /// <summary>
        /// Called when the player instance moves and reacts to said movement as needed.
        /// </summary>
        private void ReportPlayerMoved()
        {
            FindClosestItemToPlayer();
            FindClosestBotToPlayer();
        }

        /// <summary>
        /// Uses the manager's internal list to find the closest Item to the player and colors it as needed.
        /// </summary>
        /// <seealso cref="UpdateItemColors">UpdateItemColors</seealso>
        private void FindClosestItemToPlayer()
        {
            var sortedArray = itemListRef.OrderBy(t=>(t.transform.position - playerRef.transform.position).
                    sqrMagnitude).ToArray();

            UpdateItemColors(sortedArray);
        }
        
        /// <summary>
        /// Uses the manager's internal list to find the closest Bot to the player and colors it as needed.
        /// </summary>
        /// <seealso cref="UpdateBotColors">UpdateBotColors</seealso>
        private void FindClosestBotToPlayer()
        {
            var sortedArray = botListRef.OrderBy(t=>(t.transform.position - playerRef.transform.position).
                    sqrMagnitude).ToArray();

            UpdateBotColors(sortedArray);
        }

        /// <summary>
        /// Takes an Item list sorted by distance closest to player and colors the first item on the list with
        /// <c>itemDefaultColor</c> and everything else with <c>itemDefaultColor.</c>
        /// </summary>
        /// <param name="itemArray">Sorted array of Item instances.</param>
        private void UpdateItemColors(Item[] itemArray)
        {
            for (int i=0; i<itemArray.Length; i++)
            {
                if (i == 0)
                {
                    itemArray[i].SetColor(itemClosestColor);
                }
                else
                {
                    itemArray[i].SetColor(itemDefaultColor);
                }
            }
        }
        
        /// <summary>
        /// Takes a Bot list sorted by distance closest to player and colors the first item on the list with
        /// <c>botDefaultColor</c> and everything else with <c>botDefaultColor.</c>
        /// </summary>
        /// <param name="itemArray">Sorted array of Bot instances.</param>
        private void UpdateBotColors(Bot[] botArray)
        {
            for (int i=0; i<botArray.Length; i++)
            {
                if (i == 0)
                {
                    botArray[i].SetColor(botClosestColor);
                }
                else
                {
                    botArray[i].SetColor(botDefaultColor);
                }
            }
        }

        /// <summary>
        /// Clears the scene of Item and Bot objects.
        /// </summary>
        private void ClearCurrentObjects()
        {
            Destroy(playerRef.gameObject);
            
            foreach (var item in itemListRef)
            {
                Destroy(item.gameObject);
            }
            itemListRef.Clear();
            
            foreach (var bot in botListRef)
            {
                Destroy(bot.gameObject);
            }
            botListRef.Clear();
        }

        /// <summary>
        /// Saves Player, Item, and Bot positions to a file.
        /// </summary>
        private void SaveToFile()
        {
            var saveStruct = new SaveData();

            saveStruct.playerPosition = playerRef.transform.position;
                
            saveStruct.itemPositions = new List<Vector3>();
            foreach (var itemObj in itemListRef)
            {
                saveStruct.itemPositions.Add(itemObj.transform.position);
            }
                
            saveStruct.botPositions = new List<Vector3>();
            foreach (var botObj in botListRef)
            {
                saveStruct.botPositions.Add(botObj.transform.position);
            }
                
            var destination = Application.persistentDataPath + "/" + fileName;
            var saveString = JsonUtility.ToJson(saveStruct);
                
            var writer = new StreamWriter(destination);
            writer.WriteLine(saveString);

            writer.Close();
            
            Debug.Log($"File saved. [{destination}]");
        }

        /// <summary>
        /// Loads Player, Item, and Bot positions from a file located in the app's persistent data path.
        /// </summary>
        private void LoadFromFile()
        {
            var destination = Application.persistentDataPath + "/" + fileName;

            var reader = new StreamReader(destination);
            var data = reader.ReadToEnd();
            
            var saveData = JsonUtility.FromJson<SaveData>(data);
            
            var playerObj = GameObject.Instantiate(playerPrefab);
            playerObj.name = "Player";
            playerObj.transform.position = saveData.playerPosition;
            
            var counter = 1;
            foreach (var itemPos in saveData.itemPositions)
            {
                var itemObj = GameObject.Instantiate(itemPrefab);
                itemObj.name = "Item" + counter;
                itemObj.transform.position = itemPos;
                counter++;
            }

            counter = 1;
            foreach (var botPos in saveData.botPositions)
            {
                var botObj = GameObject.Instantiate(botPrefab);
                botObj.name = "Bot" + counter;
                botObj.transform.position = botPos;
                counter++;
            }
            
            Debug.Log("Loading complete.");
        }

        /// <summary>
        /// Manager singleton instance.
        /// </summary>
        public static Manager Instance { get; private set; }
        
        private void Awake() 
        { 
            if (Instance != null && Instance != this) 
            { 
                Destroy(this); 
            } 
            else 
            { 
                Instance = this;
                itemListRef = new List<Item>();
                botListRef = new List<Bot>();
            } 
        }

        private void Start()
        {
            if (loadFromFileAtStart)
            {
                ClearCurrentObjects();
                LoadFromFile();
            }
        }

        private void Update()
        {
            //add bot
            if (Input.GetKeyUp(KeyCode.B))
            {
                var newBot = GameObject.Instantiate(botPrefab);
                newBot.name = "Bot" + botListRef.Count;
                newBot.transform.position = new Vector3(Random.Range(-5f, 5f), 
                    Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            }
            
            //add item
            if (Input.GetKeyUp(KeyCode.I))
            {
                var newItem = GameObject.Instantiate(itemPrefab);
                newItem.name = "Item" + itemListRef.Count;
                newItem.transform.position = new Vector3(Random.Range(-5f, 5f), 
                    Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            }
            
            //save to file
            if (Input.GetKeyUp(KeyCode.S))
            {
                SaveToFile();
            }
            
            //load from file
            if (Input.GetKeyUp(KeyCode.L))
            {
                if (File.Exists(Application.persistentDataPath + "/" + fileName))
                {
                    ClearCurrentObjects();
                    LoadFromFile();
                    Debug.Log("Loading file...");
                }
                else
                {
                    Debug.LogWarning("[Manager] No data file found on the system.");
                }
            }
        }
    }

    /// <summary>
    /// Struct used for saving data.
    /// </summary>
    [Serializable]
    public struct SaveData
    {
        public Vector3 playerPosition;
        public List<Vector3> itemPositions;
        public List<Vector3> botPositions;
    }
}
