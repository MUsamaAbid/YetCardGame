using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class Card : MonoBehaviour
{
    [SerializeField] CardType cardType;

    [Header("For Army card")]
    [SerializeField] CardElement cardElement;
    [SerializeField] CardName cardName;

    [Header("For Fate Cards")]
    [SerializeField] FateCard fateCard;
    [SerializeField] Curses curses;
    [SerializeField] Spells spells;

    [SerializeField] GameObject ImageGameobject;
    [SerializeField] Sprite BackSide;

    [SerializeField] GameObject GeneratedBacksideImage;

    Player player;

    [Header("Card Properties")]
    [ReadOnly]
    public int AttackNumber;
    [ReadOnly]
    public int DefenceNumber;

    Vector2 Targetpos;
    bool MoveToTargetPos = false;
    int speed = 1000;

    private void Start()
    {
        AssignProperties();
        gameObject.GetComponent<Button>().onClick.AddListener(OnCardDown);

        transform.rotation = Quaternion.Euler(0, 180, 0);
        if (ImageGameobject && BackSide)
        {
            GenerateBackside();
        }
    }
    public void OnCardDown()
    {
        if(player == Player.Player2)
        {
            return;
        }

        Debug.Log("Button pressed");
    }
    public void AssignPlayer(Player p)
    {
        player = p;
    }
    void AssignProperties()
    {
        AttackNumber = CardEnums.instance.ReturnAttack(cardName);
        DefenceNumber = CardEnums.instance.ReturnDefence(cardName);
    }
    public void RotateInHand()
    {
        GetComponent<RotateObject>().RotateInHand(2f, ObjectType.Card);
        //GenerateBackside();
    }
    public void RotateUpsideDown()
    {
        GetComponent<RotateObject>().RotateUpsideDown(4f, ObjectType.Card);
        //GenerateBackside();
    }
    public void GenerateBackside()
    {
        GeneratedBacksideImage = Instantiate(ImageGameobject, transform);
        GeneratedBacksideImage.GetComponent<Image>().sprite = BackSide;
        GeneratedBacksideImage.GetComponent<Image>().SetNativeSize();
        ExtendAnchorsToCanvas(GeneratedBacksideImage.GetComponent<RectTransform>());
    }
    public void DeleteBackside()
    {
        if (GeneratedBacksideImage)
        {
            Debug.Log("Generated: " + GeneratedBacksideImage.name);
            Destroy(GeneratedBacksideImage);
            Debug.Log("Backside deleted");
        }
    }

    public void ExtendAnchorsToCanvas(RectTransform rectTransform)
    {
        if (rectTransform != null && rectTransform.parent != null)
        {
            RectTransform parentRectTransform = rectTransform.parent.GetComponent<RectTransform>();

            if (parentRectTransform != null)
            {
                // Set the size to match the parent
                rectTransform.sizeDelta = parentRectTransform.sizeDelta;

                // Reset the anchored position (center it in the parent)
                rectTransform.anchoredPosition = Vector2.zero;
            }
            else
            {
                Debug.LogError("Parent does not have a RectTransform component!");
            }
        }
        else
        {
            Debug.LogError("RectTransform or parent is null!");
        }
    }
    public void SetTargetPosition(Vector2 Pos)
    {
        Targetpos = Pos;
        MoveToTargetPos = true;
    }
    private void Update()
    {
        if (MoveToTargetPos)
        {
            Vector2 currentAnchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            Vector2 targetAnchoredPosition = Targetpos;

            // Calculate the direction and distance to the target
            Vector2 direction = (targetAnchoredPosition - currentAnchoredPosition).normalized;
    //        float distance = Vector2.Distance(currentAnchoredPosition, targetAnchoredPosition);

            // Calculate the new anchored position based on the constant speed
            Vector2 newAnchoredPosition = currentAnchoredPosition + direction * speed * Time.deltaTime;

            // Ensure that we don't overshoot the target
            if (Vector2.Distance(newAnchoredPosition, targetAnchoredPosition) < speed * Time.deltaTime)
            {
                newAnchoredPosition = targetAnchoredPosition;
            }

            // Update the anchored position of the RectTransform
            GetComponent<RectTransform>().anchoredPosition = newAnchoredPosition;
        }
    }
}