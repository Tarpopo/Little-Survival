using UnityEngine;

namespace Game.Scripts
{
    public class PrefabsChanger : MonoBehaviour
    {
        [SerializeField] private GameObject[] _prefabs;
        private int _currentIndex;

        private void Start() => ResetPrefabs();

        public void TryChangePrefab()
        {
            if (_currentIndex + 1 >= _prefabs.Length) return;
            _prefabs[_currentIndex].SetActive(false);
            _prefabs[++_currentIndex].SetActive(true);
        }

        public void ResetPrefabs()
        {
            foreach (var prefab in _prefabs) prefab.SetActive(false);
            _prefabs[0].SetActive(true);
            _currentIndex = 0;
        }
    }
}