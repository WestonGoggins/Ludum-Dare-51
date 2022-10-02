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

    public int knightDamage = 10;
    #endregion

    #region CLASS VARIABLES
    [HideInInspector]
    public BoxCollider2D hitBox;
    [HideInInspector]
    public BoxCollider2D hurtBox;
    [HideInInspector]
    public bool inSwapState = false;
    [HideInInspector]
    public int currentLane;
    
    private Animator animator;
    [HideInInspector]
    public float attackCooldown = 0.0f;
    private float hurtBoxTimer = 0.0f;
    private List<EnemyController> enemiesHit;
    private float castLength;
    private LightningBallController lightningBall;
    #endregion

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        hitBox = GetComponent<BoxCollider2D>();
        if (characterClass == PlayerClass.Knight)
        {
            hurtBox = transform.Find("Hurtbox").GetComponent<BoxCollider2D>();
            hurtBox.enabled = false;
        }
        if (characterClass == PlayerClass.Mage)
        {
            lightningBall = transform.Find("LightningBall").GetComponent<LightningBallController>();
            lightningBall.enabled = false;
        }
        //implement throwing axe here
        animator = GetComponent<Animator>();
        currentLane = startingLane;
    }

    void Update()
    {
        if (attackCooldown > 0.0f) attackCooldown -= Time.deltaTime;
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
        if (hurtBoxTimer > 0.0f) hurtBoxTimer -= Time.deltaTime;
        if (hurtBoxTimer <= 0.0f && hurtBox.enabled)
        {
            hurtBox.enabled = false;
            Destroy(hurtBox.transform.GetChild(0).gameObject);
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && attackCooldown <= 0.0f && !gameController.swapping && 
            !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            attackCooldown = attackCooldownStart;
            hurtBoxTimer = hurtBoxTimeStart;
            hurtBox.enabled = true;
            animator.Play("knightattack");
            Instantiate(swordSwipe, hurtBox.transform);
        }

        if (hurtBox.isActiveAndEnabled)
        {
            ContactFilter2D filter = new ContactFilter2D();
            filter.NoFilter();
            Collider2D[] results = new Collider2D[0];
            int collisions = hurtBox.OverlapCollider(filter, results);
            for (int i = 0; i < collisions; i++)
            {
                if (results[i].gameObject.tag == "Enemy")
                {
                    if (!enemiesHit.Contains(results[i].gameObject.GetComponent<EnemyController>()))
                    {
                        enemiesHit.Add(results[i].gameObject.GetComponent<EnemyController>());
                        results[i].gameObject.GetComponent<EnemyController>().isHit(knightDamage);
                    }
                }
            }
        }
    }

    private void HandleMage()
    {
        if (Input.GetKey(KeyCode.LeftArrow) && attackCooldown <= 0.0f && !gameController.swapping &&
           !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            castLength += 0.01f;
        }
        else if (Input.GetKeyUp(KeyCode.LeftArrow) && attackCooldown <= 0.0f && !gameController.swapping &&
           !Input.GetKey(KeyCode.LeftShift) && !Input.GetKey(KeyCode.RightShift))
        {
            lightningBall.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            lightningBall.castLength = castLength;
            lightningBall.enabled = true;
        }
        
        castLength = 0;
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
