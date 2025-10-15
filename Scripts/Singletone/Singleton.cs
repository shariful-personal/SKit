using UnityEngine;

namespace SKit
{
    public class Singleton<T> : MonoBehaviour
    {
        private static T instance;

        private void Awake()
        {
            if (instance == null)
            {
                instance = GetComponent<T>();
            }
            else
            {
                Destroy(gameObject);
            }

            Awake2();
        }

        public virtual void Awake2() { }

        public static T Instance
        {
            get
            {
                return instance;
            }
        }

        public void Delete()
        {
            instance = default;
            Destroy(gameObject);
        }
    }

    public abstract class SingletonClass<T> where T : class, new()
    {
        private static T instance;

        public static T Instance
        {
            get
            {
                instance ??= new T();
                return instance;
            }
        }

        public void Delete()
        {
            instance = null;
        }
    }
}