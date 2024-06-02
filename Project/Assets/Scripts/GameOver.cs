using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameOver : MonoBehaviour
{
    [Header("主畫面場景名稱")]
    public string BackSceneName;
    [Header("重新開始場景名稱")]
    public string RestartSceneName;
    [Header("音效清單")]
    public AudioObjectList audioObjectList;
    [Header("音效來源")]
    public AudioSource audioSource;
    [Header("背景音樂來源")]
    public AudioSource audioSource_BGM;
    [Header("遮罩")]
    public Image masking;

    private void OnEnable()
    {
        masking.color = new Color(0, 0, 0, 0);
        masking.gameObject.SetActive(false);
    }

    public void BackScene()
    {
        StartCoroutine(GoToSceneButtonClick(BackSceneName));
    }
    public void RestartScene()
    {
        StartCoroutine(GoToSceneButtonClick(RestartSceneName));
    }
    /// <summary>
    /// 執行更換場景及按鈕程式
    /// </summary>
    /// <param name="_SceneName">場景名稱</param>
    /// <returns></returns>
    IEnumerator GoToSceneButtonClick(string _SceneName)
    {
        float _alpha = 0;
        audioSource.clip = audioObjectList.clipList[2];
        audioSource.Play();
        yield return new WaitForSecondsRealtime(.2f);
        masking.gameObject.SetActive(true);
        audioSource_BGM.loop = false;
        audioSource_BGM.clip = audioObjectList.clipList[4];
        while (masking.color.a < 1)
        {
            _alpha += .1f;
            masking.color = new Color(0, 0, 0, _alpha);
            yield return new WaitForSecondsRealtime(.08f);
        }
        yield return new WaitForSecondsRealtime(.2f);
        audioSource_BGM.Play();
        yield return new WaitForSecondsRealtime(2f);
        Time.timeScale = 1;
        SceneManager.LoadScene(_SceneName);
    }
}
