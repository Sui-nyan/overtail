using System.Collections;
using NUnit.Framework;
using Overtail.PlayerModule;
using UnityEngine;
using UnityEngine.TestTools;

public class PlayerMovementTest
{
    private Rigidbody2D rb;
    private PlayerMovement pm;

    [SetUp]
    public void Setup()
    {
        var gameObject = new GameObject();
        rb = gameObject.AddComponent<Rigidbody2D>();
        rb.gravityScale = 0;
        pm = gameObject.AddComponent<PlayerMovement>();
    }

    [TearDown]
    public void Cleanup()
    {
        rb = null;
        pm = null;
    }

    [UnityTest]
    public IEnumerator MoveRight()
    {
        pm.direction = new Vector2(1, 0);
        pm.Move();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(0, rb.position.y);
        Assert.Greater(rb.position.x, 0);
    }

    [UnityTest]
    public IEnumerator MoveLeft()
    {
        pm.direction = new Vector2(-1, 0);
        pm.Move();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(0, rb.position.y);
        Assert.Less(rb.position.x, 0);
    }

    [UnityTest]
    public IEnumerator MoveUp()
    {
        pm.direction = new Vector2(0, 1);
        pm.Move();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(0, rb.position.x);
        Assert.Greater(rb.position.y, 0);
    }

    [UnityTest]
    public IEnumerator MoveDown()
    {
        pm.direction = new Vector2(0, -1);
        pm.Move();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(0, rb.position.x);
        Assert.Less(rb.position.y, 0);
    }

    [UnityTest]
    public IEnumerator MoveDiagonal()
    {
        pm.direction = new Vector2(1, 1);
        pm.Move();

        yield return new WaitForSeconds(.1f);

        Assert.AreEqual(rb.position.y, rb.position.x);
        Assert.Greater(rb.position.y, 0);
    }
}
