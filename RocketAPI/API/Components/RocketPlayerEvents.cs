using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public partial class RocketPlayer : RocketPlayerComponent
    {
        public void Start() {
            DontDestroyOnLoad(transform.gameObject);

            if (!RocketPermissionManager.CheckWhitelisted(PlayerInstance.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;

            #region PlayerLife
            PlayerInstance.PlayerLife.OnDamaged += onDamaged;
            PlayerInstance.PlayerLife.OnUpdateBleeding += onUpdateBleeding;
            PlayerInstance.PlayerLife.OnUpdateBroken += onUpdateBroken;
            PlayerInstance.PlayerLife.OnUpdateFoot += onUpdateFoot;
            PlayerInstance.PlayerLife.OnUpdateFreezing += onUpdateFreezing;
            PlayerInstance.PlayerLife.OnUpdateHealth += onUpdateHealth;
            PlayerInstance.PlayerLife.OnUpdateLife += onUpdateLife;
            PlayerInstance.PlayerLife.OnUpdateOxygen += onUpdateOxygen;
            PlayerInstance.PlayerLife.OnUpdateStamina += onUpdateStamina;
            PlayerInstance.PlayerLife.OnUpdateVirus += onUpdateVirus;
            PlayerInstance.PlayerLife.OnUpdateVision += onUpdateVision;
            PlayerInstance.PlayerLife.OnUpdateWater += onUpdateWater;
            #endregion

            #region PlayerInventory
            PlayerInstance.Inventory.OnInventoryAdded += onInventoryAdded;
            PlayerInstance.Inventory.OnInventoryRemoved += onInventoryRemoved;
            PlayerInstance.Inventory.OnInventoryResized += onInventoryResized;
            PlayerInstance.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion
        }


        #region PlayerLife
        /// <summary>
        ///
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="bleeding">Flag for bleeding</param>
        public delegate void PlayerUpdateBleeding(SDG.Player player, bool bleeding);
        public static event PlayerUpdateBleeding OnPlayersUpdateBleeding;
        public event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        private void onUpdateBleeding(bool J)
        {
            try
            {
                if (OnPlayersUpdateBleeding != null) OnPlayersUpdateBleeding(PlayerInstance, J);
                if (OnPlayerUpdateBleeding != null) OnPlayerUpdateBleeding(PlayerInstance, J);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="broken">Flag for broken legs</param>
        public delegate void PlayerUpdateBroken(SDG.Player player, bool broken);
        public static event PlayerUpdateBroken OnPlayersUpdateBroken;
        public event PlayerUpdateBroken OnPlayerUpdateBroken;
        private void onUpdateBroken(bool D)
        {
            try
            {
                if (OnPlayersUpdateBroken != null) OnPlayersUpdateBroken(PlayerInstance, D);
                if (OnPlayerUpdateBroken != null) OnPlayerUpdateBroken(PlayerInstance, D);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="freezing"></param>
        public delegate void PlayerUpdateFreezing(SDG.Player player, bool freezing);
        public static event PlayerUpdateFreezing OnPlayersUpdateFreezing;
        public event PlayerUpdateFreezing OnPlayerUpdateFreezing;
        private void onUpdateFreezing(bool h)
        {
            try
            {
                if (OnPlayersUpdateFreezing != null) OnPlayersUpdateFreezing(PlayerInstance, h);
                if (OnPlayerUpdateFreezing != null) OnPlayerUpdateFreezing(PlayerInstance, h);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="life"></param>
        public delegate void PlayerUpdateLife(SDG.Player player, bool life);
        public static event PlayerUpdateLife OnPlayersUpdateLife;
        public event PlayerUpdateLife OnPlayerUpdateLife;
        private void onUpdateLife(bool C)
        {
            try
            {
                if (OnPlayersUpdateLife != null) OnPlayersUpdateLife(PlayerInstance, C);
                if (OnPlayerUpdateLife != null) OnPlayerUpdateLife(PlayerInstance, C);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="vision"></param>
        public delegate void PlayerUpdateVision(SDG.Player player, bool vision);
        public static event PlayerUpdateVision OnPlayersUpdateVision;
        public event PlayerUpdateVision OnPlayerUpdateVision;
        private void onUpdateVision(bool o)
        {
            try
            {
                if (OnPlayersUpdateVision != null) OnPlayersUpdateVision(PlayerInstance, o);
                if (OnPlayerUpdateVision != null) OnPlayerUpdateVision(PlayerInstance, o);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="foot"></param>
        public delegate void PlayerUpdateFoot(SDG.Player player, byte foot);
        public static event PlayerUpdateFoot OnPlayersUpdateFoot;
        public event PlayerUpdateFoot OnPlayerUpdateFoot;
        private void onUpdateFoot(byte R)
        {
            try
            {
                if (OnPlayersUpdateFoot != null) OnPlayersUpdateFoot(PlayerInstance, R);
                if (OnPlayerUpdateFoot != null) OnPlayerUpdateFoot(PlayerInstance, R);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="health">New player health value</param>
        public delegate void PlayerUpdateHealth(SDG.Player player, byte health);
        public static event PlayerUpdateHealth OnPlayersUpdateHealth;
        public event PlayerUpdateHealth OnPlayerUpdateHealth;
        private void onUpdateHealth(byte Y)
        {
            try
            {
                if (OnPlayersUpdateHealth != null) OnPlayersUpdateHealth(PlayerInstance, Y);
                if (OnPlayerUpdateHealth != null) OnPlayerUpdateHealth(PlayerInstance, Y);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="oxygen">Flag for wether or not the player is above water</param>
        public delegate void PlayerUpdateOxygen(SDG.Player player, byte oxygen);
        public static event PlayerUpdateOxygen OnPlayersUpdateOxygen;
        public event PlayerUpdateOxygen OnPlayerUpdateOxygen;
        private void onUpdateOxygen(byte t)
        {
            try
            {
                if (OnPlayersUpdateOxygen != null) OnPlayersUpdateOxygen(PlayerInstance, t);
                if (OnPlayerUpdateOxygen != null) OnPlayerUpdateOxygen(PlayerInstance, t);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="stamina">New player stamina value</param>
        public delegate void PlayerUpdateStamina(SDG.Player player, byte stamina);
        public static event PlayerUpdateStamina OnPlayersUpdateStamina;
        public event PlayerUpdateStamina OnPlayerUpdateStamina;
        private void onUpdateStamina(byte D)
        {
            try
            {
                if (OnPlayersUpdateStamina != null) OnPlayersUpdateStamina(PlayerInstance, D);
                if (OnPlayerUpdateStamina != null) OnPlayerUpdateStamina(PlayerInstance, D);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="virus">New player radiation/infection value</param>
        public delegate void PlayerUpdateVirus(SDG.Player player, byte virus);
        public static event PlayerUpdateVirus OnPlayersUpdateVirus;
        public event PlayerUpdateVirus OnPlayerUpdateVirus;
        private void onUpdateVirus(byte E)
        {
            try
            {
                if (OnPlayersUpdateVirus != null) OnPlayersUpdateVirus(PlayerInstance, E);
                if (OnPlayerUpdateVirus != null) OnPlayerUpdateVirus(PlayerInstance, E);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="water">New player water value</param>
        public delegate void PlayerUpdateWater(SDG.Player player, byte water);
        public static event PlayerUpdateWater OnPlayersUpdateWater;
        public event PlayerUpdateWater OnPlayerUpdateWater;
        private void onUpdateWater(byte A)
        {
            try
            {
                if (OnPlayersUpdateWater != null) OnPlayersUpdateWater(PlayerInstance, A);
                if (OnPlayerUpdateWater != null) OnPlayerUpdateWater(PlayerInstance, A);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="damage">Damage to apply to the player</param>
        public delegate void PlayerDamaged(SDG.Player player, byte damage);
        public static event PlayerDamaged OnPlayersDamaged;
        public event PlayerDamaged OnPlayerDamaged;
        private void onDamaged(byte P)
        {
            try
            {
                if (OnPlayersDamaged != null) OnPlayersDamaged(PlayerInstance, P);
                if (OnPlayerDamaged != null) OnPlayerDamaged(PlayerInstance, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
        #endregion

        #region PlayerInventory

        public delegate void PlayerInventoryUpdated(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayersInventoryUpdated;
        public event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayersInventoryUpdated != null) OnPlayersInventoryUpdated(PlayerInstance, E, O, P);
                if (OnPlayerInventoryUpdated != null) OnPlayerInventoryUpdated(PlayerInstance, E, O, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(SDG.Player player, byte I, byte O, byte U);
        public static event PlayerInventoryResized OnPlayersInventoryResized;
        public event PlayerInventoryResized OnPlayerInventoryResized;
        private void onInventoryResized(byte I, byte M, byte U)
        {
            try
            {
                if (OnPlayersInventoryResized != null) OnPlayersInventoryResized(PlayerInstance, I, M, U);
                if (OnPlayerInventoryResized != null) OnPlayerInventoryResized(PlayerInstance, I, M, U);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayersInventoryRemoved;
        public event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        private void onInventoryRemoved(byte V, byte y, ItemJar f)
        {
            try
            {
                if (OnPlayersInventoryRemoved != null) OnPlayersInventoryRemoved(PlayerInstance, V, y, f);
                if (OnPlayerInventoryRemoved != null) OnPlayerInventoryRemoved(PlayerInstance, V, y, f);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(SDG.Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayersInventoryAdded;
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        private void onInventoryAdded(byte Q, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayersInventoryAdded != null) OnPlayersInventoryAdded(PlayerInstance, Q, u, J);
                if (OnPlayerInventoryAdded != null) OnPlayerInventoryAdded(PlayerInstance, Q, u, J);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        #endregion

    }
}
