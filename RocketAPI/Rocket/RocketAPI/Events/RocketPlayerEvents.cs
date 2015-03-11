﻿using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;
using System;
using UnityEngine;

namespace Rocket.RocketAPI.Events
{
    public partial class RocketPlayerEvents : RocketPlayerComponent
    {
        private new void Awake()
        {
            base.Awake();

            #region RocketPlayerLifeEvents
                PlayerInstance.PlayerLife.OnUpdateStamina += onUpdateStamina;
            #endregion RocketPlayerLifeEvents

            #region RocketPlayerEvents
                PlayerInstance.Inventory.OnInventoryAdded += onInventoryAdded;
                PlayerInstance.Inventory.OnInventoryRemoved += onInventoryRemoved;
                PlayerInstance.Inventory.OnInventoryResized += onInventoryResized;
                PlayerInstance.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion RocketPlayerEvents
        }
    
        public static void Send(SteamPlayer s, string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            if (s == null || R == null) return;
            RocketPlayerEvents instance = s.Player.transform.GetComponent<RocketPlayerEvents>();

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
                    if (OnPlayerDead != null) OnPlayerDead(s.Player, (Vector3)R[0]);
                    if (instance.OnDead != null) instance.OnDead(s.Player, (Vector3)R[0]);
                    break;
                case "tellDeath":
                    if (OnPlayerDeath != null) OnPlayerDeath(s.Player, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                    if (instance.OnDeath != null) instance.OnDeath(s.Player, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
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
                case "tellGesture":
                    if (OnPlayerUpdateGesture != null) OnPlayerUpdateGesture(s.Player, (PlayerGesture)Enum.Parse(typeof(PlayerGesture),R[0].ToString()));
                    if (instance.OnUpdateGesture != null) instance.OnUpdateGesture(s.Player, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
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

        public delegate void PlayerDeath(SDG.Player player, EDeathCause cause, ELimb limb, CSteamID murderer);
        public static event PlayerDeath OnPlayerDeath;
        public event PlayerDeath OnDeath;

        public delegate void PlayerDead(SDG.Player player, Vector3 position);
        public static event PlayerDead OnPlayerDead;
        public event PlayerDead OnDead;

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

        public enum PlayerGesture { None = 0, InventoryOpen = 1, InventoryClose = 2, Pickup = 3, PunchLeft = 4, PunchRight = 5, SurrenderStart = 6, SurrenderStop = 7, Point = 8, Wave = 9 };
        public delegate void PlayerUpdateGesture(SDG.Player player, PlayerGesture gesture);
        public static event PlayerUpdateGesture OnPlayerUpdateGesture;
        public event PlayerUpdateGesture OnUpdateGesture;

        public delegate void PlayerUpdateStance(SDG.Player player, byte stance);
        public static event PlayerUpdateStance OnPlayerUpdateStance;
        public event PlayerUpdateStance OnUpdateStance;

        public delegate void PlayerRevive(SDG.Player player, Vector3 position, byte angle);
        public static event PlayerRevive OnPlayerRevive;
        public event PlayerRevive OnRevive;
    }
}