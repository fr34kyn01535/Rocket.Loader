using Rocket.Unturned.Events;
using Rocket.Unturned.Logging;
using Rocket.Unturned.Player;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.Unturned
{
    public enum EffectEventTypes { Join = 1 , Die = 2};

    public class RocketEffectManager : MonoBehaviour
    {
        public void Start(){
            RocketServerEvents.OnPlayerConnected += (RocketPlayer player) =>
            {
                foreach (EffectEvent effect in getEffectsByEvent(EffectEventTypes.Join))
                {
                    TriggerEffect(effect.EffectID, player, effect.Global);
                }
            };
            RocketPlayerEvents.OnPlayerDeath += (RocketPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer) => {
                foreach (EffectEvent effect in getEffectsByEvent(EffectEventTypes.Die))
                {
                    TriggerEffect(effect.EffectID, player, effect.Global);
                }
            };
        }

        public void TriggerEffect(ushort effectID,RocketPlayer player,bool global){
            if(!global){
                EffectManager.Instance.SteamChannel.send("tellEffectPoint", player.CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { effectID, player.Player.transform.position });
            }else{
                EffectManager.Instance.E.send("tellEffectPoint", ESteamCall.CLIENTS, player.Player.transform.position,1024, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { effectID, player.Player.transform.position });
            }
        }

        private List<EffectEvent> getEffectsByEvent(EffectEventTypes e)
        {
            return effects.Where(k => k.Type == e).ToList();
        }

        private static List<EffectEvent> effects = new List<EffectEvent>();

        public static void RegisterRocketEffect(Bundle b, Data q, ushort k)
        {
            short s = q.readInt16("RocketEffect");
            if (s != 0){
                bool global = q.readBoolean("Global");

                effects.Add(new EffectEvent((EffectEventTypes)s, k, global));
            }
        }
    
        private class EffectEvent
        {
            public EffectEvent(EffectEventTypes type, ushort effectID, bool global)
            {
                this.Type = type;
                this.EffectID = effectID;
                this.Global = global;
            }
            public EffectEventTypes Type;
            public ushort EffectID;
            public bool Global;
        }

    }

}
