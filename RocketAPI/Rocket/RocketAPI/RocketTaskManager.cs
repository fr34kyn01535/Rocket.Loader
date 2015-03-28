using Rocket.Components;
using Rocket.Logging;
using System;
using System.Collections.Generic;

namespace Rocket.RocketAPI
{
    public sealed class RocketTaskManager : RocketManagerComponent
    {
        private Queue<Action> work = new Queue<Action>();
        public static RocketTaskManager Instance;

        private new void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketTaskManager");
#endif
            base.Awake();
            Instance = this;
        }

        public static void Enqueue(Action a)
        {
            if (a != null) RocketTaskManager.Instance.enqueue(a);
        }

        private void enqueue(Action a)
        {
            lock (work)
            {
                work.Enqueue(a);
            }
        }

        private void FixedUpdate()
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