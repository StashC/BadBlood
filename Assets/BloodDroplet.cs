using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodDroplet : MonoBehaviour
{
    [SerializeField] private int _healingAmount = 5;

    public float GetHealAmount()
    {
        return _healingAmount;
    }

    private void OnParticleCollision(GameObject other)
    {
        if(other.TryGetComponent<healthScript>(out healthScript health))
        {
            health.Heal(_healingAmount);
        }
    }
}
