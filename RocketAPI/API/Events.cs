using Rocket.RocketAPI;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;
using UnityEngine;

namespace Rocket.RocketAPI
{
    public class Events : RocketPlayerComponent
    {
        private Player player;
        private static bool bound;

        private void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            player = gameObject.transform.GetComponent<Player>();


            if (!RocketPermissionManager.CheckWhitelisted(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID)) return;
               





            #region PlayerLife
            player.PlayerLife.OnDamaged += onDamaged;
            player.PlayerLife.OnUpdateBleeding += onUpdateBleeding;
            player.PlayerLife.OnUpdateBroken += onUpdateBroken;
            player.PlayerLife.OnUpdateFoot += onUpdateFoot;
            player.PlayerLife.OnUpdateFreezing += onUpdateFreezing;
            player.PlayerLife.OnUpdateHealth += onUpdateHealth;
            player.PlayerLife.OnUpdateLife += onUpdateLife;
            player.PlayerLife.OnUpdateOxygen += onUpdateOxygen;
            player.PlayerLife.OnUpdateStamina += onUpdateStamina;
            player.PlayerLife.OnUpdateVirus += onUpdateVirus;
            player.PlayerLife.OnUpdateVision += onUpdateVision;
            player.PlayerLife.OnUpdateWater += onUpdateWater;
            #endregion

            #region PlayerInventory
            player.Inventory.OnInventoryAdded += onInventoryAdded;
            player.Inventory.OnInventoryRemoved += onInventoryRemoved;
            player.Inventory.OnInventoryResized += onInventoryResized;
            player.Inventory.OnInventoryUpdated += onInventoryUpdated;
            #endregion

            #region Steam
            if(!bound){
                Steam.OnServerShutdown += onServerShutdown;
                Steam.OnServerDisconnected += onPlayerDisconnected;
                bound = true;
            }
            #endregion

            //player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName = RocketPermissionManager.GetChatPrefix(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID) + player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName;

            try{
                if (OnPlayerConnected != null) OnPlayerConnected(player);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }


