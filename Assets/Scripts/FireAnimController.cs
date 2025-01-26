using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class FireAnimController : MonoBehaviour
{
    private void Update(){
        if(Input.GetKeyDown(KeyCode.Q)){
            PlayerMidFire();
        }
        if(Input.GetKeyDown(KeyCode.E)){
            PlayerHighFire();
        }
    }
    public void PlayerMidFire(){
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Middle");
    }

    public void PlayerHighFire(){
        Animator animator = GetComponent<Animator>();
        animator.SetTrigger("Huge");
    }

   
}

