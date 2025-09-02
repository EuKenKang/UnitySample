using System.Collections.Generic;
using UnityEngine;

namespace Script
{
    public class PoolManager : Singleton<PoolManager>
    {
        public GameObject[] prefabs;
        private List<GameObject>[] _pools;

        private void Awake()
        {
            _pools = new List<GameObject>[prefabs.Length];
            for(int i = 0; i < prefabs.Length; i++)
            {
                _pools[i] = new List<GameObject>();
            }
        }

        public GameObject Get(int index)
        {
            GameObject select = null;
        
            foreach(GameObject item in _pools[index])
            {
                if (!item.activeSelf)
                {
                    select = item;
                    select.SetActive(true);
                    break;
                }
            }

            if (!select)
            {
                select = Instantiate(prefabs[index], transform);
                _pools[index].Add(select);
            }

            return select;
        }
    }
}
