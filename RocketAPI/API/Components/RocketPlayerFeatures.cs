using Rocket.RocketAPI.Components;
using SDG;
using System.Collections.Generic;

namespace Rocket.RocketAPI
{
    public class RocketPlayerFeatures : RocketPlayerComponent
    {
        private RocketPlayer p = null;
        private RocketEvents e = null;
        private Player pl = null;
        private bool godMode = false;

        internal bool GodMode
        {
            set
            {
                if (value)
                {
                    e.OnUpdateHealth += events_OnPlayerUpdateHealth;
                    e.OnUpdateWater += events_OnPlayerUpdateWater;
                    e.OnUpdateFood += events_OnPlayerUpdateFood;
                    e.OnUpdateVirus += events_OnPlayerUpdateVirus;
                }
                else
                {
                    e.OnUpdateHealth -= events_OnPlayerUpdateHealth;
                    e.OnUpdateWater -= events_OnPlayerUpdateWater;
                    e.OnUpdateFood -= events_OnPlayerUpdateFood;
                    e.OnUpdateVirus -= events_OnPlayerUpdateVirus;
                }
                godMode = value;
            }
            get
            {
                return godMode;
            }
        }

        List<float> lastY = new List<float>();

        void RocketEvents_OnPlayerUpdatePosition(Player player, UnityEngine.Vector3 position)
        {
            if (lastY.Count >= 6) lastY.RemoveAt(0);
            lastY.Add(position.y);

            float distance = lastY[lastY.Count - 1] - lastY[0];

            if (distance > 5)
            {
                Logger.Log(player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName + " changed his height by " + distance);
            }

        }

        private void Start()
        {
            pl = gameObject.transform.GetComponent<Player>();
            if (!RocketPermissionManager.CheckWhitelisted(pl.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;

            p = gameObject.transform.GetComponent<RocketPlayer>();
            e = gameObject.transform.GetComponent<RocketEvents>();

            e.OnUpdatePosition += RocketEvents_OnPlayerUpdatePosition;

            if (godMode)
            {
                e.OnUpdateHealth += events_OnPlayerUpdateHealth;
                e.OnUpdateWater += events_OnPlayerUpdateWater;
                e.OnUpdateFood += events_OnPlayerUpdateFood;
                e.OnUpdateVirus += events_OnPlayerUpdateVirus;
                p.Heal(100);
                p.Infection = 0;
                p.Hunger = 0;
                p.Thirst = 0;
            }
        }

        private void events_OnPlayerUpdateVirus(Player player, byte virus)
        {
            if (virus < 95) p.Infection = 0;
        }

        private void events_OnPlayerUpdateFood(Player player, byte food)
        {
            if (food < 95 ) p.Hunger = 0;
        }

        private void events_OnPlayerUpdateWater(Player player, byte water)
        {
            if (water < 95) p.Thirst = 0;
        }

        private void events_OnPlayerUpdateHealth(Player player, byte health)
        {
            if (health < 95) p.Heal(100);
        }
    }
}