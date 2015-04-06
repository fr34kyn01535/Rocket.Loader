using Rocket.Components;
using Rocket.Logging;
using SDG;
using Steamworks;

namespace Rocket.RocketAPI
{
    public sealed class RocketChatManager : RocketManagerComponent
    {
        private static ChatManager chatmanager;

        private new void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketChatManager");
#endif
            base.Awake();
            chatmanager = gameObject.transform.GetComponent<ChatManager>();
        }

        public static void Say(string message, EChatMode chatmode = EChatMode.GLOBAL)
        {
            Logger.Log("Broadcast: " + message);
            ChatManager.Instance.SteamChannel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
        }

        public static void Say(RocketPlayer player, string message, EChatMode chatmode = EChatMode.SAY)
        {
            if (player.IsConsole)
            {
                Logger.Log(message);
             }
            else {
                ChatManager.Instance.SteamChannel.send("tellChat", player.CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
            }
        }

        public static void Say(CSteamID CSteamID, string message, EChatMode chatmode = EChatMode.SAY)
        {
            if (CSteamID == null || CSteamID.ToString() == "0")
            {
                Logger.Log(message);
             }
            else {
                ChatManager.Instance.SteamChannel.send("tellChat", CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
            }
        }
    }
}