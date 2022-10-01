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

    public BoxCollider hitBox;
    public BoxCollider hurtBox;

    public PlayerClass characterClass;

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
