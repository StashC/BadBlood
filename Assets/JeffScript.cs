using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class JeffScript : MonoBehaviour
{
    [Header("Componenet References")]
    [SerializeField] private GameObject _moneyBag;
    [Tooltip("where thrown objects are instantiated at")]
    [SerializeField] private Transform _throwPoint;
    [SerializeField] private GameObject _timerPrefab;
    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private projectileLauncher _weapon;


    [Header("General")]
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _vertSpeed;
    [SerializeField] private float _maxVertDist;
    [SerializeField] private float _minVertDist;
    [SerializeField] private float _gunCooldown;

    private GameObject _player;

    [Header("Lightning Attack")]
    [SerializeField] private GameObject _marker;
    [SerializeField] private GameObject _lightningImpactParticles;
    [SerializeField] private GameObject _lightningSprite;
    [SerializeField] private int _minCount;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _lightningSpread;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _lightningDamage;
    [SerializeField] private float _hitboxWidth;
    [SerializeField] private float _lightningCooldown;

    [Header("MoneyBag Bomb")]
    [SerializeField] private float _bombCooldown;
    private bool canBomb;
    private float bombCooldownTimer;

    //Dynamic Variables 
    private int _floor;
    private int _projectileMask;
    private List<Vector2> lightningPositions;
    private float moveDir;
    private float gunTimer;
    private bool canShoot;
    private float lightningTimer;

    // Start is called before the first frame update
    void Start()
    {
        _rigidbody = gameObject.GetComponent<Rigidbody2D>();
        _player = GameObject.FindGameObjectWithTag("Player");
        lightningPositions = new List<Vector2>();

        //layermask of only floor layer. only collide with floor.
        _floor = 1 << LayerMask.NameToLayer("Floor");

        //Only collide with floor or player or cover
        int plyr = 1 << LayerMask.NameToLayer("Player");
        int cover = 1 << LayerMask.NameToLayer("Cover");

        _projectileMask = plyr | cover | _floor;

        canBomb = true;
        lightningTimer = _lightningCooldown;

    }

    // Update is called once per frame
    void Update()
    {
        //MOVEMENT_____________________________________________
        //change movement from left to right.
        if (_player.transform.position.x + 3 < transform.position.x)
        {
            moveDir = -1;
        }
        else if(_player.transform.position.x - 3 > transform.position.x)
        {
            moveDir = 1;
        }

        //Boss movement
        if (Mathf.Abs(_rigidbody.velocity.x) < _horizontalSpeed)
        {
            transform.Translate(Vector2.right * moveDir * Time.deltaTime * _horizontalSpeed);
        }

        //We want the vertical gap between the boss and the player to be between [_minVertDist, _maxVertDist]
        //gap is too small (< _minVertDist)
        if (Mathf.Abs(transform.position.y - _player.transform.position.y) < _minVertDist)
        {
            if(_player.transform.position.y > transform.position.y)
            {//boss is below the player
                transform.Translate(Vector2.down * _vertSpeed * Time.deltaTime);
            } else
            {
                transform.Translate(Vector2.up * _vertSpeed * Time.deltaTime);
            }
        }
        //gap is too big (> _maxVetDist)
        if (Mathf.Abs(transform.position.y - _player.transform.position.y) > _maxVertDist)
        {
            if (_player.transform.position.y < transform.position.y)
            {//boss is below the player
                transform.Translate(Vector2.down * _vertSpeed * Time.deltaTime);
            }
            else
            {
                transform.Translate(Vector2.up * _vertSpeed * Time.deltaTime);
            }
        }

        if (gunTimer > 0 && canShoot == false)
        {
            gunTimer -= Time.deltaTime;
        } else
        {
            canShoot = true;
        }

        if (canShoot)
        {
            RaycastHit2D hit = Physics2D.Linecast(transform.position, _player.transform.position, _projectileMask);
            if (hit.transform.CompareTag("Player"))
            {
                canShoot = false;
                Shoot();
                gunTimer = _gunCooldown;
            }
        }

        //bomb attack
        if (canBomb)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, _projectileMask);
            if (hit)
            {
                if (hit.transform.CompareTag("Player"))
                {
                    moneyBagAttack();
                }
            }
        }

        if (bombCooldownTimer > 0 && canBomb == false)
        {
            bombCooldownTimer -= Time.deltaTime;
        } else
        {
            canBomb = true;
        }

        //Lightning Attack
        if (lightningTimer > 0)
        {
            lightningTimer -= Time.deltaTime;
        }
        else
        {
            spawnLightningAttack();
            lightningTimer = _lightningCooldown;
        }

    }

    void Shoot()
    {
        _weapon.Shoot();
    }

    void moneyBagAttack()
    {
        //More like a bomb, just drop 
        Instantiate(_moneyBag, _throwPoint.transform.position, _throwPoint.transform.rotation);
        canBomb = false;
        bombCooldownTimer = _bombCooldown;       
    }

    void spawnLightningAttack()
    {
        //lightning attacks are basically raycasts that deal damage.
        //1st. instantiate the lightning indicator particles where the lightning will hit.  use a raycast which only hits floor type.
        //after _attackDelay seconds, do another raycast and instantiate particle effects on impact
        //if we hit player, deal damage aswell

        Vector3 strikePos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height, 0));

        for (int i = 1; i <= Random.Range(_minCount, _maxCount + 1); i++)
        {
            //point at top of the screen with x value of lightning strike.
            Vector2 curStrikePos = new Vector2(strikePos.x + Random.Range(-1 * _lightningSpread, _lightningSpread), strikePos.y);
    
            //where the lightning makes contact with the ground
            RaycastHit2D hit = Physics2D.Raycast(curStrikePos, Vector2.down, 100f, _floor);
            if (hit)
            { 
                Instantiate(_marker, hit.point, _marker.transform.rotation);
                lightningPositions.Add(new Vector2(hit.point.x, hit.point.y));      
            }
        }
        //timer to countdown till lightning strike happens
        InstantiateTimer(_attackDelay, true, lightningAttack);
    }

    void lightningAttack()
    {
        foreach(Vector2 pos in lightningPositions)
        {
            //raycast from above point downwards  
            Vector2 topPos = new Vector2(pos.x, pos.y + 20f);
            RaycastHit2D hit = Physics2D.Raycast(topPos, Vector2.down, 100f, _projectileMask);

            //These other two raycasts are slightly to the left / right of the lightning strike.  Used to make the hitbox for the strike a bit wider.
            Vector2 topPosTwo = new Vector2(pos.x + _hitboxWidth / 2, pos.y + 20f);
            Vector2 topPosThree = new Vector2(pos.x - _hitboxWidth / 2, pos.y + 20f);
            RaycastHit2D hitTwo = Physics2D.Raycast(topPosTwo, Vector2.down, 100f, _projectileMask);
            RaycastHit2D hitThree = Physics2D.Raycast(topPosThree, Vector2.down, 100f, _projectileMask);

            //spawn lightning sprite/trail
            Instantiate(_lightningSprite, topPos, transform.rotation);
            //spawn impact particles
            Instantiate(_lightningImpactParticles, new Vector3(hit.point.x, hit.point.y, 0), _lightningImpactParticles.transform.rotation);
                
            if (hit.transform.CompareTag("Player") || hitTwo.transform.CompareTag("Player") || hitThree.transform.CompareTag("Player"))
            {
                Debug.Log("applied dmg to player: " + _lightningDamage);
                //*** apply damage to player
            }
        }
        lightningPositions.Clear();
    }
    private void InstantiateTimer(float duration, bool selfDestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(_timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfDestruct);
    }
}
