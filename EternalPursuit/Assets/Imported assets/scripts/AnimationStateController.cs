using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationStateController : MonoBehaviour
{
    Animator animator;
    int isWalkingHash;
    int isRunningHash;
    int isRunningLeftHash;
    int isRunningRightHash;
    int isRunningBackHash;
    int isWalkingBackHash;
    int isShootingHash;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        isWalkingHash = Animator.StringToHash("isWalking");
        isRunningHash = Animator.StringToHash("isRunning");
        isRunningLeftHash = Animator.StringToHash("isRunningLeft");
        isRunningRightHash = Animator.StringToHash("isRunningRight");
        isRunningBackHash = Animator.StringToHash("isRunningBack");
        isWalkingBackHash = Animator.StringToHash("isWalkingBack");
        isShootingHash = Animator.StringToHash("isShooting");
    }

    // Update is called once per frame
    void Update()
    {
        bool isWalkingBack = animator.GetBool(isWalkingBackHash);
        bool isrunningBack = animator.GetBool(isRunningBackHash);
        bool isrunning = animator.GetBool(isRunningHash);
        bool isWalking = animator.GetBool(isWalkingHash);
        bool isShooting = animator.GetBool(isShootingHash);
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");
        bool isshooting = Input.GetMouseButton(0);

        if (!isShooting && isshooting)
        {
            animator.SetBool(isShootingHash, true);
        }
        if (isShooting && !isshooting)
        {
            animator.SetBool(isShootingHash, false);
        }
       

        if (!isWalkingBack && backPressed)
        {
            animator.SetBool(isWalkingBackHash, true);
        }
        if (isWalkingBack && !backPressed)
        {
            animator.SetBool(isWalkingBackHash, false);
        }

        if (!isrunningBack && (backPressed && runPressed))
        {
            animator.SetBool(isRunningBackHash, true);
        }
        if (isrunningBack && (!backPressed || !runPressed))
        {
            animator.SetBool(isRunningBackHash, false);
        }

        if (!isWalking && forwardPressed)
        {
            animator.SetBool(isWalkingHash, true);
        }
        if (isWalking && !forwardPressed)
        {
            animator.SetBool(isWalkingHash, false);
        }

        if (!isrunning && (forwardPressed && runPressed))
        {
            animator.SetBool(isRunningHash, true);
        }
        if (isrunning && (!forwardPressed || !runPressed))
        {
            animator.SetBool(isRunningHash, false);
        }

        if (isrunning && leftPressed)
        {
            animator.SetBool(isRunningLeftHash, true);
        }
        else
        {
            animator.SetBool(isRunningLeftHash, false);
        }

        if (isrunning && rightPressed)
        {
            animator.SetBool(isRunningRightHash, true);
        }
        else
        {
            animator.SetBool(isRunningRightHash, false);
        }
    }
}
