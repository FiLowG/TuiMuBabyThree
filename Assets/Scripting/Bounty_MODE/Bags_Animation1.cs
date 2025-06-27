using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bags_Animation_Online: MonoBehaviour
{
    public Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void StopAnimation()
    {
        animator.speed = 0;
    }
}
