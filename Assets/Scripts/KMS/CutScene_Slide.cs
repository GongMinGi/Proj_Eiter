using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

[System.Serializable]       // �ν����� â���� �Է¹��� ����ü
public class SlideData
{

    public Sprite image;       // �����̵� �̹���
    public float displayTime;  // �����̵� ǥ�� �ð�

}

public class CutScene_Slide : MonoBehaviour
{

    public Image currentImage;       // ���� �̹����� ǥ���� Image
    public Image nextImage;          // ���� �̹����� ǥ���� Image
    public SlideData[] slides;       // �����̵� ������ �迭

    public float fadeDuration = 1f; // ���̵� ��/�ƿ� �ð�

    private int currentSlideIndex = 0;

    private void Start()
    {

        StartCoroutine(PlaySlideShow());

    }

    private IEnumerator PlaySlideShow()
    {

        while (currentSlideIndex < slides.Length)
        {
            
            SlideData currentSlide = slides[currentSlideIndex];     // ���� �����̵� ������ ����

            Sprite nextSlideImage = (currentSlideIndex + 1 < slides.Length) ? slides[currentSlideIndex + 1].image : null;       // ���� �����̵� ������ ���� (���� �����̵尡 �ִ� ��츸)
            
            currentImage.sprite = currentSlide.image;       // ���� �̹����� ����
            currentImage.color = new Color(1, 1, 1, 1);         // ������ ������


            if (nextSlideImage != null)     // ���� �̹����� �̸� ���� (���� ����)
            {

                nextImage.sprite = nextSlideImage;
                nextImage.color = new Color(1, 1, 1, 0);        // ������ ����

            }

            yield return new WaitForSeconds(currentSlide.displayTime);      // �����̵� ǥ�� �ð� ���

            if (nextSlideImage != null)     // ���̵� �ƿ�/�� ȿ���� ���ÿ� ����
            {

                yield return StartCoroutine(FadeTransition());

            }
           
            currentSlideIndex++;         // ���� �����̵�� �̵�

        }

        // �ƽ� ����
        Debug.Log("Cutscene Finished");
        SceneManager.LoadScene("LabABasementScene"); 

    }

    private IEnumerator FadeTransition()
    {

        float elapsedTime = 0f;

        while (elapsedTime < fadeDuration)
        {

            float alpha = elapsedTime / fadeDuration;
            
            currentImage.color = new Color(1, 1, 1, 1 - alpha);     // ���� �̹����� ���� ��������

            nextImage.color = new Color(1, 1, 1, alpha);        // ���� �̹����� ���� ����������

            elapsedTime += Time.deltaTime;
            yield return null;

        }

        // ���̵� �Ϸ� ��, ���� ����
        currentImage.color = new Color(1, 1, 1, 0); // ������ ����
        nextImage.color = new Color(1, 1, 1, 1);   // ������ ������

    }

}
