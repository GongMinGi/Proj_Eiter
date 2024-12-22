using UnityEngine;

public class ParallaxBackground_test01 : MonoBehaviour
{

    [SerializeField]
    private Transform target;   // ���� ���� �̾����� ���
    [SerializeField]
    private float scrollAmount; // �̾����� �� ��� ������ �Ÿ�
    [SerializeField]
    private float scrollSpeed; // �̵��ӵ�
    [SerializeField]
    private Vector3 moveDirection; // �̵� ����
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //����� moveDirection �������� moveSpeed �ӵ��� �̵�
        transform.position += moveDirection * scrollSpeed * Time.deltaTime;

        //����� ������ ������ ����� ��ġ �缳��
        if(transform.position.y <= -scrollAmount)
        {
            transform.position = target.position - moveDirection * scrollAmount;
        }
        
    }
}
