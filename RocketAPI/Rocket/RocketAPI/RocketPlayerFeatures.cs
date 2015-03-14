using Rocket.Components;
using Rocket.Logging;
using Rocket.RocketAPI.Events;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.RocketAPI
{
    public class RocketPlayerFeatures : RocketPlayerComponent
    {
        private RocketPlayerEvents e = null;
        private Player pl = null;
        private bool godMode = false;

        internal bool GodMode
        {
            set
            {
                if (value)
                {
                    e.OnUpdateHealth += e_OnPlayerUpdateHealth;
                    e.OnUpdateWater += e_OnPlayerUpdateWater;
                    e.OnUpdateFood += e_OnPlayerUpdateFood;
                    e.OnUpdateVirus += e_OnPlayerUpdateVirus;
                }
                else
                {
                    e.OnUpdateHealth -= e_OnPlayerUpdateHealth;
                    e.OnUpdateWater -= e_OnPlayerUpdateWater;
                    e.OnUpdateFood -= e_OnPlayerUpdateFood;
                    e.OnUpdateVirus -= e_OnPlayerUpdateVirus;
                }
                godMode = value;
            }
            get
            {
                return godMode;
            }
        }

        private void Start()
        {
            pl = gameObject.transform.GetComponent<Player>();

            if (!RocketPermissionManager.CheckReservedSlotSpace(pl.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;
            if (!RocketPermissionManager.CheckWhitelisted(pl.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;

            e = gameObject.transform.GetComponent<RocketPlayerEvents>();

            if (godMode)
            {
                e.OnUpdateHealth += e_OnPlayerUpdateHealth;
                e.OnUpdateWater += e_OnPlayerUpdateWater;
                e.OnUpdateFood += e_OnPlayerUpdateFood;
                e.OnUpdateVirus += e_OnPlayerUpdateVirus;
                pl.Heal(100);
                pl.SetInfection(0);
                pl.SetHunger(0);
                pl.SetThirst(0);
                pl.SetBleeding(false);
                pl.SetBroken(false);
            }
        }

        private void e_OnPlayerUpdateVirus(Player player, byte virus)
        {
            if (virus < 95) pl.SetInfection(0);
        }

        private void e_OnPlayerUpdateFood(Player player, byte food)
        {
            if (food < 95) pl.SetHunger(0);
        }

        private void e_OnPlayerUpdateWater(Player player, byte water)
        {
            if (water < 95) pl.SetThirst(0);
        }

        private void e_OnPlayerUpdateHealth(Player player, byte health)
        {
            if (health < 95)
            {
                pl.Heal(100);
                pl.SetBleeding(false);
                pl.SetBroken(false);
            }
        }
    }
}