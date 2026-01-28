using System;
using System.Collections.Generic;
using UnityEngine;

public class MisilPool : MonoBehaviour
{
    [SerializeField] private GameObject firePrefab;
    [SerializeField] private int initialPoolSize = 5;
    private List<GameObject> _pool;

    private void Awake()
    {
        _pool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject fire = Instantiate(firePrefab, transform, true);
            fire.SetActive(false);
            _pool.Add(fire);
        }
    }

    public GameObject GetBullet()
    {
        for (int i = 0; i < _pool.Count; i++)
        {
            if (!_pool[i].activeSelf)
            {
                _pool[i].SetActive(true);
                return _pool[i];
            }
        }
        
        return null;
    }
}
