using TMPro;
using UnityEngine;

public enum State {PickUpPending, InDelivery}
public enum DeliveryType {None, Fire, Slime, Protection}

public class DeliveryManager : MonoBehaviour
{
    private PointManager pointsManger;

    public State currentState {  get; private set; }
    public DeliveryType currentDeliveryType {  get; private set; }

    [Header("Canvas")]
    [SerializeField] private TextMeshProUGUI currentObjectiveTMP;
    [SerializeField] private string[] costumerNames;

    [Header("Setup")]
    [SerializeField] private Transform pickUpTransform;
    [SerializeField] private Transform pickUpVisual;
    [SerializeField] private Transform[] costumers;
    private Transform currentObjective;

    private void Awake()
    {
        pointsManger = GetComponent<PointManager>();

        currentState = State.PickUpPending;
        currentObjective = pickUpTransform;
    }

    private void Start()
    {
        DisableCostumer();

        currentDeliveryType = DeliveryType.None;
        pickUpVisual.gameObject.SetActive(true);
    }

    public Vector2 ReturnCurrentObjective() => currentObjective.position;

    private void DisableCostumer()
    {
        for (int i = 0; i < costumers.Length; i++)
            costumers[i].gameObject.SetActive(false);
    }


    #region Delivery Methods
    public void CompletePickUp()
    {
        if (currentState == State.PickUpPending)
        {
            currentState = State.InDelivery;
            pickUpVisual.gameObject.SetActive(false);
            SetupNextObjective();
        }
    }

    public void CompleteDelivery()
    {
        if (currentState == State.InDelivery)
        {
            currentState = State.PickUpPending;
            currentObjective.gameObject.SetActive(false);
            pointsManger.AddTime();

            currentDeliveryType = DeliveryType.None;
            pickUpVisual.gameObject.SetActive(true);
            currentObjective = pickUpTransform;

            //OBJECTIVE
            if (MainSingletone.inst.language.LanguageId == 1)
                currentObjectiveTMP.text = "Return to the shop to pick up the next potions delivery";
            else if (MainSingletone.inst.language.LanguageId == 2)
                currentObjectiveTMP.text = "Vuelve a la tienda para recoge el siguiente pedido de pociones";
        }
    }

    private void SetupNextObjective()
    {
        Transform nextObjective = ReturnRandomCostumerTransform(costumers);
        currentObjective = nextObjective;

        if (MainSingletone.inst.language.LanguageId == 1)
            currentObjectiveTMP.text = "Deliver To " + ReturnRandomName(costumerNames);
        else if (MainSingletone.inst.language.LanguageId == 2)
            currentObjectiveTMP.text = "Entrega A " + ReturnRandomName(costumerNames);

        RandomEffect();
    }

    private void RandomEffect()
    {
        int randomChoice = Random.Range(0, 3);

        if (randomChoice == 0)
            currentDeliveryType = DeliveryType.Fire;
        if (randomChoice == 1)
            currentDeliveryType = DeliveryType.Slime;
        if (randomChoice == 2)
            currentDeliveryType = DeliveryType.Protection;
    }

    private Transform ReturnRandomCostumerTransform(Transform[] myTransforms)
    {
        Transform instance = myTransforms[Random.Range(0, myTransforms.Length)];
        instance.gameObject.SetActive(true);

        foreach (Transform item in myTransforms)
        {
            if (item == instance) item.gameObject.SetActive(true);
            else item.gameObject.SetActive(false);
        }

        return instance;
    }

    private string ReturnRandomName(string[] myNames) => myNames[Random.Range(0,myNames.Length)];

    #endregion

}
