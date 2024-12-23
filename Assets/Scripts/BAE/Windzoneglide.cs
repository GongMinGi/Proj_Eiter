using UnityEngine;

public class WindZoneglide : MonoBehaviour
{
    [SerializeField]
    private float windForce = 5f; // �ٶ��� �������� ��
    [SerializeField]
    private float upperHeight = 3f; // WindZone ���� ��� ����
    [SerializeField]
    private float lowerHeight = 0.5f; // WindZone ���� �ϴ� ����
    [SerializeField]
    private float horizontalRange = 2f; // WindZone �߽����κ����� ���� ����
    [SerializeField]
    private float defaultGravityScale = 1f; // �÷��̾��� �⺻ �߷� ��

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private PlayerControllerbyBae playerController;
    private bool playerInZone = false; // �÷��̾ WindZone �ȿ� �ִ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerControllerbyBae>();

            if (playerRb != null)
            {
                defaultGravityScale = playerRb.gravityScale; // �ʱ� �߷°� ����
            }
            playerInZone = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            if (playerRb != null)
            {
                // �߷��� ����
                playerRb.gravityScale = defaultGravityScale;
            }
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!playerInZone || playerTransform == null || playerRb == null || playerController == null) return;

        // �÷��̾��� ��ġ �� WindZone ���� ���
        float playerHeight = playerTransform.position.y;
        float playerHorizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        // �۶��̵� ���̰�, WindZone ���� ���� �ִ� ���
        if (playerController.IsGliding &&
            playerHeight >= zoneBottom &&
            playerHeight <= zoneTop &&
            playerHorizontalDistance <= horizontalRange)
        {
            // �߷��� ����
            playerRb.gravityScale = 0;

            // �������� �ٶ��� �� ����
            Vector2 windDirection = Vector2.up; // �׻� �������� ���� ����
            playerRb.velocity = new Vector2(playerRb.velocity.x, windForce);
        }
        else
        {
            // WindZone�� ����ų� �۶��̵��� �����ϸ� �߷��� ����
            playerRb.gravityScale = defaultGravityScale;
        }
    }
}




/*using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField]
    private float windForce = 5f; // �ٶ��� �������� ��
    [SerializeField]
    private float upperHeight = 3f; // ���� �� ���� ��� ����
    [SerializeField]
    private float lowerHeight = 0.5f; // ���� �� ���� �ϴ� ����
    [SerializeField]
    private float horizontalRange = 2f; // ���� �߽����κ����� ���� ����
    [SerializeField]
    private float defaultGravityScale = 1f; // �÷��̾��� �⺻ �߷� ��

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private PlayerControllerbyBae playerController;
    private bool playerInZone = false; // �÷��̾ WindZone �ȿ� �ִ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerControllerbyBae>();

            if (playerRb != null)
            {
                defaultGravityScale = playerRb.gravityScale; // �ʱ� �߷°� ����
            }
            playerInZone = true;
        }
    }

    [System.Obsolete]
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            if (playerRb != null)
            {
                // �߷°� �ӵ� ����
                playerRb.gravityScale = defaultGravityScale;
                playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
            }
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!playerInZone || playerTransform == null || playerRb == null || playerController == null) return;

        // ���� ���� ���̿� ���� �Ÿ� ���
        float playerHeight = playerTransform.position.y;
        float playerHorizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        // ���� Ȯ��: �۶��̵� ���̸� �÷��̾ ���̿� ���� ���� ���� ���� ��
        if (playerController.IsGliding && playerHeight <= zoneTop && playerHorizontalDistance <= horizontalRange)
        {
            // �߷��� ����
            playerRb.gravityScale = 0;

            // ���� �������� ���������� ���� ����
            Vector2 windDirection = (Vector2)transform.up.normalized;
            playerRb.AddForce(windDirection * windForce, ForceMode2D.Force);

            // ���(upperHeight)�� �����ϸ� �ӵ��� ����
            if (playerHeight >= zoneTop)
            {
                playerRb.velocity = new Vector2(playerRb.velocity.x, Mathf.Min(playerRb.velocity.y, 0));
            }
        }
        else
        {
            // ������ ����ų� �۶��̵��� �������� ��� �߷� ����
            playerRb.gravityScale = defaultGravityScale;
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
        }
    }
}




/*using UnityEngine;

public class WindZone : MonoBehaviour
{
    [SerializeField]
    private float constantWindSpeed = 5f; // �ٶ��� ������ �ӵ�
    [SerializeField]
    private float upperHeight = 3f; // ���� �� ���� ��� ����
    [SerializeField]
    private float lowerHeight = 0.5f; // ���� �� ���� �ϴ� ����
    [SerializeField]
    private float horizontalRange = 2f; // ���� �߽����κ����� ���� ����
    [SerializeField]
    private float defaultGravityScale = 1f; // �÷��̾��� �⺻ �߷� ��

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private PlayerControllerbyBae playerController;
    private bool playerInZone = false; // �÷��̾ WindZone �ȿ� �ִ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // �÷��̾ WindZone�� ������ ��
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerControllerbyBae>();

            if (playerRb != null)
            {
                defaultGravityScale = playerRb.gravityScale; // �ʱ� �߷°� ����
            }
            playerInZone = true;
        }
    }

    [System.Obsolete]
    private void OnTriggerExit2D(Collider2D collision)
    {
        // �÷��̾ WindZone�� ������ ��
        if (collision.CompareTag("Player"))
        {
            playerInZone = false;
            if (playerRb != null)
            {
                // �߷°� �ӵ� ����
                playerRb.gravityScale = defaultGravityScale;
                playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
            }
        }
    }

    [System.Obsolete]
    private void FixedUpdate()
    {
        if (!playerInZone || playerTransform == null || playerRb == null || playerController == null) return;

        // ���� ���� ���̿� ���� �Ÿ� ���
        float playerHeight = playerTransform.position.y;
        float playerHorizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        // ���� Ȯ��: �÷��̾ ���� ������ ���� ���� ���� �ְ� �۶��̵� ������
        if (playerHeight >= zoneBottom && playerHeight <= zoneTop &&
            playerHorizontalDistance <= horizontalRange && playerController.IsGliding)
        {
            // �߷��� ����
            playerRb.gravityScale = 0;

            // �ʷϻ� ��(���� Y��) �������� �ٶ� ����
            Vector2 windDirection = (Vector2)transform.up.normalized;

            // ��� � ����
            playerRb.velocity = windDirection * constantWindSpeed;
        }
        else
        {
            // ������ ����� �߷��� ���� ������ ����
            playerRb.gravityScale = defaultGravityScale;
            playerRb.velocity = new Vector2(playerRb.velocity.x, playerRb.velocity.y);
        }
    }
}


*/

