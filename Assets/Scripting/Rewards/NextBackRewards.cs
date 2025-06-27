using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextBackRewards : MonoBehaviour
{
    public GameObject box1, box2, box3, box4, box5, box6, box7, box8;
    public Text TypeRewards;
    public GameObject ArrowBack, ArrowNext;
    public GameObject ChangePopUp;
    public GameObject HopMuALl;
    public GameObject SecretALt;
     void Start()
    {
        box1.SetActive(true);
        box2.SetActive(false);
        box3.SetActive(false);
        box4.SetActive(false);
        box5.SetActive(true);
        box6.SetActive(false);
        box7.SetActive(false);
        box8.SetActive(false);
    }
    void Update()
    {
        if (TypeRewards.text == "Hộp Mù")
        {
            HopMuALl.SetActive(true);
            SecretALt.SetActive(false);
            ArrowBack.SetActive(!box1.activeSelf);
            ArrowNext.SetActive(!box4.activeSelf);
        }
        if (TypeRewards.text == "Secret")
        {
            SecretALt.SetActive(true);
            HopMuALl.SetActive(false);
            ArrowBack.SetActive(!box5.activeSelf);
            ArrowNext.SetActive(!box8.activeSelf);
        }
    }

    public void OnClickNext()
    {
        if (TypeRewards.text == "Hộp Mù")
        {
            if (box1.activeSelf && !box2.activeSelf && !box3.activeSelf && !box4.activeSelf)
                SetActiveBoxes(box2, box1);
            else if (box2.activeSelf && !box1.activeSelf && !box3.activeSelf && !box4.activeSelf)
                SetActiveBoxes(box3, box2);
            else if (box3.activeSelf && !box1.activeSelf && !box2.activeSelf && !box4.activeSelf)
                SetActiveBoxes(box4, box3);
        }
        else if (TypeRewards.text == "Secret")
        {
            if (box5.activeSelf && !box6.activeSelf && !box7.activeSelf && !box8.activeSelf)
                SetActiveBoxes(box6, box5);
            else if (box6.activeSelf && !box5.activeSelf && !box7.activeSelf && !box8.activeSelf)
                SetActiveBoxes(box7, box6);
            else if (box7.activeSelf && !box5.activeSelf && !box6.activeSelf && !box8.activeSelf)
                SetActiveBoxes(box8, box7);
        }
    }

 
    public void OnClickBack()
    {
        if (TypeRewards.text == "Hộp Mù")
        {
            if (box4.activeSelf && !box1.activeSelf && !box2.activeSelf && !box3.activeSelf)
                SetActiveBoxes(box3, box4);
            else if (box3.activeSelf && !box1.activeSelf && !box2.activeSelf && !box4.activeSelf)
                SetActiveBoxes(box2, box3);
            else if (box2.activeSelf && !box1.activeSelf && !box3.activeSelf && !box4.activeSelf)
                SetActiveBoxes(box1, box2);
        }
        else if (TypeRewards.text == "Secret")
        {
            if (box8.activeSelf && !box5.activeSelf && !box6.activeSelf && !box7.activeSelf)
                SetActiveBoxes(box7, box8);
            else if (box7.activeSelf && !box5.activeSelf && !box6.activeSelf && !box8.activeSelf)
                SetActiveBoxes(box6, box7);
            else if (box6.activeSelf && !box5.activeSelf && !box7.activeSelf && !box8.activeSelf)
                SetActiveBoxes(box5, box6);
        }
    }

    private void SetActiveBoxes(GameObject activate, GameObject deactivate)
    {
        activate.SetActive(true);
        deactivate.SetActive(false);
    }

    public void OnChangeType()
    {
        box1.SetActive(true);
        box5.SetActive(true);
        box2.SetActive(false);
        box3.SetActive(false);
        box4.SetActive(false);
        box6.SetActive(false);
        box7.SetActive(false);
        box8.SetActive(false);
    }
    public void ChangeToHopMu()
    {
        ArrowBack.SetActive(false);
        ArrowNext.SetActive(true);
        OnChangeType();
        TypeRewards.text = "Hộp Mù";
        ChangePopUp.SetActive(false);
    }
    public void ChangeToSecret()
    {
        ArrowBack.SetActive(false);
        ArrowNext.SetActive(true);
        OnChangeType();
        TypeRewards.text = "Secret";
        ChangePopUp.SetActive(false);
    }
}
