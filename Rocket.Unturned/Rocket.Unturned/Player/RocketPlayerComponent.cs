using Rocket.API;
using SDG.Unturned;
using UnityEngine;

namespace Rocket.Unturned.Player
{
    public class RocketPlayerComponent : MonoBehaviour
    {
        private RocketPlayer player;
        public RocketPlayer Player
        {
            get { return player; }
        }

        private void Awake()
        {
            player = RocketPlayer.FromPlayer(gameObject.transform.GetComponent<SDG.Unturned.Player>());
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}