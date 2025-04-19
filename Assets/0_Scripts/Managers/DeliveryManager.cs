using TMPro;
using UnityEngine;

public enum State {PickUpPending, InDelivery}

public class DeliveryManager : MonoBehaviour
{
    private PointManager pointsManger;

    private State currentState;

    [Header("Canvas")]
    [SerializeField] private TextMeshProUGUI currentObjectiveTMP;
    [SerializeField] private string[] costumerNames;

    [Header("Setup")]
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private Transform[] costumers;
    private Transform currentObjective;
    private bool isPickUpComplete = false;
    private bool isDeliveryComplete = false;


    private void Awake()
    {
        pointsManger = GetComponent<PointManager>();

        currentState = State.PickUpPending;
        currentObjective = pickUpTransform;
    }

    private void Start()
    {
        DisableCostumer();
    }

    public Vector2 ReturnCurrentObjective() => currentObjective.position;

    private void DisableCostumer()
    {
        for (int i = 0; i < costumers.Length; i++)
            costumers[i].gameObject.SetActive(false);
    }

    private void Update()
    {
        if (currentState == State.PickUpPending)
        {
            currentObjective = pickUpTransform;

            if (MainSingletone.inst.language.LanguageId == 1)
                currentObjectiveTMP.text = "Pick up the Brew";
            else if (MainSingletone.inst.language.LanguageId == 2)
                currentObjectiveTMP.text = "Recoge el brebaje";

            if (isPickUpComplete)
            {
                currentState = State.InDelivery;
                SetupNextObjective();
                isPickUpComplete = false;
            }
        }

        if (currentState == State.InDelivery && isDeliveryComplete)
        {
            currentState = State.PickUpPending;
            currentObjective.gameObject.SetActive(false);
            isDeliveryComplete = false;
            pointsManger.AddTime();
        }
    }

    #region Delivery Methods

    public void CompletePickUp() => isPickUpComplete = true;

    public void CompleteDelivery() => isDeliveryComplete = true;

    private void SetupNextObjective()
    {
        Transform nextObjective = ReturnRandomCostumerTransform(costumers);
        nextObjective.gameObject.SetActive(true);
        currentObjective = nextObjective;
        currentObjectiveTMP.text = ReturnRandomName(costumerNames);
    }

    private Transform ReturnRandomCostumerTransform(Transform[] myTransforms) => myTransforms[Random.Range(0,myTransforms.Length)];

    private string ReturnRandomName(string[] myNames) => myNames[Random.Range(0,myNames.Length)];

    #endregion

}
