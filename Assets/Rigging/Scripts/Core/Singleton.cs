using System;

namespace Rigging.Core
{
    public class Singleton<T> : IDisposable where T : new()
    {
        private static T _instance;

        public static T I
        {
            get
            {
                _instance ??= new T();
                return _instance;
            }
        }

        protected virtual void OnDispose()
        {

        }

        public void Dispose()
        {
            OnDispose();
        }
    }
}