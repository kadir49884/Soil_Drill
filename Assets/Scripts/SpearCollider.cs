using DG.Tweening;
using MoreMountains.NiceVibrations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpearCollider : MonoBehaviour
{

    private GameObject drillPrefabObject;
    private GameObject newDrill;
    private Quaternion newRotation;
    private GameObject spearParent;
    private GameObject particle;
    private Transform followTransform;
    private GameObject parent;
    private Vector3 followPos;

    private float axisZ = 0.5f;

    private void Start()
    {
        drillPrefabObject = ObjectManager.Instance.SpearObject;
        
        spearParent = ObjectManager.Instance.SpearParent;
    }

    public void InstantiateSpear(Transform newPos)
    {
        followTransform = spearParent.transform.GetChild(0);
        parent = transform.parent.parent.gameObject;
        newRotation = parent.GetLastChild().transform.rotation;

        followPos = followTransform.position;

        MMVibrationManager.Haptic(HapticTypes.LightImpact);
        spearParent.transform.GetChild(0).transform.position  = followPos.AddZ(axisZ);
        newDrill = Instantiate(drillPrefabObject, followPos, newRotation, parent.transform);
        particle = GameManager.Instance.ParticleManager.GetParticle(ParticleType.StackParticle, newPos.position ,rotation: new Vector3(0,-90,0));
        particle.transform.position = particle.transform.position.AddZ(0.5f);
        particle.transform.position = particle.transform.position.AddY(-0.5f);
        ParticleSystem.MainModule main = particle.GetComponent<ParticleSystem>().main;
        main.startDelay = 0;

        newDrill.transform.parent.GetComponent<SpearControl>()?.AddSpear(newDrill);
    }

}
