using UnityEngine;
using System.Collections;

namespace Helpers
{
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        private static T _instance = default(T);

        public static T Instance
        {
            get
            {
                return _instance;
            }
            private set { }
        }

        protected virtual void Awake()
        {
            if(_instance == null)
            {
                _instance = this as T;
            }
            else
            {
                Destroy(gameObject);
                //Debug.LogError("There can only be one of these");
            }
        }
    }
}

