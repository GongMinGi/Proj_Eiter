using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.Composites;

// made by mingi

public class PlayerController : MonoBehaviour
{

    [SerializeField]
    float moveSpeed = 1f;
    [SerializeField]
    float jumpForce = 3f;
    bool isGround = true;
    Rigidbody2D playerRigid;
    Transform playerDirection;
    Animator playerAnim;
    SpriteRenderer spriteRenderer;


    void Awake()
    {
        playerRigid = GetComponent<Rigidbody2D>();
        playerAnim = GetComponent<Animator>();
        playerDirection = GetComponent<Transform>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        Jump();

        //sprite�� ������ �ڵ�. GetAxisRaw(horizontal)�� a�� ������ -1, d�� ���� �� 1�̹Ƿ� 
        //a�� ������ -1�� ������ true, d�� ������ -1�� �޶����� false�� ��ȯ�ϰ� ��.
        // ���� getbuttondown�� ��ư�� ó������ �������븸 true�� ��ȯ�Ѵ�.
        if (Input.GetButton("Horizontal"))
            spriteRenderer.flipX = Input.GetAxisRaw("Horizontal") == -1;
        
    }

    void FixedUpdate()
    {
        float moveInput = Input.GetAxisRaw("Horizontal");
        playerRigid.linearVelocity = new Vector2(moveInput * moveSpeed, playerRigid.linearVelocity.y);


        //landing platform
        if(playerRigid.linearVelocity.y < 0 ) //�߶��Ҷ��� ���̸� �Ʒ��� ���.
        {
            Debug.DrawRay(playerRigid.position, Vector3.down, new Color(0, 1, 0));
            RaycastHit2D rayHit = Physics2D.Raycast(playerRigid.position, Vector3.down, 1, LayerMask.GetMask("Platform"));
            if (rayHit.collider != null)
            {
                Debug.Log(rayHit.collider.name);

                if (rayHit.distance < 1f)
                {
                    Debug.Log(rayHit.collider.name);
                    playerAnim.SetBool("isJumping", false);
                }
            }
        }

    }

    void Jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !playerAnim.GetBool("isJumping"))
        {
            playerRigid.linearVelocity = new Vector2(playerRigid.linearVelocity.y, jumpForce);
            playerAnim.SetBool("isJumping", true);
        }
    }
}
