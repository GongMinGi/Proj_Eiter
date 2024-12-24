using UnityEngine;

public class ZombieMoveTemp : BaseMonster
{
    public float speed; // �̵� �ӵ�
    public float attackRange; // ���� ����
    public float attackCooldown; // ���� ��Ÿ��
    public Rigidbody2D target; // ���ΰ� Ÿ��

    private SpriteRenderer spriteRenderer; // ��������Ʈ ������: ���� �̹��� ���� �� ǥ��
    private float lastAttackTime = 0f; // ������ ���� �ð��� �����
    private bool isMovingToTarget = false; // ���ΰ����� �̵� ������ ����

    protected override void Awake()
    {
        base.Awake(); // BaseMonster�� Awake �޼��� ȣ��
        spriteRenderer = GetComponent<SpriteRenderer>(); // SpriteRenderer ������Ʈ ��������
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate(); // �θ� Ŭ������ FixedUpdate ȣ��

        float distanceToTarget = Vector2.Distance(transform.position, target.position); // ���ΰ����� �Ÿ� ���

        if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {
            //Attack(); // ���� ����
        }
        else if (distanceToTarget > attackRange)
        {
            MoveToTarget(); // ���ΰ��� ���� �̵�
        }
        else
        {
            Idle(); // Idle ���·� ��ȯ
        }
    }

    private void MoveToTarget()
    {
        Vector2 dirVec = target.position - (Vector2)transform.position; // ���ΰ����� ���� ���� ���
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; // �̵��� ��ġ ��� (�ӵ� * �ð�)

        animator.SetTrigger("Walk");

        transform.position += (Vector3)nextVec; // ������ ��ġ ������Ʈ

        spriteRenderer.flipX = dirVec.x > 0 ? true : false ; // ���ΰ� ���⿡ ���� ��������Ʈ ����
    }

    private void Attack()
    {
        // ���� ó��
        lastAttackTime = Time.time; // ������ ���� �ð� ����
        animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ����

        // ���ΰ����� ���ظ� �ִ� ���� (�ʿ� �� �߰�)
        target.GetComponentInChildren<PlayerHealth>().TakeDamage(1);
        Idle();
    }

    private void Idle()
    {
        // Idle ���� ó��
        animator.SetTrigger("Idle"); // Idle �ִϸ��̼� ����
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            if(Time.time >= lastAttackTime + attackCooldown)
            {
                Attack();
            }

            Debug.Log("�÷��̾�� �浹");
            // �浹�� ��ü�� ���ΰ��� ��� (�ʿ� �� �߰� ���� ���� ����)
        }
    }
}
