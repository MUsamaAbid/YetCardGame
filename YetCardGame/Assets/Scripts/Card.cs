using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Card : MonoBehaviour
{
    [Header("For Element card")]
    [SerializeField] CardElement cardElement;
    [SerializeField] CardName cardName;

    [Header("For Fate Cards")]
    [SerializeField] FateCard fateCard;
    [SerializeField] Curses curses;
    [SerializeField] Spells spells;

    [SerializeField] GameObject ImageGameobject;
    [SerializeField] Sprite BackSide;

    [SerializeField] GameObject GeneratedBacksideImage;

    private void Start()
    {
        transform.rotation = Quaternion.Euler(0, 180, 0);
        if (ImageGameobject && BackSide)
        {
            GenerateBackside();
        }
    }
    public void Rotate()
    {
        GetComponent<RotateObject>().Rotate(2f, ObjectType.Card);
        GenerateBackside();
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
}
