using UnityEngine;

public class Chargeattackfire : MonoBehaviour
{
    public Animator animator; // Animator ������Ʈ ����

    void Update()
    {
        // ���콺 ��ư�� ���Ҵ��� Ȯ��
        if (Input.GetMouseButtonUp(0)) // ���� ���콺 ��ư
        {
            // Animator �Ķ���͸� Trigger�� ����
            animator.SetTrigger("Chargeattackfire");
        }
    }
}
