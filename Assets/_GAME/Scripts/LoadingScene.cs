using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScene : MonoBehaviour
{
    [SerializeField] Slider slider;          // có thể bỏ nếu không cần
    [SerializeField] TMP_Text txtProgress;   // có thể bỏ nếu không cần

    [SerializeField] float loadingTime = 3f; // thời gian fake load

    private void Start()
    {
        Application.targetFrameRate = 120;
        StartCoroutine(CoLoading());
    }

    IEnumerator CoLoading()
    {
        float timer = 0f;

        while (timer < loadingTime)
        {
            timer += Time.deltaTime;

            float progress = Mathf.Clamp01(timer / loadingTime);

            if (slider != null)
                slider.value = (int)(progress * 100);

            if (txtProgress != null)
                txtProgress.text = $"Loading {(int)(progress * 100)}%";

            yield return null;
        }

        gameObject.SetActive(false);    
        // Sau 3 giây → chuyển scene luôn
        GameController.I.OnInit();
    }
}