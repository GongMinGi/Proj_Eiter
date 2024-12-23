using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Tilemaps;


public class PlayerControllerbyBae : MonoBehaviour
{
    // �⺻������
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerTransform;
    Vector2 playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;

    // �̵� ���� ������
    [Header("�̵� ����")]
    [SerializeField]
    float moveSpeed = 1f; // �̵��ÿ� �����ִ� ���ǵ� 
    [SerializeField]
    float jumpForce = 3f;

    // �뽬 ���� ������
    [Header("��� ����")]
    [SerializeField] float dashVelocity = 20f;  // ��� �ӵ�
    [SerializeField] float dashTime = 0.2f;     // �뽬 ���ӽð�
    [SerializeField] float dashCoolDown = 1f;   // ��� ��Ÿ��

    private Vector2 dashDirection;      // ��� ����
    private bool isDashing;             // ��� ������ ����
    private bool canDash = true;        // ��� ���� ����

    private TrailRenderer trailRenderer;

    // ���� ���� ������
    [Header("���� ����")]
    [SerializeField] int atk = 1;   //���ݷ�
    [SerializeField] float attackForce = 20f;
    [SerializeField]
    float attackCurTime = 0f;
    [SerializeField]
    float attackCoolTime = 0.5f;
    public Transform attackBoxPos;
    public Vector2 boxSize;

    // �۶��̵� ���� ������
    [Header("�۶��̵� ����")]
    [SerializeField] private float glideGravityScale = 0.5f;
    public bool IsGliding { get; private set; } // �ܺο��� �б� ����

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();

        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    [System.Obsolete]
    void Update()
    {
        Jump();
        Attack();

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }

        // Sprite�� ������ �ڵ�
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        UpdateAttackBoxPosition();
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        Move();

        // Landing platform
        if (playerRigid.velocity.y < 0) // �߶��� ���� ���̸� �Ʒ��� ���
        {
            Debug.DrawRay(playerRigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1f)
                {
                    playerAnim.SetBool("isJumping", false);
                }
            }
        }
    }

    [System.Obsolete]
    void Jump()
    {
        // ����
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }

        // �۶��̵�
        if (Input.GetKey(KeyCode.Space) && playerRigid.velocity.y < 0) // �߶� ���� �� �۶��̵� Ȱ��ȭ
        {
            IsGliding = true;
            playerRigid.gravityScale = glideGravityScale; // �۶��̵� �� �߷� ����
            playerAnim.SetBool("IsGliding", true);
        }
        else
        {
            IsGliding = false;
            playerRigid.gravityScale = 1f; // �߷� ���� �� ����
            playerAnim.SetBool("IsGliding", false);
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

    [System.Obsolete]
    void Move()
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            playerRigid.velocity = new Vector2(moveInput * moveSpeed, playerRigid.velocity.y);
        }
    }

    [System.Obsolete]
    void StartDash()
    {
        isDashing = true;   // ��� ���� ����
        canDash = false; // ��� ���� ���� ��Ȱ��ȭ

        // x�ุ ��� ����
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        // �Է��� ���� ��� ĳ������ ���� ������ �������� ����
        if (dashDirection == Vector2.zero)
        {
            return;
        }

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        // ��� �ڷ�ƾ ����
        StartCoroutine(PerformDash());
    }

    [System.Obsolete]
    private IEnumerator PerformDash()
    {
        float startTime = Time.time; // ��� ���� �ð� ����

        // ��� ���� �ð� ���� �ӵ� ����
        while (Time.time < startTime + dashTime)
        {
            playerRigid.velocity = dashDirection * dashVelocity;
            yield return null; // ���� �����ӱ��� ���
        }

        // ��� ���� ����
        playerRigid.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDashing = false;

        // Trail Renderer ��Ȱ��ȭ
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        // ��� ��ٿ� ����
        yield return new WaitForSeconds(dashCoolDown); // ��ٿ� �ð� ���� 
        canDash = true;
    }
}




