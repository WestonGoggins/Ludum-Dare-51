using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public enum PlayerClass
    {
        Knight,
        Mage,
        Barbarian
    }

    #region SERIALIZED FIELDS
    public GameController gameController;
    public PlayerClass characterClass;

    [SerializeField]
    private int startingLane;
    [SerializeField]
    private float attackCooldownStart = 0.5f;
    [SerializeField]
    private float hurtBoxTimeStart = 0.2f;

    public GameObject swordSwipe;
    #endregion

    #region CLASS VARIABLES
    [HideInInspector]
    public BoxCollider2D hitBox;
    [HideInInspector]
    public BoxCollider2D hurtBox;
    [HideInInspector]
    public bool inSwapState = false;
    //[HideInInspector]
    public int currentLane;
    
    private Animator animator;
    [HideInInspector]
    public float attackCooldown = 0.0f;
    private float hurtBoxTimer = 0.0f;
    #endregion

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        hitBox = GetComponent<BoxCollider2D>();
        hurtBox = transform.Find("Hurtbox").GetComponent<BoxCollider2D>();
        hurtBox.enabled = false;
        animator = GetComponent<Animator>();
        currentLane = startingLane;
    }

    void Update()
    {
        if (attackCooldown > 0.0f) attackCooldown -= Time.deltaTime;
        if (hurtBoxTimer > 0.0f) hurtBoxTimer -= Time.deltaTime;
        if (hurtBoxTimer <= 0.0f && hurtBox.enabled)
        {
            hurtBox.enabled = false;
            Destroy(hurtBox.transform.GetChild(0).gameObject);
        }
        switch (characterClass)
        {
            case PlayerClass.Knight:
                HandleKnight();
                break;
            case PlayerClass.Mage:
                HandleMage();
                break;
            case PlayerClass.Barbarian:
                HandleBarbarian();
                break;
            default:
                Debug.Log("Invalid Character Class!");
                break;
        }    
    }

    private void HandleKnight()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && attackCooldown <= 0.0f && !gameController.swapping && 
            !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            attackCooldown = attackCooldownStart;
            hurtBoxTimer = hurtBoxTimeStart;
            hurtBox.enabled = true;
            animator.Play("knightattack");
            Instantiate(swordSwipe, hurtBox.transform);
        }
    }

    private void HandleMage()
    {

    }

    private void HandleBarbarian()
    {

    }

    public void GoToLane(Vector3 pos, int lane)
    {
        transform.position = pos;
        currentLane = lane;
    }    
}
