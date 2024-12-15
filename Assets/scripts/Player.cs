using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] private float speed = 10.0f; 
    [SerializeField] private float jumpForce = 15;
    [SerializeField] private int extraJumps = 2;
    [SerializeField] private int life = 5;

    [SerializeField] private GameObject life1;
    [SerializeField] private GameObject life2;
    [SerializeField] private GameObject life3;
    [SerializeField] private GameObject life4;
    [SerializeField] private GameObject life5;
    [SerializeField] private GameObject life6;
    [SerializeField] private GameObject life7;

    [SerializeField] private GameObject DieAnimation;

    [SerializeField] private Transform WeaponPoint;
    [SerializeField] private Transform LifePoint;

    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;
    private bool isMoving;
    public bool onGround;
    private float moveHorizontal;
    private int exJumps;
    private string thisLevelName;

    void Start()
    {
        thisLevelName = GameObject.Find("EndPoint").GetComponent<NextLevel>().thisLevelName;
        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
    }

    private void goStart()
    {

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Finish"))
        {
            Die();
        }



        if (collision.CompareTag("Life"))
        {
            life++;
            setLifes();

            if (LifePoint.childCount > 0)
            {
                Destroy(LifePoint.GetChild(0).gameObject);
            }

            GameObject RealLife = collision.gameObject;

            RealLife.transform.SetParent(LifePoint.transform);

            RealLife.transform.localPosition = new Vector3(0, 0, 0);

            Destroy(RealLife, 2);

        }

      
    }

    private void Die()
    {
        sr.enabled = false;
        rb.velocity = new Vector2(0, 0);
        Instantiate(DieAnimation, transform.position, transform.rotation);
        InvokeRepeating("restartLevel", 1, 2);
    }

    private void restartLevel()
    {
        SceneManager.LoadScene(thisLevelName);
        CancelInvoke("restartLevel");
    }

    private void setLifes()
    {
        life7.SetActive(false);
        life6.SetActive(false);
        life5.SetActive(false);
        life4.SetActive(false);
        life3.SetActive(false);
        life2.SetActive(false);
        life1.SetActive(false);
        switch (life)
        {
            case 7: life7.SetActive(true); goto case 6;
            case 6: life6.SetActive(true); goto case 5;
            case 5: life5.SetActive(true); goto case 4;
            case 4: life4.SetActive(true); goto case 3;
            case 3: life3.SetActive(true); goto case 2;
            case 2: life2.SetActive(true); goto case 1;
            case 1: life1.SetActive(true); break;

        }
    } 

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = true;
            exJumps = extraJumps;
            anim.SetBool("isJump", false);
            anim.SetBool("isFall", false);
        }

        if (collision.CompareTag("Fire"))
        {
            anim.SetBool("isHit", true);
        }

    }

    private void lifeLose()
    {
        life--;
        if (life == 0) Die();
        setLifes();
    }

    private void endJump2()
    {
        anim.SetBool("isJump2", false);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            onGround = false;
        }

        if (collision.CompareTag("Fire"))
        {
            anim.SetBool("isHit", false);
        }
    }

    private void Update()
    {
        moveHorizontal = Input.GetAxis("Horizontal");
        if (Input.GetKeyDown(KeyCode.Space) && exJumps > 0)
        {
            
            exJumps--;
            if (exJumps == 0)
            {
                Debug.Log("Jumpo");
                anim.SetBool("isJump2", true);
            }
            rb.velocity = transform.up * jumpForce;
        }

        isMoving = moveHorizontal != 0 ? true : false;

        if (isMoving)
        {
            sr.flipX = moveHorizontal > 0 ? false : true;
        }

        if (!onGround)
        {
            if (rb.velocity.y > 0) anim.SetBool("isJump", true);

            if (rb.velocity.y < 0)
            {
                anim.SetBool("isJump", false);
                anim.SetBool("isFall", true);
            }
        }
      

        anim.SetBool("isMoving", isMoving);
    }

    private void FixedUpdate()
    {

        rb.velocity = new Vector2(moveHorizontal * speed * Time.fixedDeltaTime, rb.velocity.y);

    }


}
