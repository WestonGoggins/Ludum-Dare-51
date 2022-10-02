using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningBallController : MonoBehaviour
{
    public float castLength = 0.0f;
    public int damage = 5;
    public float speed = 5.0f;
    private float counter = 0.0f;
    private List<EnemyController> enemiesHit;
    [HideInInspector]
    public BoxCollider2D hurtBox;
    private float globalY = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        hurtBox = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(transform.position.x + (speed * Time.deltaTime), globalY, transform.position.z);
        counter += speed * Time.deltaTime;
        if (counter > castLength)
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
        counter = 0.0f;
        enemiesHit = new List<EnemyController>();
    }
}
