using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using MoreMountains.NiceVibrations;

public enum TypeCollider
{
    Stone,
    Gold,
}

public class OnlyStoneObstacle : MonoBehaviour
{
    [SerializeField] bool inWood = false;
    private SpearControl spearControl;
    private Transform followObject;
    private PlayerMovementController playerMovementController;

    private bool vibrationControl = false;
    private GameManager gameManager;
    private ParticleManager particleManager;
    [SerializeField] private TypeCollider typeCollider;


    private void Start()
    {
        spearControl = ObjectManager.Instance.SpearParent.GetComponent<SpearControl>();
        playerMovementController = ObjectManager.Instance.SpearParent.GetChild(0).GetComponent<PlayerMovementController>();
        followObject = ObjectManager.Instance.SpearParent.GetChild(0);
        gameManager = GameManager.Instance;
        particleManager = GameManager.Instance.ParticleManager;
    }
    private void OnTriggerEnter(Collider other)
    {
        GetComponent<BoxCollider>().enabled = false;
        playerMovementController?.ExplodeStone();
        StartCoroutine(VibrationStack());


        //GameObject sparkParticle = GameManager.Instance.ParticleManager.GetParticle(ParticleType.SparkParticle, followObject.position, rotation : new Vector3(0,-90,0), destroyTime: 1f);
        //sparkParticle.transform.DOMoveZ(sparkParticle.transform.position.z + 1f, 1f);

        if (typeCollider == TypeCollider.Gold)
        {
            particleManager.GetParticle(ParticleType.GoldParticle, position: transform.position, destroyTime: 1f);

        }
        else
        {
            particleManager.GetParticle(ParticleType.StoneParticle, position : transform.position, destroyTime: 1f);
        }


        spearControl?.ExplodeStone(transform, inWood);
        //transform.DOLocalMoveY(transform.localPosition.y - 1, 0.1f);
        //transform.DOScale(new Vector3(0.1f, 0.1f, 0.1f), 0.1f);
        Destroy(gameObject, 0.1f);
        vibrationControl = true;


    }


    public IEnumerator VibrationStack()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while (!vibrationControl && gameManager.GameStatus != GameStatus.LastFinish)
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            yield return waitTime;
        }
    }
}
