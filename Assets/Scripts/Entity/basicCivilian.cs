using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class basicCivilian : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject _timerPrefab;

    [Header("Customization")]
    [Tooltip("Range the entity will react to the player")]
    [SerializeField] private float _activationRange = 4;
    [SerializeField] private float _stopFleeDist;
    [SerializeField] private float _idleSpeed;
    [SerializeField] private float _fleeSpeed;
    [Tooltip("how close player should be till fleespeed is maxed")]
    [SerializeField] private float _maxFleeSpeedDist;
    [Tooltip("how often unit will idle move per minute")]
    [SerializeField] private float _idleRate;
    [Tooltip("how long unit will idle move")]
    [SerializeField] private float _idleMoveDur;

    [Header("Performance")]
    [Tooltip("how often playerscan will be called per minute")]
    [SerializeField] private float _playerScanRate;
    [SerializeField] private bool debug = false;


    //constants
    private GameObject _player;
    private int _raymask;

    //changing
    private float currSpeed;
    private float distanceToPlayer;
    private Vector2 moveDir;

    private enum State { Idle, Fleeing };
    private State currState;

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        InstantiateTimer(2f, true, DoIdleMove);
        currState = State.Idle;
        playerScan();
        _raymask = ~(1 << LayerMask.NameToLayer("NPCs") | 1 << LayerMask.NameToLayer("Ignore Raycast"));

    }

void Update()
    {
       if(debug) drawDebugLines();

       if (currSpeed == 0) return;

       if(currState == State.Idle)
        {
            transform.Translate(moveDir * Time.deltaTime * currSpeed);
        }

       if(currState == State.Fleeing)
        {
            distanceToPlayer = Vector3.Distance(_player.transform.position, gameObject.transform.position);
            //make faster if closer to player
            float fleeMultiplier = 1.8f;
            if (distanceToPlayer >= _maxFleeSpeedDist)
            {
                //fleeing and we are not super close yet, flee slowly.
                transform.Translate(moveDir * Time.deltaTime * currSpeed);
            }
            else
            {
                //fleeing but close, player should run;
                transform.Translate(moveDir * Time.deltaTime * currSpeed * fleeMultiplier);
            }
        }
    }
    void DoIdleMove()
    {
        //Every x seconds, move either left or right for y seconds  If fleeing do nothing
        if (currState == State.Idle)
        {
            currSpeed = _idleSpeed;
            if(Random.Range(0,2)%2 == 1)
            {
                moveDir = new Vector2(-1, 0);
            } else
            {
                moveDir = new Vector2(1, 0);
            }
            //timer to stop moving
            InstantiateTimer(_idleMoveDur, true, StopIdleMove);
        }              
    }
    void StopIdleMove()
    {
        if (currState == State.Idle)
        {
            currSpeed = 0; //stop moving
            InstantiateTimer(60 / _idleRate, true, DoIdleMove); //move again after correct delay
        }
    }
    private void DoFleeing()
    {
        //run away from player.
        currState = State.Fleeing;
        if(_player.transform.position.x > transform.position.x) //player to the right -> move left
        {
            moveDir = new Vector2(-1, 0);
        }
        else //player to the left -> move right
        {
            moveDir = new Vector2(1, 0);
        }
        currSpeed = _fleeSpeed;
    }

    private void stopFleeing()
    {
        currState = State.Idle;
        currSpeed = 0;
        InstantiateTimer(60 / _idleRate, true, DoIdleMove); //start idle movement
    }
    private bool CanSeePlayer()
    {
        bool returnValue = false;
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, _raymask); //creates layermask which ignores NPCs layer.
        if (hit.collider != null)
        {
            Debug.Log(hit.transform.name);
            if (hit.collider.gameObject.CompareTag("Player")) //if we hit the playeer
            {
                returnValue = true;
            }
        }
        return returnValue;
    }

    private void UpdateState()
    {
        distanceToPlayer = Vector3.Distance(_player.transform.position, gameObject.transform.position);
        //if close enough and has sightline, run away
        if (distanceToPlayer <= _activationRange && CanSeePlayer())
        {
            DoFleeing();
        }
        //if too far away or can not see the player, stop fleeing.
        if ((distanceToPlayer >= _stopFleeDist || !CanSeePlayer()) && currState == State.Fleeing)
        {
            stopFleeing();
        }

    }
    private void playerScan()
    {
        UpdateState();
        InstantiateTimer(60/_playerScanRate, true, playerScan);
    }





    private void drawDebugLines()
    {
        //color line green if player is in activation range and see it
        //color line red if player is outside activation range or dont see it
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, ~(1 << LayerMask.NameToLayer("NPCs"))); //creates layermask which ignores NPCs layer.
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player") && hit.distance < _activationRange)
            {
                //idle and in activation range
                Debug.DrawLine(transform.position, _player.transform.position, Color.green);
            } else
            {
                Debug.DrawLine(transform.position, _player.transform.position, Color.black);
            }
        }  
    }    

    private void InstantiateTimer(float duration, bool selfDestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(_timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfDestruct);
    }
}
