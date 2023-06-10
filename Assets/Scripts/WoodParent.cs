using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodParent : Singleton<WoodParent>
{
    private void Start()
    {
        ObjectManager.Instance.WoodParent = gameObject;
    }
}
