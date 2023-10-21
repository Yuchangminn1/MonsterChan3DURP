using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationTrigger : MonoBehaviour
{
    PlayerController player;
    Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponentInParent<PlayerController>();
        animator = transform.GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void AnimationTrigger()
    {
        player.animationTrigger = false;
    }
    public void WeaponColOn()
    {
        player.weaponcol.WeaponColOn();
    }
    public void WeaponColOff()
    {
        player.weaponcol.WeaponColOff();

    }
}
