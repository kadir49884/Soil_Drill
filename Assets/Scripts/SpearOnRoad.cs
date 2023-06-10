using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearOnRoad : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<SpearCollider>())
        {
            GetComponent<BoxCollider>().enabled = false;
            other.GetComponent<SpearCollider>()?.InstantiateSpear(transform);
            Destroy(gameObject);
        }
        
    }
}
