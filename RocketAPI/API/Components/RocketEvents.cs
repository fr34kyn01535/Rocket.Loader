using Rocket.RocketAPI.Components;
using Rocket.RocketAPI.Managers;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class RocketEvents : RocketPlayerComponent
    {
        private bool initialized = false;
        private new void Awake() {
            base.Awake();

            #region PlayerLife
                PlayerInstance.PlayerLife.OnUpdateStamina += onUpdateStamina;
            #endregion

            #region PlayerInventory
                PlayerInstance.Inventory.OnInventoryAdded += onInventoryAdded;
                PlayerInstance.Inventory.OnInventoryRemoved += onInventoryRemoved;
                PlayerInstance.Inventory.OnInventoryResized += onInventoryResized;
                PlayerInstance.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion

            if (!initialized) {
                initialized = true;
                #region Steam
                    Steam.OnServerShutdown += onServerShutdown;
                    Steam.OnServerDisconnected += onPlayerDisconnected;
                    Steam.OnServerConnected += onPlayerConnected;
                #endregion
            }
        }

        public static void send(SteamPlayer s,string W, ESteamCall X, ESteamPacket l, params object[] R)
        {
            if (s == null || R == null) return;

            switch (W) {
                case "tellBleeding":
                    if (OnPlayerUpdateBleeding != null) OnPlayerUpdateBleeding(s.Player, (bool)R[0]);
                    break;
                case "tellBroken":
                    if (OnPlayerUpdateBroken != null) OnPlayerUpdateBroken(s.Player, (bool)R[0]);
                    break;
                case "tellPosition":
                    if (OnPlayerUpdatePosition != null) OnPlayerUpdatePosition(s.Player, (Vector3)R[0]);
                    break;
                case "tellLife":
                    if (OnPlayerUpdateLife != null) OnPlayerUpdateLife(s.Player, (bool)R[0]);
                    break;
                case "tellFood":
                    if (OnPlayerUpdateFood != null) OnPlayerUpdateFood(s.Player, (byte)R[0]);
                    break;
                case "tellHealth":
                    if (OnPlayerUpdateHealth != null) OnPlayerUpdateHealth(s.Player, (byte)R[0]);
                    break;
                case "tellVirus":
                    if (OnPlayerUpdateVirus != null) OnPlayerUpdateVirus(s.Player, (byte)R[0]);
                    break;
                case "tellWater":
                    if (OnPlayerUpdateWater != null) OnPlayerUpdateWater(s.Player, (byte)R[0]);
                    break;
                case "tellRevive":
                    if (OnPlayerRevive != null) OnPlayerRevive(s.Player, (Vector3)R[0], (byte)R[1]);
                    break;
                default:
#if DEBUG
                    string o = "";
                    foreach(object r in R){
                        o += r.ToString();
                    }
                    Logger.Log(s.SteamPlayerID.CSteamID.ToString() + ": " +W + " - " + o);
#endif
                    break;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="bleeding">Flag for bleeding</param>
        public delegate void PlayerUpdateBleeding(SDG.Player player, bool bleeding);    
        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;   

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="broken">Flag for broken legs</param>
        public delegate void PlayerUpdateBroken(SDG.Player player, bool broken);    
        public static event PlayerUpdateBroken OnPlayerUpdateBroken;

        public delegate void PlayerUpdatePosition(SDG.Player player, Vector3 position);
        public static event PlayerUpdatePosition OnPlayerUpdatePosition;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="life">is dead or not</param>
        public delegate void PlayerUpdateLife(SDG.Player player, bool life);
        public static event PlayerUpdateLife OnPlayerUpdateLife;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="foot"></param>
        public delegate void PlayerUpdateFood(SDG.Player player, byte food);
        public static event PlayerUpdateFood OnPlayerUpdateFood;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="health">New player health value</param>
        public delegate void PlayerUpdateHealth(SDG.Player player, byte health);
        public static event PlayerUpdateHealth OnPlayerUpdateHealth;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="virus">New player radiation/infection value</param>
        public delegate void PlayerUpdateVirus(SDG.Player player, byte virus);
        public static event PlayerUpdateVirus OnPlayerUpdateVirus;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="water">New player water value</param>
        public delegate void PlayerUpdateWater(SDG.Player player, byte water);
        public static event PlayerUpdateWater OnPlayerUpdateWater;

        public delegate void PlayerRevive(SDG.Player player, Vector3 position, byte angle);
        public static event PlayerRevive OnPlayerRevive;

        #region PlayerLife
        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="stamina">New player stamina value</param>
        public delegate void PlayerUpdateStamina(SDG.Player player, byte stamina); 
        public static event PlayerUpdateStamina OnPlayerUpdateStamina; 
        private void onUpdateStamina(byte stamina)
        {
            try
            {
                if (OnPlayerUpdateStamina != null) RocketTaskManager.Enqueue(() =>OnPlayerUpdateStamina(PlayerInstance, stamina));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

         
        #endregion

        #region PlayerInventory

        public delegate void PlayerInventoryUpdated(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayerInventoryUpdated != null) RocketTaskManager.Enqueue(() =>OnPlayerInventoryUpdated(PlayerInstance, E, O, P));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(SDG.Player player, byte I, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        private void onInventoryResized(byte I, byte M, byte U)
        {
            try
            {
                if (OnPlayerInventoryResized != null) RocketTaskManager.Enqueue(() =>OnPlayerInventoryResized(PlayerInstance, I, M, U));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        private void onInventoryRemoved(byte V, byte y, ItemJar f)
        {
            try
            {
                if (OnPlayerInventoryRemoved != null) RocketTaskManager.Enqueue(() =>OnPlayerInventoryRemoved(PlayerInstance, V, y, f));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        private void onInventoryAdded(byte Q, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayerInventoryAdded != null) RocketTaskManager.Enqueue(() =>OnPlayerInventoryAdded(PlayerInstance, Q, u, J));
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        #endregion

        #region Steam

            /// <summary>
            /// 
            /// </summary>
            /// <param name="player">The disconnecting player</param>
            public delegate void PlayerDisconnected(SDG.Player player);
            public static event PlayerDisconnected OnPlayerDisconnected;
            private static void onPlayerDisconnected(CSteamID r)
            {
                try
                {
                    if (OnPlayerDisconnected != null)  RocketTaskManager.Enqueue(() =>OnPlayerDisconnected(PlayerTool.getPlayer(r)));
                }
                catch (System.Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="player">The connecting player</param>
            public delegate void PlayerConnected(SDG.Player player);
            public static event PlayerConnected OnPlayerConnected;
            private static void onPlayerConnected(CSteamID r)
            {
                try
                {
                    if (OnPlayerConnected != null) RocketTaskManager.Enqueue(() =>OnPlayerConnected(PlayerTool.getPlayer(r)));
                }
                catch (System.Exception ex)
                {
                    Logger.Log(ex);
                }
            }

            public delegate void ServerShutdown();
            public static event ServerShutdown OnServerShutdown;
            private static void onServerShutdown()
            {
                try
                {
                    if (OnServerShutdown != null) RocketTaskManager.Enqueue(() =>OnServerShutdown());
                }
                catch (System.Exception ex)
                {
                    Logger.Log(ex);
                }
            }

        #endregion
    }
}
