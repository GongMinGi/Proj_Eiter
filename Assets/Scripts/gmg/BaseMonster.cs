using System.Collections;
using UnityEngine;

public class BaseMonster : MonoBehaviour
{
    [Header("Monster Settings")]
    [SerializeField] public int health = 5;  //ü��
    [SerializeField] public float knockbackDistance = 3f;    //�˹� �Ÿ�
    [SerializeField] public float knockbackSpeed = 20f;  //�˹� �ӵ�
    public float proximityThreshold = 0.2f; //�˹� ��ġ ���� ����

    protected Rigidbody2D rigid;        // Rigidbody2D ������Ʈ
    protected Animator animator;        // Animator ������Ʈ

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

        //���� ó��
        if (health <= 0 )
        {
            Die();
            return;
        }



        // �˹� ���� ��� ( x�ุ)
        Vector2 knockbackDir = new Vector2(rigid.position.x - damageSourcePosition.x, 0).normalized;

        Debug.Log(knockbackDir);

        //�˹� ��ǥ ��ġ ���
        knockbackPosition = rigid.position + knockbackDir * knockbackDistance;
        Debug.Log(knockbackPosition);
        //isKnockback = true;


        isKnockback = true;
    }


    protected virtual void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        Destroy(gameObject);
    }


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





    // Update is called once per frame
    void Update()
    {
        
    }
}
