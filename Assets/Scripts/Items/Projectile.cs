using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform _transform;
    [SerializeField] private float _speed;
    [SerializeField] private float _lifetime;

    private float timeLeft;

    // Start is called before the first frame update
    void Start()
    {
        timeLeft = _lifetime;
        _transform = GetComponent<Transform>();
    }
    // Update is called once per frame
    void Update()
    {
        if(timeLeft < 0) {
            Destroy(gameObject);
        }
        _transform.Translate(Vector2.right * Time.deltaTime * _speed);
        timeLeft -= Time.deltaTime;
    }
}
