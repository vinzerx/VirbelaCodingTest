using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
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
        
        private Dictionary<Item, float> itemReference;
        private Dictionary<Bot, float> botReference;
        private Player playerRef;
        
        public void RegisterPlayer(Player newPlayer)
        {
            playerRef = newPlayer;
        }

        public void RegisterItem(Item newItem)
        {
            var distToPlayer = Vector3.Distance(playerRef.transform.position, newItem.transform.position);
            itemReference.Add(newItem, distToPlayer);

            FindClosestItemToPlayer();
        }
        
        public void RegisterBot(Bot newBot)
        {
            var distToPlayer = Vector3.Distance(playerRef.transform.position, newBot.transform.position);
            botReference.Add(newBot, distToPlayer);

            FindClosestBotToPlayer();
        }

        public void UnregisterMovable(MovableObject deletedMovable)
        {
            if (deletedMovable is Item)
            {
                itemReference.Remove((Item)deletedMovable);
                FindClosestItemToPlayer();
            }
            else if (deletedMovable is Bot)
            {
                botReference.Remove((Bot)deletedMovable);
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
            var distance = Vector3.Distance(playerRef.transform.position, reporter.transform.position);
            botReference[reporter] = distance;

            FindClosestBotToPlayer();
        }

        private void ReportItemMoved(Item reporter)
        {
            var distance = Vector3.Distance(playerRef.transform.position, reporter.transform.position);
            itemReference[reporter] = distance;

            FindClosestItemToPlayer();
        }

        private void ReportPlayerMoved()
        {
            var itemList = itemReference.Keys.ToList();
            for (int i=0; i<itemList.Count; i++)
            {
                var distance = Vector3.Distance(playerRef.transform.position, 
                    itemList[i].transform.position);
                itemReference[itemList[i]] = distance;
            }
            
            var botList = botReference.Keys.ToList();
            for (int i=0; i<botList.Count; i++)
            {
                var distance = Vector3.Distance(playerRef.transform.position, 
                    botList[i].transform.position);
                botReference[botList[i]] = distance;
            }

            FindClosestItemToPlayer();
            FindClosestBotToPlayer();
        }

        private void FindClosestItemToPlayer()
        {
            var sortedDict = from entry in itemReference 
                orderby entry.Value ascending select entry;
            var sortedList =sortedDict.ToList();

            UpdateItemColors(sortedList);
        }
        
        private void FindClosestBotToPlayer()
        {
            var sortedDict = from entry in botReference 
                orderby entry.Value ascending select entry;
            var sortedList =sortedDict.ToList();

            UpdateBotColors(sortedList);
        }

        private void UpdateItemColors(List<KeyValuePair<Item, float>> itemList)
        {
            for (int i=0; i<itemList.Count; i++)
            {
                if (i == 0)
                {
                    itemList[i].Key.SetColor(itemClosestColor);
                }
                else
                {
                    itemList[i].Key.SetColor(itemDefaultColor);
                }
            }
        }
        
        private void UpdateBotColors(List<KeyValuePair<Bot, float>> itemList)
        {
            for (int i=0; i<itemList.Count; i++)
            {
                if (i == 0)
                {
                    itemList[i].Key.SetColor(botClosestColor);
                }
                else
                {
                    itemList[i].Key.SetColor(botDefaultColor);
                }
            }
        }

        private void ClearCurrentObjects()
        {
            Destroy(playerRef.gameObject);
            
            var itemList = itemReference.Keys.ToList();
            foreach (var item in itemList)
            {
                Destroy(item.gameObject);
            }
            
            var botList = botReference.Keys.ToList();
            foreach (var bot in botList)
            {
                Destroy(bot.gameObject);
            }
        }

        private void SaveToFile()
        {
            var saveStruct = new SaveData();

            saveStruct.playerPosition = playerRef.transform.position;
                
            saveStruct.itemPositions = new List<Vector3>();
            foreach (var pair in itemReference)
            {
                saveStruct.itemPositions.Add(pair.Key.transform.position);
            }
                
            saveStruct.botPositions = new List<Vector3>();
            foreach (var pair in botReference)
            {
                saveStruct.botPositions.Add(pair.Key.transform.position);
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

            counter = 0;
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
                itemReference = new Dictionary<Item, float>();
                botReference = new Dictionary<Bot, float>();
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
                newBot.name = "Bot" + botReference.Count;
                newBot.transform.position = new Vector3(Random.Range(-5f, 5f), 
                    Random.Range(-5f, 5f), Random.Range(-5f, 5f));
            }
            
            //add item
            if (Input.GetKeyUp(KeyCode.I))
            {
                var newItem = GameObject.Instantiate(itemPrefab);
                newItem.name = "Item" + itemReference.Count;
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
