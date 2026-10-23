using System;
using System.Collections;
using UnityEngine;
[RequireComponent(typeof(ParticleSystem))]
public class DestroyAfterPlayed : MonoBehaviour
{
    private ParticleSystem particleSystem;
    void Awake()
    {
        particleSystem = GetComponent<ParticleSystem>();
        StartCoroutine(DestroyWhenFinished());
    }

    IEnumerator DestroyWhenFinished()
    {
        yield return new WaitUntil(() => !particleSystem.isPlaying);
        Destroy(gameObject);
    }
}
