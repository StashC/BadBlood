using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class simpleEnemy : Enemy
{
    [SerializeField] private projectileLauncher weapon;

    void Start()
    {
        weapon = GetComponentInChildren<projectileLauncher>();
    }
    public override void Attack()
    {
        base.canAttack = false;
        //code the actual attack in here, then after attack is finished call stopAttack
        //Debug.Log("enemy attacked lol");
        Shoot();

        base.StopAttack();
        
    }   

    private void Shoot() {
        weapon.Shoot();
    }

}
