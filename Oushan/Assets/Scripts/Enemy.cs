using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Status")]
    public int life;

    Rigidbody2D rb;
    Animator anim;

    Player player;

    public enum stated { Idle, Patrol, Attack};
    public stated state;
    Transform colPlayerUp;

    int damagedQuant;
    // Start is called before the first frame update
    void Start()
    {
        damagedQuant = 1;
        rb = GetComponent<Rigidbody2D>();
        //anim = GetComponent<Animator>();
        colPlayerUp = transform.GetChild(0).GetComponent<Transform>();

        life = 100;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();

        state = stated.Idle;
    }

    // Update is called once per frame
    void Update()
    {
        ModificationState();
    }

    void ModificationState()
    {
        /*switch (state)
        {
            case stated.Idle:
                anim.SetTrigger("Idle");
                break;
            case stated.Patrol:
                anim.SetTrigger("Patrol");
                break;
            case stated.Attack:
                anim.SetTrigger("Attack");
                break;
        }*/
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
           
        }
    }

    void Dead()
    {
        if(life <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void Damage(int damage)
    {
        life -= damage;
        Dead();
    }


}
