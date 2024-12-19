using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

// made by mingi

public class PlayerController : MonoBehaviour
{

    // �⺻������
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;

    //�̵� ���� ������
    [SerializeField]
    float walkSpeed = 1f;
    float moveSpeed = 1f; // �̵��ÿ� �����ִ� ���ǵ� 
    [SerializeField]
    float jumpForce = 3f;
    [SerializeField]
    float dashSpeed = 1f;
    float dashTimeRemain = 0f; // ���� �뽬 ���̸� ���
    float dashTime = 0.5f; // �ش簪���� �ʱ�ȭ
    float dashCoolRemain = 0f;
    float dashCool = 1f;
    float dashDirection = 0f;
    bool isDashing = false;

    // ���� ���� ������
    [SerializeField]
    float attackCurTime = 0f;
    [SerializeField]
    float attackCoolTime = 0.5f;
    public Transform attackBoxPos;
    public Vector2 boxSize;

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerDirection = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        moveSpeed = walkSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();


        dashTimeRemain -= Time.deltaTime; // ����ؼ� �뽬 ���������� ��Ÿ���� ���ҽ����ش�.
        dashCoolRemain -= Time.deltaTime;

        //�뽬 �ߵ� �ƴϰ� ��Ÿ�ӵ� �ȳ��Ұ� �뽬Ÿ�ӵ� �ƴ� ���¿��� ����Ʈ�� ������ �뽬 ����
        if (!isDashing && dashCoolRemain < 0f && dashTimeRemain < 0f && Input.GetKeyDown(KeyCode.LeftShift))
        {
            Dash();
        }


        //sprite�� ������ �ڵ�. GetAxisRaw(horizontal)�� a�� ������ -1, d�� ���� �� 1�̹Ƿ� 
        //a�� ������ -1�� ������ true, d�� ������ -1�� �޶����� false�� ��ȯ�ϰ� ��.
        // ���� getbuttondown�� ��ư�� ó������ �������븸 true�� ��ȯ�Ѵ�.
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        
    }

    void FixedUpdate()
    {
        Move();
        


        //landing platform
        if(playerRigid.linearVelocity.y < 0 ) //�߶��Ҷ��� ���̸� �Ʒ��� ���.
        {
            Debug.DrawRay(playerRigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                Debug.Log(rayHit.collider.name);

                if (rayHit.distance < 1f)
                {
                    Debug.Log(rayHit.collider.name);
                    playerAnim.SetBool("isJumping", false);
                }
            }
        }

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.linearVelocity = new Vector2(playerRigid.linearVelocity.y, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }
    }


    void Attack()
    {
        if(attackCurTime <= 0f)
        {
            if(Input.GetKeyDown(KeyCode.Mouse0))
            {

                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);
                }
                playerAnim.SetTrigger("Attack");
                attackCurTime = attackCoolTime;
            }
        }
        else
        {
            attackCurTime -= Time.deltaTime;
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(attackBoxPos.position, boxSize);
    }


    void Move()
    {

        if (!isDashing)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            playerRigid.linearVelocity = new Vector2(moveInput * moveSpeed, playerRigid.linearVelocity.y);
        }

        
    }

    void Dash()
    {
        isDashing = true;
        dashTimeRemain = dashTime;
        dashCoolRemain = dashCool;

        dashDirection = spriteRenderer.flipX ? 1 : -1;

        playerRigid.linearVelocity = new Vector2( dashDirection * dashSpeed, playerRigid.linearVelocity.y);
        isDashing = false;
    }
    
}
