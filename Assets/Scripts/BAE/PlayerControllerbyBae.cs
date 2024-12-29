using System.Collections;
using Unity.Mathematics;
using UnityEngine;

public class PlayerControllerbyBae : MonoBehaviour
{
    // �⺻ ������
    bool isGround = true; // ���� �ִ��� ����
    Rigidbody2D playerRigid; // �÷��̾� Rigidbody2D
    Transform playerTransform; // �÷��̾� Transform
    Animator playerAnim; // �÷��̾� Animator
    SpriteRenderer spriteRenderer; // SpriteRenderer

    // �̵� ���� ������
    [Header("�̵� ����")]
    [SerializeField] float moveSpeed = 1f; // �̵� �ӵ�
    [SerializeField] float jumpForce = 3f; // ���� ��

    // �۶��̵� ���� ������
    [Header("�۶��̵� ����")]
    [SerializeField] private float glideGravityScale = 0.5f; // �۶��̵� �� �߷� ��
    public bool IsGliding { get; private set; } // �۶��̵� ���� �÷���

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
    [SerializeField] int atk = 1;   // ���ݷ�
    [SerializeField] float attackCoolTime = 0.5f; // �Ϲ� ���� ��Ÿ��
    [SerializeField] float chargeTime = 0.5f; // ���� ���� �غ� �ð�
    private float attackCurTime = 0f;
    private float chargeCounter = 0f; // ��ư ���� �ð�
    private bool isCharging = false;
    private bool movementDisabled = false; // �̵� ���� �÷���

    public Transform attackBoxPos; // ���� �ڽ� ��ġ
    public Vector2 boxSize; // ���� �ڽ� ũ��

    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerTransform = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        trailRenderer = GetComponent<TrailRenderer>();
    }

    [System.Obsolete]
    void Update()
    {
        if (!movementDisabled) // ���� ���°� �ƴ� ���� �Է� ó��
        {
            Jump();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.jump1);
        }

        HandleAttack();

        if (Input.GetMouseButton(1)) // ���콺 ���� ��ư ����
        {
            StartGliding();
        }
        else if (IsGliding)
        {
            StopGliding();
        }

        if (!movementDisabled && Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            StartDash();
            AudioManager.instance.PlaySfx(AudioManager.Sfx.dash1);
        }

        if (!movementDisabled && Input.GetButton("Horizontal"))
        {
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        }

        if (Mathf.Abs(playerRigid.velocity.normalized.x) >= 0.1)
        {
            playerAnim.SetBool("isMoving", true);
        }
        else
        {
            playerAnim.SetBool("isMoving", false);
        }

        UpdateAttackBoxPosition();
    }

    [System.Obsolete]
    void FixedUpdate()
    {
        if (!movementDisabled) // ���� ���°� �ƴ� ���� �̵� ó��
        {
            Move();
        }

        if (playerRigid.velocity.y < 0 && !isGround)
        {
            playerAnim.SetBool("isFalling", true);
        }
        else
        {
            playerAnim.SetBool("isFalling", false);
        }

        if (playerRigid.velocity.y < 0)
        {
            Debug.DrawRay(playerRigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                if (rayHit.distance < 1f)
                {
                    playerAnim.SetBool("isJumping", false);
                    isGround = true;
                }
            }
            isGround = false;
        }
    }

    [System.Obsolete]
    void HandleAttack()
    {
        if (Input.GetMouseButton(0)) // ���콺 ���� ��ư ����
        {
            chargeCounter += Time.deltaTime; // ��ư ���� �ð� ����
            playerAnim.SetFloat("chargeCounter", chargeCounter); // Animator�� �� ����

            if (chargeCounter >= chargeTime && !isCharging) // ���� ���� ����
            {
                isCharging = true;
                playerAnim.SetBool("IsCharging", true); // ���� �ִϸ��̼� ����
                movementDisabled = true; // �̵� ����
                StartCoroutine(HandleChargeState()); // ���� ���� ����
            }
        }

        if (Input.GetMouseButtonUp(0)) // ���콺 ���� ��ư ��
        {
            if (!isCharging && chargeCounter < chargeTime) // �Ϲ� ���� ����
            {
                PerformNormalAttack(); // �Ϲ� ���� ����
                AudioManager.instance.PlaySfx(AudioManager.Sfx.attack3);
            }

            // ���� �ʱ�ȭ
            chargeCounter = 0f;
            playerAnim.SetFloat("chargeCounter", chargeCounter); // Animator �� �ʱ�ȭ
            isCharging = false;
            movementDisabled = false;
            playerAnim.SetBool("IsCharging", false); // ���� ���� ����
        }
    }

    private IEnumerator HandleChargeState()
    {
        yield return new WaitForSeconds(2.5f); // ���� ���� �ð�

        if (isCharging)
        {
            PerformChargeAttack(); // ���� ���� ����
        }

        // ���� ���� ����
        isCharging = false;
        playerAnim.SetBool("IsCharging", false);
        movementDisabled = false; // �̵� ����
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

    private void PerformNormalAttack()
    {
        Debug.Log("�Ϲ� ���� ����!");
        playerAnim.SetTrigger("Attack"); // �Ϲ� ���� �ִϸ��̼�
        attackCurTime = attackCoolTime; // ��Ÿ�� �ʱ�ȭ

        // ���� ����
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0);
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster"))
            {
                collider.GetComponent<BaseMonster>().TakeDamage(atk, attackBoxPos.position);
            }
        }
    }

    private void PerformChargeAttack()
    {
        Debug.Log("���� ���� ����!");
        playerAnim.SetTrigger("ChargeAttack"); // ���� ���� �ִϸ��̼�

        // ���� ���� ȿ�� �߰�
        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize * 1.5f, 0); // �� ���� ����
        foreach (Collider2D collider in collider2Ds)
        {
            if (collider.CompareTag("Monster"))
            {
                collider.GetComponent<BaseMonster>().TakeDamage(atk * 2, attackBoxPos.position); // ���� ������ 2�� ������
            }
        }
    }

    [System.Obsolete]
    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.velocity = new Vector2(playerRigid.velocity.x, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }
    }

    [System.Obsolete]
    void StartGliding()
    {
        if (playerRigid.velocity.y < 0) // �Ʒ��� ������ ���� �۶��̵� ����
        {
            IsGliding = true;
            playerRigid.gravityScale = glideGravityScale;
            playerAnim.SetBool("IsGliding", true);
        }
    }

    void StopGliding()
    {
        IsGliding = false;
        playerRigid.gravityScale = 1f;
        playerAnim.SetBool("IsGliding", false);
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
        dashDirection = new Vector2(Input.GetAxisRaw("Horizontal"), 0).normalized;

        if (dashDirection == Vector2.zero)
        {
            return;
        }

        isDashing = true;
        playerAnim.SetBool("isDashing", true);
        canDash = false;

        if (trailRenderer != null)
        {
            trailRenderer.emitting = true;
        }

        StartCoroutine(PerformDash());
    }

    [System.Obsolete]
    private IEnumerator PerformDash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            playerRigid.velocity = dashDirection * dashVelocity;
            yield return null;
        }

        playerRigid.velocity = Vector2.zero;
        isDashing = false;

        playerAnim.SetBool("isDashing", false);

        if (playerAnim.GetBool("isJumping"))
        {
            playerAnim.SetBool("isFalling", true);
        }
        else
        {
            playerAnim.SetBool("isFalling", false);
            playerAnim.SetBool("isMoving", false);
        }

        if (trailRenderer != null)
        {
            trailRenderer.emitting = false;
        }

        yield return new WaitForSeconds(dashCoolDown);
        canDash = true;
    }
}













