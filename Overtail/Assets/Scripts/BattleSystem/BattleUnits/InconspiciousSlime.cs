using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Battle
{
    public class InconspiciousSlime : EnemyUnit
    {
        bool finalForm = false;
        List<string> reactions = new List<string>();
        int index = 0;

        internal override IEnumerator OnGreeting(BattleSystem system)
        {
            system.GUI.QueueMessage("Could you please leave me alone?");
            system.GUI.QueueMessage("I don't want any trouble.");

            // <Insert qq animation>

            yield break;
        }

        internal override IEnumerator GetAttacked(BattleSystem system)
        {
            if (finalForm)
            {
                system.GUI.QueueMessage($".");
                system.GUI.QueueMessage($"..");
                system.GUI.QueueMessage($"...");
                system.GUI.QueueMessage($"{Name} is unfazed by {system.Player.Name}'s attack.");
                yield break;
            }

            var dmg = Math.Max(system.Player.Attack - this.Defense, 0);
            HP = Mathf.Clamp(HP - dmg, (int)(0.3*MaxHP), MaxHP); // Won't drop below threshold
            yield return new WaitForSeconds(1f);

            if(HP < MaxHP * 0.4)
            {
                yield return StartCoroutine(GoSuperSaiyan(system));
                yield break;
            }

            // Text Reactions

            reactions.Add("Please let me go.");
            reactions.Add("I'll get angry!");
            reactions.Add("I'm warning you!");

            var r = UnityEngine.Random.value * reactions.Count;

            system.GUI.QueueMessage(reactions[(int)r]);
        }

        private IEnumerator GoSuperSaiyan(BattleSystem system)
        {
            finalForm = true;
            system.GUI.QueueMessage("#!?");
            yield return new WaitUntil(() => system.IsIdle);
            yield return new WaitForSeconds(1f);



            yield return system.StartCoroutine(Transform(system));
            // TODO Change sprite

            maxHp = 999;
            yield return null;
            HP = MaxHP;
            attack = 999;
            level = 99;
            name = name.Replace("Small", "Small(?)").Replace("small", "small(?)");

            system.GUI.QueueMessage("THAT'S IT, YOU LITTLE SH*T");

            system.GUI.UpdateHud();
        }

        private IEnumerator Transform(BattleSystem system)
        {
            IEnumerator SizeUp(float target, float smoothTime)
            {
                Vector3 scale = this.gameObject.transform.localScale;
                float timeElapsed = 0f;

                while (scale.x != target)
                {
                    timeElapsed += Time.deltaTime;
                    scale.y = Mathf.SmoothStep(1, target*1.5f, timeElapsed / smoothTime);
                    scale.x = Mathf.SmoothStep(1, target, timeElapsed / smoothTime);
                    this.gameObject.transform.localScale = scale;

                    var pos = transform.localPosition;
                    pos.y += scale.y/ 2;
                    transform.localPosition = pos;
                    yield return null;
                }
            }

            IEnumerator Shake(float time)
            {
                Vector3 v = this.gameObject.transform.localPosition;
                float timeElapsed = 0;

                while(timeElapsed < time)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;
                    Func<float> rnd = () => (float)(UnityEngine.Random.value * 0.1 - 0.2) * (UnityEngine.Random.value > 0.5 ? 1 : -1);
                    transform.localPosition = v + new Vector3(rnd(), rnd(), rnd());
                }

                v.y = transform.localScale.y / 2;
                transform.localPosition = v;
            }

            StartCoroutine(SizeUp(2, 2f));
            StartCoroutine(Shake(2f));
            yield return new WaitForSeconds(2);
        }
    }
}