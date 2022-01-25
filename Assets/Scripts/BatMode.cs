using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class BatMode : MonoBehaviour
{
    [Header("Component References")]
    [SerializeField] private GameObject _timerPrefab;

    [Header("Customization")] 
    [SerializeField] private float _abilityCooldown = 5f;
    [SerializeField] private float _abilityDuration = 2;

    [Header("Feedback")]
    [SerializeField] private bool _abilityAvailable = true;

    [Header("Unity Events")]
    [SerializeField] private UnityEvent _batModeStart;
    [SerializeField] private UnityEvent _batModeEnd;


    public void ActivateBatMode()
    {
        Debug.Log("Trying bat mode");
        if(_abilityAvailable)
        {
            EnterBatMode();
        }
    }

    public bool CheckBatModeAvailable()
    {
        return _abilityAvailable;
    }

    private void EnterBatMode()
    {
        Debug.Log("ENTER BAT MODE");
        _abilityAvailable = false;
        _batModeStart.Invoke();
        InstantiateTimer(_abilityDuration, true, ExitBatMode);
        InstantiateTimer(_abilityCooldown, true, ResetBatMode);
    }

    private void ExitBatMode()
    {
        Debug.Log("EXIT BAT MODE");
        _batModeEnd.Invoke();
    }

    private void ResetBatMode()
    {
        Debug.Log("RESET BAT MODE");
        _abilityAvailable = true;
    }

    private void InstantiateTimer(float duration, bool selfdestruct, UnityAction functionCall)
    {
        GameObject timerObject = Instantiate(_timerPrefab);
        Timer timer = timerObject.GetComponent<Timer>();
        timer.CountdownDoneEvent.AddListener(functionCall);
        timer.StartCountdown(duration, selfdestruct);
    }

    
}
