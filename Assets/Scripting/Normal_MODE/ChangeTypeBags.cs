using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeTypeBags : MonoBehaviour
{
    public GameObject TypeBags;
    public GameObject Bags_Blue;
    public GameObject Bags_Pink;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (TypeBags.tag == Bags_Blue.tag)
        {
            Bags_Blue.SetActive(true);
            Bags_Pink.SetActive(false);
        }
        if (TypeBags.tag == Bags_Pink.tag)
        {
            Bags_Blue.SetActive(false);
            Bags_Pink.SetActive(true);
        }
    }
    public void Change_TypeBags(int type)
    {
        if (type == 1)
        {
            TypeBags.tag = "TuiMu_Pink";

        }
        else if (type == 2)
        {
            TypeBags.tag = "TuiMu_Blue";
        }
    }
}
