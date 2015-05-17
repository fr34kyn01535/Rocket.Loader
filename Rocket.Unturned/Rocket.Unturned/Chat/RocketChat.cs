using Rocket.API;
using Rocket.Core.Logging;
using Rocket.Unturned.Player;
using SDG;
using Steamworks;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Unturned
{
    public sealed class RocketChat : MonoBehaviour
    {
        public static Color GetColorFromName(string colorName, Color fallback)
        {
            switch (colorName.Trim().ToLower())
            {
                case "black": return Color.black;
                case "blue": return Color.blue;
                case "clear": return Color.clear;
                case "cyan": return Color.cyan;
                case "gray": return Color.gray;
                case "green": return Color.green;
                case "grey": return Color.grey;
                case "magenta": return Color.magenta;
                case "red": return Color.red;
                case "white": return Color.white;
                case "yellow": return Color.yellow;
            }
            return fallback;
        }

        public static Color GetColorFromRGB(byte R,byte G,byte B,short A = 100)
        {
            return new Color((1f / 255f) * R, (1f / 255f) * G, (1f / 255f) * B,(1f/100f) * A);
        }

        public static void Say(string message)
        {
            Say(message, Color.white);
        }

        public static void Say(string message,Color color)
        {
            Logger.Log("Broadcast: " + message);
            foreach (string m in wrapMessage(message))
            {
                ChatManager.Instance.SteamChannel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)EChatMode.GLOBAL,color, m });
            }
        }
       
        public static void Say(RocketPlayer player, string message)
        {
            if (player == null)
            {
                Logger.Log(message);
             }
            else {
                Say(player.CSteamID, message, Color.white);
            }
        }

        public static void Say(RocketPlayer player, string message, Color color)
        {
            if (player == null)
            {
                Logger.Log(message);
            }
            else
            {
                Say(player.CSteamID, message, Color.white);
            }
        }

        public static void Say(CSteamID CSteamID, string message)
        {
            Say(CSteamID, message, Color.white);
        }

        public static void Say(CSteamID CSteamID, string message, Color color)
        {
            if (CSteamID == null || CSteamID.ToString() == "0")
            {
                Logger.Log(message);
            }
            else
            {   
                foreach (string m in wrapMessage(message))
                {
                    ChatManager.Instance.SteamChannel.send("tellChat", CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)EChatMode.SAY,color, m });
                }
            }
        }

         public static List<string> wrapMessage(string text)
         {
             if (text.Length == 0) return new List<string>();
             string[] words = text.Split(' ');
             List<string> lines = new List<string>();
             string currentLine = "";
             int maxLength = 90;
             foreach (var currentWord in words)
             {
  
                 if ((currentLine.Length > maxLength) ||
                     ((currentLine.Length + currentWord.Length) > maxLength))
                 {
                     lines.Add(currentLine);
                     currentLine = "";
                 }
  
                 if (currentLine.Length > 0)
                     currentLine += " " + currentWord;
                 else
                     currentLine += currentWord;
  
             }
  
             if (currentLine.Length > 0)
                 lines.Add(currentLine);
                 return lines;
            }
    }
}