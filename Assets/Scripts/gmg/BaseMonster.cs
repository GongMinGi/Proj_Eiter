using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    [SerializeField] public int health = 5;  //ü��
    [SerializeField] public float knockbackDistance = 3f;    //�˹� �Ÿ�
    [SerializeField] public float knockbackSpeed = 20f;  //�˹� �ӵ�
    public float proximityThreshold = 0.1f; //�˹� ��ġ ���� ����

    protected Rigidbody2D rigid;        // Rigidbody2D ������Ʈ
    protected Animator animator;        // Animator ������Ʈ

    private Vector2 knockbackPosition;  // �˹� ��ǥ ��ġ
    private bool isKnockback = false; 

    protected virtual void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    public virtual void TakeDamage(int damage, Vector2 damageSourcePosition)
    {
        //ü�°���
        health -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage. Remaining health: {health}");

        //���� ó��
        if (health <= 0 )
        {
            Die();
            return;
        }

        //�˹�ó��
        CalculateKnockback(damageSourcePosition);

    }


    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }


    private void CalculateKnockback(Vector2 damageSourcePosition)
    {
        if (rigid == null) return;

        // �˹� ���� ��� ( x�ุ)
        Vector2 dirVec = new Vector2(rigid.position.x - damageSourcePosition.x, 0).normalized;

        //�˹� ��ǥ ��ġ ���
        knockbackPosition = rigid.position + dirVec * knockbackDistance;

        isKnockback = true;

        //�˹� ó�� 
        //StartCoroutine(KnockBackRoutine());

    }

    private void HandleKnockback()
    {
        rigid.MovePosition(Vector2.MoveTowards(rigid.position, knockbackPosition, knockbackSpeed * Time.fixedDeltaTime));

        // ��ǥ ��ġ�� �����ϸ� �˹� ����
        if (Vector2.Distance(rigid.position, knockbackPosition) <= proximityThreshold)
        {
            isKnockback = false;
            OnKnockbackComplete();
        }

    }

    protected virtual void OnKnockbackComplete()
    {
        //�˹� �Ϸ� �� �߰� �ൿ( ��ӹ��� Ŭ������ Ȯ�� ����)
        Debug.Log($"{gameObject.name} completed knockback.");
    }


    //private IEnumerator KnockBackRoutine()
    //{
    //    while(isKnockback)
    //    {
    //        rigid.MovePosition(Vector2.MoveTowards(rigid.position, knockbackPosition, knockbackSpeed * Time.fixedDeltaTime));

    //        if (Vector2.Distance(rigid.position, knockbackPosition) <= 0.1f)
    //        {
    //            isKnockback = false;
    //        }

    //        yield return null;
    //    }
    //}

    // Update is called once per frame
    void Update()
    {
        
    }
}
