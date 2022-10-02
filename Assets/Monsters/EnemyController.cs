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
    private enum EnemyForm
    {
        Overworld,
        Hell,
        Faerie
    }

    public EnemyType enemyType = EnemyType.Melee;
    [HideInInspector]
    private EnemyForm enemyForm;
    private GameController gameController;
    [HideInInspector]
    public int hp;
    public int lane;
    public float speed;
    public int damage;

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
        else if (enemyType == EnemyType.Ranged) HandleRanged();
        else if (enemyType == EnemyType.Canine) HandleCanine();
        else if (enemyType == EnemyType.Tough) HandleTough();
        
    }

    private void HandleMelee()
    {
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                speed = 3.0f;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                speed = 4.0f;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                speed = 5.0f;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
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
