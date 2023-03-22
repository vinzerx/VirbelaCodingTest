using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

namespace VirbelaTest
{
    //implemented as singleton to reduce need for finding objects at runtime
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

        public Color ItemClosestColor => itemClosestColor;
        public Color ItemDefaultColor => itemDefaultColor;
        public Color BotClosestColor => botClosestColor;
        public Color BotDefaultColor => botDefaultColor;
        
        public void RegisterPlayer(Player newPlayer)
        {
            playerRef = newPlayer;
        }

        public void RegisterItem(Item newItem)
        {
            itemListRef.Add(newItem);

            FindClosestItemToPlayer();
        }
        
        public void RegisterBot(Bot newBot)
        {
            botListRef.Add(newBot);

            FindClosestBotToPlayer();
        }

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
        
        private void ReportBotMoved(Bot reporter)
        {
            FindClosestBotToPlayer();
        }

        private void ReportItemMoved(Item reporter)
        {
            FindClosestItemToPlayer();
        }

        private void ReportPlayerMoved()
        {
            FindClosestItemToPlayer();
            FindClosestBotToPlayer();
        }

        private void FindClosestItemToPlayer()
        {
            var sortedArray = itemListRef.OrderBy(t=>(t.transform.position - playerRef.transform.position).
                    sqrMagnitude).ToArray();

            UpdateItemColors(sortedArray);
        }
        
        private void FindClosestBotToPlayer()
        {
            var sortedArray = botListRef.OrderBy(t=>(t.transform.position - playerRef.transform.position).
                    sqrMagnitude).ToArray();

            UpdateBotColors(sortedArray);
        }

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

    [Serializable]
    public struct SaveData
    {
        public Vector3 playerPosition;
        public List<Vector3> itemPositions;
        public List<Vector3> botPositions;
    }
}
