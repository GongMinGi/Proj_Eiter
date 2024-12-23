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

    protected Vector2 knockbackPosition;  // �˹� ��ǥ ��ġ
    [SerializeField]protected bool isKnockback = false; 

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
        Knockback(damageSourcePosition);
    }


    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }


    protected virtual void Knockback(Vector2 damageSourcePosition)
    {
        if (rigid == null) return;

        // �˹� ���� ��� ( x�ุ)

        float normalizedDirection = 0 < rigid.position.x - damageSourcePosition.x ? 1 : -1;

        Vector2 dirVec = new Vector2(normalizedDirection, rigid.position.y);

        Debug.Log(dirVec);

        //�˹� ��ǥ ��ġ ���
        knockbackPosition = rigid.position + dirVec * knockbackDistance;

        isKnockback = true;

        rigid.MovePosition(Vector2.MoveTowards(rigid.position, knockbackPosition, knockbackSpeed));

        // ��ǥ ��ġ�� �����ϸ� �˹� ����
        if (Vector2.Distance(rigid.position, knockbackPosition) <= proximityThreshold)
        {
            isKnockback = false;

        }

    }





    // Update is called once per frame
    void Update()
    {
        
    }
}
