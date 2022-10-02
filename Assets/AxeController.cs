using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    public float throwLength = 0.0f;
    public int damage = 15;
    public float speed = 5.0f;
    public float throwTime = 0.0f;
    private float globalY = 0.0f;
    private List<EnemyController> enemiesHit;
    [HideInInspector]
    public BoxCollider2D hurtBox;
    // Start is called before the first frame update
    void Start()
    {
        hurtBox = GetComponent<BoxCollider2D>();
    }
    void Update()
    {
        //transform.position = MathParabola.Parabola(new Vector3(transform.parent.position.x, transform.parent.position.y, transform.parent.position.z),
        //    new Vector3(transform.parent.position.x + throwLength, transform.parent.position.y - 0.5f, transform.parent.position.z), 1.0f, speed);
        //if (transform.position.x > throwLength)
        //{
        //    if (!hurtBox.enabled) hurtBox.enabled = true;
        //}
        //else
        //{
        //    if (hurtBox.enabled) hurtBox.enabled = false;
        //}

        transform.position = new Vector3(transform.position.x, globalY, transform.position.z);
        throwTime += Time.deltaTime;
        
        if (throwTime >= 0.9f)
        {
            transform.position = new Vector3(-100f, 0f, 0f);
            gameObject.SetActive(false);
        }

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
                    results[i].gameObject.GetComponent<EnemyController>().isHit(damage);
                }
            }
        }
    }

    public void Reset()
    {
        globalY = transform.parent.position.y;
        throwTime = 0.0f;
        enemiesHit = new List<EnemyController>();
    }
}
