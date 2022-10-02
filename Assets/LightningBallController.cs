using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallController : MonoBehaviour
{
    public float castLength = 0.0f;
    public int damage = 5;
    public float speed = 0.01f;
    private float counter = 0.0f;
    private List<EnemyController> enemiesHit;
    [HideInInspector]
    public BoxCollider2D hurtBox;
    // Start is called before the first frame update
    void Start()
    {
        hurtBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + speed * Time.deltaTime, transform.position.y, transform.position.z);
        counter += speed * Time.deltaTime;
        if (counter > castLength)
        {
            enabled = false;
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
}
