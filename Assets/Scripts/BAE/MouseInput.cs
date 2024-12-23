using UnityEngine;

public class MouseInput : MonoBehaviour
{
    public Transform playerTransform; // �÷��̾��� Transform
    public GameObject boomClone; // ��ź ������
    public float bombSpeed = 5f; // ��ź�� �ӵ�

    private void OnDrawGizmos()
    {
        // ������� ���� ���콺 ��ġ�� �ð�ȭ
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(Camera.main.ScreenToWorldPoint(Input.mousePosition), 0.2f);
    }

    [System.Obsolete]
    void Update()
    {
        if (Input.GetMouseButtonDown(1)) // ���콺 ��Ŭ��
        {
            Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0; // 2D ��ǥ���� Z �� ����

            // ��ź ����
            GameObject bomb = Instantiate(boomClone, playerTransform.position, Quaternion.identity);

            // ��ź�� ���� ����
            Vector2 direction = (mousePosition - playerTransform.position).normalized;

            // ��ź�� Rigidbody2D �߰� �� �ӵ� ����
            Rigidbody2D rb = bomb.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.velocity = direction * bombSpeed;
            }
        }
    }
}




/*using UnityEngine;

public class MouseInput : MonoBehaviour
{
    Vector3 MousePosition;
    public LayerMask whatisPlatform;
    public GameObject boomClone;
    private void OnDrawGizmos ()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere (MousePosition, 0.2f);
    }
    void Update()
    {
       if (Input.GetMouseButtonDown (1)) 
                    {
            MousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Instantiate(boomClone, MousePosition, Quaternion.identity);

        }
    }
}
*/