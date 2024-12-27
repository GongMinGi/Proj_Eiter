using UnityEngine;

public class SpiderLanded : BaseMonster
{

    public float speed; // �̵� �ӵ�
    public float attackRange; // ���� ����
    public float attackCooldown; // ���� ��Ÿ��

    public Rigidbody2D target; // ���ΰ� Ÿ��

    private SpriteRenderer spriteRenderer; // ��������Ʈ ������: ���� �̹��� ���� �� ǥ��
    private float lastAttackTime = 0f; // ������ ���� �ð��� �����

    public Transform attackBoxPos;

    public Vector2 boxSize;

    private bool isAttack = false;

    protected override void Awake()
    {

        this.enabled = false;

        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    protected override void FixedUpdate()
    {

        if (isAttack)
        {

            return;

        }

        float distanceToTarget = Vector2.Distance(transform.position, target.position); // ���ΰ����� �Ÿ� ���

        if (distanceToTarget <= attackRange && Time.time >= lastAttackTime + attackCooldown)
        {

            StartAttack(); // ���� ����

        }
        else if (distanceToTarget > attackRange && !isAttack)
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

        transform.localScale = dirVec.x > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1); // ���ΰ� ���⿡ ���� ��������Ʈ ����

    }


    public void StartAttack()
    {

        isAttack = true;

        lastAttackTime = Time.time; // ������ ���� �ð� ����

        animator.SetTrigger("Attack"); // ���� �ִϸ��̼� ����

    }

    public void Attack()
    {

        Collider2D[] collider2Ds = Physics2D.OverlapBoxAll(attackBoxPos.position, boxSize, 0);

        foreach (Collider2D collider in collider2Ds)
        {

            Debug.Log(collider.tag);

            if (collider.CompareTag("Player")) 
            {

                collider.GetComponentInChildren<PlayerHealth>().TakeDamage(1, transform.position);

            }

        }

    }

    public void EndAttack()
    {

        isAttack = false;

        Idle();

    }

    private void Idle()
    {

        animator.SetTrigger("Idle"); 

    }

    void OnCollisionEnter2D(Collision2D collision)
    {

        if (collision.collider.CompareTag("Player"))
        {

            Debug.Log("�÷��̾�� �浹");
            // �浹�� ��ü�� ���ΰ��� ��� (�ʿ� �� �߰� ���� ���� ����)

        }

    }

    private void OnDrawGizmos()
    {

        Gizmos.color = Color.red;

        Gizmos.DrawWireCube(attackBoxPos.position, boxSize);

    }

}
