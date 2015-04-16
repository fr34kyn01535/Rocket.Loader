using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public sealed partial class RocketPlayerEvents : RocketPlayerComponent
    {
        private new void Awake()
        {
            base.Awake();

            #region RocketPlayerLifeEvents
                PlayerInstance.Player.PlayerLife.OnUpdateStamina += onUpdateStamina;
            #endregion RocketPlayerLifeEvents

            #region RocketPlayerEvents
                PlayerInstance.Player.Inventory.OnInventoryAdded += onInventoryAdded;
                PlayerInstance.Player.Inventory.OnInventoryRemoved += onInventoryRemoved;
                PlayerInstance.Player.Inventory.OnInventoryResized += onInventoryResized;
                PlayerInstance.Player.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion RocketPlayerEvents
        }

        public static void Receive(SteamChannel instance,CSteamID d, byte[] a, int b)
        {
//#if DEBUG
//            ESteamPacket eSteamPacket = (ESteamPacket)a[0];
//            int num = a[1];

//            if (eSteamPacket == ESteamPacket.UPDATE_UDP_CHUNK || eSteamPacket == ESteamPacket.UPDATE_TCP_CHUNK)
//            {
//                string o = "";
//                foreach (Type r in instance.Methods[num].Types)
//                {
//                    o += r.ToString() + ",";
//                }
//                Logger.Log("Receive+Invoke+" + d.ToString() + ": " + o + " - " + b);
//            }
//            else if (eSteamPacket != ESteamPacket.UPDATE_VOICE)
//            {
//                object[] objects = SteamPacker.getObjects(d, 2, a, instance.Methods[num].Types);


//                string o = "";
//                foreach (byte r in objects)
//                {
//                    o += r.ToString() + ",";
//                }
//                Logger.Log("Receive+" + d.ToString() + ": " + o + " - " + b);
//            }
//#endif
            return;
        }

        public static void Send(SteamPlayer s, string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            if (s == null || R == null) return;
            RocketPlayerEvents instance = s.Player.transform.GetComponent<RocketPlayerEvents>();
            RocketPlayer rp = RocketPlayer.FromSteamPlayer(s);

            switch (W)
            {
                case "tellBleeding":
                    if (OnPlayerUpdateBleeding != null) OnPlayerUpdateBleeding(rp, (bool)R[0]);
                    if (instance.OnUpdateBleeding != null) instance.OnUpdateBleeding(rp, (bool)R[0]);
                    break;
                case "tellBroken":
                    if (OnPlayerUpdateBroken != null) OnPlayerUpdateBroken(rp, (bool)R[0]);
                    if (instance.OnUpdateBroken != null) instance.OnUpdateBroken(rp, (bool)R[0]);
                    break;
                case "tellPosition":
                    if (OnPlayerUpdatePosition != null) OnPlayerUpdatePosition(rp, (Vector3)R[0]);
                    if (instance.OnUpdatePosition != null) instance.OnUpdatePosition(rp, (Vector3)R[0]);
                    break;
                case "tellLife":
                    if (OnPlayerUpdateLife != null) OnPlayerUpdateLife(rp, (byte)R[0]);
                    if (instance.OnUpdateLife != null) instance.OnUpdateLife(rp, (byte)R[0]);
                    break;
                case "tellDead":
                    if (OnPlayerDead != null) OnPlayerDead(rp, (Vector3)R[0]);
                    if (instance.OnDead != null) instance.OnDead(rp, (Vector3)R[0]);
                    break;
                case "tellDeath":
                    if (OnPlayerDeath != null) OnPlayerDeath(rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                    if (instance.OnDeath != null) instance.OnDeath(rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                    break;
                case "tellFood":
                    if (OnPlayerUpdateFood != null) OnPlayerUpdateFood(rp, (byte)R[0]);
                    if (instance.OnUpdateFood != null) instance.OnUpdateFood(rp, (byte)R[0]);
                    break;
                case "tellHealth":
                    if (OnPlayerUpdateHealth != null) OnPlayerUpdateHealth(rp, (byte)R[0]);
                    if (instance.OnUpdateHealth != null) instance.OnUpdateHealth(rp, (byte)R[0]);
                    break;
                case "tellVirus":
                    if (OnPlayerUpdateVirus != null) OnPlayerUpdateVirus(rp, (byte)R[0]);
                    if (instance.OnUpdateVirus != null) instance.OnUpdateVirus(rp, (byte)R[0]);
                    break;
                case "tellWater":
                    if (OnPlayerUpdateWater != null) OnPlayerUpdateWater(rp, (byte)R[0]);
                    if (instance.OnUpdateWater != null) instance.OnUpdateWater(rp, (byte)R[0]);
                    break;
                case "tellStance":
                    if (OnPlayerUpdateStance != null) OnPlayerUpdateStance(rp, (byte)R[0]);
                    if (instance.OnUpdateStance != null) instance.OnUpdateStance(rp, (byte)R[0]);
                    break;
                case "tellGesture":
                    if (OnPlayerUpdateGesture != null) OnPlayerUpdateGesture(rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                    if (instance.OnUpdateGesture != null) instance.OnUpdateGesture(rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                    break;
                case "tellRevive":
                    if (OnPlayerRevive != null) OnPlayerRevive(rp, (Vector3)R[0], (byte)R[1]);
                    if (instance.OnRevive != null) instance.OnRevive(rp, (Vector3)R[0], (byte)R[1]);
                    break;
                case "askStat":
                    if (OnPlayerUpdateStat != null) OnPlayerUpdateStat(rp, (EPlayerStat)(byte)R[0]);
                    if (instance.OnUpdateStat != null) instance.OnUpdateStat(rp, (EPlayerStat)(byte)R[0]);
                    break;
                default:
#if DEBUG
                   string o = "";
                    foreach (object r in R)
                    {
                        o += r.ToString();
                    }
                    Logger.Log("Send+"+s.SteamPlayerID.CSteamID.ToString() + ": " + W + " - " + o);
#endif
                    break;
            }
            return;
        }

        public delegate void PlayerUpdateBleeding(RocketPlayer player, bool bleeding);
        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        public event PlayerUpdateBleeding OnUpdateBleeding;

        public delegate void PlayerUpdateBroken(RocketPlayer player, bool broken);
        public static event PlayerUpdateBroken OnPlayerUpdateBroken;
        public event PlayerUpdateBroken OnUpdateBroken;

        public delegate void PlayerUpdatePosition(RocketPlayer player, Vector3 position);
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;
        public event PlayerUpdatePosition OnUpdatePosition;

        public delegate void PlayerDeath(RocketPlayer player, EDeathCause cause, ELimb limb, CSteamID murderer);
        public static event PlayerDeath OnPlayerDeath;
        public event PlayerDeath OnDeath;

        public delegate void PlayerDead(RocketPlayer player, Vector3 position);
        public static event PlayerDead OnPlayerDead;
        public event PlayerDead OnDead;

        public delegate void PlayerUpdateLife(RocketPlayer player, byte life);
        public static event PlayerUpdateLife OnPlayerUpdateLife;
        public event PlayerUpdateLife OnUpdateLife;

        public delegate void PlayerUpdateFood(RocketPlayer player, byte food);
        public static event PlayerUpdateFood OnPlayerUpdateFood;
        public event PlayerUpdateFood OnUpdateFood;

        public delegate void PlayerUpdateHealth(RocketPlayer player, byte health);
        public static event PlayerUpdateHealth OnPlayerUpdateHealth;
        public event PlayerUpdateHealth OnUpdateHealth;

        public delegate void PlayerUpdateVirus(RocketPlayer player, byte virus);
        public static event PlayerUpdateVirus OnPlayerUpdateVirus;
        public event PlayerUpdateVirus OnUpdateVirus;

        public delegate void PlayerUpdateWater(RocketPlayer player, byte water);
        public static event PlayerUpdateWater OnPlayerUpdateWater;
        public event PlayerUpdateWater OnUpdateWater;

        public enum PlayerGesture { None = 0, InventoryOpen = 1, InventoryClose = 2, Pickup = 3, PunchLeft = 4, PunchRight = 5, SurrenderStart = 6, SurrenderStop = 7, Point = 8, Wave = 9 };
        public delegate void PlayerUpdateGesture(RocketPlayer player, PlayerGesture gesture);
        public static event PlayerUpdateGesture OnPlayerUpdateGesture;
        public event PlayerUpdateGesture OnUpdateGesture;

        public delegate void PlayerUpdateStance(RocketPlayer player, byte stance);
        public static event PlayerUpdateStance OnPlayerUpdateStance;
        public event PlayerUpdateStance OnUpdateStance;

        public delegate void PlayerRevive(RocketPlayer player, Vector3 position, byte angle);
        public static event PlayerRevive OnPlayerRevive;
        public event PlayerRevive OnRevive;

        public delegate void PlayerUpdateStat(RocketPlayer player, EPlayerStat stat);
        public static event PlayerUpdateStat OnPlayerUpdateStat;
        public event PlayerUpdateStat OnUpdateStat;
    }
}