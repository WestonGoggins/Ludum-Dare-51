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
        Tough,
        Projectile
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
    private SpriteRenderer renderSprite;
    private bool rotateRight = true;
    public float rotationSpeed = 10.0f;
    private float hitTimer = 0.0f;
    public bool isProjectile = false;
    private float maxWalk;
    private float lifeTime = 0.0f;
    private float projectileTimer = 3.0f;
    public GameObject projectileSpawn;

    #region AUDIOCLIPS
    //private AudioSource audioSource;
    //private AudioClip lightningBallClip;
    #endregion

    void Start()
    {
        if (gameController == null)
        {
            gameController = FindObjectOfType<GameController>();
        }
        //audioSource = gameController.GetComponent<AudioSource>();
        animator = gameObject.GetComponentInChildren<Animator>();
        render = transform.Find("Sprite");
        renderSprite = render.GetComponent<SpriteRenderer>();

        maxWalk = 3.0f + Random.Range(0.0f, 2.0f);
        speedMod = Random.Range(-0.30f, 0.30f);
        rotationSpeed += speedMod * 5;

        if (enemyType == EnemyType.Melee) hp = 15;
        else if (enemyType == EnemyType.Ranged) hp = 10;
        else if (enemyType == EnemyType.Canine) hp = 5;
        else if (enemyType == EnemyType.Tough) hp = 40;
        else if (enemyType == EnemyType.Projectile) hp = 1;
    }

    void LateUpdate()
    {
        lifeTime += Time.deltaTime;
        if (!isProjectile)
        {
            if (enemyType != EnemyType.Ranged || lifeTime < maxWalk)
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
            }
            else
            {
                if (rotateRight)
                {
                    render.eulerAngles = new Vector3(0, 0, render.eulerAngles.z - Time.deltaTime * (rotationSpeed/2));
                    if (render.eulerAngles.z < 345.0f && render.eulerAngles.z > 180.0f) rotateRight = false;
                }
                else
                {
                    render.eulerAngles = new Vector3(0, 0, render.eulerAngles.z + Time.deltaTime * (rotationSpeed/2));
                    if (render.eulerAngles.z > 15.0f && render.eulerAngles.z < 180.0f) rotateRight = true;
                }
            }
            
        }

        if (hp <= 0)
        {
            Destroy(gameObject);
        }
        if (transform.position.x < -11)
        {
            Destroy(gameObject);
        }
        if (hitTimer > 0.0f) hitTimer -= Time.deltaTime;
        else renderSprite.color = Color.white;
        if (enemyType == EnemyType.Melee) HandleMelee();
        else if (enemyType == EnemyType.Ranged) HandleRanged();
        else if (enemyType == EnemyType.Canine) HandleCanine();
        else if (enemyType == EnemyType.Tough) HandleTough();
        else if (enemyType == EnemyType.Projectile) HandleProjectile();
        
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
                speed = 1.8f + speedMod;
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
                speed = 2.1f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else Debug.LogWarning("Invalid Dimension!");
    }

    private void HandleRanged()
    {
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                animator.Play("rangedmonsteroverworld");
                speed = 1.1f + speedMod;
                damage = 0;
            }
            if (lifeTime < maxWalk) transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
            else
            {
                projectileTimer -= Time.deltaTime;
                if (projectileTimer <= 0.0f)
                {
                    GameObject temp = Instantiate(projectileSpawn, transform.parent);
                    temp.transform.position = transform.position;
                    projectileTimer = 6.0f;
                }
            }
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Hell)
            {
                enemyForm = EnemyForm.Hell;
                animator.Play("rangedmonsterhell");
                speed = 1.2f + speedMod;
                damage = 0;
            }
            if (lifeTime < maxWalk) transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
            else
            {
                projectileTimer -= Time.deltaTime;
                if (projectileTimer <= 0.0f)
                {
                    GameObject temp = Instantiate(projectileSpawn, transform.parent);
                    temp.transform.position = transform.position;
                    projectileTimer = 6.0f;
                }
            }
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            if (enemyForm != EnemyForm.Faerie)
            {
                enemyForm = EnemyForm.Faerie;
                animator.Play("rangedmonsterfaerie");
                speed = 1.3f + speedMod;
                damage = 0;
            }
            if (lifeTime < maxWalk) transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
            else
            {
                projectileTimer -= Time.deltaTime;
                if (projectileTimer <= 0.0f)
                {
                    GameObject temp = Instantiate(projectileSpawn, transform.parent);
                    temp.transform.position = transform.position;
                    projectileTimer = 6.0f;
                }
            }
        }
        else Debug.LogWarning("Invalid Dimension!");
    }

    private void HandleCanine()
    {

    }

    private void HandleTough()
    {

    }

    private void HandleProjectile()
    {
        speed = 4.0f;
        damage = 2;
        transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
    }

    //public void isHit(int damageTaken)
    //{
    //    hp -= damageTaken;
    //}

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isProjectile)
        {
            if (collision.gameObject.tag == "PlayerAttack")
            {
                renderSprite.color = Color.red;
                hitTimer = 0.2f;
                if (collision.gameObject.name == "Sword")
                {
                    hp -= 10;
                }
                else if (collision.gameObject.name == "LightningBall")
                {
                    //audioSource?.PlayOneShot(lightningBallClip);
                    hp -= 5;
                }
                else if (collision.gameObject.name == "Axe")
                {
                    hp -= 15;
                }
            }
        }
    }
}
