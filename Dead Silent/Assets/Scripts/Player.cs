using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] CharacterController Controller;
   
    [SerializeField] private float interactionDistance;

    [SerializeField] int Health;

    [SerializeField] float Speed;
    [SerializeField] float SprintMod;
    [SerializeField] float CrouchMod;

    [SerializeField] float CrouchHeightMod;
    [SerializeField] int JumpMax;
    [SerializeField] int JumpSpeed;
    [SerializeField] int Gravity;
    
    [SerializeField] LayerMask InteractionMask;

    [SerializeField] GameObject intIcon;

    [SerializeField] FireArm Weapon;

    Vector3 MoveDir;
    Vector3 PlayerVel;

    bool IsCrouch;
    bool IsSprint;

    IInteractable interact;
    int JumpCount;

    private int MaxHealth;

    // Start is called before the first frame update
    void Start()
    {
        MaxHealth = Health;
        UpdatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();

        if (Input.GetButton("Fire1") && Weapon != null)
        {
            Weapon.Attack();
        }

        CheckInteraction();
        if (Input.GetButton("Fire2") && interact != null)
        {
            interact.Interact();
        }

    }

    void Movement()
    {
        if (Controller.isGrounded)
        {
            JumpCount = 0;
            PlayerVel = Vector3.zero;
        }

        MoveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        Controller.Move(MoveDir * Speed * Time.deltaTime);
        
        Sprint();
        Crouch();

        if (Input.GetButtonDown("Jump") && JumpCount < JumpMax)
        {
            JumpCount++;
            PlayerVel.y = JumpSpeed;
        }

        PlayerVel.y -= Gravity * Time.deltaTime;
        Controller.Move(PlayerVel * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            if (IsCrouch)
            {
                UnCrouch();
            }
            DoSprint();
        }
        else if (Input.GetButtonUp("Sprint") && !IsCrouch)
        {
            UnSprint();
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (IsSprint)
            {
                UnSprint();
            }
            DoCrouch();
        }
        else if (Input.GetButtonUp("Crouch") && !IsSprint)
        {
            UnCrouch();
        }
    }

    void DoCrouch()
    {
        IsCrouch = true;
        Speed *= CrouchMod;
        Controller.transform.localScale =
            new Vector3(transform.localScale.x, transform.localScale.y * CrouchHeightMod, transform.localScale.z);
    }

    void UnCrouch()
    {
        IsCrouch = false;
        Speed /= CrouchMod;
        Controller.transform.localScale =
            new Vector3(transform.localScale.x, transform.localScale.y / CrouchHeightMod, transform.localScale.z);
    }

    void DoSprint()
    {
        Speed *= SprintMod;
        IsSprint = true;
    }

    void UnSprint()
    {
        IsSprint = false;
        Speed /= SprintMod;
    }

    public void TakeDamage(int amount)
    {
        Health -= amount;
        UpdatePlayerUI();
        StartCoroutine(flashDamage());

        if (Health <= 0)
        {
            // trigger loss
            GameManager.Instance.lostState();
        }
    }
    IEnumerator flashDamage()
    {
        GameManager.Instance.playerDFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.playerDFlash.SetActive(false);
    }

    void UpdatePlayerUI()
    {
        // update health bar
        GameManager.Instance.PlayerHPBar.fillAmount = (float)Health / MaxHealth;
    }

    void CheckInteraction()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, interactionDistance, InteractionMask))
        {
         

           if (hit.collider.TryGetComponent(out IInteractable interactable))
            {
                interact = interactable;
            }
            else
            {
                interact = null;
            }
                
                
        }
    }

}
