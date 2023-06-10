using DG.Tweening;
using MoreMountains.NiceVibrations;
using Sirenix.OdinInspector;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpearControl : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> stackList;


    public List<GameObject> StackList { get => stackList; set => stackList = value; }
    public bool InObstacle { get => inObstacle; set => inObstacle = value; }

    public bool Control { get => control; set => control = value; }

    private bool inObstacle;

    [SerializeField] private float lerpX = 0.01f;
    private float lerpRotation = 0.8f;

    private Color color;
    private Color emissionColor;
    private GameObject circleControl;
    private GameManager gameManager;
    private bool control = false;
    private bool gameResultWin = false;
    private TypeObstacle typeObstacle;
    private GameStatus gameStatus;
    private ParticleManager particleManager;
    private MeshRenderer meshRenderer1;
    private MeshRenderer meshRenderer2;
    private ObjectManager objectManager;
    private Vector3 rotValue;
    private Quaternion lookRotation;

    private Vector3 newPos;

    Vector3 stopPos;
    GameObject particleStone;
    private void Start()
    {
        typeObstacle = TypeObstacle.Space;
        stackList.AddRange(transform.GetChildren().ToList());

        GameManager.Instance.GameStart += GameStart;
        gameManager = GameManager.Instance;
        objectManager = ObjectManager.Instance;

        color = objectManager.Color;
        emissionColor = objectManager.EmissionColor;
        circleControl = objectManager.CircleParent;
        particleManager = gameManager.ParticleManager;
    }

    

    private void GameStart()
    {
        transform.GetChild(0).GetChild(0).DOLocalRotate(new Vector3(0, 0, -360), 1f, RotateMode.LocalAxisAdd)
            .SetLoops(-1, LoopType.Incremental)
            .SetUpdate(UpdateType.Fixed)
            .SetEase(Ease.Linear);
    }
    private void FixedUpdate()
    {
        gameStatus = GameManager.Instance.GameStatus;

        if (gameManager.GameStatus == GameStatus.LastFinish || !gameManager.IsGameStarted)
            return;

        for (int i = stackList.Count - 1; i > 0; i--)
        {
            switch (typeObstacle)
            {
                //case TypeObstacle.Stone:
                //    lerpX = 0.05f;
                //    lerpRotation = 0.1f;
                //    break;
                //case TypeObstacle.Wood:
                //    lerpX = 0.14f;
                //    lerpRotation = 0.3f;
                //    break;
                //case TypeObstacle.Space:
                //    lerpX = 0.15f;
                //    lerpRotation = 0.5f;
                //    break;
            }

           
            newPos = stackList[i - 1].transform.localPosition;
            newPos.x = stackList[i - 1].transform.localPosition.x;

            if (gameStatus == GameStatus.Play || gameStatus == GameStatus.Finish3 || gameStatus == GameStatus.Step2)
            {
                stackList[i].transform.localPosition = Vector3.Lerp(stackList[i].transform.localPosition, newPos, lerpX);

                if (typeObstacle == TypeObstacle.Space)
                {
                    //StackList[i].transform.localPosition = Vector3.Lerp(StackList[i].transform.localPosition, StackList[i].transform.localPosition.SetZ(newPos.z - 0.4f), 0.7f);
                    //stackList[i].transform.SetLocalPosZ(newPos.z - 0.4f);
                    if (i != 1)
                    {
                        stackList[i].transform.SetLocalPosZ(newPos.z - 0.3f);
                    }
                }
                //else if (typeObstacle == TypeObstacle.Wood || typeObstacle == TypeObstacle.Stone)
                //{
                //    if (i != 1)
                //    {
                //        //StackList[i].transform.localPosition = Vector3.Lerp(StackList[i].transform.localPosition, StackList[i].transform.localPosition.SetZ(newPos.z - 0.4f), 0.3f);
                //        //stackList[i].transform.SetLocalPosZ(newPos.z - 0.4f);
                //        stackList[i].transform.SetLocalPosZ(newPos.z - 0.3f);
                //    }
                //}
            }
            //else if (gameStatus == GameStatus.Finish1)
            //{
            //    stackList[i].transform.localPosition = Vector3.Lerp(stackList[i].transform.localPosition, newPos, lerpX);
            //}
            //else if (gameStatus == GameStatus.Finish2)
            //{
            //    stackList[i].transform.localPosition = Vector3.Lerp(stackList[i].transform.localPosition, newPos, lerpX);
            //}

            lookRotation = Quaternion.LookRotation(stackList[i - 1].transform.position - stackList[i].transform.position);
            
            if( i == 1)
            {
                lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, stackList[i - 1].transform.GetChild(0).localEulerAngles.z);
            }
            else
            {
                lookRotation = Quaternion.Euler(lookRotation.eulerAngles.x, lookRotation.eulerAngles.y, stackList[i - 1].transform.localEulerAngles.z);
            }

            stackList[i].transform.rotation = Quaternion.Slerp(stackList[i].transform.rotation, lookRotation, lerpRotation);
            rotValue = stackList[i].transform.localEulerAngles;

            //if (gameStatus == GameStatus.Play || gameStatus == GameStatus.Finish3 || gameStatus == GameStatus.Step2)
            //{
            //    //rotValue.x = 0;
            //}
            //else if (gameStatus == GameStatus.Finish1)
            //{
            //    //rotValue.y = 0;
            //}
            stackList[i].transform.localEulerAngles = rotValue;
        }
    }

    public void GetTypeObstacle(GameObject newObject)
    {
        typeObstacle = newObject.GetComponent<DrillOnObstacle>().TypeObstacle;
    }

    public void ExitObstacle(TypeObstacle type)
    {
        typeObstacle = type;
    }


    public IEnumerator Vibration()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.2f);
        while ((typeObstacle == TypeObstacle.Wood || typeObstacle == TypeObstacle.Stone) && gameManager.GameStatus != GameStatus.LastFinish)
        {
            MMVibrationManager.Haptic(HapticTypes.HeavyImpact);
            yield return waitTime;
        }
    }
    public void ExplodeStone(Transform newTransform, bool inWood)
    {

        //if (inWood)
        //{
        //    particleStone = particleManager.GetParticle(ParticleType.StoneParticle, particlePos, destroyTime: 1f);
        //}
        //else
        //{
        //particlePos.y -= 0.3f;
        if(stackList.Count > 1)
        {
            GameObject particle = particleManager.GetParticle(ParticleType.BoomParticle, destroyTime: 1f);
            particle.transform.position = stackList[1].transform.position.AddZ(0.5f);
            RemoveSpear(stackList[1]);
        }
        
        //}
        //particlePos.z += 1f;
        //particleStone.transform.position = particlePos;

    }
    public IEnumerator ExplodeDrill()
    {
        WaitForSeconds waitTime = new WaitForSeconds(0.8f);
        while ((typeObstacle == TypeObstacle.Wood || typeObstacle == TypeObstacle.Stone)
            && transform.childCount > 0 && gameManager.GameStatus != GameStatus.LastFinish)
        {
            if (transform.childCount > 2)
            {
                MeshRenderer meshRenderer0 = stackList[1].GetComponent<MeshRenderer>();
                meshRenderer0.materials[0].SetColor(UpperUtil.PROP_EMISSION_COLOR, emissionColor);
                for (int i = 1; i < 3; i++)
                {
                    meshRenderer0.materials[i].SetColor(UpperUtil.PROP_EMISSION_COLOR, Color.black);
                }
                meshRenderer0.materials[0].DOColor(color, 0.6f);
                meshRenderer0.materials[1].DOColor(color, 0.6f);
                meshRenderer0.materials[2].DOColor(color, 0.6f).OnComplete((TweenCallback)(() =>
                {
                    if ((typeObstacle == TypeObstacle.Wood || typeObstacle == TypeObstacle.Stone) && transform.childCount > 2)
                    {
                        circleControl.GetComponent<DrillController>()?.SetNewDrill();
                        GameObject particle = particleManager.GetParticle(ParticleType.BoomParticle, destroyTime: 1f);
                        Vector3 newPos = stackList[1].transform.position;
                        newPos.z += 0.5f;
                        particle.transform.position = newPos;
                        RemoveSpear(stackList[1]);
                    }
                    else
                    {
                        meshRenderer1 = stackList[1].GetComponent<MeshRenderer>();
                        meshRenderer2 = stackList[(stackList.Count - 1)].GetComponent<MeshRenderer>();

                        for (int i = 0; i < 3; i++)
                        {
                            meshRenderer1.materials[i].DOColor(meshRenderer2.materials[i].color, 1.5f);
                        }
                        Run.After(1, () =>
                        {
                            meshRenderer1.materials[0].SetColor(UpperUtil.PROP_EMISSION_COLOR, meshRenderer2.materials[0].GetColor(UpperUtil.PROP_EMISSION_COLOR));
                        });
                    }
                }));
            }
            else
            {
                GameObject particle = particleManager.GetParticle(ParticleType.BoomParticle, destroyTime: 1f);
                Vector3 newPos = stackList[1].transform.position;
                newPos.z += 0.5f;
                particle.transform.position = newPos;
                transform.GetLastChild().gameObject.SetActive(false);
                gameManager.GameStatus = GameStatus.LastFinish;
                if (gameResultWin)
                {
                    Run.After(5f, () =>
                    {
                        SceneManager.LoadScene("Main");
                    });
                }
                else
                {
                    Run.After(1f, () =>
                    {
                        SceneManager.LoadScene("Main");
                    });
                }

            }

            yield return waitTime;
        }
    }

    public void AddSpear(GameObject newObject)
    {
        stackList.Insert(1, newObject);
    }
    public void RemoveSpear(GameObject newObject)
    {
        stackList.Remove(newObject);
        Destroy(newObject);
    }
    public void FinishIsOpen1(GameObject newObject)
    {
        gameResultWin = true;
        for (int i = 0; i < stackList.Count; i++)
        {
            transform.GetChild(0).parent = null;
        }
        transform.rotation = Quaternion.Euler(90, 0, 0);
        stopPos = newObject.transform.position;
        stopPos.z += 1f;
        transform.position = transform.position.SetZ(stopPos.z);

        for (int i = 1; i < StackList.Count; i++)
        {
            Transform temp = StackList[i].transform;
            temp.SetParent(transform);

            temp.SetAsFirstSibling();
        }
        stackList[0].SetParent(transform);
        stackList[0].transform.SetAsFirstSibling();

        transform.GetComponent<Rigidbody>().SetConstraints(RigidbodyConstraints.FreezeAll);
        transform.GetChild(0).GetComponent<Rigidbody>().SetConstraints(RigidbodyConstraints.FreezePositionZ,
                RigidbodyConstraints.FreezeRotation);

        stackList[0].transform.DOMoveY(stackList[0].transform.position.y - 6f, 1f).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);

        Run.After(2f, () =>
        {
            gameManager.GameStatus = GameStatus.Finish3;
        });
    }

    public void StepControl(GameObject newObject)
    {
        gameManager.GameStatus = GameStatus.Step1;
        for (int i = 0; i < stackList.Count; i++)
        {
            transform.GetChild(0).parent = null;

        }
        transform.rotation = Quaternion.Euler(0, 0, 0);
        //stopPos = newObject.transform.position;
        //stopPos.y += 3f;
        //transform.position = transform.position.SetY(stopPos.y);


        for (int i = 1; i < StackList.Count; i++)
        {
            Transform temp = StackList[i].transform;
            temp.SetParent(transform);
            temp.SetAsFirstSibling();
        }
        stackList[0].SetParent(transform);
        stackList[0].transform.SetAsFirstSibling();

        transform.GetComponent<Rigidbody>().SetConstraints(RigidbodyConstraints.FreezeAll);
        transform.GetChild(0).GetComponent<Rigidbody>().SetConstraints(RigidbodyConstraints.FreezePositionY,
                RigidbodyConstraints.FreezeRotation);

        //stackList[0].transform.DOMoveY(stackList[0].transform.position.y + 0.5f, 0.1f).SetEase(Ease.Linear).SetUpdate(UpdateType.Fixed);

        stackList[0].transform.DOLocalMoveZ(stackList[0].transform.localPosition.z + 8f, 0.5f).SetEase(Ease.Linear)
           .SetUpdate(UpdateType.Fixed);

        for (int i = 1; i < StackList.Count - 1; i++)
        {
            stackList[i].transform.DOLocalMoveZ(stackList[i].transform.localPosition.z + (15 - i) * 0.7f, 1f).SetEase(Ease.Linear)
           .SetUpdate(UpdateType.Fixed);
            stackList[i].transform.DOMoveY(-21.21f, (0.2f + i) * 0.05f).SetEase(Ease.Linear)
           .SetUpdate(UpdateType.Fixed);
        }

        stackList[StackList.Count - 1].transform.DOLocalMoveZ(stackList[StackList.Count - 1].transform.localPosition.z + (15 - (StackList.Count - 1)) * 0.7f, 1f).SetEase(Ease.Linear)
           .SetUpdate(UpdateType.Fixed);
        stackList[StackList.Count - 1].transform.DOMoveY(-21.21f, (0.3f + StackList.Count - 1) * 0.04f).SetEase(Ease.Linear)
           .SetUpdate(UpdateType.Fixed).OnComplete(() =>
           {
               gameManager.GameStatus = GameStatus.Step2;
           });




    }


}
