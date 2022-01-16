using System.Collections;
using System.Collections.Generic;
using Overtail.Battle;
using Overtail.PlayerModule;
using UnityEngine;

public class tmp_PlayerDataLoader : MonoBehaviour
{
    public BattleResult data;

    void Awake()
    {
        var stats = gameObject.GetComponent<StatComponent>();
        stats.Level = data.Level;
        stats.Experience = data.Experience;
        stats.StatusEffects = data.StatusEffects;
        stats.HP = data.Hp;

        stats.RecalcStats();

        var rb = gameObject.GetComponent<Rigidbody2D>();
        var pos = rb.position;

        pos.x = data.Position.x;
        pos.y = data.Position.y;

        rb.MovePosition(pos);
    }
}
