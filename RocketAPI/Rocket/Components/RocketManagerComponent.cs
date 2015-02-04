using UnityEngine;

namespace Rocket.Components
{
    public class RocketManagerComponent : MonoBehaviour
    {
        public virtual void Awake()
        {
            DontDestroyOnLoad(transform.gameObject);
        }
    }
}