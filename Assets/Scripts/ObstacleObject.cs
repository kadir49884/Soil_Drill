using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleObject : MonoBehaviour
{
    private GameObject spearObject;

    private void OnTriggerEnter(Collider other)
    {
        spearObject  = other.gameObject.transform.parent.gameObject;

        spearObject.transform.parent.GetComponent<SpearControl>()?.RemoveSpear(other.gameObject.transform.parent.gameObject);
        Destroy(spearObject);
       
        Destroy(gameObject);
    }
}
