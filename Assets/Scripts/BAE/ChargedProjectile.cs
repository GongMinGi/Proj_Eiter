using UnityEngine;

public class ChargedProjectile : MonoBehaviour
{
    [SerializeField] private float speed; // ���� ���� �߻� �ӵ�
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private float explosionRadius; // ���� �ݰ�
    [SerializeField] private LayerMask whatisPlatform; // Ÿ�ϸ��̳� ���� ����� ���� ���̾�
    [SerializeField] private GameObject explosionEffectPrefab; // ���� ����Ʈ ������ (����)

    [System.Obsolete]
    void Start()
    {
        rb.velocity = transform.right * speed; // ������ �������� �߻�
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // �浹 �� ���� ȿ�� ����
        Explode();
        Destroy(gameObject); // �浹�� ���� ���� ����
    }

    private void Explode()
    {
        // ���� �ݰ� ���� ��� Collider Ž��
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, whatisPlatform);

        foreach (var collider in colliders)
        {
            // Ÿ�ϸ� �ı�
            Bricks brick = collider.GetComponent<Bricks>();
            if (brick != null)
            {
                brick.MakeDot(collider.transform.position);
            }

            // �� ó�� ���� (����)
            // �� ��ũ��Ʈ�� �ִٸ� ���⿡ �߰� ����
        }

        // ���� ����Ʈ ���� (���� ����)
        if (explosionEffectPrefab != null)
        {
            Instantiate(explosionEffectPrefab, transform.position, Quaternion.identity);
        }
    }

    private void OnDrawGizmosSelected()
    {
        // ���� �ݰ��� �ð������� ǥ�� (����׿�)
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, explosionRadius);
    }
}
