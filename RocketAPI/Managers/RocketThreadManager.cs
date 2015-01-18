using Rocket.RocketAPI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Rocket.RocketAPI.Managers
{
    public class RocketThreadManager : RocketManagerComponent
    {

        private Queue<Action> work;
        public static RocketThreadManager Instance;

        public RocketThreadManager()
        {
            work = new Queue<Action>();
            Instance = this;
        }

        public static void Enqueue(Action a)
        {
            RocketThreadManager.Instance.enqueue(a);
        }

        private void enqueue(Action a)
        {
            lock (work)
            {
                work.Enqueue(a);
            }
        }

        void FixedUpdate()
        {
            if (work.Count > 0)
            {
                lock (work)
                {
                    foreach (var a in work)
                    {
                        a();
                    }
                    work.Clear();
                }
            }
        }
    }
}
