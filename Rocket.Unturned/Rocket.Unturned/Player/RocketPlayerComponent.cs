using Rocket.API;
using SDG;
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

        public void Awake()
        {
            player = RocketPlayer.FromPlayer(gameObject.transform.GetComponent<SDG.Player>());
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}