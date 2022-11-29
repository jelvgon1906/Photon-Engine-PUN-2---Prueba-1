using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{
    private float speed = 20;
    private float activeTime = 3f;

    private void Start()
    {
        Destroy(gameObject, activeTime);
    }

    void Update()
    {
        transform.position += transform.forward * speed * Time.deltaTime;
    }
}
