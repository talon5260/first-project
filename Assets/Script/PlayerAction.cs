using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class PlayerAction : MonoBehaviour
{
    float h, v;
    bool isRun;
    public GameManager gameManager;
    public float PlayerSpeed;
    public float RunSpeed;
    public int CheeseCount;
    AudioSource audioSource;

    bool isHorizonMove; // act only one direction Horizontal or vertical
    Vector3 directionVector;
    GameObject scanObject;
    BoxCollider2D boxCollider;
    SpriteRenderer Sprite_Renderer;
    Rigidbody2D rigid;
    Animator animator;

    public AudioClip Audio_Run;
    public AudioClip Audio_Attack;
    public AudioClip Audio_Damaged;
    public AudioClip Audio_Item;
    public AudioClip Audio_Die;
    public AudioClip Audio_Finish;
    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider2D>();
        Sprite_Renderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>();
    }

    
    void Update()
    {
        h = gameManager.isTalkPanelActive ? 0 : Input.GetAxisRaw("Horizontal");
        v = gameManager.isTalkPanelActive ? 0 : Input.GetAxisRaw("Vertical");
        isRun = Input.GetButton("Fire3");
        if (Input.GetButtonDown("Fire3") == true)
        {
            PlaySound("RUN");
        }

        bool hDown = gameManager.isTalkPanelActive ? false : Input.GetButtonDown("Horizontal");
        bool vDown = gameManager.isTalkPanelActive ? false : Input.GetButtonDown("Vertical");
        bool hUp = gameManager.isTalkPanelActive ? false : Input.GetButtonUp("Horizontal");
        bool vUp = gameManager.isTalkPanelActive ? false : Input.GetButtonUp("Vertical");

        if (hDown)
        {
            isHorizonMove = true;               
        }
        else if(vDown)
        {
            isHorizonMove = false;
        }  
        else if (vUp || hUp)
        {
            isHorizonMove = h != 0;
           
        }

        // player run animation
       if(isRun == true)
        {
            animator.SetBool("isRun", true);
        }
        else
        {
            animator.SetBool("isRun", false);
        }

        // player animation setting
        if (animator.GetInteger("hAxisRaw") != h)
        {
            animator.SetBool("isChange", true);
            animator.SetInteger("hAxisRaw", (int)h);
        }
        else if (animator.GetInteger("vAxisRaw") != v)
        {
            animator.SetBool("isChange", true);
            animator.SetInteger("vAxisRaw", (int)v);
        }
        else
        {
            animator.SetBool("isChange", false);
        }

        //raycast direction set
        if (vDown && v == 1) 
        {
            directionVector = Vector3.up;
        }
        else if (vDown && v == -1) 
        {
            directionVector = Vector3.down;
        }
        else if (hDown && h == -1)
        {
            directionVector = Vector3.left;
        }
        else if (hDown && h == 1)
        {
            directionVector = Vector3.right;
        }

        if (Input.GetButtonDown("Jump") && scanObject != null)
        {
            gameManager.SearchAction(scanObject);
        }

        
        
    }

    private void FixedUpdate()
    {
        if (isRun == true)
        {
            Vector2 moveVector = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
            rigid.velocity = moveVector * RunSpeed;        
        }
        else
        {
            Vector2 moveVector = isHorizonMove ? new Vector2(h, 0) : new Vector2(0, v);
            rigid.velocity = moveVector * PlayerSpeed;
        }

        // ray hit for searching object 
        Debug.DrawRay(rigid.position, directionVector * 0.9f, new Color(0, 1, 0));
        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, directionVector, 0.7f,
            LayerMask.GetMask("SearchObject"));

        if (rayHit.collider != null)
        {
            scanObject = rayHit.collider.gameObject;
        }
        else
        {
            scanObject = null;
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {           
            OnDamaged(collision.transform.position);
            PlaySound("DAMAGED");
        }
        if (collision.gameObject.tag == "Spike")
        {
            OnDamaged(collision.transform.position);
            PlaySound("DAMAGED");

        }
       
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Teleport")
        {
            PlaySound("FINISH");
            SceneManager.LoadScene("Scene2");
        }
        else if (collision.gameObject.tag == "Cheese")
        {
            collision.gameObject.SetActive(false);
            CheeseCount++;
            gameManager.GetItem(CheeseCount);
            PlaySound("ITEM");
        }


    }
        void OnDamaged(Vector2 Target_Position)
    {
        gameManager.HealthDown();
        gameObject.layer = 9;
        //Sprite_Renderer.color = new Color(1, 1, 1, 0.4f);
        TurnColor();
        int Direction = transform.position.x - Target_Position.x > 0 ? 1 : -1;
        rigid.AddForce(new Vector2(Direction, 1) * 7, ForceMode2D.Impulse);       
        Invoke("OffDamaged", 2);
    }
    void OffDamaged()
    {
        gameObject.layer = 2;
        Sprite_Renderer.color = new Color(1, 1, 1, 1);
    }
    void TurnColor()
    {
        Sprite_Renderer.color = new Color(1, 0.9f, 0.02f, 0.5f);
        Invoke("ReTurnColor", 0.5f);
    }
    void ReTurnColor()
    {
        Sprite_Renderer.color = new Color(1, 1, 1, 0.5f);
        Invoke("TurnColor1", 0.5f);
    }

    void TurnColor1()
    {
        Sprite_Renderer.color = new Color(1, 0.9f, 0.02f, 0.5f);
        Invoke("ReTurnColor1", 0.5f);
    }
    void ReTurnColor1()
    {
        Sprite_Renderer.color = new Color(1, 1, 1, 0.5f);
    }

    public void OnDie()
    {
        Sprite_Renderer.color = new Color(1, 0, 1, 0.5f);
        Sprite_Renderer.flipY = true;
        boxCollider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        PlaySound("DIE");
       
        // Do not Move
        gameManager.isTalkPanelActive = true;


    }
    public void PlaySound(string action)
    {
        switch (action)
        {
            case "RUN":
                audioSource.clip = Audio_Run;
                break;
            case "ATTACK":
                audioSource.clip = Audio_Attack;
                break;
            case "DAMAGED":
                audioSource.clip = Audio_Damaged;
                break;
            case "ITEM":
                audioSource.clip = Audio_Item;
                break;
            case "DIE":
                audioSource.clip = Audio_Die;
                break;
            case "FINISH":
                audioSource.clip = Audio_Finish;
                break;
        }
        audioSource.Play();

    }
}
