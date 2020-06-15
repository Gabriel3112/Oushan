using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    [HideInInspector]
    public Rigidbody2D rb;
    [HideInInspector]
    public Animator anim;
    
    [Header("Status")]
    public float initialLife;
    public float life;

    public float initialStamina;
    public float stamina;

    public float reloadStamina;
    public float reloadLife;

    public float initialConsumptionJump, initialConsumptionJab, initialConsumptionDash;
    public float consumptionJump, consumptionJab, consumptionDash;



    [Header("Components Config")]
    public float groundDistance;
    public float speed;
    public float forceJump;
    public float move;
    public float distanceDamage;
    UIPlayer UI;
    SpriteRenderer spritePlayer;
    public bool flip;
    public enum states { move, fight};
    public states state;
    Ghost ghost;
    public bool isMove;

    public float speedWallSliding;
    public bool wallSliding;
    public bool wallJumping;
    public float xWallForce;
    public float yWallForce;
    public float wallDistance;


    public bool isDissolve;
    public float timeFadeDissolve;
    public Material materialDissolve;
    


    public Transform inventory;
    public Transform handR;

    [Header("Timers")]
    public float initialTimeReloadStamina;
    public float timeReloadStamina;

    public float initialTimeReloadLife;
    public float timeReloadLife;

    public float timeModePlayer;

    public float initialTimeMove;
    public float timeMove;

    public float initialTimeWallSliding;
    public float timeWallSliding;

    
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spritePlayer = GetComponent<SpriteRenderer>();
        inventory = transform.GetChild(0).GetComponent<Transform>();
        handR = inventory.transform.GetChild(0).GetComponent<Transform>();

        materialDissolve = GetComponent<SpriteRenderer>().material;

        ghost = GetComponent<Ghost>();
        
        UI = GetComponent<UIPlayer>();

        state = states.move;
        
        //inicialização dos status
        life = initialLife;
        stamina = initialStamina;

        //inicialização dos timers
        timeReloadLife = initialTimeReloadLife;
        timeReloadStamina = initialTimeReloadStamina;
        timeWallSliding = initialTimeWallSliding;

        //inicialização de consumo de stamina
        consumptionJump = initialConsumptionJump;
        consumptionDash = initialConsumptionDash;
        consumptionJab = initialConsumptionJab;

        timeModePlayer = 5;
        isMove = true;
        timeMove = initialTimeMove;
    }

    // Update is called once per frame
    void Update()
    {
        //Punch();
        MovePlayer();
        Jump();
        ModePlayer();
        OpenStatsWeapon();
        WallSlidingAndJumping();
        OnDissolve();
        Dead();
        CheckGround();
        CheckWall();
        Debug.Log("Wall: " + CheckWall() + " Ground: " + CheckGround());
        anim.SetBool("WallSliding", wallSliding);
    }




    //Movemment to player
    void MovePlayer()
    {
        move = Input.GetAxisRaw("Horizontal");
        if (isMove)
        {
            rb.velocity = new Vector2(move * speed, rb.velocity.y);

            anim.SetFloat("X", Mathf.Abs(move));
            anim.SetFloat("Y", Mathf.Abs(rb.velocity.y));
        }
        Flip();
        spritePlayer.flipX = flip;
    }
    void Flip()
    {
        if (move <= -0.01f  && !CheckWall())
        {
            flip = true;
        }
        else if (move >= 0.01f  && !CheckWall())
        {
            flip = false;
        }
    }
    
    //Stats to Player
    void Stamina(float stamina)
    {
        this.stamina -= stamina;
    }

    void ModePlayer()
    {
        switch (state)
        {
            case states.move:
                anim.SetLayerWeight(1, 0);
                break;
            case states.fight:
                anim.SetLayerWeight(1, 1);
                break;
        }

        if (move == 0 && state == states.fight)
        {
            timeModePlayer -= Time.deltaTime;
            if (timeModePlayer <= 0)
            {
                state = states.move;
                timeModePlayer = 5;
            }
        }
    }

    public void Damage(int damage)
    {
        life -= damage;
        UI.LifeBar(life);
        isMove = false;
        if (flip)
        {
            knockBack(15);
        }
        else
        {
            knockBack(-15);
        }
        if (!isMove)
        {
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", 0);
            Invoke("SetIsMoveToTrue", timeMove);
        }

    }

    public void Dead()
    {
        if (life <= 0)
        {
            isDissolve = true;
            isMove = false;
        }
    }
    
    //Actions to Player
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && CheckGround() && stamina >= consumptionJump && !CheckWall())
        {
            rb.velocity = new Vector2(rb.velocity.x, forceJump);
            Stamina(5);
        }
    }

    void WallSlidingAndJumping()
    {
        if (CheckWall() && !CheckGround() && move !=  0)
        {
            timeWallSliding -= Time.deltaTime;
            if(timeWallSliding <= 0)
            {
                wallSliding = false;
            }
            else
            {
                wallSliding = true;
                isMove = false;
            }
            if (rb.velocity.y < speedWallSliding && wallSliding)
            {
                rb.velocity = new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y, -speedWallSliding, float.MaxValue));
            }
            if (!CheckWall())
            {
                wallSliding = false;
            }
        }
        else if(!CheckWall() && CheckGround())
        {
            wallSliding = false;
            isMove = true;
            timeWallSliding = initialTimeWallSliding;
        }
        else if(CheckWall() && CheckGround())
        {
            anim.SetFloat("X", 0);
            anim.SetFloat("Y", 0);
            isMove = false;
            wallSliding = false;
        }
 

        if (Input.GetKeyDown(KeyCode.Space)&& !CheckGround() && CheckWall() && move != 0)
        {
            wallJumping = true;
            //wallSliding = false;
        }

        if (wallJumping)
        {
            rb.velocity = new Vector2(xWallForce * -move, yWallForce);
            if (CheckWallLeft())
            {
                flip = true;
                wallJumping = false;
            }else if(CheckWallRight())
            {
                flip = false;
                wallJumping = false;
            }
            else if(CheckGround()){
                wallJumping = false;
            }
            else
            {
                Invoke("SetWallJumpingToFalse", 0.2f);
            }
        }
    }
    
    public void knockBack(float force)
    {
        rb.velocity = Vector2.right * force;
    }

    //Check Collisions
    bool CheckGround()
    {
        if (Physics2D.Raycast(transform.position, Vector2.down * groundDistance, groundDistance, LayerMask.GetMask("Ground")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool CheckWall()
    {
        if (flip)
        {
           return CheckWallLeft();
        }
        else
        {
           return CheckWallRight();
        }
    }

    bool CheckWallLeft(){
        if(Physics2D.Raycast(transform.position, Vector2.left * wallDistance, wallDistance, LayerMask.GetMask("Wall")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    bool CheckWallRight(){
        if (Physics2D.Raycast(transform.position, Vector2.right * wallDistance, wallDistance, LayerMask.GetMask("Wall")))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        
       if (collision.gameObject.CompareTag("Enemy"))
        {
            if (Physics2D.Raycast(transform.position, Vector2.right * distanceDamage, LayerMask.GetMask("Enemy")))
            {
                Damage(25);
            }
            if (Physics2D.Raycast(transform.position, Vector2.left * distanceDamage, LayerMask.GetMask("Enemy")))
            {
                Damage(25);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.CompareTag("Weapon"))
        {
            UIWeaponStats wm = collision.gameObject.GetComponent<UIWeaponStats>();
            GameObject go = Instantiate(wm.weapon.weaponPrefab, handR.position, Quaternion.identity);
            go.transform.parent = handR;
            Destroy(wm.gameObject);
        }
    }

    public void OpenStatsWeapon()
    {
        RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);

        if (Input.GetMouseButtonDown(0))
        {
            if (hit.collider.gameObject.tag == "Weapon")
            {
                UIWeaponStats UI = hit.collider.gameObject.GetComponent<UIWeaponStats>();
                UI.isActivate = !UI.isActivate;
            }
            else if (hit.collider == null)
            {
                Debug.Log("Não tocou em nada");
            }
        }

    }

    //Draw Gizmos
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;

        //OverlapCircle for player collision to enemy
        Gizmos.DrawRay(transform.position, Vector2.right * distanceDamage);
        Gizmos.DrawRay(transform.position, Vector2.left * distanceDamage);

        //Ray for wallCollision
        Gizmos.DrawRay(transform.position, Vector2.left * wallDistance);
        Gizmos.DrawRay(transform.position, Vector2.right * wallDistance);

        //Ray groundCollission
        Gizmos.DrawRay(transform.position, Vector2.down * groundDistance);
    }
   
    //FX to Player
    public void OnDissolve()
    {
        if (isDissolve)
        {
            timeFadeDissolve -= Time.deltaTime;
            materialDissolve.SetFloat("_Fade", timeFadeDissolve);
        }
    }

    //Set variables
    public void SetIsMoveToTrue()
    {
        isMove = true;
    }
    public void SetWallJumpingToFalse()
    {
        wallJumping = false;
    }
}
