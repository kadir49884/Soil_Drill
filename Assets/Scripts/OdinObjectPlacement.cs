using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OdinObjectPlacement : MonoBehaviour
{
    [Button]
    public void GetPlacement()
    {
        List<GameObject> childList = new List<GameObject>();
        for (int i = 0; i < transform.childCount; i++)
        {
            childList.Add(transform.GetChild(i).gameObject);
        }

        childList = childList.OrderBy(a => a.transform.localPosition.z).ToList();

        for (int i = 0; i < childList.Count; i++)
        {
            childList[i].transform.SetSiblingIndex(i);
        }
    }
}
