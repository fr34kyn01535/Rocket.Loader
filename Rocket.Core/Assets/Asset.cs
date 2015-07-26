using System;

namespace Rocket.Core.Assets
{
    public class Asset<T> : IAsset<T> where T : class
    {
        protected T instance = null;

        public T Instance
        {
            get
            {
                if (instance == null) Load();
                return instance;
            }
            set
            {
                if (value != null)
                {
                    instance = value;
                    Save(instance);
                }
            }
        }

        public virtual T Save(T instance = null)
        {
            return instance;
        }

        public virtual void Load(AssetLoaded<T> callback = null, bool update = false)
        {
            callback(this);
        }
        public virtual void Reload()
        {
            Load(null, true);
        }

        public virtual void Unload(AssetUnloaded<T> callback = null)
        {
            callback(this);
        }
    }
}