        #region PlayerLife
        /// <summary>
        ///
        /// </summary>
        /// <param name="player">The victim player</param>
        /// <param name="bleeding">Flag for bleeding</param>
        public delegate void PlayerUpdateBleeding(Player player, bool bleeding);
        public static event PlayerUpdateBleeding OnPlayerUpdateBleeding;
        private void onUpdateBleeding(bool J)
        {
            try{
                if (OnPlayerUpdateBleeding != null) OnPlayerUpdateBleeding(player, J);
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
        public delegate void PlayerUpdateBroken(Player player, bool broken);
        public static event PlayerUpdateBroken OnPlayerUpdateBroken;
        private void onUpdateBroken(bool D)
        {
            try {
                if (OnPlayerUpdateBroken != null) OnPlayerUpdateBroken(player, D);
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
        public delegate void PlayerUpdateFreezing(Player player, bool freezing);
        public static event PlayerUpdateFreezing OnPlayerUpdateFreezing;
        private void onUpdateFreezing(bool h)
        {
            try{
                if (OnPlayerUpdateFreezing != null) OnPlayerUpdateFreezing(player, h);
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
        public delegate void PlayerUpdateLife(Player player, bool life);
        public static event PlayerUpdateLife OnPlayerUpdateLife;
        private void onUpdateLife(bool C)
        {
            try {
                if (OnPlayerUpdateLife != null) OnPlayerUpdateLife(player, C);
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
        public delegate void PlayerUpdateVision(Player player, bool vision);
        public static event PlayerUpdateVision OnPlayerUpdateVision;
        private void onUpdateVision(bool o)
        {
            try {
                if (OnPlayerUpdateVision != null) OnPlayerUpdateVision(player, o);
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
        public delegate void PlayerUpdateFoot(Player player, byte foot);
        public static event PlayerUpdateFoot OnPlayerUpdateFoot;
        private void onUpdateFoot(byte R)
        {
            try {
                if (OnPlayerUpdateFoot != null) OnPlayerUpdateFoot(player, R);
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
        public delegate void PlayerUpdateHealth(Player player, byte health);
        public static event PlayerUpdateHealth OnPlayerUpdateHealth;
        private void onUpdateHealth(byte Y)
        {
            try {
                if (OnPlayerUpdateHealth != null) OnPlayerUpdateHealth(player, Y);
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
        public delegate void PlayerUpdateOxygen(Player player, byte oxygen);
        public static event PlayerUpdateOxygen OnPlayerUpdateOxygen;
        private void onUpdateOxygen(byte t)
        {
            try {
                if (OnPlayerUpdateOxygen != null) OnPlayerUpdateOxygen(player, t);
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
        public delegate void PlayerUpdateStamina(Player player, byte stamina);
        public static event PlayerUpdateStamina OnPlayerUpdateStamina;
        private void onUpdateStamina(byte D)
        {
            try {
                if (OnPlayerUpdateStamina != null) OnPlayerUpdateStamina(player, D);
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
        public delegate void PlayerUpdateVirus(Player player, byte virus);
        public static event PlayerUpdateVirus OnPlayerUpdateVirus;
        private void onUpdateVirus(byte E)
        {
            try {
                if (OnPlayerUpdateVirus != null) OnPlayerUpdateVirus(player, E);
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
        public delegate void PlayerUpdateWater(Player player, byte water);
        public static event PlayerUpdateWater OnPlayerUpdateWater;
        private void onUpdateWater(byte A) {
            try {
                if (OnPlayerUpdateWater != null) OnPlayerUpdateWater(player, A);
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
        public delegate void PlayerDamaged(Player player, byte damage);
        public static event PlayerDamaged OnPlayerDamaged;
        private void onDamaged(byte P)
        {
            try {
                if (OnPlayerDamaged != null) OnPlayerDamaged(player, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
        #endregion

        #region PlayerInventory

        public delegate void PlayerInventoryUpdated(Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryUpdated OnPlayerInventoryUpdated;
        private void onInventoryUpdated(byte E, byte O, ItemJar P)
        {
            try
            {
                if (OnPlayerInventoryUpdated != null) OnPlayerInventoryUpdated(player, E, O, P);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryResized(Player player, byte I, byte O, byte U);
        public static event PlayerInventoryResized OnPlayerInventoryResized;
        private void onInventoryResized(byte I, byte M, byte U)
        {
            try {
                if (OnPlayerInventoryResized != null) OnPlayerInventoryResized(player, I, M, U);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryRemoved(Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryRemoved OnPlayerInventoryRemoved;
        private void onInventoryRemoved(byte V, byte y, ItemJar f)
        {
            try {
                if (OnPlayerInventoryRemoved != null) OnPlayerInventoryRemoved(player, V, y, f);
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }

        public delegate void PlayerInventoryAdded(Player player, byte inventoryGroup, byte inventoryIndex, ItemJar P);
        public static event PlayerInventoryAdded OnPlayerInventoryAdded;
        private void onInventoryAdded(byte Q, byte u, ItemJar J)
        {
            try
            {
                if (OnPlayerInventoryAdded != null) OnPlayerInventoryAdded(player, Q, u, J);
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
        public delegate void PlayerDisconnected(Player player);
        public static event PlayerDisconnected OnPlayerDisconnected;
        private static void onPlayerDisconnected(CSteamID r)
        {
            try {
                if (OnPlayerDisconnected != null) OnPlayerDisconnected(PlayerTool.getPlayer(r));
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
        public delegate void PlayerConnected(Player player);
        public static event PlayerConnected OnPlayerConnected;

        public delegate void ServerShutdown();
        public static event ServerShutdown OnServerShutdown;
        private static void onServerShutdown()
        {
            try
            {
                if (OnServerShutdown != null) OnServerShutdown();
            }
            catch (System.Exception ex)
            {
                Logger.Log(ex);
            }
        }
            
        #endregion

    }
}
