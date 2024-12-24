using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DialogueController : MonoBehaviour
{

    public GameObject dialogueBox;   // ��ǳ�� �̹���
    public Text dialogueText;   // �ؽ�Ʈ

    public string fullText;     // ����� ��ü �ؽ�Ʈ

    public float typingSpeed = 0.05f;    // Ÿ���� �ӵ�

    private bool isTyping = false;  // Ÿ���� ������ Ȯ��
    private bool skipRequested = false;     // ��ŵ ��û ���� Ȯ��
    public Rigidbody2D playerRigidbody;     // �÷��̾� Rigidbody

    [HideInInspector]
    public bool isTalking = false;

    void Start()
    {

        dialogueBox.SetActive(false);    // �ʱ� ���¿��� ��ǳ�� �����
        dialogueText.text = "";     // �ʱ� ���¿��� �ؽ�Ʈ �����

    }

    public void StartDialogue()
    {

        dialogueBox.SetActive(true); 

        isTalking = true;

        playerRigidbody.linearVelocity = Vector2.zero; 
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezePositionY;
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;

        StartCoroutine(TypeText());

    }

    void Update()
    {

        if (isTyping && Input.GetKeyDown(KeyCode.Q))
        {

            skipRequested = true;   // ��ŵ ��û ����

        }

    }

    IEnumerator TypeText()
    {

        isTyping = true;
        skipRequested = false;
        dialogueText.text = ""; 

        foreach (char c in fullText)
        {

            if (skipRequested)
            {

                dialogueText.text = fullText;   // ��ü �ؽ�Ʈ �ٷ� ���

                break;

            }

            dialogueText.text += c;     // �� ���ھ� ���

            yield return new WaitForSeconds(typingSpeed);

        }

        // Ÿ������ �������Ƿ� ����
        isTyping = false;
        skipRequested = false;

        yield return new WaitForSeconds(1f);    // Ÿ���� �Ϸ� �� ���

        EndDialogue();  // ��ȭ ����

    }

    public void EndDialogue()
    {

        dialogueBox.SetActive(false);   // ��ǳ�� �����
        dialogueText.text = "";     // �ؽ�Ʈ �����

        isTalking = false;

        playerRigidbody.constraints = RigidbodyConstraints2D.None;  // �̵� ���� ����
        playerRigidbody.constraints = RigidbodyConstraints2D.FreezeRotation;        // ���ϸ� ĳ���� ��۹�� ���ư�

    }

}
