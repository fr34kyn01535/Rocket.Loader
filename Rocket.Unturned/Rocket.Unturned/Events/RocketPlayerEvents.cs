using Rocket.API;
using Rocket.Core.Events;
using Rocket.Core.Logging;
using Rocket.Unturned.Enumerations;
using Rocket.Unturned.Player;
using SDG;
using Steamworks;
using System;
using System.ComponentModel;
using UnityEngine;
using System.Linq;

namespace Rocket.Unturned.Events
{
    public sealed class RocketPlayerEvents : RocketPlayerComponent
    {
        private new void Awake()
        {
#if DEBUG
            Logger.Log("RocketPlayerEvents > Awake");
#endif
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
#if LINUX
                 string o = "";
                 foreach (object r in R)
                 {
                     o += r.ToString();
                 }
                 Logger.Log("Send+" + s.SteamPlayerID.CSteamID.ToString() + ": " + W + " - " + o);
#endif
                if (W.StartsWith("tellWear")) {
                    RocketEvents.TryTrigger<PlayerWear>(OnPlayerWear, rp, Enum.Parse(typeof(Wearables), W.Replace("tellWear", "")), (ushort)R[0], R.Count() > 1 ? (byte?)R[1] : null);
                }
                switch (W)
                {
                    case "tellBleeding":
                        RocketEvents.TryTrigger<PlayerUpdateBleeding>(OnPlayerUpdateBleeding, rp, (bool)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateBleeding>(instance.OnUpdateBleeding, rp, (bool)R[0]);
                        break;
                    case "tellBroken":
                        RocketEvents.TryTrigger<PlayerUpdateBroken>(OnPlayerUpdateBroken,rp, (bool)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateBroken>(instance.OnUpdateBroken, rp, (bool)R[0]);
                        break;
                    case "tellPosition":
                        RocketEvents.TryTrigger<PlayerUpdatePosition>(OnPlayerUpdatePosition,rp, (Vector3)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdatePosition>(instance.OnUpdatePosition, rp, (Vector3)R[0]);
                        break;
                    case "tellLife":
                        RocketEvents.TryTrigger<PlayerUpdateLife>(OnPlayerUpdateLife,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateLife>(instance.OnUpdateLife, rp, (byte)R[0]);
                        break;
                    case "tellFood":
                        RocketEvents.TryTrigger<PlayerUpdateFood>(OnPlayerUpdateFood,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateFood>(instance.OnUpdateFood,rp, (byte)R[0]);
                        break;
                    case "tellHealth":
                        RocketEvents.TryTrigger<PlayerUpdateHealth>(OnPlayerUpdateHealth,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateHealth>(instance.OnUpdateHealth,rp, (byte)R[0]);
                        break;
                    case "tellVirus":
                        RocketEvents.TryTrigger<PlayerUpdateVirus>(OnPlayerUpdateVirus,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateVirus>(instance.OnUpdateVirus,rp, (byte)R[0]);
                        break;
                    case "tellWater":
                        RocketEvents.TryTrigger<PlayerUpdateWater>(OnPlayerUpdateWater,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateWater>(instance.OnUpdateWater,rp, (byte)R[0]);
                        break;
                    case "tellStance":
                        RocketEvents.TryTrigger<PlayerUpdateStance>(OnPlayerUpdateStance,rp, (byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateStance>(instance.OnUpdateStance, rp, (byte)R[0]);
                        break;
                    case "tellGesture":
                        RocketEvents.TryTrigger<PlayerUpdateGesture>(OnPlayerUpdateGesture,rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                        RocketEvents.TryTrigger<PlayerUpdateGesture>(instance.OnUpdateGesture, rp, (PlayerGesture)Enum.Parse(typeof(PlayerGesture), R[0].ToString()));
                        break;
                    case "tellStat":
                        RocketEvents.TryTrigger<PlayerUpdateStat>(OnPlayerUpdateStat, rp, (EPlayerStat)(byte)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateStat>(instance.OnUpdateStat, rp, (EPlayerStat)(byte)R[0]);
                        break;
                    case "tellExperience":
                        RocketEvents.TryTrigger<PlayerUpdateExperience>(OnPlayerUpdateExperience, rp, (uint)R[0]);
                        RocketEvents.TryTrigger<PlayerUpdateExperience>(instance.OnUpdateExperience, rp, (uint)R[0]);
                        break;
                    case "tellRevive":
                        RocketEvents.TryTrigger<PlayerRevive>(OnPlayerRevive,rp, (Vector3)R[0], (byte)R[1]);
                        RocketEvents.TryTrigger<PlayerRevive>(instance.OnRevive, rp, (Vector3)R[0], (byte)R[1]);
                        break;
                    case "tellDead":
                        RocketEvents.TryTrigger<PlayerDead>(OnPlayerDead, rp, (Vector3)R[0]);
                        RocketEvents.TryTrigger<PlayerDead>(instance.OnDead, rp, (Vector3)R[0]);
                        break;
                    case "tellDeath":
                        RocketEvents.TryTrigger<PlayerDeath>(OnPlayerDeath, rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                        RocketEvents.TryTrigger<PlayerDeath>(instance.OnDeath, rp, (EDeathCause)(byte)R[0], (ELimb)(byte)R[1], new CSteamID(ulong.Parse(R[2].ToString())));
                        break;
                    default:
#if DEBUG
                        Logger.Log("Send+" + s.SteamPlayerID.CSteamID.ToString() + ": " + W + " - " + String.Join(",",R.Select(e => e.ToString()).ToArray()));
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
            RocketEvents.TryTrigger<PlayerUpdateStamina>(OnPlayerUpdateStamina, Player, stamina);
            RocketEvents.TryTrigger<PlayerUpdateStamina>(OnUpdateStamina, Player, stamina);
        }

        public delegate void PlayerInventoryUpdated(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        public event PlayerInventoryUpdated OnInventoryUpdated;

        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            RocketEvents.TryTrigger<PlayerInventoryUpdated>(OnPlayerInventoryUpdated, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
            RocketEvents.TryTrigger<PlayerInventoryUpdated>(OnInventoryUpdated, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), O, P);
        }

        public delegate void PlayerInventoryResized(RocketPlayer player, InventoryGroup inventoryGroup, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        public event PlayerInventoryResized OnInventoryResized;

        private void onInventoryResized(byte E, byte M, byte U)
        {
            RocketEvents.TryTrigger<PlayerInventoryResized>(OnPlayerInventoryResized, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
            RocketEvents.TryTrigger<PlayerInventoryResized>(OnInventoryResized, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), M, U);
        }

        public delegate void PlayerInventoryRemoved(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        public event PlayerInventoryRemoved OnInventoryRemoved;

        private void onInventoryRemoved(byte E, byte y, ItemJar f)
        {
            RocketEvents.TryTrigger<PlayerInventoryRemoved>(OnPlayerInventoryRemoved, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
            RocketEvents.TryTrigger<PlayerInventoryRemoved>(OnInventoryRemoved, Player, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), y, f);
        }

        public delegate void PlayerInventoryAdded(RocketPlayer player, InventoryGroup inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        public event PlayerInventoryAdded OnInventoryAdded;

        private void onInventoryAdded(byte E, byte u, ItemJar J)
        {
            RocketEvents.TryTrigger<PlayerInventoryAdded>(OnPlayerInventoryAdded, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
            RocketEvents.TryTrigger<PlayerInventoryAdded>(OnInventoryAdded, (InventoryGroup)Enum.Parse(typeof(InventoryGroup), E.ToString()), u, J);
        }

        public delegate void PlayerChatted(RocketPlayer player, ref Color color, string message);
        public static event PlayerChatted OnPlayerChatted;

        internal static Color firePlayerChatted(RocketPlayer player, EChatMode chatMode, Color color, string msg)
        {
            if (OnPlayerChatted != null)
            {
                foreach (var handler in OnPlayerChatted.GetInvocationList().Cast<PlayerChatted>())
                {
                    try
                    {
                        handler(player, ref color, msg);
                    }
                    catch (Exception ex)
                    {
                        Logger.LogException(ex);
                    }
                }
            }

            return color;
        }

        public enum Wearables { Hat = 0, Mask = 1, Vest = 2, Pants = 3, Shirt = 4, Glasses = 5, Backpack = 6};
        public delegate void PlayerWear(RocketPlayer player, Wearables wear, ushort id, byte? quality);
        public static event PlayerWear OnPlayerWear;

    }
}