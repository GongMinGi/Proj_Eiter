using UnityEngine;

public class WindZoneglide : MonoBehaviour
{
    [SerializeField] private float windForce = 6f; // �ٶ��� �������� ��
    [SerializeField] private float upperHeight = 3f; // WindZone ���� ��� ����
    [SerializeField] private float lowerHeight = 0.5f; // WindZone ���� �ϴ� ����
    [SerializeField] private float horizontalRange = 2f; // WindZone �߽����κ����� ���� ����
    [SerializeField] private float defaultGravityScale = 1f; // �÷��̾��� �⺻ �߷� ��

    private Transform playerTransform;
    private Rigidbody2D playerRb;
    private PlayerController playerController;
    private bool playerInZone = false; // �÷��̾ WindZone �ȿ� �ִ��� ����

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerTransform = collision.transform;
            playerRb = collision.GetComponent<Rigidbody2D>();
            playerController = collision.GetComponent<PlayerController>();

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

    private void FixedUpdate()
    {
        if (!playerInZone || playerTransform == null || playerRb == null || playerController == null) return;

        float playerHeight = playerTransform.position.y;
        float playerHorizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        if (playerController.isGliding &&
            playerHeight >= zoneBottom &&
            playerHeight <= zoneTop &&
            playerHorizontalDistance <= horizontalRange)
        {
            ApplyWindEffect();
        }
        else
        {
            ResetGravity();
        }
    }

    private void ApplyWindEffect()
    {
        playerRb.gravityScale = 0;

        float playerHeight = playerTransform.position.y;
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        float normalizedHeight = (playerHeight - zoneBottom) / (zoneTop - zoneBottom);
        float dynamicWindForce = Mathf.Lerp(1f, windForce, normalizedHeight);

        Vector2 windDirection = transform.up.normalized;

        // �� ������ ��ġ ������Ʈ
        Vector2 newPosition = playerRb.position + windDirection * dynamicWindForce * Time.fixedDeltaTime;
        playerRb.MovePosition(newPosition);

        // ����� �α�
        Debug.Log($"WindZone ���� ��: {newPosition}");
    }

    private void ResetGravity()
    {
        if (playerRb != null)
        {
            playerRb.gravityScale = defaultGravityScale;
        }
    }
}




/*
using UnityEngine;

public class WindZoneglide : MonoBehaviour
{
    [SerializeField]
    private float windForce = 6f; // �ٶ��� �������� ��
    [SerializeField]
    private float upperHeight = 3f; // WindZone ���� ��� ����G
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

    private void FixedUpdate()
    {
        if (!playerInZone || playerTransform == null || playerRb == null || playerController == null) return;

        float playerHeight = playerTransform.position.y;
        float playerHorizontalDistance = Mathf.Abs(playerTransform.position.x - transform.position.x);
        float zoneBottom = transform.position.y + lowerHeight;
        float zoneTop = transform.position.y + upperHeight;

        if (playerController.IsGliding &&
            playerHeight >= zoneBottom &&
            playerHeight <= zoneTop &&
            playerHorizontalDistance <= horizontalRange)
        {
            playerRb.gravityScale = 0;

            float normalizedHeight = (playerHeight - zoneBottom) / (zoneTop - zoneBottom);
            float dynamicWindForce = Mathf.Lerp(1f, windForce, normalizedHeight);

            Vector2 windDirection = transform.up.normalized;

            // �� ������ ��ġ ������Ʈ
            Vector2 newPosition = playerRb.position + windDirection * dynamicWindForce * Time.fixedDeltaTime;
            playerRb.MovePosition(newPosition);

            Debug.Log($"WindZone ���� ��: {newPosition}");
        }
        else
        {
            playerRb.gravityScale = defaultGravityScale;
        }
    }

}
*/






