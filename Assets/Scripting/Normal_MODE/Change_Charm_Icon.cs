using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Charm_Icon : MonoBehaviour
{
    public GameObject Type_Charm;
    public GameObject Icon_Lububu;
    public GameObject Icon_Turtle;
    public GameObject Icon_Labubu;

     void Start()
    {

    }
    // Update is called once per frame
    void Update()
    {
        if (Type_Charm == null) return;


        if (Type_Charm.tag == Icon_Lububu.tag)
        {
            Icon_Lububu.SetActive(true);
            Icon_Turtle.SetActive(false);
            Icon_Labubu.SetActive(false);
        }
        else if (Type_Charm.tag == Icon_Turtle.tag)
        {
            Icon_Lububu.SetActive(false);
            Icon_Turtle.SetActive(true);
            Icon_Labubu.SetActive(false);
        }
        else if (Type_Charm.tag == Icon_Labubu.tag)
        {
            Icon_Lububu.SetActive(false);
            Icon_Turtle.SetActive(false);
            Icon_Labubu.SetActive(true);
        }
        else
        {
            // Nếu không khớp với bất kỳ tag nào, tắt tất cả Icon
            Icon_Lububu.SetActive(false);
            Icon_Turtle.SetActive(false);
            Icon_Labubu.SetActive(false);
        }
    }


    public void OnSelectCharm(string Charm_Name)
    {
        Type_Charm.tag = Charm_Name;
    }
    public void timeSet()
    {
        Time.timeScale = 1;
    }
}
