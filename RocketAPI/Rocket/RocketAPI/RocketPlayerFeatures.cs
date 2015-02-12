using Rocket.Components;
using Rocket.RocketAPI.Events;
using SDG;
using System;
using System.Collections.Generic;

namespace Rocket.RocketAPI
{
    public class RocketPlayerFeatures : RocketPlayerComponent
    {
        private RocketPlayer p = null;
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
            if (!RocketPermissionManager.CheckWhitelisted(pl.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;

            p = gameObject.transform.GetComponent<RocketPlayer>();
            e = gameObject.transform.GetComponent<RocketPlayerEvents>();
        /*

            pl.Inventory.updateItems(PlayerInventory.A, storage.items);
            player.inventory.sendStorage();
            */
            if (godMode)
            {
                e.OnUpdateHealth += e_OnPlayerUpdateHealth;
                e.OnUpdateWater += e_OnPlayerUpdateWater;
                e.OnUpdateFood += e_OnPlayerUpdateFood;
                e.OnUpdateVirus += e_OnPlayerUpdateVirus;
                p.Heal(100);
                p.Infection = 0;
                p.Hunger = 0;
                p.Thirst = 0;
            }
        }

        private void e_OnPlayerUpdateVirus(Player player, byte virus)
        {
            if (virus < 95) p.Infection = 0;
        }

        private void e_OnPlayerUpdateFood(Player player, byte food)
        {
            if (food < 95 ) p.Hunger = 0;
        }

        private void e_OnPlayerUpdateWater(Player player, byte water)
        {
            if (water < 95) p.Thirst = 0;
        }

        private void e_OnPlayerUpdateHealth(Player player, byte health)
        {
            if (health < 95) p.Heal(100);
        }
    }
}