using System.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;
using UnityEngine.Tilemaps;


public class PlayerControllerbyBae : MonoBehaviour
{
    // 기본변수들
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerTransform;
    Vector2 playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;

    // 이동 관련 변수들
    [Header("이동 세팅")]
    [SerializeField]
    float moveSpeed = 1f; // 이동시에 곱해주는 스피드 
    [SerializeField]
    float jumpForce = 3f;

    // 대쉬 관련 변수들
    [Header("대시 세팅")]
    [SerializeField] float dashVelocity = 20f;  // 대시 속도
    [SerializeField] float dashTime = 0.2f;     // 대쉬 지속시간
    [SerializeField] float dashCoolDown = 1f;   // 대시 쿨타임

    private Vector2 dashDirection;      // 대시 방향
    private bool isDashing;             // 대시 중인지 여부
    private bool canDash = true;        // 대시 가능 여부

    private TrailRenderer trailRenderer;
    public Transform firePoint; // FirePoint Transform
    // 공격 관련 변수들
    [Header("공격 세팅")]
    [SerializeField] int atk = 1;   //공격력
    [SerializeField] float attackForce = 20f;
    [SerializeField]
    float attackCurTime = 0f;
    [SerializeField]
    float attackCoolTime = 0.5f;
    public Transform attackBoxPos;
    public Vector2 boxSize;

    // 글라이딩 관련 변수들
    [Header("글라이딩 세팅")]
    [SerializeField] private float glideGravityScale = 0.5f;
    public bool IsGliding { get; private set; } // 외부에서 읽기 가능

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

        // Sprite를 뒤집는 코드
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;

        UpdateAttackBoxPosition();
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        Move();

        // Landing platform
        if (playerRigid.velocity.y < 0) // 추락할 때만 레이를 아래로 쏜다
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
        // 점프
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }

        // 글라이딩
        if (Input.GetKey(KeyCode.Space) && playerRigid.velocity.y < 0) // 추락 중일 때 글라이딩 활성화
        {
            IsGliding = true;
            playerRigid.gravityScale = glideGravityScale; // 글라이딩 중 중력 감소
            playerAnim.SetBool("IsGliding", true);
        }
        else
        {
            IsGliding = false;
            playerRigid.gravityScale = 1f; // 중력 원래 값 복구
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
        isDashing = true;   // 대시 시작 설정
        canDash = false; // 대시 가능 상태 비활성화

        // x축만 대시 가능
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        // 입력이 없을 경우 캐릭터의 현재 방향을 기준으로 설정
        if (dashDirection == Vector2.zero)
        {
            return;
        }

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        // 대시 코루틴 실행
        StartCoroutine(PerformDash());
    }

    [System.Obsolete]
    private IEnumerator PerformDash()
    {
        float startTime = Time.time; // 대시 시작 시간 저장

        // 대시 지속 시간 동안 속도 적용
        while (Time.time < startTime + dashTime)
        {
            playerRigid.velocity = dashDirection * dashVelocity;
            yield return null; // 다음 프레임까지 대기
        }

        // 대시 중지 설정
        playerRigid.velocity = Vector2.zero; // 속도 초기화
        isDashing = false;

        // Trail Renderer 비활성화
        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        // 대시 쿨다운 적용
        yield return new WaitForSeconds(dashCoolDown); // 쿨다운 시간 설정 
        canDash = true;
    }
}
