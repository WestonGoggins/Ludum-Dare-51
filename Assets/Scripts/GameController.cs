using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Dimension
    {
        Overworld,
        Hell,
        Faerie
    }

    #region SERIALIZED FIELDS
    [SerializeField]
    private float dimensionShiftTimeStart = 10.0f;
    [SerializeField]
    private float levelTimeStart = 120.0f;
    [SerializeField]
    private float swapCooldownStart = 0.5f;
    [SerializeField]
    private Dimension startingDimension = Dimension.Overworld;

    public PlayerController knight;
    public PlayerController mage;
    public PlayerController barbarian;
    public Camera mainCamera;
    public Transform[] lanePositions;
    public SpawnerController spawnerController;
    #endregion

    #region CLASS VARIABLES
    private float dimensionShiftTimer;
    [HideInInspector]
    public float roundTimer;
    private float swapCooldown = 0.0f;
    [HideInInspector]
    public Dimension currentDimension;
    [HideInInspector]
    public bool swapping = false;
    #endregion

    void Awake()
    {
        if (spawnerController == null)
        {
            spawnerController = FindObjectOfType<SpawnerController>();
        }
        dimensionShiftTimer = dimensionShiftTimeStart;
        roundTimer = levelTimeStart;
        currentDimension = startingDimension;
        ChangeToCurrentDimension();
    }

    void Update()
    {
        HandleTimers();
        HandleSwap();
    }

    private void HandleTimers()
    {
        roundTimer -= Time.deltaTime;
        if (roundTimer <= 0.0f)
        {
            spawnerController.IncrementRound();
        }
        dimensionShiftTimer -= Time.deltaTime;
        if (dimensionShiftTimer < 0.0f)
        {
            int rand = Random.Range(0, 2);
            switch (currentDimension)
            {
                case Dimension.Overworld:
                    if (rand == 0) currentDimension = Dimension.Hell;
                    else currentDimension = Dimension.Faerie;
                    break;
                case Dimension.Hell:
                    if (rand == 0) currentDimension = Dimension.Overworld;
                    else currentDimension = Dimension.Faerie;
                    break;
                case Dimension.Faerie:
                    if (rand == 0) currentDimension = Dimension.Overworld;
                    else currentDimension = Dimension.Hell;
                    break;
                default:
                    Debug.LogError("Invalid Dimension!");
                    break;
            }
            ChangeToCurrentDimension();
            dimensionShiftTimer = dimensionShiftTimeStart;
        }
        if (swapCooldown > 0.0f) swapCooldown -= Time.deltaTime;
    }
    private void ChangeToCurrentDimension()
    {
        switch (currentDimension)
        {
            case Dimension.Overworld:
                mainCamera.backgroundColor = Color.green;
                break;
            case Dimension.Hell:
                mainCamera.backgroundColor = Color.red;
                break;
            case Dimension.Faerie:
                mainCamera.backgroundColor = Color.blue;
                break;
        }
    }

    private void HandleSwap()
    {
        if (swapCooldown <= 0.0f && (Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift)))
        {
            if (!swapping) swapping = true;
            if (Input.GetKeyDown(KeyCode.UpArrow) && knight.attackCooldown <= 0.0f)
            {
                if (!knight.inSwapState) knight.inSwapState = true;
                if (mage.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int knightLane = knight.currentLane;
                    knight.GoToLane(lanePositions[mage.currentLane - 1].position, mage.currentLane);
                    mage.GoToLane(lanePositions[knightLane - 1].position, knightLane);
                }
                else if (barbarian.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int knightLane = knight.currentLane;
                    knight.GoToLane(lanePositions[barbarian.currentLane - 1].position, barbarian.currentLane);
                    barbarian.GoToLane(lanePositions[knightLane - 1].position, knightLane);
                }
            }
            else if (Input.GetKeyDown(KeyCode.LeftArrow) && mage.attackCooldown <= 0.0f)
            {
                if (!mage.inSwapState) mage.inSwapState = true;
                if (knight.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int mageLane = mage.currentLane;
                    mage.GoToLane(lanePositions[knight.currentLane - 1].position, knight.currentLane);
                    knight.GoToLane(lanePositions[mageLane - 1].position, mageLane);
                }
                else if (barbarian.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int mageLane = mage.currentLane;
                    mage.GoToLane(lanePositions[barbarian.currentLane - 1].position, barbarian.currentLane);
                    barbarian.GoToLane(lanePositions[mageLane - 1].position, mageLane);
                }
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow) && barbarian.attackCooldown <= 0.0f)
            {
                if (!barbarian.inSwapState) barbarian.inSwapState = true;
                if (knight.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int barbarianLane = barbarian.currentLane;
                    barbarian.GoToLane(lanePositions[knight.currentLane - 1].position, knight.currentLane);
                    knight.GoToLane(lanePositions[barbarianLane - 1].position, barbarianLane);
                }
                else if (mage.inSwapState)
                {
                    swapCooldown = swapCooldownStart;
                    knight.inSwapState = false;
                    mage.inSwapState = false;
                    barbarian.inSwapState = false;
                    int barbarianLane = barbarian.currentLane;
                    barbarian.GoToLane(lanePositions[mage.currentLane - 1].position, mage.currentLane);
                    mage.GoToLane(lanePositions[barbarianLane - 1].position, barbarianLane);
                }
            }
            
        }
        else
        {
            if (swapping) swapping = false;
            if (knight.inSwapState) knight.inSwapState = false;
            if (mage.inSwapState) mage.inSwapState = false;
            if (barbarian.inSwapState) barbarian.inSwapState = false;
        }
    }
}
