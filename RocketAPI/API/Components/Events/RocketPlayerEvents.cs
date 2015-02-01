using Rocket.RocketAPI.Components;
using Rocket.RocketAPI.Managers;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public partial class RocketEvents : RocketPlayerComponent
    {
        public static void send(SteamPlayer s, string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            if (s == null || R == null) return;
            RocketEvents instance = s.Player.transform.GetComponent<RocketEvents>();

            switch (W)
            {
                case "tellBleeding":
                    if (OnPlayerUpdateBleeding != null) OnPlayerUpdateBleeding(s.Player, (bool)R[0]);
                    if (instance.OnUpdateBleeding != null) instance.OnUpdateBleeding(s.Player, (bool)R[0]);
                    break;
                case "tellBroken":
                    if (OnPlayerUpdateBroken != null) OnPlayerUpdateBroken(s.Player, (bool)R[0]);
                    if (instance.OnUpdateBroken != null) instance.OnUpdateBroken(s.Player, (bool)R[0]);
                    break;
                case "tellPosition":
                    if (OnPlayerUpdatePosition != null) OnPlayerUpdatePosition(s.Player, (Vector3)R[0]);
                    if (instance.OnUpdatePosition != null) instance.OnUpdatePosition(s.Player, (Vector3)R[0]);
                    break;
                case "tellLife":
                    if (OnPlayerUpdateLife != null) OnPlayerUpdateLife(s.Player, (byte)R[0]);
                    if (instance.OnUpdateLife != null) instance.OnUpdateLife(s.Player, (byte)R[0]);
                    break;
                case "tellDead":
                    if (OnPlayerDeath != null) OnPlayerDeath(s.Player, (Vector3)R[0]);
                    if (instance.OnDeath != null) instance.OnDeath(s.Player, (Vector3)R[0]);
                    break;
                case "tellFood":
                    if (OnPlayerUpdateFood != null) OnPlayerUpdateFood(s.Player, (byte)R[0]);
                    if (instance.OnUpdateFood != null) instance.OnUpdateFood(s.Player, (byte)R[0]);
                    break;
                case "tellHealth":
                    if (OnPlayerUpdateHealth != null) OnPlayerUpdateHealth(s.Player, (byte)R[0]);
                    if (instance.OnUpdateHealth != null) instance.OnUpdateHealth(s.Player, (byte)R[0]);
                    break;
                case "tellVirus":
                    if (OnPlayerUpdateVirus != null) OnPlayerUpdateVirus(s.Player, (byte)R[0]);
                    if (instance.OnUpdateVirus != null) instance.OnUpdateVirus(s.Player, (byte)R[0]);
                    break;
                case "tellWater":
                    if (OnPlayerUpdateWater != null) OnPlayerUpdateWater(s.Player, (byte)R[0]);
                    if (instance.OnUpdateWater != null) instance.OnUpdateWater(s.Player, (byte)R[0]);
                    break;
                case "tellStance":
                    if (OnPlayerUpdateStance != null) OnPlayerUpdateStance(s.Player, (byte)R[0]);
                    if (instance.OnUpdateStance != null) instance.OnUpdateStance(s.Player, (byte)R[0]);
                    break;
                case "tellRevive":
                    if (OnPlayerRevive != null) OnPlayerRevive(s.Player, (Vector3)R[0], (byte)R[1]);
                    if (instance.OnRevive != null) instance.OnRevive(s.Player, (Vector3)R[0], (byte)R[1]);
                    break;

                default:
#if DEBUG
                   string o = "";
                    foreach (object r in R)
                    {
                        o += r.ToString();
                    }
                    Logger.Log(s.SteamPlayerID.CSteamID.ToString() + ": " + W + " - " + o);
#endif
                    break;
            }
        }

        public delegate void PlayerUpdateBleeding(SDG.Player player, bool bleeding);
        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        public event PlayerUpdateBleeding OnUpdateBleeding;

        public delegate void PlayerUpdateBroken(SDG.Player player, bool broken);
        public static event PlayerUpdateBroken OnPlayerUpdateBroken;
        public event PlayerUpdateBroken OnUpdateBroken;

        public delegate void PlayerUpdatePosition(SDG.Player player, Vector3 position);
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;
        public event PlayerUpdatePosition OnUpdatePosition;

        public delegate void PlayerDeath(SDG.Player player, Vector3 position);
        public static event PlayerDeath OnPlayerDeath;
        public event PlayerDeath OnDeath;

        public delegate void PlayerUpdateLife(SDG.Player player, byte life);
        public static event PlayerUpdateLife OnPlayerUpdateLife;
        public event PlayerUpdateLife OnUpdateLife;

        public delegate void PlayerUpdateFood(SDG.Player player, byte food);
        public static event PlayerUpdateFood OnPlayerUpdateFood;
        public event PlayerUpdateFood OnUpdateFood;

        public delegate void PlayerUpdateHealth(SDG.Player player, byte health);
        public static event PlayerUpdateHealth OnPlayerUpdateHealth;
        public event PlayerUpdateHealth OnUpdateHealth;

        public delegate void PlayerUpdateVirus(SDG.Player player, byte virus);
        public static event PlayerUpdateVirus OnPlayerUpdateVirus;
        public event PlayerUpdateVirus OnUpdateVirus;

        public delegate void PlayerUpdateWater(SDG.Player player, byte water);
        public static event PlayerUpdateWater OnPlayerUpdateWater;
        public event PlayerUpdateWater OnUpdateWater;

        public delegate void PlayerUpdateStance(SDG.Player player, byte stance);
        public static event PlayerUpdateStance OnPlayerUpdateStance;
        public event PlayerUpdateStance OnUpdateStance;

        public delegate void PlayerRevive(SDG.Player player, Vector3 position, byte angle);
        public static event PlayerRevive OnPlayerRevive;
        public event PlayerRevive OnRevive;
    }
}