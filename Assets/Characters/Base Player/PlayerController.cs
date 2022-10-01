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
    #endregion

    void Start()
    {
        hitBox = GetComponent<BoxCollider2D>();
        hurtBox = GetComponentInChildren<BoxCollider2D>();
        hurtBox.enabled = false;
        animator = GetComponent<Animator>();
        currentLane = startingLane;
    }

    void Update()
    {
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
