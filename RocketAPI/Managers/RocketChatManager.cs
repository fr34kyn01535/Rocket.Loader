using Rocket;
using Rocket.RocketAPI.Components;
using SDG;
using Steamworks;

namespace Rocket.RocketAPI
{
    public class RocketChatManager : RocketManagerComponent
    {
        static ChatManager chatmanager;
        public new void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
            chatmanager = gameObject.transform.GetComponent<ChatManager>();
        }

        public static void Say(string message, EChatMode chatmode = EChatMode.GLOBAL)
        {
            chatmanager.SteamChannel.send("tellChat", ESteamCall.OTHERS, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
        }

        public static void Say(CSteamID CSteamID, string message, EChatMode chatmode = EChatMode.SAY)
        {
            chatmanager.SteamChannel.send("tellChat", CSteamID, ESteamPacket.UPDATE_UDP_BUFFER, new object[] { CSteamID.Nil, (byte)chatmode, message });
        }
    }
}
