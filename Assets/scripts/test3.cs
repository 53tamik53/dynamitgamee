using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test3 : MonoBehaviour
{
    public float enemySpeed = 2f;
    private Rigidbody2D rb;

    public Transform Hands;
    private bool canPickupBomb = true;
    [SerializeField] private GameObject bomba;

    public Transform[] patrolPoints;
    private int currentPatrolPointIndex;

    private GameObject[] potentialTargets;
    private Transform nearestTarget;

    private float time=0f;
    public float Freezetime = 0;


    private Transform ebeOyuncu;
    public float rundirection;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentPatrolPointIndex = Random.Range(0, patrolPoints.Length);
    }

    private void Update()
    {
        time -= Time.deltaTime * 0.2f;
        Freezetime -= Time.deltaTime * 0.75f;
        if (this.gameObject.tag == "human")
        {
            ebeOyuncu = GameObject.FindGameObjectWithTag("bomberman").transform;
            Vector2 runrange = (Vector2)transform.position - (Vector2)ebeOyuncu.position;
            float range = runrange.magnitude;

            if (range > rundirection)
            {
                if (Vector2.Distance(transform.position, patrolPoints[currentPatrolPointIndex].position) < 0.2f)
                {
                    currentPatrolPointIndex = Random.Range(0, patrolPoints.Length);
                }
                else
                {
                    Vector2 moveDirection = (patrolPoints[currentPatrolPointIndex].position - transform.position).normalized;
                    float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
                    rb.velocity = new Vector2(moveDirection.x * enemySpeed, moveDirection.y * enemySpeed);
                }
            }
            else
            {
                if (time <= 0)
                {
                    Vector2 randomPoint = new Vector2(Random.Range(-10f, 10f), Random.Range(5f, 15f)); // Rastgele bir nokta belirleyin
                    Vector2 moveDirection = (randomPoint - (Vector2)transform.position).normalized;
                    transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg));
                    rb.velocity = new Vector2(moveDirection.x * enemySpeed, moveDirection.y * enemySpeed);
                    time = 0.2f;

                }
            }
        }
        else if (this.gameObject.tag == "bomberman")
        {
            if(Freezetime<=0)
            {
                potentialTargets = GameObject.FindGameObjectsWithTag("human");


                float minDistance = Mathf.Infinity;
                foreach (GameObject target in potentialTargets)
                {
                    float distance = Vector2.Distance(transform.position, target.transform.position);
                    if (distance < minDistance)
                    {
                        minDistance = distance;
                        nearestTarget = target.transform;
                    }
                }

                Vector2 moveDirection = nearestTarget.position - transform.position;
                moveDirection.Normalize();
                float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

                rb.velocity = new Vector2(moveDirection.x * enemySpeed, moveDirection.y * enemySpeed);

            }


        }
        
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if(collision.gameObject.GetComponent<PlayerMovement>())
        {
            return;
        }
        else
        {
            if (this.gameObject.tag == "bomberman")
            {
                Transform enemyTransform = collision.gameObject.transform;
                test3 enemy = collision.gameObject.GetComponent<test3>();
                
                if (collision.gameObject.tag == "human")
                {

                    if (!canPickupBomb)
                    {
                        return;
                    }
                    else if (collision.gameObject.GetComponent<test3>().canPickupBomb != true)
                    {
                        return;
                    }



                    if (enemyTransform.childCount > 3)
                    {
                        
                        return;
                    }

                    if (transform.childCount <= 3)
                    {
                        return;
                    }

                    if (canPickupBomb)
                    {

                        bomba.transform.parent = enemyTransform.transform;
                        bomba.transform.position = enemy.Hands.position;
                        bomba.GetComponent<Bomb>().currentObject = enemyTransform.gameObject;
                        bomba.GetComponent<Bomb>().previousObject = this.gameObject;

                        time = 0;
                        enemy.Freezetime = 1;
                        enemy.rb.velocity = new Vector2(0,0);
                        enemyTransform.tag = "bomberman";
                        collision.gameObject.GetComponent<test3>().enemySpeed = 1.3f;
                        this.enemySpeed = 1.5f;
                        this.gameObject.transform.tag = "human";

                        Vector2 randomPoint = enemyTransform.position - transform.position;
                        Vector2 moveDirection = (randomPoint - (Vector2)transform.position-new Vector2(0,10f)).normalized;
                        transform.rotation = Quaternion.Euler(new Vector3(0, 0, Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg));
                        rb.velocity = new Vector2(moveDirection.x * enemySpeed, moveDirection.y * enemySpeed);
                      



                        enemyTransform = null;

                        enemy = null;
                        canPickupBomb = false;

                        StartCoroutine(ResetBombPickup());

                    }
                }
            }
        }
        

    }

    private IEnumerator ResetBombPickup()
    {
        yield return new WaitForSeconds(1.5f);
        canPickupBomb = true;
    }

   
}