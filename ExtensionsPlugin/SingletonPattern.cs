
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace yours_indie_gameDev.Plugin.Extensions
{
    [DefaultExecutionOrder(-2)]
    public class Singleton<T> : MonoBehaviour where T : Component
    {
        public static T instance { get; private set; }

        public virtual void Awake()
        {
            if (instance == null)
            {
                instance = this as T;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

}

