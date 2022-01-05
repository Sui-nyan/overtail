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
            system.GUI.QueueMessage("#!?", 1f);
            system.GUI.QueueCoroutine(Transform);

            yield return system.GUI.AwaitIdle();

            // TODO Change sprite

            maxHp = 999;
            HP = MaxHP;
            attack = 999;
            level = 99;
            name = name.Replace("Small", "Small(?)").Replace("small", "small(?)");

            system.GUI.QueueMessage("THAT'S IT, YOU LITTLE SH*T");
            //system.GUI.SetHUDs();
        }

        private IEnumerator Transform(MonoBehaviour obj)
        {
            IEnumerator GROW(float scalar, float time)
            {
                Vector3 originalPos = transform.localPosition;
                Vector3 originalScale = transform.localScale;
                float timeElapsed = 0;

                while(timeElapsed < time)
                {
                    timeElapsed += Time.deltaTime;
                    yield return null;
                    
                    var newScale = originalScale;
                    newScale.x = Mathf.SmoothStep(originalScale.x, originalScale.x * scalar * 1.5f, timeElapsed / time);
                    newScale.y = Mathf.SmoothStep(originalScale.y, originalScale.y * scalar, timeElapsed / time);

                    transform.localScale = newScale;

                    Func<float> rnd = () => (float)(UnityEngine.Random.value - 1); // shaky
                    var newPos = originalPos + new Vector3(.1f * rnd(), .1f * rnd(), .1f * rnd());
                    newPos.y = newScale.y / 2;

                    transform.localPosition = newPos;
                }

                originalPos.y = transform.localScale.y / 2;
                transform.localPosition = originalPos;
            }

            return GROW(4, 2);
        }
    }
}