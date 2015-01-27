using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketPlayerFeatures : RocketPlayerComponent
    {
        RocketPlayer p = null;
        Player pl = null;
        private bool godMode = false;
        internal bool GodMode{
            set{
                if(value){
                    RocketEvents.OnPlayerUpdateHealth += events_OnPlayerUpdateHealth;
                    RocketEvents.OnPlayerUpdateWater += events_OnPlayerUpdateWater;
                    RocketEvents.OnPlayerUpdateFood += events_OnPlayerUpdateFood;
                    RocketEvents.OnPlayerUpdateVirus += events_OnPlayerUpdateVirus;
                }else{
                    RocketEvents.OnPlayerUpdateHealth -= events_OnPlayerUpdateHealth;
                    RocketEvents.OnPlayerUpdateWater -= events_OnPlayerUpdateWater;
                    RocketEvents.OnPlayerUpdateFood -= events_OnPlayerUpdateFood;
                    RocketEvents.OnPlayerUpdateVirus -= events_OnPlayerUpdateVirus;
                }
                godMode = value;
            }
            get{
                return godMode;
            }
        }

        private void Start(){

            p = gameObject.transform.GetComponent<RocketPlayer>();
            pl = gameObject.transform.GetComponent<Player>();

            if (godMode)
            {
                RocketEvents.OnPlayerUpdateHealth += events_OnPlayerUpdateHealth;
                RocketEvents.OnPlayerUpdateWater += events_OnPlayerUpdateWater;
                RocketEvents.OnPlayerUpdateFood += events_OnPlayerUpdateFood;
                RocketEvents.OnPlayerUpdateVirus += events_OnPlayerUpdateVirus;
                p.Heal(100);
                p.Infection = 0;
                p.Hunger = 0;
                p.Thirst = 0;
            }
            if (!RocketPermissionManager.CheckWhitelisted(pl.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;

        }


        
        void events_OnPlayerUpdateVirus(Player player, byte virus)
        {
            if (virus != 100) p.Infection = 0;
        }

        void events_OnPlayerUpdateFood(Player player, byte food)
        {
            if (food != 100) p.Hunger = 0;
        }

        void events_OnPlayerUpdateWater(Player player, byte water)
        {
            if (water != 100) p.Thirst = 0;
        }

        void events_OnPlayerUpdateHealth(Player player, byte health)
        {
            if (health != 100) p.Heal(100);
        }

    }
}
