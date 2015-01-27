using Rocket.RocketAPI.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using UnityEngine;

namespace Rocket.RocketAPI.Managers
{
    public class RocketTaskManager : RocketManagerComponent
    {

        private Queue<Action> work;
        public static RocketTaskManager Instance;

        public RocketTaskManager()
        {
            work = new Queue<Action>();
            Instance = this;
        }

        public static void Enqueue(Action a)
        {
            if(a!=null)RocketTaskManager.Instance.enqueue(a);
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
                        try
                        {
                            a();
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Log(ex);
                        }
                    }
                    work.Clear();
                }
            }
        }
    }
}
