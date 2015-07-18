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

        public void Reload(AssetLoaded<T> callback = null)
        {
            Unload((asset) => { asset.Load(callback, true); });
        }

        public virtual T Save(T instance = null)
        {
            return instance;
        }

        public virtual void Load(AssetLoaded<T> callback = null, bool update = false)
        {

        }

        public virtual void Unload(AssetUnloaded<T> callback = null)
        {

        }
    }
}
