using Rocket.Core.Logging;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Rocket.Core.Tasks
{
    public class RocketTask{
        public string Name;
        public Action Action;
        public int Delay;
        public int? Interval;
        public DateTime DueTime;
        public RocketTask(string name,Action action,int delay,int? interval)
        {
            Name = name;
            Action = action;
            Delay = delay;
            Interval = interval;
            DueTime = DateTime.Now.AddMilliseconds(delay);
        }
    }

    public sealed class RocketTaskManager : MonoBehaviour
    {
        private static RocketTaskManager Instance;
        private List<RocketTask> work = new List<RocketTask>();

        private void Awake()
        {
#if DEBUG
            Logger.Log("RocketTaskManager > Awake");
#endif
            Instance = this;
        }

        public static void Enqueue(Action action, string name = "", int delay = 0, int? interval = null)
        {
            RocketTaskManager.Instance.enqueue(new RocketTask(name, action, delay, interval));
        }

        public static void Dequeue(string name)
        {
            RocketTaskManager.Instance.dequeue(name);
        }

        private void dequeue(string name)
        {
            lock (work)
            {
                work.RemoveAll(w => w.Name == name);
            }
        }

        private void enqueue(RocketTask task)
        {
            lock (work)
            {
                work.Add(task);
            }
        }

        private void FixedUpdate()
        {
            if (work.Count > 0)
            {
                lock (work)
                {
                    for(int i = 0;i<work.Count;i++)
                    {
                        try
                        {
                            RocketTask task  = work[i];
                            if(task.DueTime < DateTime.Now){
                                try
                                {
                                    task.Action();
                                }
                                catch (Exception ex)
                                {
                                    if (String.IsNullOrEmpty(task.Name))
                                    {
                                        Logger.LogError("Error while executing anonymous action: " + ex.ToString());
                                    }
                                    else
                                    {
                                        Logger.LogError("Error while executing named action " + task.Name + ": " + ex.ToString());
                                    }
                                }
                                finally
                                {
                                    if (task.Interval.HasValue)
                                    {
                                        task.DueTime = DateTime.Now.AddMilliseconds(task.Interval.Value);
                                    }
                                    else
                                    {
                                        work.RemoveAt(i);
                                    }
                                }
                            }
                        }
                        catch (System.Exception ex)
                        {
                            Logger.Log(ex);
                        }
                    }
                }
            }
        }
    }
}