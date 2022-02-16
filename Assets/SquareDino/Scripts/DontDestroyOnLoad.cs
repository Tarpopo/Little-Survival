using UnityEngine;

namespace SquareDino.Scripts
{
    public class DontDestroyOnLoad : MonoBehaviour
    {
        private static DontDestroyOnLoad _instance;
        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }
    }
}