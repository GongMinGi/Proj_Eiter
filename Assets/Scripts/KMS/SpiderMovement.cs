using UnityEngine;

public class SpiderMovement : BaseMonster
{

    public Transform[] waypoints;       // õ�� ���� �̵��� ��������Ʈ �迭

    public float speed = 2f;           // �Ź� �̵� �ӵ�
    public float descendSpeed = 3f;    // �������� �ӵ�
    public float rayDistance = 20f;    // Ray�� ����

    public LayerMask playerLayer;      // �÷��̾ ���� ���̾�

    public TrailRenderer webTrail;     // �Ź��� ȿ��

    public Transform target;  // ���ΰ��� Transform
    public Rigidbody2D Rigidbody2D;

    private int currentWaypointIndex = 0; // ���� �̵� ���� ��������Ʈ
    
    private bool isDescending = false;    // �������� ������ Ȯ��
    public bool isOnGround = false;      // �Ź̰� �ٴڿ� �����ߴ��� Ȯ��

    private SpriteRenderer spriteRenderer; // SpriteRenderer ������Ʈ
    private SpiderLanded spiderLanded;

    protected override void Awake()
    {

        base.Awake();

        spriteRenderer = GetComponent<SpriteRenderer>();

        spriteRenderer.flipY = true;

        animator.SetBool("Walk", true);

        webTrail.enabled = false;        // �Ź��� ��Ȱ��ȭ
        rigid.gravityScale = 0;         // õ�忡�� �߷� ����

    }

    private void Update()
    {

        if (waypoints.Length == 0 || isDescending || isOnGround)
        {

            return;

        }
        
        MoveAlongWaypoints();       // Waypoints ���� �̵�

        DetectPlayer();     // Ray�� �Ʒ��� �߻��Ͽ� �÷��̾� ����

    }

    private void MoveAlongWaypoints()
    {

        Transform targetWaypoint = waypoints[currentWaypointIndex];

        transform.position = Vector2.MoveTowards(transform.position, targetWaypoint.position, speed * Time.deltaTime);

        Vector2 dirVec = waypoints[currentWaypointIndex].position - transform.position;
        spriteRenderer.flipX = dirVec.x > 0 ? true : false;

        if (Vector2.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {

            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;   // ��ȯ �̵�

            Debug.Log($"Spider moved to waypoint {currentWaypointIndex}");

        }

    }

    private void DetectPlayer()
    {

        Vector2 rayOrigin = new Vector2(transform.position.x, transform.position.y - spriteRenderer.bounds.extents.y);
        Debug.DrawRay(rayOrigin, Vector2.down * rayDistance, Color.red); // Ray �ð��� �����

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.down, rayDistance, playerLayer);

        if (hit.collider != null && hit.collider.CompareTag("Player") && !isDescending)
        {

            Debug.Log("Player detected!");
            StartDescending();

        }

    }

    private void StartDescending()
    {

        isDescending = true; // �������� ���·� ��ȯ
        rigid.gravityScale = 1; // Rigidbody2D�� �߷� Ȱ��ȭ
        webTrail.enabled = true; // �Ź��� ȿ�� Ȱ��ȭ
        spriteRenderer.flipY = false; // �Ź��� Sprite ���� ����
        rigid.linearVelocity = Vector2.down * descendSpeed; // �Ʒ� �������� �ӵ� ����

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        if (isDescending && collision.collider.CompareTag("Ground"))
        {

            LandOnGround();

        }

    }

    private void LandOnGround()
    {

        isDescending = false;
        isOnGround = true;

        webTrail.enabled = false; // �Ź��� ȿ�� ��Ȱ��ȭ

        rigid.linearVelocity = Vector2.zero; // �ӵ� �ʱ�ȭ

        spiderLanded = GetComponent<SpiderLanded>();

        spiderLanded.enabled = true;

        this.enabled = false;       // SpiderMovement ��ũ��Ʈ�� ��Ȱ��ȭ

    }

    public override void TakeDamage(int damage, Vector2 damageSourcePosition)
    {

        base.TakeDamage(damage, damageSourcePosition);

        animator.SetTrigger("Damaged");

    }

}
