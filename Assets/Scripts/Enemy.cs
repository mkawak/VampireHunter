using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] 
    public Transform player;

    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;

    private Animator anim;
    private string walkAnimation = "walk";
    private string attackAnimation = "attack";

    private float health = 5f;
    private float minDistance = 0.30f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
        anim = this.GetComponent<Animator>();
    }

    private void Update()
    {
        getMovement(player);

        if (getHealth() <= 0) //if health is 0 or less, enemy is dead
        {
            Die();
            Debug.Log(gameObject + " being destroyed.");
        }
    }

    private void FixedUpdate()
    {
        MoveEnemy(movement);
        AnimateEnemy(movement);
    }

    void setHealth(float health)
    {
        this.health = health;
    }

    float getHealth()
    {
        return this.health;
    }


    void getMovement(Transform player)
    {
        Vector3 direction = player.position - transform.position; //tranform is the enemy since our script is attached to enemy character
        /*float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg; ////Atan2- calculates the angle between enemy and player
        rb.rotation = angle;*/
        direction.Normalize();
        movement = direction;
    }


    void MoveEnemy(Vector2 direction)
    {
        rb.MovePosition((Vector2)transform.position + (direction * moveSpeed * Time.deltaTime));
        if (player.position.x > transform.position.x) //flip sprite
        {
            //player on right
            transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else
        {
            //player on left
            transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    void AnimateEnemy(Vector2 movement)
    {
        if (movement.x != 0)
        {
            anim.SetBool(walkAnimation, true);
        } else
        {
            anim.SetBool(walkAnimation, false);
        }
        if (getDistance(player) < minDistance) //attack animation
        {
            anim.SetBool(attackAnimation, true);
            Attack(player);
        }
        else
        {
            anim.SetBool(attackAnimation, false);
        }
            
    }

    public float TakeDamage (float damage)
    {
        float damageTaken = damage;
        float newHealth;

        if(damageTaken >= health)
        {
            damageTaken = health;
        }

        newHealth = getHealth() - damageTaken;
        setHealth(newHealth);
        Debug.Log("Health " + getHealth());
        return damageTaken;
    }

    public void Die()
    {
        Destroy(gameObject);
    }

    public float getDistance(Transform player)
    {
        float distance = Vector3.Distance(player.position, transform.position);
        return distance;
    }

    
    public void Attack(Transform player)
    {
        //call function to deal player damage
        Debug.Log("Attacking " + player);
    }
}
