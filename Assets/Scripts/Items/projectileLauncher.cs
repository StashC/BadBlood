using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
    
    [Header("Componenet References")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _team;

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
        GameObject projectile = Instantiate(_projectile, _firePoint.transform.position, _firePoint.transform.rotation);
        Projectile projectileScript = projectile.GetComponent<Projectile>();
        projectileScript._team = _team;

    }
}
