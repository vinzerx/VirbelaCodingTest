using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using VirbelaTest;

public class PlayTests
{
    private Manager managerObj;
    private Player playerObj;
    private List<Item> createdItems;

    [SetUp]
    public void Setup()
    {
        var newManager = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Manager"));
        managerObj = newManager.GetComponent<Manager>();
        
        var newPlayer = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Player"));
        newPlayer.transform.position = Vector3.zero;
        playerObj = newPlayer.GetComponent<Player>();
    }
    
    [TearDown]
    public void Teardown()
    {
        Object.Destroy(managerObj.gameObject);
        Object.Destroy(playerObj.gameObject);
    }

    
    [UnityTest]
    public IEnumerator CanSpawnItems()
    {
        var newItem = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        yield return new WaitForEndOfFrame();
        
        Assert.NotNull(newItem);
        Object.Destroy(newItem.gameObject);
    }
    
    [UnityTest]
    public IEnumerator CanSpawnBots()
    {
        var newItem = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        yield return new WaitForEndOfFrame();
        
        Assert.NotNull(newItem);
        Object.Destroy(newItem.gameObject);
    }
    
    [UnityTest]
    public IEnumerator SetItemColorsCorrectly()
    {
        var newItem1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem1.transform.position = new Vector3(5, 0, 0);

        var newItem2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem2.transform.position = new Vector3(3, 0, 0);

        var newItem3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem3.transform.position = new Vector3(6, 0, 0);

        yield return new WaitForSeconds(0.02f);
        
        Assert.AreEqual(managerObj.ItemDefaultColor, newItem1.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.ItemClosestColor, newItem2.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.ItemDefaultColor, newItem3.GetComponentInChildren<Renderer>().material.color);
        
        Object.Destroy(newItem1.gameObject);
        Object.Destroy(newItem2.gameObject);
        Object.Destroy(newItem3.gameObject);
    }
    
    [UnityTest]
    public IEnumerator SetBotColorsCorrectly()
    {
        var newBot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot1.transform.position = new Vector3(5, 0, 0);

        var newBot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot2.transform.position = new Vector3(3, 0, 0);

        var newBot3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot3.transform.position = new Vector3(6, 0, 0);

        yield return new WaitForSeconds(0.02f);

        Assert.AreEqual(managerObj.BotDefaultColor, newBot1.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.BotClosestColor, newBot2.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.BotDefaultColor, newBot3.GetComponentInChildren<Renderer>().material.color);
        
        Object.Destroy(newBot1.gameObject);
        Object.Destroy(newBot2.gameObject);
        Object.Destroy(newBot3.gameObject);
    }
    
    [UnityTest]
    public IEnumerator SetItemColorsCorrectlyOnPlayerMove()
    {
        var newItem1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem1.transform.position = new Vector3(5, 0, 0);

        var newItem2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem2.transform.position = new Vector3(3, 0, 0);

        var newItem3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Item"));
        newItem3.transform.position = new Vector3(6, 0, 0);

        yield return new WaitForSeconds(0.02f);

        playerObj.transform.position = new Vector3(7, 0, 0);
        
        yield return new WaitForSeconds(0.02f);
        
        Assert.AreEqual(managerObj.ItemDefaultColor, newItem1.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.ItemDefaultColor, newItem2.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.ItemClosestColor, newItem3.GetComponentInChildren<Renderer>().material.color);
        
        Object.Destroy(newItem1.gameObject);
        Object.Destroy(newItem2.gameObject);
        Object.Destroy(newItem3.gameObject);
    }
    
    [UnityTest]
    public IEnumerator SetBotColorsCorrectlyOnPlayerMove()
    {
        var newBot1 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot1.transform.position = new Vector3(5, 0, 0);

        var newBot2 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot2.transform.position = new Vector3(3, 0, 0);

        var newBot3 = MonoBehaviour.Instantiate(Resources.Load<GameObject>("Prefabs/Bot"));
        newBot3.transform.position = new Vector3(6, 0, 0);

        yield return new WaitForSeconds(0.02f);
        
        playerObj.transform.position = new Vector3(7, 0, 0);
        
        yield return new WaitForSeconds(0.02f);

        Assert.AreEqual(managerObj.BotDefaultColor, newBot1.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.BotDefaultColor, newBot2.GetComponentInChildren<Renderer>().material.color);
        Assert.AreEqual(managerObj.BotClosestColor, newBot3.GetComponentInChildren<Renderer>().material.color);
        
        Object.Destroy(newBot1.gameObject);
        Object.Destroy(newBot2.gameObject);
        Object.Destroy(newBot3.gameObject);
    }
}
