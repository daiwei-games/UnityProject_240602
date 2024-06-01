using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class MainMenuButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{

    [Header("按鈕焦點箭頭")]
    public Image FocusArrow;
    [SerializeField]
    [Header("Rect Transform 按鈕焦點箭頭")]
    private RectTransform _focusArrowRect;
    [Header("秒數間隔"), Range(0, 1)]
    public float IntervalSecondsCn;
    [Header("顯示文本物件")]
    public TextMeshProUGUI AppearTMPro;
    [Header("顯示文本")]
    public string Textarea;
    [Header("場景名稱")]
    public string SceneName;
    [Header("音效清單")]
    public AudioObjectList audioObjectList;
    [Header("音效撥放器")]
    public AudioSource audioSource;
    [Header("背景音樂撥放器")]
    public AudioSource audioSource_BGM;
    [Header("遮罩")]
    public Image masking;
    /// <summary>
    /// 是否以經點擊了按鈕
    /// </summary>
    private bool isSceneClick;

    private void Awake()
    {
        AppearTMPro.text = "";
        _focusArrowRect = FocusArrow.GetComponent<RectTransform>();
        isSceneClick = false;
        masking.gameObject.SetActive(false);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        StopAllCoroutines();
        StartCoroutine(ArrowAppear());
        StartCoroutine(ArrowRotation());
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isSceneClick) StopAllCoroutines();
        StartCoroutine(ArrowDisappear());
    }
    /// <summary>
    /// 執行更換場景
    /// </summary>
    public void GotoScene()
    {
        isSceneClick = true;
        StartCoroutine(GoToSceneButtonClick(SceneName));
    }
    /// <summary>
    /// 執行更換場景及按鈕程式
    /// </summary>
    /// <param name="_SceneName">場景名稱</param>
    /// <returns></returns>
    IEnumerator GoToSceneButtonClick(string _SceneName)
    {
        float _alpha = 0;
        audioSource.Play();
        yield return new WaitForSeconds(.2f);
        masking.gameObject.SetActive(true);
        audioSource_BGM.loop = false;
        audioSource_BGM.clip = audioObjectList.clipList[4];
        while (masking.color.a < 1)
        {
            _alpha += .1f;
            masking.color = new Color(0, 0, 0, _alpha);
            yield return new WaitForSeconds(.08f);
        }
        yield return new WaitForSeconds(.2f);
        audioSource_BGM.Play();
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(_SceneName);
    }
    /// <summary>
    /// 出現箭頭
    /// </summary>
    /// <returns></returns>
    IEnumerator ArrowAppear()
    {
        AppearTMPro.text = Textarea.Replace("\\n","\n");

        float _alpha = 0;
        while (FocusArrow.color.a < 1)
        {
            _alpha += .1f;
            FocusArrow.color = new Color(1, 1, 1, _alpha);
            yield return new WaitForSeconds(IntervalSecondsCn);
        }
    }
    /// <summary>
    /// 消失箭頭
    /// </summary>
    /// <returns></returns>
    IEnumerator ArrowDisappear()
    {
        float _alpha = 1;
        while (FocusArrow.color.a > 0)
        {
            _alpha -= .1f;
            FocusArrow.color = new Color(1, 1, 1, _alpha);
            yield return new WaitForSeconds(IntervalSecondsCn);
        }
    }
    IEnumerator ArrowRotation()
    {
        while (true)
        {
            _focusArrowRect.Rotate(0,3,0);
            yield return null;
        }
    }
}

