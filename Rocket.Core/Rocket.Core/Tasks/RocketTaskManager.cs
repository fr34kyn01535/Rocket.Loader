using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Rocket.Core.Tasks
{
    public sealed class RocketTaskManager : MonoBehaviour
    {
        private static RocketTaskManager Instance; 
        private Queue<Action> work = new Queue<Action>();

        private void Awake()
        {
#if DEBUG
            Logger.Log("Awake RocketTaskManager");
#endif
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