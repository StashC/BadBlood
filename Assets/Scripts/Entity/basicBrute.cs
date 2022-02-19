using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class basicBrute : bruteEnemy
{
    [SerializeField] private float _chargeDist;
    [SerializeField] private float _chargeSpeed;
    [SerializeField] private float _windUpLength;
    [SerializeField] private float _stunDuration;
    
    public override void Attack()
    {
        base.canAttack = false;
        InstantiateTimer(_windUpLength, true, Charge);
        //do wind up stuff
    }   

    private void Charge()
    {
        Debug.Log("enemy charged lol");
        //possibly stop windup stuff

        //do charging stuff, set sprite etc.  
        base.currSpeed = _chargeSpeed;  // will trigger movement in base.Update()
        InstantiateTimer(_chargeDist / _chargeSpeed, true, StopCharge);

    }

    private void StopCharge()
    {
        //stop charging stuff, set sprite to stunned or idle.
        Debug.Log("stopcharge called");

        base.currSpeed = 0f;
        InstantiateTimer(_stunDuration, true, stopAttack);
    }

    private void stopAttack()
    {
        Debug.Log("stopattack called");
        base.StopAttack();
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    private void InstantiateTimer(float duration, bool selfDestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(base._timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfDestruct);
    }

}
