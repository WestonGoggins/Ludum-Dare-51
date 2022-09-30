using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    enum Dimension
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
    private Dimension startingDimension = Dimension.Overworld;

    public Transform knight;
    public Transform mage;
    public Transform barbarian;
    #endregion

    #region PRIVATE VARIABLES
    private float dimensionShiftTimer;
    private float levelTimer;
    private Dimension currentDimension;
    #endregion

    void Awake()
    {
        dimensionShiftTimer = dimensionShiftTimeStart;
        levelTimer = levelTimeStart;
        currentDimension = startingDimension;
    }

    void Update()
    {
        levelTimer -= Time.deltaTime;
        if (levelTimer <= 0.0f)
        {

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
            dimensionShiftTimer = dimensionShiftTimeStart;
        }
    }
}
