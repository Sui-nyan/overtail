using System.Collections;
using NUnit.Framework;
using Overtail.Dialogue;
using Overtail.Items;
using Overtail.PlayerModule;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;

public class InteractableTest
{
    // Check interactable in range
    private Interactor actor;
    private IInteractable interactable;

    [SetUp]
    public void Setup()
    {
        actor = null;
        interactable = null;
        var go = new GameObject();
        actor = go.AddComponent<Interactor>();
    }

    [TearDown]
    public void CleanUp()
    {
        GameObject.Destroy(actor);
        GameObject.Destroy((MonoBehaviour)interactable);
    }

    [UnityTest]
    public IEnumerator ItemInRange()
    {
        var go = new GameObject();
        Lootable loot = go.AddComponent<Lootable>();
        loot.stack = new ItemStack(ItemDatabase.GetFromId("overtail:felix"), 1);

        yield return new WaitForSeconds(.1f);

        interactable = loot;
        Assert.AreEqual(true, actor.CanAct(), "Lootable not found");
    }

    [UnityTest]
    public IEnumerator NPCInRange()
    {
        var go = new GameObject();
        var npc = go.AddComponent<DialogueTrigger>();
        var collider = go.AddComponent<CircleCollider2D>();
        collider.isTrigger = true;

        yield return new WaitForSeconds(.1f);

        interactable = npc;
        Assert.AreEqual(true, actor.CanAct(), "NPC was not found");
    }


    [UnityTest]
    public IEnumerator NothingInRange()
    {

        yield return new WaitForSeconds(.1f);
        Assert.AreEqual(false, actor.CanAct(), "Something interactable was found in range");
    }

    [UnityTest]
    public IEnumerator InteractableDissappears()
    {
        var go = new GameObject();
        Lootable loot = go.AddComponent<Lootable>();
        loot.stack = new ItemStack(ItemDatabase.GetFromId("overtail:felix"), 1);
        interactable = loot;

        yield return null;

        loot.SELFDESTROY();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(false, actor.CanAct(), "Lootable did not dissappear");
    }

}
