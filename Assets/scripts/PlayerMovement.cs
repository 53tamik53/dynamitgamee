using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;


public class PlayerMovement : MonoBehaviour
{
    public MovementJoystick movementJoystick;
    public float playerSpeed;

    private Rigidbody2D rb;

    public GameObject bomba;
    

    private bool canPickupBomb = true; 
    public Transform Hands; 


    void Start()
    {
        bomba.GetComponent<Bomb>().currentObject = this.gameObject;
        rb = GetComponent<Rigidbody2D>();
       
    }

    void FixedUpdate()
    {
        if (movementJoystick.joystickVec.y != 0 || movementJoystick.joystickVec.x != 0)
        {
            Vector2 moveDirection = new Vector2(movementJoystick.joystickVec.x, movementJoystick.joystickVec.y).normalized;

            rb.velocity = new Vector2(moveDirection.x * playerSpeed, moveDirection.y * playerSpeed);

            float angle = Mathf.Atan2(moveDirection.y, moveDirection.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("human") || collision.gameObject.CompareTag("bomberman"))
        {
            test3 enemy = collision.gameObject.GetComponent<test3>();
            if (!canPickupBomb)
            {
                return;
            }
          
            Transform enemyTransform = collision.gameObject.transform;
            Transform playerTransform = transform;

            if (enemyTransform.childCount <= 3 && playerTransform.childCount <= 3)
            {
                return;
            }

            if (enemyTransform.childCount > 3)
            {
                if (canPickupBomb)
                {
                    bomba.transform.parent = playerTransform;
                    bomba.transform.position = Hands.position;
                    bomba.GetComponent<Bomb>().currentObject = this.gameObject;
                    bomba.GetComponent<Bomb>().previousObject = enemyTransform.gameObject;

                    this.gameObject.transform.tag = "bomberman";
                    enemyTransform.tag = "human";
                    this.playerSpeed = 1.75f;
                    collision.gameObject.GetComponent<test3>().enemySpeed = 1.5f;

                    canPickupBomb = false; 
                    StartCoroutine(ResetBombPickup());
                }

            }
            else if (playerTransform.childCount > 3)
            {
                if (canPickupBomb)
                {
                    
                    
                    bomba.transform.parent = enemyTransform.transform;
                    bomba.transform.position = enemy.Hands.position;
                    bomba.GetComponent<Bomb>().currentObject = enemyTransform.gameObject;
                    bomba.GetComponent<Bomb>().previousObject = this.gameObject;

                    enemy.Freezetime = 1;
                    collision.gameObject.GetComponent<Rigidbody2D>().velocity = new Vector2(0, 0);

                    this.gameObject.transform.tag = "human";
                    this.playerSpeed = 2f;
                    collision.gameObject.GetComponent<test3>().enemySpeed = 1.3f;
                    
                    enemyTransform.tag = "bomberman";

                    canPickupBomb = false;
                    StartCoroutine(ResetBombPickup());
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