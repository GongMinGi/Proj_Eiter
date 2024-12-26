/*using UnityEngine;

public class ProjectileBAE : MonoBehaviour
{
    [SerializeField] private float speed; // �߻� �ӵ�
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private float lifetime; // ����
    [SerializeField] private float explosionRadius; // ���� �ݰ�
    [SerializeField] private LayerMask whatisPlatform; // Ÿ�ϸ� �ı� ���
    [SerializeField] private GameObject explosionEffectPrefab; // ���� ����Ʈ (����)

    public bool isCharged = false; // ���� ���� ����

    [System.Obsolete]
    void Start()
    {
        rigid.velocity = transform.right * speed; // �Ѿ� �̵�
        Destroy(gameObject, lifetime); // ���� �ð� �� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharged)
        {
            Explode(); // ���� ������ ��� ����
        }
        Destroy(gameObject); // �浹 �� ����
    }

    private void Explode()
    {
        // ���� �ݰ� �� Collider Ž��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatisPlatform);

        foreach (var collider in colliders)
        {
            // Ÿ�ϸ� �ı�
            Bricks brick = collider.GetComponent<Bricks>();
            if (brick != null)
            {
                brick.MakeDot(collider.transform.position);
            }
        }

        // ���� ����Ʈ ���� (���� ����)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (explosionRadius > 0)
        {
            Gizmos.color = isCharged ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius); // ���� �ݰ� �ð�ȭ
        }
    }
}




*/

using UnityEngine;

public class ProjectileBAE : MonoBehaviour
{
    [SerializeField] private float speed; // �߻� �ӵ�
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifetime; // ����
    [SerializeField] private float explosionRadius; // ���� �ݰ�
    [SerializeField] private LayerMask whatisPlatform; // Ÿ�ϸ� �ı� ���
    [SerializeField] private GameObject explosionEffectPrefab; // ���� ����Ʈ (����)

    public bool isCharged = false; // ���� ���� ����

    [System.Obsolete]
    void Start()
    {
        rb.velocity = transform.right * speed; // �Ѿ� �̵�
        Destroy(gameObject, lifetime); // ���� �ð� �� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharged)
        {
            Explode(); // ���� ������ ��� ����
        }
        Destroy(gameObject); // �浹 �� ����
    }

    private void Explode()
    {
        // ���� �ݰ� �� Collider Ž��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatisPlatform);

        foreach (var collider in colliders)
        {
            // Ÿ�ϸ� �ı�
            Bricks brick = collider.GetComponent<Bricks>();
            if (brick != null)
            {
                brick.MakeDot(collider.transform.position);
            }
        }

        // ���� ����Ʈ ���� (���� ����)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (explosionRadius > 0)
        {
            Gizmos.color = isCharged ? Color.red : Color.yellow;
            Gizmos.DrawWireSphere(transform.position, explosionRadius); // ���� �ݰ� �ð�ȭ
        }
    }
}





/*using System;
using UnityEngine;

public class ProjectileBAE : MonoBehaviour
{
    [SerializeField] private float speed; // �߻� �ӵ�
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private float lifetime; // ����
    [SerializeField] private float explosionRadius; // ���� �ݰ�
    [SerializeField] private LayerMask whatisPlatform; // Ÿ�ϸ� �� �ı� ���
    [SerializeField] private GameObject explosionEffectPrefab; // ���� ����Ʈ (����)

    public bool isCharged = false; // ���� ���� ����

    [System.Obsolete]
    void Start()
    {
        rigid.velocity = transform.right * speed; // �Ѿ� �̵�
        Destroy(gameObject, lifetime); // ���� �ð� �� ����
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isCharged)
        {
            Explode(); // ���� ������ ��� ����
        }
        Destroy(gameObject); // �浹 �� ����
    }

    private void Explode()
    {
        // ���� �ݰ� �� Collider Ž��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatisPlatform);

        foreach (var collider in colliders)
        {
            // Ÿ�ϸ� �ı�
            Bricks brick = collider.GetComponent<Bricks>();
            if (brick != null)
            {
                brick.MakeDot(collider.transform.position);
            }
        }

        // ���� ����Ʈ ���� (���� ����)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ���� �ݰ� �ð�ȭ
        Gizmos.color = isCharged ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }

    internal void Initialize(bool v)
    {
        throw new NotImplementedException();
    }
}
*/


/*using UnityEngine;

public class ProjectileBAE : MonoBehaviour

{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rigid;
    [SerializeField] private float lifetime;
    [SerializeField] private LayerMask whatisPlatform;
    [SerializeField] private float explosionRadius;

    [System.Obsolete]
    void Start()
    {
        rigid.velocity = transform.right * speed;
        Invoke("DestroyProjectile", lifetime); // ���� �ð� �� ����
    }

    void DestroyProjectile()
    {
        Explode(); // ���� ���� ó��
        Destroy(gameObject);
    }

    void Explode()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatisPlatform);

        foreach (var collider in colliders)
        {
            Bricks brick = collider.GetComponent<Bricks>();
            if (brick != null)
            {
                brick.MakeDot(collider.transform.position);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius); // ���� ���� ǥ��
    }
}

*/

