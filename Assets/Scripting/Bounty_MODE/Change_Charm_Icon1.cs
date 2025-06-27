using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Change_Charm_Icon_Online : MonoBehaviour
{
    public GameObject Type_Charm;
    public GameObject Icon_Lububu;
    public GameObject Icon_Turtle;
    public GameObject Icon_Labubu;

    // Update is called once per frame
    void Update()
    {
        if (Type_Charm == null) return;

        // So sánh tag của Type_Charm và setActive các Icon tương ứng
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
}
