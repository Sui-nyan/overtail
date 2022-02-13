using System.Collections;
using NUnit.Framework;
using Overtail.NPCs;
using UnityEngine;
using UnityEngine.TestTools;

public class NPCWalkingTest
{
    private GameObject npc;

    float x = 2;
    float width = 4;
    float y = 2;
    float height = 4;

    [SetUp]
    public void Setup()
    {
        if (npc != null)
        {
            GameObject.Destroy(npc);
        }

        npc = new GameObject();
        var rb = npc.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        rb.bodyType = RigidbodyType2D.Kinematic;
        var col = npc.AddComponent<BoxCollider2D>();
        col.isTrigger = true;

    }

    [TearDown]
    public void CleanUp()
    {
        GameObject.Destroy(npc);
    }


    [UnityTest]
    public IEnumerator StayInBox()
    {
        BoundNPCMovement npcMove = npc.AddComponent<BoundNPCMovement>();
        npcMove.moveSpeed = 3.5f;
        npcMove.boundary = CreateBoundingBox(x, y, width, height);

        Time.timeScale = 100f;
        yield return new WaitForSeconds(50f);

        Debug.Log(npc.transform.position);
        Assert.AreEqual(true, IsInsideBox(npc.transform.position));
    }

    [UnityTest]
    public IEnumerator OutsideOfBox()
    {
        BoundNPCMovement npcMove = npc.AddComponent<BoundNPCMovement>();
        npcMove.moveSpeed = 10f;
        npcMove.boundary = CreateBoundingBox(x, y, width*30, height*30);

        Time.timeScale = 100f;
        yield return new WaitForSeconds(50f);

        Assert.AreEqual(false, IsInsideBox(npc.transform.position));
    }

    private bool IsInsideBox(Vector3 pos)
    {
        bool insideX = npc.transform.position.x > x-width/2 && npc.transform.position.x < x + width/2;
        bool insideY = npc.transform.position.y > y-height/2 && npc.transform.position.y < y + height/2;
        return insideX && insideY;
    }

    public Collider2D CreateBoundingBox(float x, float y, float width, float height)
    {
        var boxObj = new GameObject();
        boxObj.transform.localScale = new Vector3(width, height, 1);
        boxObj.transform.position = new Vector3(x, y, 0);
        return boxObj.AddComponent<BoxCollider2D>();
    }

}