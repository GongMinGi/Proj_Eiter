using UnityEngine;


// ����� �ൿ( ����, Ÿ������ �̵�, ����)�� �����ϴ� Ŭ����
public class Mosquito : BaseMonster
{
    public float speed; //�̵��ӵ�  
    public float chargeDelay; //Ÿ������ �����ϱ� �� ��� �ð�
    public float retreatDistanceX; //���� �� x�� �̵� �Ÿ�
    public float retreatDistanceY; //���� �� y�� �̵� �Ÿ�
    public float retreatSpeed; //���� �ӵ�
    public float activationDistance; //Ÿ�ٰ��� �Ÿ� �ȿ� ������ �� ��� Ȱ��ȭ �Ÿ�


    public Rigidbody2D target; // �÷��̾��� Rigidbody2D (Ÿ�� ���)
    //Vector2 dirVec;             // Ÿ�� ����

    //private Rigidbody2D rigid; // ����� Rigidbody2D
    private SpriteRenderer spriteRenderer; // ����� SpriteRenderer
    //private Animator animator; //����� �ִϸ�����

    private float timeSinceLastAction = 0f; // ���� ���¿��� ����� �ð�
    private Vector2 retreatPosition; //��Ⱑ ������ ��ġ
    private Vector2 targetPosition; // ���� ��ǥ ��ġ
    private const float TARGET_PROXIMITY_THRESHOLD = 1.0f; // Ÿ�� ���� �����Ÿ�
    private const float RETREAT_PROXIMITY_THRESHOLD = 0.5f; // ���� ��ġ ���� �����Ÿ�


    //����� �ൿ ����
    private enum MosquitoState
    {
        Oscillating,        // ���ڸ����� �����ϴ� ����
        MovingToTarget,     // Ÿ������ �̵��ϴ� ����   
        Retreating,          // �����ϴ� ����
        Knockback           // �˹�
    }

    private MosquitoState currentState = MosquitoState.Oscillating; // ���� ����(�ʱⰪ: Oscillating)
    private bool isActivated = false;   // Ȱ��ȭ ����(Ȱ��ȭ �Ÿ� �ȿ� ���;� Ȱ��ȭ)

    protected override void Awake()
    {
        base.Awake();
        //rigidbody2D, SpriteRenderer, Animator ������Ʈ �ʱ�ȭ
        //rigid = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //animator = GetComponent<Animator>();
    }

    protected override void FixedUpdate()
    {
        //base.fixedUpdate();

        //Ȱ��ȭ���� �ʾҴٸ� Ȱ��ȭ �Ÿ� Ȯ��
        if (!isActivated)
        {
            float distanceToTarget = Vector2.Distance(rigid.position, target.position);
            if (distanceToTarget <= activationDistance)
            {
                isActivated = true; //Ȱ��ȭ
                targetPosition = target.position;    // Ÿ���� ���� ��ġ ����
            }
            else
            {
                OscillateInPlace(); //Ȱ��ȭ ���� ���� ���¿��� ���ڸ� ����
                return;
            }
        }


        // ���� ���¿� ���� ���� ����
        timeSinceLastAction += Time.fixedDeltaTime;

        switch (currentState)
        {
            case MosquitoState.Oscillating:     //���ڸ� ���� ����
                OscillateInPlace();
                if (timeSinceLastAction >= chargeDelay) // ��� �ð��� ������
                {
                    SetState(MosquitoState.MovingToTarget); // Ÿ������ �̵� ���·� ��ȯ
                }
                break;
            case MosquitoState.MovingToTarget:  // Ÿ������ �̵� ����
                MoveToTarget();
                break;
            case MosquitoState.Retreating:  // ���� ����
                Retreat();
                break;

            case MosquitoState.Knockback:
                Knockback();
                break;
        }
    }


    private void MoveToTarget()
    {
        //Ÿ�� ���� ���
        Vector2 dirVec = targetPosition - rigid.position;
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime;  // �̵� ���� ���
        rigid.MovePosition(rigid.position + nextVec); //��� �̵�


        // Ÿ���� ��ġ�� ���� ��������Ʈ ���� ����
        //spriteRenderer.flipX = dirVec.x < 0;
        transform.localScale = dirVec.x < 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1); // ���ΰ� ���⿡ ���� ��������Ʈ ����

