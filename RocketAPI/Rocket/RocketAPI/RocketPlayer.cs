﻿using Rocket.Components;
using Rocket.Logging;
using Rocket.RocketAPI.Events;
using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rocket.RocketAPI
{
    public class PlayerIsConsoleException : Exception { }

    public sealed class RocketPlayer
    {
        private Player player;
        public Player Player
        {
            get { return player; }
        }

        public CSteamID CSteamID {
            get { return player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID;  }
        }

        public Exception PlayerIsConsoleException;

        private RocketPlayer(CSteamID cSteamID)
        {
            if (string.IsNullOrEmpty(cSteamID.ToString()) || cSteamID.ToString() == "0")
            {
                throw new PlayerIsConsoleException();
            }
            else
            {
                player = PlayerTool.getPlayer(cSteamID);
            }
        }

        public bool Equals(RocketPlayer p)
        {
            if ((object)p == null)
            {
                return false;
            }

            return (this.CSteamID.ToString() == p.CSteamID.ToString());
        }

        private RocketPlayer(Player p)
        {
            player = p;
        }

        public static RocketPlayer FromName(string name)
        {
            Player p = PlayerTool.getPlayer(name);
            if (p == null) return null;
            return new RocketPlayer(p);
        }

        public static RocketPlayer FromCSteamID(CSteamID cSteamID)
        {
            if (string.IsNullOrEmpty(cSteamID.ToString()) || cSteamID.ToString() == "0")
            {
                return null;
            }
            else
            {
                return new RocketPlayer(cSteamID);
            }
        }

        public static RocketPlayer FromPlayer(Player player)
        {
            return new RocketPlayer(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public static RocketPlayer FromSteamPlayer(SteamPlayer player)
        {
            return new RocketPlayer(player.SteamPlayerID.CSteamID);
        }

        public RocketPlayerFeatures Features
        {
            get { return player.gameObject.transform.GetComponent<RocketPlayerFeatures>(); }
        }

        public RocketPlayerEvents Events
        {
            get { return player.gameObject.transform.GetComponent<RocketPlayerEvents>(); }
        }

        public override string ToString()
        {
            return CSteamID.ToString();
        }

        public PlayerInventory Inventory
        {
            get { return player.Inventory; }
        }

        public bool GiveItem(ushort itemId,byte amount) {
            return ItemTool.tryForceGiveItem(player, itemId, amount);
        }

        public bool GiveVehicle(ushort vehicleId)
        {
            return VehicleTool.giveVehicle(player, vehicleId);
        }

        public void GiveZombie(byte amount)
        {
            ZombieTool.giveZombie(player,amount);
        }

        public List<Group> Groups
        {
            get
            {
                return RocketPermissionManager.GetGroups(this.CSteamID);
            }
        }

        public List<string> Permissions
        {
            get { 
                return RocketPermissionManager.GetPermissions(this.CSteamID); 
            }
        }

        public void Kick(string reason)
        {
            Steam.kick(this.CSteamID, reason);
        }

        public void Ban(string reason, uint duration)
        {
            Steam.ban(this.CSteamID, reason, duration);
        }

        public bool IsAdmin
        {
            get { 
                return player.SteamChannel.SteamPlayer.IsAdmin; 
            }
        }

        public void Admin(bool admin, RocketPlayer issuer = null)
        {
            if (admin)
            {
                if (issuer == null)
                {
                    SteamAdminlist.admin(this.CSteamID, new CSteamID(0));
                }
                else
                {
                    SteamAdminlist.admin(this.CSteamID, issuer.CSteamID);
                }
            }
            else
            {
                SteamAdminlist.unadmin(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
            }
        }

        public void Teleport(RocketPlayer target)
        {
            Vector3 d1 = target.player.transform.position;
            Vector3 vector31 = target.player.transform.rotation.eulerAngles;
            player.sendTeleport(d1, MeasurementTool.angleToByte(vector31.y));
        }

        public void Teleport(Vector3 position ,float rotation)
        {
            player.sendTeleport(position, MeasurementTool.angleToByte(rotation));
        }

        public Vector3 Position
        {
            get
            {
                return player.transform.position;
            }
        }

        public EPlayerStance Stance
        {
            get
            {
                return player.Stance.Stance;
            }
        }

        public float Rotation { 
            get { 
                return player.transform.rotation.eulerAngles.y; 
            } 
        }

        public bool Teleport(string nodeName)
        {
            Node node = LevelNodes.Nodes.Where(n => n.NodeType == ENodeType.Location && ((NodeLocation)n).Name.ToLower().Contains(nodeName)).FirstOrDefault();
            if (node != null)
            {
                Vector3 c = node.Position + new Vector3(0f, 0.5f, 0f);
                player.sendTeleport(c, MeasurementTool.angleToByte(Rotation));
                return true;
            }
            return false;
        }

        public byte Stamina
        {
            get
            {
                return player.PlayerLife.Stamina;
            }
        }

        public string CharacterName
        {
            get
            {
                return player.SteamChannel.SteamPlayer.SteamPlayerID.CharacterName;
            }
        }

        public string SteamName
        {
            get
            {
                return player.SteamChannel.SteamPlayer.SteamPlayerID.SteamName;
            }
        }

        public byte Infection
        {
            get { 
                return player.PlayerLife.Infection; 
            }
            set{
                player.PlayerLife.askDisinfect(100);
                player.PlayerLife.askInfect(value);
            }
        }

        public uint Experience
        {
            get
            {
                return player.Skills.Experience;
            }
            set
            {
                player.Skills.Experience = value;
                player.SteamChannel.send("tellExperience", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }
        }

        public byte Health
        {
            get {
                return player.PlayerLife.Health; 
            }
        }

        public byte Hunger
        {
            get{
                return player.PlayerLife.Hunger;
            }
            set
            {
                player.PlayerLife.askEat(100);
                player.PlayerLife.askStarve(value);
            }
        }

        public byte Thirst
        {
            get { 
                return player.PlayerLife.Thirst; 
            }
            set
            {
                player.PlayerLife.askDrink(100);
                player.PlayerLife.askDehydrate(value);
            }
        }

        public bool Broken
        {
            get { 
                return player.PlayerLife.Broken; 
            }
            set
            {
                player.PlayerLife.SteamChannel.send("tellBroken", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }
        }
        public bool Bleeding
        {
            get{
                return player.PlayerLife.Bleeding; 
            }
            set
            {
                player.PlayerLife.Bleeding = value;
                player.PlayerLife.SteamChannel.send("tellBleeding", ESteamCall.OWNER, ESteamPacket.UPDATE_TCP_BUFFER, new object[] { value });
            }
        }

        public bool Dead
        {
            get { 
                return player.PlayerLife.Dead; 
            }
        }

        public bool Freezing
        {
            get { 
                return player.PlayerLife.Freezing; 
            }
        }

        public void Heal(byte amount, bool? bleeding = null, bool? broken = null)
        {
            player.PlayerLife.askHeal(amount, bleeding != null ? bleeding.Value : player.PlayerLife.Bleeding, broken != null ? broken.Value : player.PlayerLife.Broken);
        }

        public void Suicide()
        {
            player.PlayerLife.askSuicide(player.SteamChannel.SteamPlayer.SteamPlayerID.CSteamID);
        }

        public void Damage(byte amount, Vector3 direction, EDeathCause cause, ELimb limb, CSteamID damageDealer)
        {
            player.PlayerLife.askDamage(amount, direction, cause, limb, damageDealer);
        }
    }
}