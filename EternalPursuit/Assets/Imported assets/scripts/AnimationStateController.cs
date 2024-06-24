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
    int isReloadingHash;
    int RecoilHash;

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
        isReloadingHash = Animator.StringToHash("isReloading");
        RecoilHash = Animator.StringToHash("Recoil");
    }

    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool runPressed = Input.GetKey("left shift");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");

        bool isReloading = animator.GetBool(isReloadingHash);
        bool isshooting = Input.GetButton("Fire1");

        // Reload logic
        if (!isReloading && Input.GetKeyDown("r"))
        {
            animator.SetBool(isReloadingHash, true);
            StartCoroutine(ResetIsReloading());
        }

        // Shooting logic, allowing shooting during movement but not during reloading
        if (!isReloading && isshooting)
        {
            animator.SetBool(isShootingHash, true);
            animator.SetBool(RecoilHash, true);
        }
        else if (!isshooting)
        {
            animator.SetBool(isShootingHash, false);
            animator.SetBool(RecoilHash, false);
        }

        // Movement logic
        animator.SetBool(isWalkingHash, forwardPressed && !runPressed);
        animator.SetBool(isRunningHash, forwardPressed && runPressed);
        animator.SetBool(isWalkingBackHash, backPressed && !runPressed);
        animator.SetBool(isRunningBackHash, backPressed && runPressed);
        animator.SetBool(isRunningLeftHash, leftPressed);
        animator.SetBool(isRunningRightHash, rightPressed);

        IEnumerator ResetIsReloading()
        {
            yield return new WaitForSeconds(2); // Assuming the reload animation is 2 seconds long
            animator.SetBool(isReloadingHash, false);
        }
    }
}
