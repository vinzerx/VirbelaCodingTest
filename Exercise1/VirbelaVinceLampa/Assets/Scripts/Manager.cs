using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace VirbelaTest
{
    //implemented as singleton to reduce need for finding objects at runtime
    public class Manager : MonoBehaviour
    {
        [SerializeField] private Color itemClosestColor;
        [SerializeField] private Color itemDefaultColor;
        [SerializeField] private Color botClosestColor;
        [SerializeField] private Color botDefaultColor;
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
    }
}
