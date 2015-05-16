using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG;
using Steamworks;
using System;
using System.ComponentModel;
using UnityEngine;

namespace Rocket.Unturned.Events
{
    public sealed class RocketPlayerEvents : RocketPlayerComponent
    {
        private new void Awake()
        {
            base.Awake();

            Player.Player.PlayerLife.OnUpdateStamina += onUpdateStamina;

            Player.Player.Inventory.OnInventoryAdded += onInventoryAdded;
            Player.Player.Inventory.OnInventoryRemoved += onInventoryRemoved;
            Player.Player.Inventory.OnInventoryResized += onInventoryResized;
            Player.Player.Inventory.OnInventoryUpdated += onInventoryUpdated;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TriggerReceive(SteamChannel instance, CSteamID d, byte[] a, int b)
        {
#if DEBUG
            /*ESteamPacket eSteamPacket = (ESteamPacket)a[0];
            int num = a[1];

            if (eSteamPacket != ESteamPacket.UPDATE_VOICE && eSteamPacket != ESteamPacket.UPDATE_UDP_CHUNK && eSteamPacket != ESteamPacket.UPDATE_TCP_CHUNK)
            {
                object[] objects = SteamPacker.getObjects(d, 2, a, instance.Methods[num].Types);

                string o = "";
                foreach (object r in objects)
                {
                    o += r.ToString() + ",";
                }
                Logger.Log("Receive+" + d.ToString() + ": " + o + " - " + b);
            }*/
#endif
            return;
        }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public static void TriggerSend(SteamPlayer s, string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            try
            {
                if (s == null || s.Player == null || s.Player.transform == null || R == null) return;
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
                    case "tellStat":
                        if (OnPlayerUpdateStat != null) OnPlayerUpdateStat(rp, (EPlayerStat)(byte)R[0]);
                        if (instance.OnUpdateStat != null) instance.OnUpdateStat(rp, (EPlayerStat)(byte)R[0]);
                        break;
                    case "tellExperience":
                        if (OnPlayerUpdateExperience != null) OnPlayerUpdateExperience(rp, (uint)R[0]);
                        if (instance.OnUpdateExperience != null) instance.OnUpdateExperience(rp, (uint)R[0]);
                        break;
                    default:
#if DEBUG
                        string o = "";
                        foreach (object r in R)
                        {
                            o += r.ToString();
                        }
                        Logger.Log("Send+" + s.SteamPlayerID.CSteamID.ToString() + ": " + W + " - " + o);
#endif
                        break;
                }
                return;
            }
            catch (Exception ex)
            {
                Logger.Log(ex);
            }
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

        public delegate void PlayerUpdateExperience(RocketPlayer player, uint experience);
        public static event PlayerUpdateExperience OnPlayerUpdateExperience;
        public event PlayerUpdateExperience OnUpdateExperience;

        public delegate void PlayerUpdateStamina(RocketPlayer player, byte stamina);
        public static event PlayerUpdateStamina OnPlayerUpdateStamina;
        public event PlayerUpdateStamina OnUpdateStamina;

        private void onUpdateStamina(byte stamina)
        {
            try
            {
                if (OnPlayerUpdateStamina != null) OnPlayerUpdateStamina(Player, stamina);
                if (OnUpdateStamina != null) OnUpdateStamina(Player, stamina);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public enum InventoryGroup { Primary = 0, Secondary = 1, Hands = 2, Backpack = 3, Vest = 4, Shirt = 5, Pants = 6, Storage = 7 };


        public delegate void PlayerInventoryUpdated(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayerInventoryUpdated != null) OnPlayerInventoryUpdated(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
                if (OnInventoryUpdated != null) OnInventoryUpdated(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(RocketPlayer player, InventoryGroup inventoryGroup, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        private void onInventoryResized(byte E, byte M, byte U)
        {
            try
            {
                if (OnPlayerInventoryResized != null) OnPlayerInventoryResized(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
                if (OnInventoryResized != null) OnInventoryResized(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        private void onInventoryRemoved(byte E, byte y, ItemJar f)
        {
            try
            {
                if (OnPlayerInventoryRemoved != null) OnPlayerInventoryRemoved(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
                if (OnInventoryRemoved != null) OnInventoryRemoved(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        private void onInventoryAdded(byte E, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayerInventoryAdded != null) OnPlayerInventoryAdded(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
                if (OnInventoryAdded != null) OnInventoryAdded(Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }


    }
}