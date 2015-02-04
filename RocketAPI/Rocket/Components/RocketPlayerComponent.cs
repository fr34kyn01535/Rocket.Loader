using SDG;
using UnityEngine;

namespace Rocket.Components
{
    public class RocketPlayerComponent : MonoBehaviour
    {
        public Player PlayerInstance;

        public void Awake()
        {
            PlayerInstance = gameObject.transform.GetComponent<Player>();
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}