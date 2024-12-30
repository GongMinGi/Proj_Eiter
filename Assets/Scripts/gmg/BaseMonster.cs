using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    [SerializeField] public int health = 5;  //ü��
    [SerializeField] public float knockbackTime = 0.2f;     // �˹� ���� �ð�
    [SerializeField] public float hitDirectionForce = 10f;  // ��Ʈ ���� ��
    [SerializeField] public float constantForce = 5f;       // ��� ��
    [SerializeField] public float knockbackDistance = 3f;    //�˹� �Ÿ�
    [SerializeField] public float knockbackSpeed = 20f;  //�˹� �ӵ�
    public float proximityThreshold = 0.2f; //�˹� ��ġ ���� ����

    protected Rigidbody2D rigid;        // Rigidbody2D ������Ʈ
    protected Animator animator;        // Animator ������Ʈ

    private Coroutine KnockbackCoroutine;

    [SerializeField] private GameObject deathEffect;     // ���� ��ƼŬ


    [SerializeField] protected Vector2 knockbackPosition;  // �˹� ��ǥ ��ġ
    [SerializeField] protected bool isKnockback = false;
    private Vector2 attackSource;

    protected virtual void Awake()
    {
        Collider2D myCollider = GetComponent<Collider2D>();

        GameObject[] objectsWithTargetTag = GameObject.FindGameObjectsWithTag("Monster");
        foreach (GameObject obj in objectsWithTargetTag)
        {
            // �±װ� �´� ������Ʈ�� Collider�� �浹 ���� ����
            Collider2D targetCollider = obj.GetComponent<Collider2D>();
            if (targetCollider != null && myCollider != null)
            {
                Physics2D.IgnoreCollision(myCollider, targetCollider);
            }
        }


        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    protected virtual void FixedUpdate()
    {
        if(isKnockback)
        {
            
            Knockback(attackSource);

        }
    }

    public virtual void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        attackSource = damageSourcePosition;
        //ü�°���
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");
        AudioManager.instance.PlaySfx(AudioManager.Sfx.attacksuccess);

        //���� ó��
        if (health <= 0 )
        {
            Die();
            return;
        }



        //// �˹� ���� ��� ( x�ุ)
        //Vector2 knockbackDir = new Vector2(rigid.position.x - damageSourcePosition.x, 0).normalized;

        //Debug.Log(knockbackDir);

        ////�˹� ��ǥ ��ġ ���
        //knockbackPosition = rigid.position + knockbackDir * knockbackDistance;
        //Debug.Log(knockbackPosition);
        ////isKnockback = true;

        //isKnockback = true;

        Vector2 hitDirection = new Vector2(rigid.position.x - damageSourcePosition.x, 0 ).normalized;
        Vector2 constantForceDirection = Vector2.zero; // ������� ���� �⺻������ 0
        float inputDirection = 0;   //�Է� ������ �����Ƿ� 0���� ����

        if ( KnockbackCoroutine != null )
        {
            StopCoroutine( KnockbackCoroutine );
        }

        KnockbackCoroutine = StartCoroutine(KnockbackAction( hitDirection, constantForceDirection, inputDirection));    


    }

    //protected virtual void Die()        // ���� ��� �ڵ�
    //{
    //    Debug.Log($"{gameObject.name} died!");
    //    Destroy(gameObject);
    //}

    /*
     protected virtual void Die()
{
    Debug.Log($"{gameObject.name} died!");
    
    // ��������Ʈ ������� �˹��� ���ÿ� ó��
    StartCoroutine(DieAnimation());
}

private IEnumerator DieAnimation()
{
    // �˹� ���� ����
    Vector2 hitDirection = new Vector2(rigid.position.x - attackSource.x, 0).normalized; 
    Vector2 constantForceDirection = Vector2.zero; 
    float inputDirection = 0f; 

    // ȸ�� ���� ����
    float rotationSpeed = 200f; // ȸ�� �ӵ�
    float targetRotation = 180f; // ��ǥ ȸ�� ����
    float currentRotation = 0f;

    float elapsedTime = 0f; // �˹� ��� �ð� �ʱ�ȭ

    while (elapsedTime < knockbackTime)
    {
        elapsedTime += Time.fixedDeltaTime;

        // 1. ȸ�� ó�� (Z�� ����)
        float rotationStep = rotationSpeed * Time.fixedDeltaTime;
        if (currentRotation < targetRotation)
        {
            currentRotation += rotationStep;
            transform.Rotate(0f, 0f, rotationStep); // Z�� �������� ȸ��
        }

        // 2. �˹� ó��
        Vector2 hitForce = hitDirection * hitDirectionForce;
        Vector2 constForce = constantForceDirection * constantForce;
        Vector2 knockbackForce = hitForce + constForce;

        if (inputDirection != 0)
        {
            knockbackForce += new Vector2(inputDirection * hitDirectionForce, 0);
        }

        rigid.linearVelocity = knockbackForce;

        yield return new WaitForFixedUpdate(); // FixedUpdate�� ����ȭ
    }

    // �˹� ���� �� ����
    rigid.linearVelocity = Vector2.zero;

    // 3. �ణ�� �ð� ���
    yield return new WaitForSeconds(0.5f); // 0.5�� ���

    // 4. GameObject �ı�
    Destroy(gameObject);
}

     */     // �˹� �� ȸ�� �Ŀ� destroy��Ű�� ������ (���� �� ȸ�� ���� �ʿ�, zȸ���� Ǫ�� ��ĵ� �غôµ� ������� �� ������)

    
    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");

        if (deathEffect != null)
        {
            // Instantiate the death effect at the current position and rotation
            GameObject effect = Instantiate(deathEffect, transform.position, Quaternion.identity);

            // Get the ParticleSystem component and explicitly play it
            ParticleSystem particleSystem = effect.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                particleSystem.Play(); // Explicitly start the particle system
            }

            // Destroy the effect object after the particle system duration
            Destroy(effect, particleSystem.main.duration);
        }

        Destroy(gameObject); // Destroy the current game object
    }

          // ������ ��ƼŬ ����ϴ� ������ (�̹����� �ƹ��ų� ����)


    protected virtual void Knockback(Vector2 damageSourcePosition)
    {
        if (!isKnockback) return;



        rigid.MovePosition(Vector2.MoveTowards(rigid.position, knockbackPosition, knockbackSpeed * Time.fixedDeltaTime));

        // ��ǥ ��ġ�� �����ϸ� �˹� ����
        if (Vector2.Distance(rigid.position, knockbackPosition) <= proximityThreshold)
        {
            isKnockback = false;

        }

    }


    private IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constantForceDirection, float inputDirection)
    {
        float elapsedTime = 0f; // �˹� ��� �ð� �ʱ�ȭ

        while( elapsedTime < knockbackTime) 
        {
            elapsedTime += Time.fixedDeltaTime; // �ð� ��� ������Ʈ

            // �˹� �� ���
            Vector2 hitForce = hitDirection * hitDirectionForce;
            Vector2 constForce = constantForceDirection * constantForce;
            Vector2 knockbackForce = hitForce + constForce;

            Vector2 combinedForce = knockbackForce;

            if(inputDirection != 0 )
            {
                combinedForce += new Vector2(inputDirection * hitDirectionForce, 0);
            }

            rigid.linearVelocity = combinedForce;

            yield return new WaitForFixedUpdate(); // FixedUpdate�� ����ȭ

        }
        rigid.linearVelocity = Vector2.zero;
    }




    // Update is called once per frame
    void Update()
    {
        
    }
}
