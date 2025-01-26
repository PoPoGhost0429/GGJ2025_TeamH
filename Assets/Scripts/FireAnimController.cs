using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FireAnimController : MonoBehaviour
{
    public void PlayerMidFire(){
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Middle");
    }

    public void PlayerHighFire(){
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Huge");
    }

   
}

