using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace VirbelaTest
{
    //implemented as singleton to reduce need for finding objects at runtime
    public class Manager : MonoBehaviour
    {
        [SerializeField] private Color itemClosestColor;
        [SerializeField] private Color itemDefaultColor;
        private Dictionary<Item, float> itemReference;
        private Player playerRef;
        
        public void RegisterPlayer(Player newPlayer)
        {
            playerRef = newPlayer;
        }

        public void RegisterItem(Item newItem)
        {
            var distToPlayer = Vector3.Distance(playerRef.transform.position, newItem.transform.position);
            itemReference.Add(newItem, distToPlayer);

            FindClosestToPlayer();
        }

        public void UnregisterItem(Item deletedItem)
        {
            itemReference.Remove(deletedItem);
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
        }

        private void ReportItemMoved(Item reporter)
        {
            var distance = Vector3.Distance(playerRef.transform.position, reporter.transform.position);
            itemReference[reporter] = distance;

            FindClosestToPlayer();
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

            FindClosestToPlayer();
        }

        private void FindClosestToPlayer()
        {
            var sortedDict = from entry in itemReference 
                orderby entry.Value ascending select entry;
            var sortedList =sortedDict.ToList();

            UpdateColors(sortedList);
        }

        private void UpdateColors(List<KeyValuePair<Item, float>> itemList)
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
            } 
        }
    }
}
