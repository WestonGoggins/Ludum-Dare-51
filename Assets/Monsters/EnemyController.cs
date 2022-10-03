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
    public float teleportTimer = 100.0f;

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
        teleportTimer = 3.0f + Random.Range(0.0f, 4.0f);


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
        CheckTeleport();
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
                damage = 10;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Hell)
            {
                enemyForm = EnemyForm.Hell;
                animator.Play("meleemonsterhell");
                speed = 2.3f + speedMod;
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
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                animator.Play("rangedmonsteroverworld");
                speed = 1.0f + speedMod;
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
                    temp.GetComponent<EnemyController>().damage = 4;
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
                speed = 1.8f + speedMod;
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
                speed = 1.0f + speedMod;
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
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                animator.Play("caninemonsteroverworld");
                speed = 2.8f + speedMod;
                damage = 10;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Hell)
            {
                enemyForm = EnemyForm.Hell;
                animator.Play("caninemonsterhell");
                speed = 3.6f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            if (enemyForm != EnemyForm.Faerie)
            {
                enemyForm = EnemyForm.Faerie;
                animator.Play("caninemonsterfaerie");
                speed = 2.8f + speedMod;
                damage = 5;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else Debug.LogWarning("Invalid Dimension!");
    }

    private void HandleTough()
    {
        if (gameController.currentDimension == GameController.Dimension.Overworld)
        {
            if (enemyForm != EnemyForm.Overworld)
            {
                enemyForm = EnemyForm.Overworld;
                animator.Play("toughmonsteroverworld");
                speed = 1.0f;
                damage = 20;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Hell)
        {
            if (enemyForm != EnemyForm.Hell)
            {
                enemyForm = EnemyForm.Hell;
                animator.Play("toughmonsterhell");
                speed = 1.8f;
                damage = 10;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else if (gameController.currentDimension == GameController.Dimension.Faerie)
        {
            if (enemyForm != EnemyForm.Faerie)
            {
                enemyForm = EnemyForm.Faerie;
                animator.Play("toughmonsterfaerie");
                speed = 1.0f;
                damage = 10;
            }
            transform.position = new Vector3(transform.position.x - (speed * Time.deltaTime), transform.position.y, transform.position.z);
        }
        else Debug.LogWarning("Invalid Dimension!");
    }

    private void HandleProjectile()
    {
        speed = 4.0f;
        if (damage < 2) damage = 2;
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

    private void CheckTeleport()
    {
        teleportTimer -= Time.deltaTime;
        if (enemyForm == EnemyForm.Faerie)
        {
            if (teleportTimer <= 0.0f)
            {
                int rand = Random.Range(0, 2);
                GameObject poof = Instantiate(gameController.poof, gameController.transform);
                poof.transform.position = transform.position;
                if (transform.position.y > 0.0f)
                {
                    switch (rand)
                    {
                        case 0:
                            transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
                            break;
                        case 1:
                            transform.position = new Vector3(transform.position.x, transform.position.y - 4, transform.position.z);
                            break;
                        default:
                            Debug.LogWarning("Random is doing something wrong...");
                            break;
                    }
                }
                else  if (transform.position.y < 0.0f && transform.position.y > -2.0f)
                {
                    switch (rand)
                    {
                        case 0:
                            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                            break;
                        case 1:
                            transform.position = new Vector3(transform.position.x, transform.position.y - 2, transform.position.z);
                            break;
                        default:
                            Debug.LogWarning("Random is doing something wrong...");
                            break;
                    }
                }
                else if (transform.position.y < -2.0f)
                {
                    switch (rand)
                    {
                        case 0:
                            transform.position = new Vector3(transform.position.x, transform.position.y + 4, transform.position.z);
                            break;
                        case 1:
                            transform.position = new Vector3(transform.position.x, transform.position.y + 2, transform.position.z);
                            break;
                        default:
                            Debug.LogWarning("Random is doing something wrong...");
                            break;
                    }
                }
                GameObject poof2 = Instantiate(gameController.poof, gameController.transform);
                poof2.transform.position = transform.position;
            }
        }
        if (teleportTimer <= 0.0f) teleportTimer = 3.0f + Random.Range(0.0f, 4.0f); 
    }
}
