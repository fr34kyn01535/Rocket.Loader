using SDG;
using Steamworks;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using UnityEngine;

namespace Rocket
{
    public static class Chat
    {
        public static void Say(string message, EChatMode chatmode = EChatMode.GLOBAL)
        {
            ChatManager.Instance.SteamChannel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
        }

        public static void Say(CSteamID cSteamID,string message,EChatMode chatmode = EChatMode.GLOBAL)
        {
            ChatManager.Instance.SteamChannel.send("tellChat", cSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
        }
    }
}
