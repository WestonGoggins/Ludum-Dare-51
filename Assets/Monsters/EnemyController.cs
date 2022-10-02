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
        Faerie,
        Null
    }

    public EnemyType enemyType = EnemyType.Melee;

    private EnemyForm enemyForm = EnemyForm.Null;
    private GameController gameController;
    private Animator animator;
    //[HideInInspector]
    public int hp;
    public int lane;
    public float speed;
    public int damage;
    private float speedMod;
    private Transform render;
    private bool rotateRight = true;
    public float rotationSpeed = 10.0f;

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        animator = gameObject.GetComponentInChildren<Animator>();
        render = transform.Find("Sprite");
        //currentAngle = render.rotation.z;

        speedMod = Random.Range(-0.30f, 0.30f);
        rotationSpeed += speedMod * 5;

        if (enemyType == EnemyType.Melee) hp = 1;
        else if (enemyType == EnemyType.Ranged) hp = 15;
        else if (enemyType == EnemyType.Canine) hp = 10;
        else if (enemyType == EnemyType.Tough) hp = 60;
    }

    void LateUpdate()
    {
        if (rotateRight)
        {
            render.eulerAngles = new Vector3(0, 0, render.eulerAngles.z - Time.deltaTime * rotationSpeed);
            if (render.eulerAngles.z < 345.0f && render.eulerAngles.z > 180.0f) rotateRight = false;
        }
        else
        {
            render.eulerAngles = new Vector3(0, 0, render.eulerAngles.z + Time.deltaTime * rotationSpeed);
            if (render.eulerAngles.z > 15.0f && render.eulerAngles.z < 180.0f) rotateRight = true;
        }
            //render.rotation = Quaternion.Lerp(new Quaternion(render.rotation.x, render.rotation.y, 15.0f, render.rotation.w),
            //    new Quaternion(render.rotation.x, render.rotation.y, -15.0f, render.rotation.w), Time.deltaTime * rotationSpeed);
        //else
        //{
        //    Debug.Log("Rotating Left");
        //    currentAngle = Mathf.LerpAngle(currentAngle, 195.0f, Time.deltaTime);
        //    render.eulerAngles = new Vector3(0, 0, currentAngle);
        //    if (currentAngle <= 165.0f) rotateRight = true;
        //    //render.rotation = Quaternion.Lerp(new Quaternion(render.rotation.x, render.rotation.y, -15.0f, render.rotation.w),
        //    //    new Quaternion(render.rotation.x, render.rotation.y, 15.0f, render.rotation.w), Time.deltaTime * rotationSpeed);
        //}
        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
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
                animator.Play("meleemonsteroverworld");
                speed = 1.5f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Hell)
            {
                enemyForm = EnemyForm.Hell;
                animator.Play("meleemonsterhell");
                speed = 1.5f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            if (enemyForm != EnemyForm.Faerie)
            {
                enemyForm = EnemyForm.Faerie;
                animator.Play("meleemonsterfaerie");
                speed = 1.5f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else Debug.LogWarning("Invalid Dimension!");
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
