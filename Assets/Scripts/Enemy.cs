using System;
using UnityEngine;

public class Enemy : MonoBehaviour

{
    [SerializeField] private Vector2 speed;
    void Update()
    {
        // UPDATE POSITION
        transform.Translate(speed * Time.deltaTime);
    }

    public void HitByPlayerShot()
    {
        Destroy(gameObject);
    }
}
