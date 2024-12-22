using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifetime;
    [SerializeField] private LayerMask whatisPlatform;
    [SerializeField] private float explosionRadius;

    [System.Obsolete]
    void Start()
    {
        rb.velocity = transform.right * speed;
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



/*using System.Collections.Generic; //bullet prefab�� ���� ��ũ��Ʈ�Դϴ�.https://www.youtube.com/watch?v=ZDkMhiNQOwo&t=5s ����
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float lifetime;

    [System.Obsolete]
    void Start()
    {
        rb.velocity = transform.right * speed;
        Invoke("DestroyProjectile", lifetime); // ���� �ð� �� �ı�
    }

    void DestroyProjectile()
    {
        Destroy(gameObject); // ���� ��ü ����
    }
}

*/