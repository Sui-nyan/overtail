using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Overtail.NPC
{


    [System.Serializable]
    public class NPCObject : NPC
    {
        public string NPCName;

        public Image portrait;

        public List<EmotionEntry> emotions;

        public void SetPortrait(string portrait_id)
        {
            var sprite = emotions.FirstOrDefault(x => x.id == portrait_id);
            if (sprite == null) return;
            else
            {
                portrait.sprite = sprite.portrait;
            }
        }



    }
}

