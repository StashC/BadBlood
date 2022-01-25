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


    [Header("Lightning Attack")]
    [SerializeField] private GameObject _marker;
    [SerializeField] private GameObject _lightningImpactParticles;
    [SerializeField] private int _minCount;
    [SerializeField] private int _maxCount;
    [SerializeField] private float _lightningSpread;
    [SerializeField] private float _attackDelay;
    [SerializeField] private float _lightningDamage;

    [SerializeField] private bool test = false;

    private int _floor;
    private int _projectileMask;
    private List<Vector2> lightningPositions;
    //references
    private GameObject _player;
    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player");
        lightningPositions = new List<Vector2>();

        //layermask of only floor layer. only collide with floor.
        _floor = 1 << LayerMask.NameToLayer("Floor");

        //Only collide with floor or player or cover
        int plyr = 1 << LayerMask.NameToLayer("Player");
        int cover = 1 << LayerMask.NameToLayer("Cover");

        _projectileMask = plyr | cover | _floor;

    }

    // Update is called once per frame
    void Update()
    {
        if (test)
        {
            //Debug.Log(Random.Range(_minLightning, _maxLightning + 1));
            
            //moneyBagAttack();            
            spawnLightningAttack();

            test = !test;
        }
    }

    void moneyBagAttack()
    {
        //Throw in an Arc with the script.
        Instantiate(_moneyBag, _throwPoint.transform.position, _throwPoint.transform.rotation);
    }

    void spawnLightningAttack()
    {
        //lightning attacks are basically raycasts that deal damage.
        //1st. instantiate the lightning indicator particles where the lightning will hit.  use a raycast which only hits floor type.
        //after _attackDelay seconds, do another raycast and instantiate particle effects on impact
        //if we hit player, deal damage aswell

        //instantiate 2-4 lightning attacks

        Vector3 strikePos = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width/2, Screen.height, 0));
        //Instantiate(_marker, strikePos, _marker.transform.rotation);

        for (int i = 1; i <= Random.Range(_minCount, _maxCount + 1); i++)
        {
            //point at top of the screen with x value of lightning strike.
            Vector2 curStrikePos = new Vector2(strikePos.x + Random.Range(-1 * _lightningSpread, _lightningSpread), strikePos.y);
    
            //where the lightning makes contact with the ground
            RaycastHit2D hit = Physics2D.Raycast(curStrikePos, Vector2.down, 100f, _floor);
            //Debug.Log(curStrikePos);

            if (hit)
            { 
                Instantiate(_marker, hit.point, _marker.transform.rotation);
                lightningPositions.Add(new Vector2(hit.point.x, hit.point.y));      
                Debug.Log("spawned Lightning");
            }
        }
        //timer to countdown till lightning attack happens

        InstantiateTimer(_attackDelay, true, lightningAttack);
    }

    void lightningAttack()
    {
        Debug.Log("called lightningAttack");
        //for each loop for 
        foreach(Vector2 pos in lightningPositions)
        {
        Vector2 topPos = new Vector2(pos.x, pos.y + 20f);
            //raycast from above point downwards.  
            RaycastHit2D hit = Physics2D.Raycast(topPos, Vector2.down, 100f, _projectileMask);
            if(hit != null)
            {
                //spawn impact particles
                Instantiate(_lightningImpactParticles, new Vector3(hit.point.x, hit.point.y, 0), _lightningImpactParticles.transform.rotation);

                //spawn lightning sprite/trail
                if (hit.transform.CompareTag("Player"))
                {
                    Debug.Log("applied dmg to player: " + _lightningDamage);
                    //*** apply damage to player
                }
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
