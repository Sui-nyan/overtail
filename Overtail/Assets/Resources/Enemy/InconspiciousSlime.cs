using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Overtail.Battle
{
    public class InconspiciousSlime : EnemyUnit
    {
        // Private fields

        private bool _enraged = false;
        private readonly List<string> _responses = new List<string>();


        // Public

        public override IEnumerator OnGreeting(BattleSystem system)
        {
            yield return system.GUI.StartDialogue("Could you please leave me alone?");
            yield return system.GUI.StartDialogue("I don't want any trouble.");

            // <Insert qq animation>

            yield break;
        }

        public override IEnumerator DoTurnLogic(BattleSystem system)
        {
            if (_enraged)
            {
                yield return StartCoroutine(OnAttack(system));
                yield return StartCoroutine(system.Player.OnGetAttacked(system));
            } else if (HP < MaxHP * 0.4)
            {
                yield return StartCoroutine(GoSuperSaiyan(system));
            }
        }

        public override IEnumerator OnAttack(BattleSystem system)
        {
            yield return system.GUI.StartDialogue($"{Name.ToUpper()} body slams {system.Player.Name}.");
        }

        public override IEnumerator OnGetAttacked(BattleSystem system)
        {
            if (_enraged)
            {
                yield return system.GUI.StartDialogue($".");
                yield return system.GUI.StartDialogue($"..");
                yield return system.GUI.StartDialogue($"...");
                yield return system.GUI.StartDialogue($"{Name} is unfazed by {system.Player.Name}'s attack.");
                yield break;
            }

            var dmg = Math.Max(system.Player.Attack - this.Defense, 0);
            HP = Mathf.Clamp(HP - dmg, (int)(0.3*MaxHP), MaxHP); // Won't drop below threshold
            yield return new WaitForSeconds(1f);
            
            // Text Reactions

            _responses.Add("Please let me go.");
            _responses.Add("I'll get angry!");
            _responses.Add("I'm warning you!");

            var r = UnityEngine.Random.value * _responses.Count;

            yield return system.GUI.StartDialogue(_responses[(int)r]);
        }


        // Private Methods
        private IEnumerator GoSuperSaiyan(BattleSystem system)
        {
            _enraged = true;
            yield return system.GUI.StartDialogue("#!?", typeWriteDelay: 1f);

            MaxHP = 999;
            Attack = 999;
            Level = 99;
            Name = Name.Replace("Small", "Small(?)").Replace("small", "small(?)");
            HP = MaxHP;

            yield return StartCoroutine(GrowBig(system));



            yield return system.GUI.StartDialogue("THAT'S IT, YOU LITTLE SH*T");
        }
        private IEnumerator GrowBig(MonoBehaviour obj)
        {
            // TODO Change sprite
            return ShakeAndEnlarge(4, 2);

            IEnumerator ShakeAndEnlarge(float scalar, float time)
            {
                Vector3 originalPos = transform.localPosition;
                Vector3 originalScale = transform.localScale;
                float timeElapsed = 0;

                while (timeElapsed < time)
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
        }
    }
}