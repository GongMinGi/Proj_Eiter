using System.Collections;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    //������Ʈ�� �������� ���� �� �ִ��� ����
    [SerializeField] private bool damageable = true;
    
    // ������Ʈ�� �ִ� ü��
    [SerializeField] private int maxHealth;

    //�������� ���� �� �����ð�
    [SerializeField] private float invulnerabilityTime = 0.2f;

    //������Ʈ�� ���ݵǾ����� ����
    private bool hit = false;

    //���� ü��
    private int currentHealth;
    [SerializeField] private float knockbackForce = 5f; //�з����� ��

    private Rigidbody2D rb;
    [SerializeField]

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;      //���� ���� �� �ִ�ü������ ����
    }


    //������ ó�� �� ü�� ����
    public void Damage(int amount, Vector2 attackDirection )
    {
        if (!damageable || hit || currentHealth <= 0) return;

        hit = true;
        currentHealth -= amount;

        ApplyKnockback(attackDirection);
    

        if (currentHealth <= 0)
        {
            // ü���� 0 �����϶� ������Ʈ ��Ȱ��ȭ
            gameObject.SetActive(false);
        }
        else
        {
            //���� �ð��� ������ �ٽ� ������ ���� �� �ְ� ����
            StartCoroutine(TurnOffHit());
        }
    }

    private void ApplyKnockback(Vector2 direction)
    {
        direction.y = 0;
        if(rb != null)
        {
            rb.linearVelocity = Vector2.zero; //���� �ӵ� �ʱ�ȭ
            rb.AddForce(direction.normalized * knockbackForce, ForceMode2D.Impulse);
            
        }
    }


    private IEnumerator TurnOffHit()
    {
        yield return new WaitForSeconds(invulnerabilityTime);
        hit = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
