using Rocket.RocketAPI.Components;
using SDG;

namespace Rocket.RocketAPI
{
    public class RocketPlayerFeatures : RocketPlayerComponent
    {
        private RocketPlayer p = null;
        private Player pl = null;
        private bool godMode = false;

        internal bool GodMode
        {
            set
            {
                if (value)
                {
                    RocketEvents.OnPlayerUpdateHealth += events_OnPlayerUpdateHealth;
                    RocketEvents.OnPlayerUpdateWater += events_OnPlayerUpdateWater;
                    RocketEvents.OnPlayerUpdateFood += events_OnPlayerUpdateFood;
                    RocketEvents.OnPlayerUpdateVirus += events_OnPlayerUpdateVirus;
                }
                else
                {
                    RocketEvents.OnPlayerUpdateHealth -= events_OnPlayerUpdateHealth;
                    RocketEvents.OnPlayerUpdateWater -= events_OnPlayerUpdateWater;
                    RocketEvents.OnPlayerUpdateFood -= events_OnPlayerUpdateFood;
                    RocketEvents.OnPlayerUpdateVirus -= events_OnPlayerUpdateVirus;
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