/*using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControllerbyBae : MonoBehaviour
{
    // �⺻ ������
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;

    // �̵� ���� ������
    [Header("�̵� ����")]
    [SerializeField] float moveSpeed = 1f; // �̵� �� �����ִ� ���ǵ�
    [SerializeField] float jumpForce = 3f;

    // �뽬 ���� ������
    [Header("��� ����")]
    [SerializeField] float dashVelocity = 20f;  // ��� �ӵ�
    [SerializeField] float dashTime = 0.2f;     // ��� ���ӽð�
    [SerializeField] float dashCoolDown = 1f;   // ��� ��Ÿ��

    private Vector2 dashDirection;      // ��� ����
    private bool isDashing;             // ��� ������ ����
    private bool canDash = true;        // ��� ���� ����

    private TrailRenderer trailRenderer;

    // ���� ���� ������
    [Header("���� ����")]
    [SerializeField] float attackCurTime = 0f;
    [SerializeField] float attackCoolTime = 0.5f;
    public Transform attackBoxPos;
    public Vector2 boxSize;

    // �۶��̵� ���� ������
    [Header("�۶��̵� ����")]
    [SerializeField] private float glideGravityScale = 0.5f; // �۶��̵� �� �߷� ���� ��
    private bool isGliding = false; // �۶��̵� ���� ����
    public bool IsGliding => isGliding; // �ܺο��� �б� ����

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerDirection = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    [System.Obsolete]
    void Update()
    {
        Jump();
        Attack();
        

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }

        // Sprite�� ������ �ڵ�
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        UpdateAttackBoxPosition();
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        Move();

        // Landing platform
        if (playerRigid.velocity.y < 0) // �߶��� ���� ���̸� �Ʒ��� ���
        {
            Debug.DrawRay(playerRigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1f)
                {
                    playerAnim.SetBool("isJumping", false);
                }
            }
        }
    }

    [System.Obsolete]
    void Jump()
    {
        // ���� Ű�� ó�� ������ �� ����
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }

        // ���� Ű�� ������ �ִ� ���� �۶��̵�
        if (Input.GetKey(KeyCode.Space) && playerRigid.velocity.y <= 0) // �߶� ���� ���� �۶��̵� Ȱ��ȭ
        {
            isGliding = true;
            playerRigid.gravityScale = glideGravityScale; // �۶��̵� �� �߷� ����
            playerAnim.SetBool("isGliding", true);
        }
        else
        {
            isGliding = false;
            playerRigid.gravityScale = 1f; // �߷� ���� �� ����
            playerAnim.SetBool("isGliding", false);
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

    [System.Obsolete]
    void Move()
    {
        if (!isDashing)
        {
            float moveInput = Input.GetAxisRaw("Horizontal");
            playerRigid.velocity = new Vector2(moveInput * moveSpeed, playerRigid.velocity.y);
        }
    }

    [System.Obsolete]
    void StartDash()
    {
        isDashing = true;   // ��� ���� ����
        canDash = false; // ��� ���� ���� ��Ȱ��ȭ

        // x�ุ ��� ����
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        // �Է��� ���� ��� ĳ������ ���� ������ �������� ����
        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        // ��� �ڷ�ƾ ����
        StartCoroutine(PerformDash());
    }

    [System.Obsolete]
    private IEnumerator PerformDash()
    {
        float startTime = Time.time; // ��� ���� �ð� ����

        // ��� ���� �ð� ���� �ӵ� ����
        while (Time.time < startTime + dashTime)
        {
            playerRigid.velocity = dashDirection * dashVelocity;
            yield return null; // ���� �����ӱ��� ���
        }

        // ��� ���� ����
        playerRigid.velocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDashing = false;

        // Trail Renderer ��Ȱ��ȭ
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        // ��� ��ٿ� ����
        yield return new WaitForSeconds(dashCoolDown); // ��ٿ� �ð� ���� 
        canDash = true;
    }
}




/*

using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Tilemaps;

// made by mingi

public class PlayerControllerbyBae : MonoBehaviour
{

    // �⺻������
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerDirection;
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

    //gliding related variables by Bae NO.1

    [Header("�۶��̵� ����")]
    [SerializeField] private float glideGravityScale = 0.5f; // �۶��̵� �� �߷� ���� ��
    private bool isGliding = false; // �۶��̵� ���� ����

    public bool IsGliding => isGliding; // WindZone���� ������ �� �ֵ��� �б� ���� ������Ƽ



    //���� ���� ������
    [Header("���� ����")]
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
        trailRenderer = GetComponent<TrailRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
        Attack();


        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
        }


        //sprite�� ������ �ڵ�. GetAxisRaw(horizontal)�� a�� ������ -1, d�� ���� �� 1�̹Ƿ� 
        //a�� ������ -1�� ������ true, d�� ������ -1�� �޶����� false�� ��ȯ�ϰ� ��.
        // ���� getbuttondown�� ��ư�� ó������ �������븸 true�� ��ȯ�Ѵ�.
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;


        UpdateAttackBoxPosition();
    }

    void FixedUpdate()
    {
        Move();



        //landing platform
        if (playerRigid.linearVelocity.y < 0) //�߶��Ҷ��� ���̸� �Ʒ��� ���.
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

    void StartDash()
    {
        isDashing = true;   // ��� ���� ����
        canDash = false; // ��� ���� ���� ��Ȱ��ȭ

        //�÷��̾� �Է¿��� ��� ���� �������� 

        //������ ���
        //dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        //x�ุ ��� ���� 
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;


        //�Է��� ���� ��� ĳ������ ���� ������ �������� ����
        if (dashDirection == Vector2.zero)
        {
            dashDirection = transform.localScale.x > 0 ? Vector2.right : Vector2.left;
        }


        if (trailRenderer != null)
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
        while (Time.time < startTime + dashTime)
        {
            playerRigid.linearVelocity = dashDirection * dashVelocity;
            yield return null; // ���� �����ӱ��� ���
        }

        //��� ���� ����
        playerRigid.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ
        isDashing = false;

        //Trail Renderer ��Ȱ��ȭ
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }


        //��� ��ٿ� ����
        yield return new WaitForSeconds(dashCoolDown); //��ٿ� �ð� ���� 
        canDash = true;


    }

}
*/