        //Ÿ�� ��ó�� �����ϸ� ���� ���·� ��ȯ
        if (Vector2.Distance(rigid.position, targetPosition) <= TARGET_PROXIMITY_THRESHOLD)
        {
            //���� ��ġ ����
            retreatPosition = targetPosition + new Vector2(retreatDistanceX * (dirVec.x < 0 ? -1 : 1), retreatDistanceY);
            SetState(MosquitoState.Retreating);
        }
    }

    private void SetState(MosquitoState newState)
    {
        // ���� ����
        if (currentState != newState)
        {
            currentState = newState;
            timeSinceLastAction = 0f;   // ���� ���� �� �ð� �ʱ�ȭ

            switch (newState)
            {
                case MosquitoState.Oscillating:
                    animator.SetTrigger("Fly");
                    break;
                case MosquitoState.MovingToTarget:
                    animator.SetTrigger("Attack");
                    targetPosition = target.position;
                    break;
                case MosquitoState.Retreating:
                    animator.SetTrigger("Retreat");
                    break;
                case MosquitoState.Knockback:   //�˹� �ÿ��� �׳�fly�� ���ư�
                    animator.SetTrigger("Fly");
                    break;

            }
        }
    }

    private void OscillateInPlace()
    {
        //��Ⱑ ���ڸ����� �����ϴ� ȿ��
        float oscillationX = Mathf.Sin(Time.time * 10.0f) * 0.3f;   // x�� ����
        float oscillationY = Mathf.Cos(Time.time * 10.0f) * 0.3f;   // y�� ����
        Vector2 oscillationOffset = new Vector2(oscillationX, oscillationY);
        rigid.MovePosition(rigid.position + oscillationOffset * Time.fixedDeltaTime);   // ���� ����
    }

    private void Retreat()
    {
        // ���� ���� ���
        Vector2 dirVec = retreatPosition - rigid.position;
        rigid.MovePosition(Vector2.MoveTowards(rigid.position, retreatPosition, retreatSpeed * Time.fixedDeltaTime)); // ���� �̵�
        //spriteRenderer.flipX = dirVec.x < 0; // ���� ���⿡ ���� ��������Ʈ ���� ����
        transform.localScale = dirVec.x > 0 ? new Vector3(1, 1, 1) : new Vector3(-1, 1, 1); // ���ΰ� ���⿡ ���� ��������Ʈ ����


        //���� ��ġ ��ó�� �����ϸ� ���¸� ���� ���·� ��ȯ
        if (Vector2.Distance(rigid.position, retreatPosition) <= RETREAT_PROXIMITY_THRESHOLD)
        {
            SetState(MosquitoState.Oscillating);
        }
    }

    private void Knockback()
    {
        //�˹� �̵�
        //Vector2 dirVec = knockbackPosition - rigid.position;
        rigid.MovePosition(Vector2.MoveTowards(rigid.position, knockbackPosition, knockbackSpeed * Time.fixedDeltaTime));

        if (Vector2.Distance(rigid.position, knockbackPosition) <= RETREAT_PROXIMITY_THRESHOLD)
        {
            SetState(MosquitoState.Oscillating);
        }
    }

    public override void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        //ü�°���
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        //���� ó��
        if (health <= 0)
        {
            Die();
        }

        // �˹� ���� ��� (x �� �������θ�)
        Vector2 dirVec = new Vector2(rigid.position.x - damageSourcePosition.x, 0).normalized;

        // �˹� ��ġ ��� (���� ��ġ���� �˹� �Ÿ���ŭ �̵�)
        knockbackPosition = rigid.position + dirVec.normalized * knockbackDistance;

        // ���¸� Knockback���� ��ȯ
        SetState(MosquitoState.Knockback);
    }




    void OnTriggerEnter2D(Collider2D other)
    {
        // �浹�� ����� �÷��̾��� ���
        if (other.CompareTag("Player"))
        {

            // �ʿ��� �߰� ������ ������ ���⿡ �߰�
            // ���� ���, ���� �ִϸ��̼��� �����ϰų�, �������� �ִ� ���� ����
            Debug.Log("Mosquito collided with Player!");

            other.GetComponentInChildren<PlayerHealth>().TakeDamage(1);
        }
    }
}
