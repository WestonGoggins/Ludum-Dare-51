using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class EnemyController : MonoBehaviour
{
    public enum EnemyType
    {
        Melee,
        Ranged,
        Canine,
        Tough
    }
    public enum EnemyForm
    {
        Overworld,
        Hell,
        Faerie
    }

    public EnemyType enemyType = EnemyType.Melee;
    [HideInInspector]
    public EnemyForm enemyForm;
    private GameController gameController;
    [HideInInspector]
    public int hp;

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }

        if (gameController.currentDimension == GameController.Dimension.Overworld) enemyForm = EnemyForm.Overworld;
        else if (gameController.currentDimension == GameController.Dimension.Hell) enemyForm = EnemyForm.Hell;
        else if (gameController.currentDimension == GameController.Dimension.Faerie) enemyForm = EnemyForm.Faerie;
        else Debug.LogError("Invalid Dimension!");

        if (enemyType == EnemyType.Melee) hp = 20;
        else if (enemyType == EnemyType.Ranged) hp = 15;
        else if (enemyType == EnemyType.Canine) hp = 10;
        else if (enemyType == EnemyType.Tough) hp = 60;
    }

    void LateUpdate()
    {
        if (hp <= 0)
        {
            Destroy(this);
        }
        if (transform.position.x < -11)
        {
            Destroy(this);
        }
        if (enemyType == EnemyType.Melee) HandleMelee();
        if (enemyType == EnemyType.Ranged) HandleRanged();
        if (enemyType == EnemyType.Canine) HandleCanine();
        if (enemyType == EnemyType.Tough) HandleTough();
        
    }

    private void HandleMelee()
    {
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;

            }
            transform.position = new Vector3(transform.position.x - 0.01f * Time.deltaTime, transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {

        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {

        }
    }

    private void HandleRanged()
    {
        
    }

    private void HandleCanine()
    {

    }

    private void HandleTough()
    {

    }

    public void isHit(int damageTaken)
    {
        hp -= damageTaken;
    }    
}
