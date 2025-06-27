using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Select_Wish : MonoBehaviour
{
    public Image Labubu_Image, Lububu_Image, Turtle_Image;
    public GameObject Type_Charm;
    public GameObject Labubu, Turtle, Lububu;
    private Count_Charm_Turn _countCharm;
    private SpawnBags spawnBags;


    void Start()
    {
        _countCharm = FindObjectOfType<Count_Charm_Turn>();
        spawnBags = FindObjectOfType<SpawnBags>();
    }

    void Update()
    {
        UpdateWishImageAlpha();
    }

    public void OnButton_Select(GameObject button)
    {
        string tag = Type_Charm.tag;
        GameObject selectedObject = null;
        Image targetImage = null;

        if (tag == Labubu.tag)
        {
            selectedObject = Labubu;
            targetImage = Labubu_Image;
        }
        else if (tag == Turtle.tag)
        {
            selectedObject = Turtle;
            targetImage = Turtle_Image;
        }
        else if (tag == Lububu.tag)
        {
            selectedObject = Lububu;
            targetImage = Lububu_Image;
        }

        if (selectedObject != null && targetImage != null)
        {
            int buttonNumber = ExtractNumberFromName(button.name);
            Transform targetChild = FindChildWithNumber(selectedObject.transform, buttonNumber);

            if (targetChild != null)
            {
                Image childImage = targetChild.GetComponent<Image>();
                if (childImage != null)
                {
                    targetImage.sprite = childImage.sprite;
                    Debug.Log("Sprite assigned to target image.");

                }
            }
        }

    }

    

    private int ExtractNumberFromName(string name)
    {
        string numberString = "";
        foreach (char c in name)
        {
            if (char.IsDigit(c))
            {
                numberString += c;
            }
        }

        if (int.TryParse(numberString, out int result))
        {
            return result;
        }

        return -1;
    }


    private Transform FindChildWithNumber(Transform parent, int number)
    {
        foreach (Transform child in parent)
        {
            if (child.name.Contains(number.ToString()))
            {
                Debug.Log("Found matching child: " + child.name);
                return child;
            }
        }
        return null;
    }

    private void UpdateWishImageAlpha()
    {
        SetAlpha(Labubu_Image);
        SetAlpha(Lububu_Image);
        SetAlpha(Turtle_Image);
    }

    private void SetAlpha(Image img)
    {
        Color color = img.color;
        color.a = (img.sprite == null) ? 0f : 1f;
        img.color = color;

    }
}
