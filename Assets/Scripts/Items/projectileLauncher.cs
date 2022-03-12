using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class projectileLauncher : MonoBehaviour
{
    
    [Header("Componenet References")]
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform _firePoint;
    [SerializeField] private int _team;
    [SerializeField] private PlayerCharacterAnimController _animController;

    public bool testFire;

    void Update()
    {
        if (testFire)
        {
            Shoot();
            _animController.SetProjectileDirection(new Vector2(Mathf.Cos(_firePoint.transform.rotation.eulerAngles.z),0.0f));
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
