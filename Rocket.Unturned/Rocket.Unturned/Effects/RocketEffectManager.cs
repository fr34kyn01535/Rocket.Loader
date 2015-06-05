using Rocket.Unturned.Events;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Unturned
{
    public class RocketEffectManager : MonoBehaviour
    {
        public void Start(){
            RocketServerEvents.OnPlayerConnected += (RocketPlayer player) =>
            {
                foreach (ushort effect in getEffectsByEvent(EffectEvents.Join))
                {
                    player.TriggerEffect(effect);
                }
            };
        }

        private ushort[] getEffectsByEvent(EffectEvents e)
        {
            return effects.Where(k => k.Value == e).Select(k => k.Key).ToArray();
        }

        public enum EffectEvents { Join = 1 };

        private static Dictionary<ushort, EffectEvents> effects = new Dictionary<ushort, EffectEvents>();

        public static void RegisterRocketEffect(Bundle b, Data q, ushort k)
        {
            short s = q.readInt16("RocketEffect");
            if (s != 0){
                effects.Add(k,(EffectEvents)s);
            }
        }
    }
}
