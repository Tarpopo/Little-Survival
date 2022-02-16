using System.Collections.Generic;
using UnityEngine;

namespace DefaultNamespace
{
    public class Pool:MonoBehaviour
    {
        private Transform parentPool;
        private Dictionary<int,Stack<GameObject>>cachedObjects=new Dictionary<int, Stack<GameObject>>();
        private Dictionary<int,int>cachedIds=new Dictionary<int, int>();
        public Pool PopulateWith(GameObject prefab,int amount)
        {
            var key = prefab.GetInstanceID();
            var stack=new Stack<GameObject>(amount);
            cachedObjects.Add(key,stack);
            
            for (int i = 0; i < amount; i++)
            {
                var go = Populate(prefab, Vector3.zero, Quaternion.identity, parentPool);
                go.SetActive(false);
                cachedIds.Add(go.GetInstanceID(),key);
                cachedObjects[key].Push(go);
            }
            return this;
        }
        public Pool PopulateWith<T>(GameObject prefab,int amount,Dictionary<GameObject,T>dictionary)
        {
            var key = prefab.GetInstanceID();
            print(key);
            var stack=new Stack<GameObject>(amount);
            cachedObjects.Add(key,stack);
            for (int i = 0; i < amount; i++)
            {
                var go = Populate(prefab,Vector3.zero, Quaternion.identity, parentPool);
                go.SetActive(false);
                dictionary.Add(go,go.GetComponent<T>());
                cachedIds.Add(go.GetInstanceID(),key);
                cachedObjects[key].Push(go);
            }
            return this;
        }
        public void SetParent(Transform parent)
        {
            parentPool = parent;
        }
        public GameObject Spawn(GameObject prefab,Vector3 position=default(Vector3),Quaternion rotation=default(Quaternion),Transform parent=null,bool setActive=true)
        {
            var key=prefab.GetInstanceID();
            Stack<GameObject> stack;
            var stackted = cachedObjects.TryGetValue(key, out stack);
            if (stackted && stack.Count > 0)
            {
                var transform = stack.Pop().transform;
                transform.rotation = rotation;
                transform.gameObject.SetActive(setActive);
                if (parent)
                {
                    transform.position = position;
                    transform.SetParent(parent);
                }
                else
                {
                    transform.localPosition = position;
                    transform.SetParent(parentPool);
                }
                var poolable = transform.GetComponent<IPoolable>();
                if(poolable!=null)poolable.OnSpawn();
                return transform.gameObject;
            }
            if(!stackted)cachedObjects.Add(key,new Stack<GameObject>());

            var createdPrefab = Populate(prefab, position, rotation, parent);
            cachedIds.Add(createdPrefab.GetInstanceID(),key);
            return createdPrefab; 
        }
        public void Despawn(GameObject go)
        {
            go.SetActive(false);
            cachedObjects[cachedIds[go.GetInstanceID()]].Push(go);
            var poolable = go.GetComponent<IPoolable>();
            poolable?.OnDespawn();
            if(parentPool!=null)go.transform.SetParent(parentPool);
        }
        public void Dispose()
        {
            parentPool = null;
            cachedObjects.Clear();
            cachedIds.Clear();   
        }
        public GameObject Populate(GameObject prefab, Vector3 position = default(Vector3),
            Quaternion rotation = default(Quaternion), Transform parent = null)
        {
            var go = Object.Instantiate(prefab, position, rotation, parent).transform;
            if (parent == null)
            {
                go.position = position;
                go.transform.SetParent(parentPool);
            }
            else go.localPosition = position;
            return go.gameObject;
        }
    }
}