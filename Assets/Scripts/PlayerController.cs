using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Tilemaps;

// made by mingi

public class PlayerController : MonoBehaviour
{

    // �⺻������
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerTransform;
    Vector2 playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;

    //�̵� ���� ������
    [Header("�̵� ����")]
    [SerializeField]
    float moveSpeed = 1f; // �̵��ÿ� �����ִ� ���ǵ� 
    [SerializeField]
    float jumpForce = 3f;


    //�뽬 ���� ������
    [Header("��� ����")]
    [SerializeField] float dashVelocity = 20f;  // ��� �ӵ�
    [SerializeField] float dashTime = 0.2f;     // �뽬 ���ӽð�
    [SerializeField] float dashCoolDown = 1f;   // ��� ��Ÿ��

    private Vector2 dashDirection;      // ��� ����
    private bool isDashing;             // ��� ������ ����
    private bool canDash = true;        // ��� ���� ����

    private TrailRenderer trailRenderer;


    //���� ���� ������
    [Header("���� ����")]
    [SerializeField] int atk = 1;   //���ݷ�
    [SerializeField] float attackForce = 20f;
    [SerializeField]
    float attackCurTime = 0f;
    [SerializeField]
    float attackCoolTime = 0.5f;
    public Transform attackBoxPos;
    public Vector2 boxSize;

    // �ǰ� ���� ����
    public float playerKnockbackForce;


    // �۶��̵� ���� ������
    [Header("�۶��̵� ����")]
    [SerializeField] private float glideGravityScale = 0.5f; // �۶��̵� �� �߷� ��
    public bool isGliding = false;// �۶��̵� ���� �÷���
    [SerializeField] private float velocityLimit;

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();

