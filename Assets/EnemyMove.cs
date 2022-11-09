using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    Rigidbody2D rigid;
    Animator animation;
    SpriteRenderer Sprite_Renderer;
    BoxCollider2D collider;
    Vector3 enemyDirectionVector;

    public int Next_Move_X;
    public int Next_Move_Y;
    bool Enemy_Run;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animation = GetComponent<Animator>();
        Sprite_Renderer = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        Think();

        Invoke("Think", 3);
    }

    void Update()
    {
        //if (Mathf.Abs(rigid.velocity.x) < 0.3)
        //{
        //    animation.SetBool("Enemy_Run", false);
        //}
        //else
        //{
        //    animation.SetBool("Enemy_Run", true);
        //}
        //if (rigid.velocity.x < 0)
        //{
        //    Sprite_Renderer.flipX = false;
        //}
        //else
        //{
        //    Sprite_Renderer.flipX = true;
        //}

        //raycast direction set
        if (rigid.velocity.y > 0)
        {
            enemyDirectionVector = Vector3.up;
        }
        else if (rigid.velocity.y < 0)
        {
            enemyDirectionVector = Vector3.down;
        }
        else if (rigid.velocity.x < 0)
        {
            enemyDirectionVector = Vector3.left;
        }
        else if (rigid.velocity.x > 0)
        {
            enemyDirectionVector = Vector3.right;
        }
    }


    void FixedUpdate()
    {
        //move
        rigid.velocity = new Vector2(Next_Move_X, Next_Move_Y);

        //Platform check
        //Vector2 Front_Vec = new Vector2(rigid.position.x + Next_Move * 0.5f, rigid.position.y);
        //Debug.DrawRay(rigid.position, Vector3.down, new Color(0, 1, 0));

        RaycastHit2D rayHit = Physics2D.Raycast(rigid.position, enemyDirectionVector, 1f,
           LayerMask.GetMask("Wall"));
        Debug.DrawRay(rigid.position, enemyDirectionVector, new Color(0, 1, 0));

        //if (rayHit.collider != null)
        //{
        //    EnemyTurn();
        //}

    }

    void Think()
    {
        Next_Move_X = Random.Range(-1, 2);
        Next_Move_Y = Random.Range(-1, 2);
        float Next_Think_Time = Random.Range(2f, 4f);
        Invoke("Think", Next_Think_Time);
    }
    //void EnemyTurn()
    //{
    //    Next_Move_X = Next_Move_X * -1;
    //    Sprite_Renderer.flipX = Next_Move_X == 1;
    //    Invoke("Think", 3);
 
    //}

    public void EnemyDamaged()
    {
        Sprite_Renderer.color = new Color(1, 0.9f, 0.02f, 0.5f);
        Sprite_Renderer.flipY = true;
        Next_Move_X = 0;
        collider.enabled = false;
        rigid.AddForce(Vector2.up * 5, ForceMode2D.Impulse);
        Invoke("DeActive", 3);
    }

    public void DeActive()
    {
        gameObject.SetActive(false);
    }
}
