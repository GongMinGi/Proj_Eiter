using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform
    public Transform firePoint; // �߻�ü�� ������ FirePoint ��ġ
    public GameObject boomClone; // ��ź ������
    public float bombSpeed = 50f; // ��ź�� �ӵ�
    public float chargeTime = 1f; // �߻翡 �ʿ��� �ּ� ���� �ð�

    private float chargeCounter = 0f; // ���콺 ��ư ���� �ð�
    private SpriteRenderer spriteRenderer; // �÷��̾��� SpriteRenderer

    private void Awake()
    {
        // SpriteRenderer ��������
        spriteRenderer = playerTransform.GetComponent<SpriteRenderer>();
    }

    private void OnDrawGizmos()
    {
        // FirePoint ��ġ �ð�ȭ
        if (firePoint != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(firePoint.position, 0.1f);
        }
    }

    [System.Obsolete]
    private void Update()
    {
        // FirePoint ��ġ ����ȭ
        UpdateFirePointPosition();

        // ���콺 ��ư�� ���� ���¶�� �ð� ����
        if (Input.GetMouseButton(0)) // ���콺 ��Ŭ��
        {
            chargeCounter += Time.deltaTime; // ���� �ð� ����
        }

        // ���콺 ��ư�� �� �� �߻� ���� Ȯ��
        if (Input.GetMouseButtonUp(0)) // ���콺 ��ư���� ���� ��
        {
            if (chargeCounter >= chargeTime) // ���� �ð��� ���� ����
            {
                FireProjectile();
            }
            chargeCounter = 0f; // ���� �ð� �ʱ�ȭ
        }
    }

    private void UpdateFirePointPosition()
    {
        if (spriteRenderer.flipX)
        {
            firePoint.localPosition = new Vector3(-Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
        else
        {
            firePoint.localPosition = new Vector3(Mathf.Abs(firePoint.localPosition.x), firePoint.localPosition.y, firePoint.localPosition.z);
        }
    }

    [System.Obsolete]
    private void FireProjectile()
    {
        // �߻� ���� FirePoint ��ġ ����ȭ
        UpdateFirePointPosition();

        // �÷��̾ ���� �ִ� ���� Ȯ��
        bool isFacingRight = !spriteRenderer.flipX;
        Vector2 direction = isFacingRight ? Vector2.right : Vector2.left;

        // ��ź ����
        GameObject bomb = Instantiate(boomClone, firePoint.position, Quaternion.identity);

        // Rigidbody2D�� �ӵ� ����
        Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction * bombSpeed;
        }
    }
}



