using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Timer : MonoBehaviour
{
    [Header("Customization")]
    [SerializeField] private bool _selfDestruct = true;

    private IEnumerator _countdownCoroutine;
    public UnityEvent CountdownDoneEvent;

    public void StartCountdown(float time, bool selfDestruct)
    {
        _countdownCoroutine = Countdown(Mathf.Abs(time));
        _selfDestruct = selfDestruct;
        StartCoroutine(_countdownCoroutine);
    }

    private IEnumerator Countdown(float time)
    {
        while(time > 0)
        {
            time -= Time.deltaTime;
            yield return null;
        }
        CountdownDoneEvent.Invoke();
        ResetCoroutine();
    }

    private void ResetCoroutine()
    {
        StopCoroutine(_countdownCoroutine);
        _countdownCoroutine = null;
        if (_selfDestruct) Destroy(this.gameObject);

    }


}
