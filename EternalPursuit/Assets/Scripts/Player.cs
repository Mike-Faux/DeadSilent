
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Player : MonoBehaviour, IDamageable
{
    [SerializeField] CharacterController Controller;
    public Inventory inventory;

    public CameraController cameraController;

    [SerializeField] private float interactionDistance;

    [SerializeField] int Health;

    [SerializeField] float Speed;
    [SerializeField] float SprintMod;
    [SerializeField] float CrouchMod;


    [SerializeField] float CrouchHeightMod;
    [SerializeField] int JumpMax;
    [SerializeField] int JumpSpeed;
    [SerializeField] int Gravity;
    public FireArm firearm;
    [SerializeField] LayerMask InteractionMask;

    [SerializeField] GameObject intIcon;
    [SerializeField] GameObject weaponSlot;
    [SerializeField] IWeapon Weapon;
    [SerializeField] int StartingAmmoMags;
    
    public AudioSource aud;
    [SerializeField] AudioClip[] hurtAud;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float hurtAudVol;
    [Range(0, 1)][SerializeField] float audJumpVol;

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
        firearm = GetComponentInChildren<FireArm>();

        MaxHealth = Health;
        SpawnPlayer(GetTransform());
        aud = gameObject.AddComponent<AudioSource>();
        Weapon = weaponSlot.GetComponentInChildren<IWeapon>();
        inventory = new Inventory();

        if (Weapon.GetType() == typeof(FireArm))
        {
            FireArm fireArm = (FireArm)Weapon; 
            for (int i = 0; i < StartingAmmoMags; i++)
            {
                inventory.AddItems(new ItemStack(fireArm.Stats.Ammo_Type, fireArm.Stats.Ammo_Capacity));
            }
        }
        
        UpdateWeaponInfo();
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.y < -350) TakeDamage(1);

        if (!GameManager.Instance.pause && !GameManager.Instance.inventory)
        {
            Movement();

            
            if (Input.GetButton("Fire1") && Weapon != null && Weapon.GetType() == typeof(FireArm))
            {
                Weapon.Attack();
                
                
            }
            if (Input.GetButtonDown("Fire2") && firearm != null) 
            {
                firearm.DetermineAim();
            }


            UpdateWeaponInfo();

            CheckInteraction();
            if (Input.GetButton("Fire2") && interact != null)
            {
                interact.Interact(this);
            }

            if (Input.GetButtonDown("Reload"))
            {
                
                Reload();
            }
        }
    }

    public void Reload()
    {
        if (Weapon.GetType() == typeof(FireArm))
        {
            FireArm fireArm = (FireArm)Weapon;
            fireArm.Reload();
            GameManager.Instance.UpdateAmmoCount(fireArm.Ammo, inventory.GetItemCount(fireArm.Stats.Ammo_Type));
        }
    }

    public void UpdateWeaponInfo()
    {
        if (Weapon.GetType() == typeof(FireArm))
        {
            FireArm fireArm = (FireArm)Weapon;
            GameManager.Instance.UpdateWeaponName(fireArm.Stats.name);
            GameManager.Instance.UpdateAmmoCount(fireArm.Ammo, inventory.GetItemCount(fireArm.Stats.Ammo_Type));
        }
        else if (Weapon.GetType() == typeof(MeleeWeapon))
        {
            MeleeWeapon meleeWeapon = (MeleeWeapon)Weapon;
            GameManager.Instance.UpdateWeaponName(meleeWeapon.Stats.name);
            GameManager.Instance.UpdateAmmoCount(1, 1);
        }

    }

    public void EquipWeapon(IWeapon weapon)
    {

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

        if (!IsSprint) 
        {
            Crouch();
        }
        Sprint();

        if (Input.GetButtonDown("Jump") && JumpCount < JumpMax)
        {
           
            JumpCount++;
            PlayerVel.y = JumpSpeed;
           
            if (audJump != null && audJump.Length > 0 && aud != null)
            {
                aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            }
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
        else if (Input.GetButtonUp("Sprint"))
        {
            UnSprint();
        }
    }

    void Crouch()
    {
        if (Input.GetButtonDown("Crouch") && !IsSprint) 
        {
            DoCrouch();
        }
        else if (Input.GetButtonUp("Crouch") && IsCrouch) 
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
        StartCoroutine(FlashDamage());
        






        if (Health <= 0)
        {
            
            GameManager.Instance.gameStats.Deaths++;
            GameManager.Instance.lostState();
        }
       
    }
    IEnumerator FlashDamage()
    {
        GameManager.Instance.playerDFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        GameManager.Instance.playerDFlash.SetActive(false);
    }

    void UpdatePlayerUI()
    {
        
        GameManager.Instance.PlayerHPBar.fillAmount = (float)Health / MaxHealth;

    }

    public Transform GetTransform()
    {
        return transform;
    }

    public void SpawnPlayer(Transform transform)
    {
        Health = MaxHealth;
        UpdatePlayerUI();

        if (GameManager.Instance.playerSpawnPos != null)
        {
            Controller.enabled = false;
            transform.SetPositionAndRotation(GameManager.Instance.playerSpawnPos.transform.position, GameManager.Instance.playerSpawnPos.transform.rotation);
            Controller.enabled = true;
        }
        else
        {
            GameObject pos = GameObject.FindWithTag("Player Spawn Pos");
            if(pos != null)
            {
                GameManager.Instance.playerSpawnPos = pos;

                Controller.enabled = false;
                transform.position = GameManager.Instance.playerSpawnPos.transform.position;
                transform.rotation = GameManager.Instance.playerSpawnPos.transform.rotation;
                Controller.enabled = true;
            }
        }
    }

    void CheckInteraction()
    {
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out RaycastHit hit, interactionDistance, InteractionMask))
        {
            //Debug.Log("Interactable found!");
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
    

    public bool HasItem(ItemStack item)
    {
        return false;
    }
}
