using System;
using System.Collections;
using UnityEngine;

public class EnemyInput : MonoBehaviour, IShooting
{
    [SerializeField] private float rateOfFire = 1f;
    [SerializeField] private bool shootTickerEnabled = true;
    public Action Shoot { get; set; }

    IEnumerator EnemyShootTicker()
    {
        while (shootTickerEnabled)
        {
            yield return new WaitForSeconds(1 / rateOfFire);
            Shoot.Invoke();
        }
    }

    void Awake()
    {
        StartCoroutine(EnemyShootTicker());
    }
}