        if (Input.GetMouseButton(1) && !isGround) // ���콺 ���� ��ư ����
        {
            StartGliding();
        }
        else if (isGliding)
        {
            StopGliding();
        }


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }    


        //sprite�� ������ �ڵ�. GetAxisRaw(horizontal)�� a�� ������ -1, d�� ���� �� 1�̹Ƿ� 
        //a�� ������ -1�� ������ true, d�� ������ -1�� �޶����� false�� ��ȯ�ϰ� ��.
        // ���� getbuttondown�� ��ư�� ó������ �������븸 true�� ��ȯ�Ѵ�.
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        //if (Mathf.Abs(playerRigid.linearVelocity.normalized.x )>= 0.1 && !isDashing)
        if (Mathf.Abs(playerRigid.linearVelocity.normalized.x) >= 0.1)
        {
            playerAnim.SetBool("isMoving", true);

        }
        else
        {

            playerAnim.SetBool("isMoving", false);
        }



        UpdateAttackBoxPosition();
    }

    void FixedUpdate()
    {
        Move();

        VelocityLimit();



        if (playerRigid.linearVelocity.y < 0 && !isGround)
        {
            playerAnim.SetBool("isFalling", true);
        }
        else
        {
            playerAnim.SetBool("isFalling", false);
        }



        //landing platform
        if (playerRigid.linearVelocity.y < 0 ) //�߶��Ҷ��� ���̸� �Ʒ��� ���.
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
                    isGround = true;
                }
            }
            else
            {
                isGround = false;
            }
        }

    }

    // y������ �÷��̾��� �ӵ��� ���Ѱ��� ��� ��� �ִ�ġ�� �ٽ� �ʱ�ȭ
    void VelocityLimit()
    {
        if(playerRigid.linearVelocity.y > 0 && Mathf.Abs(playerRigid.linearVelocity.y) > velocityLimit )
        {
            playerRigid.linearVelocity = new Vector2(playerRigid.linearVelocity.x, velocityLimit);
        }
        else if (playerRigid.linearVelocity.y < 0 &&  Mathf.Abs(playerRigid.linearVelocity.y) > velocityLimit)
        {
            Debug.Log("FUck");
            playerRigid.linearVelocity = new Vector2(playerRigid.linearVelocity.x, -velocityLimit);
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


    void UpdateAttackBoxPosition()
    {
        if (spriteRenderer.flipX)
        {
            attackBoxPos.localPosition = new Vector2(-Mathf.Abs(attackBoxPos.localPosition.x), attackBoxPos.localPosition.y);
        }
        else
        {
            attackBoxPos.localPosition = new Vector2(Mathf.Abs(attackBoxPos.localPosition.x), attackBoxPos.localPosition.y);
        }

    }

    void Attack()
    {

        if (attackCurTime <= 0f)
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                playerAnim.SetTrigger("Attack");
                attackCurTime = attackCoolTime;
                Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackBoxPos.position , boxSize, 0);
                foreach (Collider2D collider in collider2Ds)
                {
                    Debug.Log(collider.tag);

                    if(collider.CompareTag("Monster")) //�±װ� Monster�� ���
                    {
                        //collider.GetComponent<EnemyHealth>().Damage(atk, collider.transform.position - transform.position);
                        collider.GetComponent<BaseMonster>().TakeDamage(1, transform.position);
                    }
                }
                
                
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

    void StartDash()
    {


        //�÷��̾� �Է¿��� ��� ���� �������� 

        //������ ���
        //dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //x�ุ ��� ���� 
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;


        //�Է��� ���� ��� ��� �Ұ�
        if ( dashDirection == Vector2.zero)
        {
            return;
            //dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        isDashing = true;   // ��� ���� ����
        playerAnim.SetBool("isDashing", true);

        canDash = false; // ��� ���� ���� ��Ȱ��ȭ

        if ( trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        // ��� �ڷ�ƾ ����
        StartCoroutine(PerformDash());
    }


    private IEnumerator PerformDash()
    {
        float startTime = Time.time; // ��� ���� �ð� ����


        //��� ���� �ð� ���� �ӵ� ����
        while ( Time.time < startTime + dashTime)
        {
            playerRigid.linearVelocity = dashDirection * dashVelocity;
            yield return null; // ���� �����ӱ��� ���
        }

        //��� ���� ����
        playerRigid.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDashing = false;

        playerAnim.SetBool("isDashing", false);


        //���� ���� Ȯ��
        if (playerAnim.GetBool("isJumping"))
        {
            playerAnim.SetBool("isFalling", true);
        }
        else
        {
            playerAnim.SetBool("isFalling", false);
            playerAnim.SetBool("isMovinng", false); //Idle�� ��ȯ
        }



        //Trail Renderer ��Ȱ��ȭ
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }


        //��� ��ٿ� ����
        yield return new WaitForSeconds(dashCoolDown); //��ٿ� �ð� ���� 
        canDash = true;


    }

    public void OnDamaged(Vector2 enemyPos)
    {
        playerAnim.SetTrigger("onDamaged");

        gameObject.layer = 7;

        spriteRenderer.color = new Color(1, 1, 1, 0.4f);


        float dirc = enemyPos.x - transform.position.x > 0 ? -1 : 1;
        playerRigid.AddForce(new Vector2(dirc, 0.2f) * playerKnockbackForce, ForceMode2D.Impulse);


        Invoke("OffDamaged", 1.5f);
    }


    private void OffDamaged()
    {
        //Animator anim = GetComponentInParent<Animator>();
        //playerAnim.SetBool("onDamaged", false);

        gameObject.layer = 0;
        spriteRenderer.color = new Color(1, 1, 1, 1);



    }


    void StartGliding()
    {
        if (playerRigid.linearVelocity.y < 0) // �Ʒ��� ������ ���� �۶��̵� ����
        {
            isGliding = true;
            velocityLimit = 2;
            playerRigid.gravityScale = glideGravityScale;
            playerAnim.SetBool("isGliding", true);
        }
    }

    void StopGliding()
    {
        isGliding = false;
        velocityLimit = 100;
        playerRigid.gravityScale = 1f;
        playerAnim.SetBool("isGliding", false);
    }

    public void StartGlideLoop()
    {
        // ���� �ִϸ��̼� ���¸� �ݺ� ���
        playerAnim.Play("PlayerGliding", 0, 0.5f); // 50%���� ����
    }

}
