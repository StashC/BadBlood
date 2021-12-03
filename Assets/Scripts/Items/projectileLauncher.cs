using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
    
    [Header("Componenet References")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firePoint;

    public bool testFire;

    void Update()
    {
        if (testFire)
        {
            Shoot();
            testFire = false;
        }
    }
    public void Shoot()
    {
        Instantiate(_projectile, _firePoint.transform.position, _firePoint.transform.rotation);
    }
}
