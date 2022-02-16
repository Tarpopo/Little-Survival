using UnityEngine;

namespace SquareDino.Scripts.MyAnalytics
{

    public class MyTenjin : MonoBehaviour
    {
        [SerializeField] private string publisherKey = "Q2VWXVRY1DWGVPZGTJENMPVHXEJAFQRX";

#if !UNITY_EDITOR && FLAG_TENJIN
        private void Start()
        {

            var instance = Tenjin.getInstance(publisherKey);
            instance.Connect();
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            var instance = Tenjin.getInstance(publisherKey);
            if (pauseStatus)
            {
                //do nothing
            }
            else
            {
                instance.Connect();
            }
        }
#endif
    }
}