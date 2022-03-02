using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class rangedEnemy : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject _timerPrefab;

    [Header("Customization")]
    [Tooltip("Range the entity will react to the player")]
    [SerializeField] private float _activationRange = 4;
    [SerializeField] private float _attackingRange;
    [SerializeField] private float _attackCoolDown;
    [SerializeField] private float _idleSpeed;
    [SerializeField] protected float _moveSpeed;
    [Tooltip("how often unit will idle move per minute")]
    [SerializeField] private float _idleRate;
    [Tooltip("how long unit will idle move")]
    [SerializeField] private float _idleMoveDur;

    [Header("Performance")]
    [Tooltip("how often playerscan will be called per minute")]
    [SerializeField] private float _playerScanRate;

    //constants
    private GameObject _player;
    private enum State { Idle, Seeking, Attacking };
    private float _xScale;


    //changing
    protected float currSpeed;
    private Vector2 moveDir;
    [SerializeField] private State currState;
    private bool canIdleMove;
    protected bool canAttack;
    int LOSCounter = 0;


    public abstract void Attack();

    void Awake()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        _xScale = transform.localScale.x;

        //start initial timers
        InstantiateTimer(2f, true, DoIdleMove);
        playerScan();

        //initialize variables
        currState = State.Idle;
        canIdleMove = true;
        canAttack = true;
        Debug.Log(transform.localScale.x);
        Debug.Log(_xScale);
    }

    void Update() //make sure to delete the update method from the class which extends this class
    {
        drawDebugLines();
        if (currSpeed != 0 && currState != State.Attacking)
        {            
            transform.Translate(moveDir * Time.deltaTime * currSpeed);
        }
    }

    protected void StopAttack()
    {
        UpdateState();
        //Debug.Log("I stopped attacking");
        InstantiateTimer(_attackCoolDown, true, RefreshAttack);
    }

    private void RefreshAttack()
    {
        canAttack = true;
    }

    void UpdateState()
    {
        //creates layermask which ignores NPCs layer, enemies can see through enemies.
        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, ~(1 << LayerMask.NameToLayer("NPCs"))); 
        //if player is not in LOS
        if (!hit.collider.gameObject.CompareTag("Player"))
        {
            LOSCounter++;
            //if out of line of sight for more than x playerScan(), idle.  
            if (LOSCounter > 4)
            {
                currState = State.Idle;
                DoIdleMove();
            }
            return;
        }
        LOSCounter = 0;

        //If we have line of sight.
        //If in range && can attack, attack.  If can't attack, do nothing.  Always face player.
        if(hit.distance <= _attackingRange)
        {
            FacePlayer();
            if (canAttack)
            {
                currState = State.Attacking;
                Attack();
            }
            return;
        }

        //we are within activationRange
        if(hit.distance <= _activationRange)
        {
            currState = State.Seeking;
            FacePlayer();
            DoSeeking();
            return;
        }

        //if we are further than activation range (outside of attacking and seeking), idle.
        if (canIdleMove) currState = State.Idle; DoIdleMove();
    }

    private void playerScan()
    {
        UpdateState();
        InstantiateTimer(60 / _playerScanRate, true, playerScan);
    }

    void FacePlayer()
    {
        if (_player.transform.position.x > transform.position.x) //player to the right, face right
        {
            transform.localScale = new Vector3(_xScale, transform.localScale.y, transform.localScale.z);
        }
        else //player to the left, face left
        {
            transform.localScale = new Vector3(_xScale * -1, transform.localScale.y, transform.localScale.z);
        }

    }
    void DoSeeking()
    {
        if (currState == State.Seeking)
        {
            if (_player.transform.position.x > transform.position.x) //player to the right -> move right
            {
                transform.localScale = new Vector3(_xScale, transform.localScale.y, transform.localScale.z);
                moveDir = new Vector2(1, 0);
            }
            else //player to the left -> move left
            {
                transform.localScale = new Vector3(_xScale*-1, transform.localScale.y, transform.localScale.z);
                moveDir = new Vector2(-1, 0);
            }
            currSpeed = _moveSpeed;
        }
    }

    void DoIdleMove()
    {
        //Every x seconds, move either left or right for y seconds  If fleeing do nothing
        if (currState == State.Idle && canIdleMove)
        {
            //Debug.Log("Start idle move");
            currSpeed = _idleSpeed;
            if (Random.Range(0, 2) % 2 == 1)
            {
                //move and face left
                transform.localScale = new Vector3(_xScale * -1, transform.localScale.y, transform.localScale.z);
                moveDir = new Vector2(-1, 0);
            }
            else
            {
                //move and face right
                transform.localScale = new Vector3(_xScale, transform.localScale.y, transform.localScale.z);
                moveDir = new Vector2(1, 0);
            }
            //timer to stop moving
            canIdleMove = false;
            InstantiateTimer(_idleMoveDur, true, StopIdleMove);
        }
    }
    void StopIdleMove()
    {
        if (currState == State.Idle)
        {
            //Debug.Log("Stopped idle moving");
            currSpeed = 0; //stop moving
            InstantiateTimer(60 / _idleRate, true, resetIdleMove); //move again after correct delay
        }
    }
    
    void resetIdleMove()
    {
        canIdleMove = true;
        DoIdleMove();
    }
      

    private void InstantiateTimer(float duration, bool selfDestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(_timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfDestruct);
    }

    private void drawDebugLines()
    {
        //green if sightline but out of range
        //yellow if in seeking range and sightline
        //red if in Attacking Range
        //black sightline obstructed

        RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, ~(1 << LayerMask.NameToLayer("NPCs"))); //creates layermask which ignores NPCs layer.
        if (hit.collider != null)
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                if (hit.distance <= _attackingRange)
                {
                    //in attacking range
                    Debug.DrawLine(transform.position, _player.transform.position, Color.red);
                }
                if (hit.distance <= _activationRange && hit.distance > _attackingRange)
                {
                    //in seeking range, outside attack range
                    Debug.DrawLine(transform.position, _player.transform.position, Color.yellow);
                }
                if(hit.distance > _activationRange)
                {
                    Debug.DrawLine(transform.position, _player.transform.position, Color.green);
                }

            } else //object between enemy and player
            {
                Debug.DrawLine(transform.position, _player.transform.position, Color.black);
            }
        }
    }
}
