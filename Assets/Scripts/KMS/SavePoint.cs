using UnityEngine;

public class SavePoint : MonoBehaviour
{

    private Vector3 savePoint;  // ������ ��ġ ����

    void Start()
    {
        
        savePoint = transform.position;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.CompareTag("SavePoint"))
        {
            
            savePoint = collision.transform.position;       // SavePoint�� ��ġ�� ����

            Debug.Log("SavePoint updated: " + savePoint);

        }

    }

    public void Respawn(Transform player)
    {
        
        player.position = savePoint;        // �÷��̾��� ��ġ�� ������ ����� SavePoint ��ġ�� �̵�

        Debug.Log("Player respawned at: " + savePoint);

    }

}
