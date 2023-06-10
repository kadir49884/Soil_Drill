using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondCollider : MonoBehaviour
{
    private ParticleManager particleManager;
    private void Start()
    {
        particleManager = GameManager.Instance.ParticleManager;
    }

    private void OnTriggerEnter(Collider other)
    {
        particleManager.GetParticle(ParticleType.DiamondParticle, position :  transform.position, destroyTime: 1f);
        Destroy(gameObject, 0.1f);
    }
}
