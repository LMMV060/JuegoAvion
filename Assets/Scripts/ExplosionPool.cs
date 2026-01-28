using System;
using System.Collections.Generic;
using UnityEngine;

public class ExplosionPool : MonoBehaviour
{
    [SerializeField] private GameObject explosionPrefab;
    [SerializeField] private int initialPoolSize = 10;
    private List<GameObject> _pool;

    private void Awake()
    {
        _pool = new List<GameObject>();

        for (int i = 0; i < initialPoolSize; i++)
        {
            GameObject explosion = Instantiate(explosionPrefab, transform, true);
            explosion.SetActive(false);
            _pool.Add(explosion);
        }
    }

    public GameObject GetExplosion()
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
