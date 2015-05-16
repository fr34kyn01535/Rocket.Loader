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
        public static void Say(string message, EChatMode chatmode = EChatMode.GLOBAL)
        {
            Logger.Log("Broadcast: " + message);
            foreach (string m in wrapMessage(message))
            {
                ChatManager.Instance.SteamChannel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, m });
            }
        }

        public static void Say(RocketPlayer player, string message, EChatMode chatmode = EChatMode.SAY)
        {
            if (player == null)
            {
                Logger.Log(message);
             }
            else {
                foreach (string m in wrapMessage(message))
                {
                    ChatManager.Instance.SteamChannel.send("tellChat", player.CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, m });
                }
            }
        }

        public static void Say(CSteamID CSteamID, string message, EChatMode chatmode = EChatMode.SAY)
        {
            if (CSteamID == null || CSteamID.ToString() == "0")
            {
                Logger.Log(message);
            }
            else
            {
                foreach (string m in wrapMessage(message))
                {
                    ChatManager.Instance.SteamChannel.send("tellChat", CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, m });
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