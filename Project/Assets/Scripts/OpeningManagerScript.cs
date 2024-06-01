using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/// <summary>
/// 開場腳本
/// </summary>
public class OpeningManagerScript : MonoBehaviour
{
    /// <summary>
    /// 文本，故事及操作說明
    /// </summary>
    [Header("文本"), Tooltip("故事及操作說明")]
    public List<MainTextareaList> mainTextareaLists;
    #region 文本顯示UI相關
    /// <summary>
    /// 中文文本框
    /// </summary>
    [Header("UI Text 中文文本框")]
    public TextMeshProUGUI TextCn;
    /// <summary>
    /// 英文文本框
    /// </summary>
    [Header("UI Text 英文文本框")]
    public TextMeshProUGUI TextEn;
    /// <summary>
    /// UI Enter
    /// </summary>
    [Header("UI Button Enter")]
    public Button EnterButton;
    /// <summary>
    /// UI Enter Image
    /// </summary>
    [SerializeField]
    [Header("UI Image Enter")]
    private Image EnterButImage;
    /// <summary>
    /// UI Enter Image Alpha
    /// </summary>
    private float EnterButImgAlpha;
    #endregion
    #region 文本計算
    /// <summary>
    /// 中文文本逐字間隔秒數
    /// </summary>
    [Header("中文文本逐字間隔秒數"), Range(0, 1)]
    public float IntervalSecondsCn;
    /// <summary>
    /// 英文文本逐字間隔秒數
    /// </summary>
    [Header("英文文本逐字間隔秒數"), Range(0, 1)]
    public float IntervalSecondsEn;
    #endregion

    #region 文本篇章、段落
    /// <summary>
    /// 總文本有多少篇
    /// </summary>
    private int AllTextareaListCount;
    /// <summary>
    /// 總文本目前讀取篇章
    /// </summary>
    private int AllTextareaListNowIndex;
    /// <summary>
    /// 中文文本目前讀取段落
    /// </summary>
    private int CnTextareaListNowIndex;
    /// <summary>
    /// 英文文本目前讀取段落
    /// </summary>
    private int EnTextareaListNowIndex;
    #endregion

    #region 文本長度、讀取
    /// <summary>
    /// 每段中文總字數
    /// </summary>
    private int TextareaCnAllLength;
    /// <summary>
    /// 每段英文總字數
    /// </summary>
    private int TextareaEnAllLength;
    /// <summary>
    /// 當前已讀取中文文本
    /// </summary>
    private string NowTextareaCn;
    /// <summary>
    /// 當前已讀取英文文本
    /// </summary>
    private string NowTextareaEn;
    /// <summary>
    /// 當前已讀取中文文本字數
    /// </summary>
    private int TextareaCnNowLength;
    /// <summary>
    /// 當前已讀取應文文本字數
    /// </summary>
    private int TextareaEnNowLength;
    #endregion

    /// <summary>
    /// 當前中文文本段落
    /// </summary>
    private List<string> NowCnList;
    /// <summary>
    /// 當前英文文本段落
    /// </summary>
    private List<string> NowEnList;

    /// <summary>
    /// 是否中文文本已讀取完畢
    /// </summary>
    private bool isCnReadEnd;
    /// <summary>
    /// 是否英文文本已讀取完畢
    /// </summary>
    private bool isEnReadEnd;


    #region 音效
    /// <summary>
    /// 音效播放器
    /// </summary>
    [SerializeField]
    [Header("音效播放器")]
    private AudioSource audioSource;
    /// <summary>
    /// 音效清單
    /// </summary>
    [SerializeField]
    [Header("音效清單")]
    private AudioObjectList AudioClipList;
    #endregion

    [SerializeField]
    [Header("字型 Silver")]
    private TMP_FontAsset fontAssetSilver;

    [SerializeField]
    [Header("字型 Unifont")]
    private TMP_FontAsset fontAssetUnifont;

    [SerializeField]
    [Header("犼 Sprite")]
    private List<SpriteRenderer> SpriteAlpha;
    /// <summary>
    /// 犼 Sprite索引
    /// </summary>
    private int SpriteAlphaIndex;

    #region 晃動效果
    [Header("晃動幅度")]
    public float shakeAmount; // 晃動的幅度
    [Header("晃動持續時間")]
    public float shakeDuration; // 晃動的持續時間
    public Transform _tf;
    private Vector3 originalPosition;
    #endregion

    [Header("場景名稱")]
    public string SceneName;
    private void Awake()
    {

        //按鈕透明度為0
        EnterButImgAlpha = 0;

        //設定透明度為0
        EnterButImage.color = new Color(1, 1, 1, EnterButImgAlpha);
        //透明度為0設定按鈕不可用
        if (EnterButImage.color.a <= 0)
            EnterButton.interactable = false;



        AllTextareaListCount = mainTextareaLists.Count;
        AllTextareaListNowIndex = 0;


        CnTextareaListNowIndex = 0;
        TextareaCnAllLength = 0;
        NowCnList = mainTextareaLists[AllTextareaListNowIndex].Ch;


        EnTextareaListNowIndex = 0;
        TextareaEnAllLength = 0;
        NowEnList = mainTextareaLists[AllTextareaListNowIndex].En;

        TextareaCnNowLength = 0; //中文文本已讀取字數預先歸零
        TextareaEnNowLength = 0; //英文文本已讀取字數預先歸零

        isCnReadEnd = false;
        isEnReadEnd = false;
        ShowText();


        SpriteAlphaIndex = 0;

        
    }
    /// <summary>
    /// 執行字串函式、設定索引
    /// </summary>
    public void ShowText()
    {
        if (CnTextareaListNowIndex < NowCnList.Count)
        {
            TextareaCnAllLength = NowCnList[CnTextareaListNowIndex].Replace("-ErrorFonts-", "").Replace("\\n", "\n").Replace("{退散符}", "<sprite=0>").Replace("{跳躍}", "<sprite=1>").Replace("{蹲下}", "<sprite=2>").Length - 1;
            TextareaCnNowLength = 0;
            NowTextareaCn = "";
            TextCn.text = NowTextareaCn;
            StartCoroutine(TextareaValueShowCn(IntervalSecondsCn));
        }
        if (EnTextareaListNowIndex < NowEnList.Count)
        {
            TextareaEnAllLength = NowEnList[EnTextareaListNowIndex].Replace("-ErrorFonts-", "").Replace("\\n", "\n").Replace("{退散符}", "<sprite=0>").Replace("{跳躍}", "<sprite=1>").Replace("{蹲下}", "<sprite=2>").Length - 1;
            TextareaEnNowLength = 0;
            NowTextareaEn = "";
            TextEn.text = NowTextareaEn;
            StartCoroutine(TextareaValueShowEn(IntervalSecondsEn));
        }
    }
    #region 中文文本
    /// <summary>
    /// 中文字串設定
    /// </summary>
    void ShowCn()
    {
        string text = TextareaProcessing(NowCnList[CnTextareaListNowIndex], TextCn);
        string NowTxt = text[TextareaCnNowLength].ToString();
        NowTextareaCn = string.Concat(NowTextareaCn, NowTxt);
        NowTextareaCn = TextareaUpdateUndoneCommand(NowTxt, ref TextareaCnNowLength, NowTextareaCn, text);


        TextCn.text = NowTextareaCn;
        AudioSourcePlay();
    }
    IEnumerator TextareaValueShowCn(float _Seconds)
    {
        ShowCn();
        while (TextareaCnNowLength < TextareaCnAllLength)
        {
            TextareaCnNowLength++;
            yield return new WaitForSeconds(_Seconds);
            ShowCn();
        }
        isCnReadEnd = true;
    }
    #endregion
    #region 英文文本
    /// <summary>
    /// 英文字串設定
    /// </summary>
    void ShowEn()
    {
        string text = TextareaProcessing(NowEnList[CnTextareaListNowIndex], TextEn);
        string NowTxt = text[TextareaEnNowLength].ToString();
        NowTextareaEn = string.Concat(NowTextareaEn, NowTxt);
        NowTextareaEn = TextareaUpdateUndoneCommand(NowTxt, ref TextareaEnNowLength, NowTextareaEn, text);
        TextEn.text = NowTextareaEn;
        AudioSourcePlay();
    }
    IEnumerator TextareaValueShowEn(float _Seconds)
    {
        ShowEn();
        while (TextareaEnNowLength < TextareaEnAllLength)
        {
            TextareaEnNowLength++;
            yield return new WaitForSeconds(_Seconds);
            ShowEn();
        }
        isEnReadEnd = true;
    }


    /// <summary>
    /// 字串加工處理
    /// </summary>
    /// <param name="text">段落字串</param>
    /// <param name="_TMPro">文本物件</param>
    /// <returns>返回字串</returns>
    string TextareaProcessing(string text, TextMeshProUGUI _TMPro)
    {
        text = text.Replace("\\n", "\n");
        text = text.Replace("{退散符}", "<sprite=0>");
        text = text.Replace("{跳躍}", "<sprite=1>");
        text = text.Replace("{蹲下}", "<sprite=2>");
        if (text.IndexOf("-ErrorFonts-") == -1)
        {
            _TMPro.font = fontAssetUnifont;
            _TMPro.fontSize = 40;
        }
        if (text.IndexOf("-ErrorFonts-") != -1)
        {
            _TMPro.font = fontAssetSilver;
            _TMPro.fontSize = 50;
        }
        text = text.Replace("-ErrorFonts-", "");
        return text;
    }
    /// <summary>
    /// 補完未完成指令
    /// </summary>
    string TextareaUpdateUndoneCommand(string NowTxt, ref int NowLength, string NowTextarea, string Text)
    {

        if (NowTxt == "<")
        {
            for (int i = 0; i < 9; i++)
            {
                NowLength++;
                NowTextarea = string.Concat(NowTextarea, Text[NowLength].ToString());
            }
        }
        return NowTextarea;
    }
    #endregion

    /// <summary>
    /// 顯示Enter按鈕
    /// </summary>
    void ShowEnterButton()
    {
        if (isEnReadEnd && isCnReadEnd)
        {
            isCnReadEnd = false;
            isEnReadEnd = false;
            StartCoroutine(EnterButtonShow());
        }
    }
    /// <summary>
    /// Enter按鈕透明度呈現
    /// </summary>
    IEnumerator EnterButtonShow()
    {
        while (EnterButImgAlpha < 1)
        {
            EnterButImgAlpha += .1f;
            yield return new WaitForSeconds(.05f);
            EnterButImage.color = new Color(1, 1, 1, EnterButImgAlpha);
        }
        EnterButton.interactable = true;
    }
    /// <summary>
    /// 播放音效
    /// </summary>
    void AudioSourcePlay()
    {
        audioSource.clip = AudioClipList.clipList[0];
        audioSource.Play();
    }
    private void Update()
    {
        ShowEnterButton();
        if (Input.GetKeyUp(KeyCode.KeypadEnter) || Input.GetKeyUp(KeyCode.Return))
        {
            TextIndexAndRoleAlpha();
        }
    }
    /// <summary>
    /// 控制文本索引、角色透明度
    /// </summary>
    public void TextIndexAndRoleAlpha()
    {
        if (!EnterButton.interactable) return;
        if (SpriteAlpha.Count - 1 > SpriteAlphaIndex && EnterButton.interactable) StartCoroutine(RoleAlpha(SpriteAlpha[SpriteAlphaIndex]));
        EnterButton.interactable = false;

        if (NowCnList.Count > CnTextareaListNowIndex) CnTextareaListNowIndex++;
        if (NowEnList.Count > EnTextareaListNowIndex) EnTextareaListNowIndex++;
        if (NowCnList.Count <= CnTextareaListNowIndex && NowEnList.Count <= EnTextareaListNowIndex)
        {
            SceneManager.LoadScene(SceneName);
            return;
        }

        EnterButImgAlpha = 0;
        EnterButImage.color = new Color(1, 1, 1, EnterButImgAlpha);

        ShowText();
    }
    /// <summary>
    /// 角色物件透明
    /// </summary>
    /// <param name="RoleSprite"></param>
    IEnumerator RoleAlpha(SpriteRenderer RoleSprite)
    {
        StartShake();
        float alpha = 1;
        while (RoleSprite.color.a > 0)
        {
            alpha -= .1f;
            RoleSprite.color = new Color(1, 1, 1, alpha);
            yield return new WaitForSeconds(.1f);
        }
        SpriteAlphaIndex++;
    }



    /// <summary>
    /// 啟動晃動
    /// </summary>
    public void StartShake()
    {
        originalPosition = _tf.localPosition; // 獲取原始位置
        StartCoroutine(Shake());
    }
    /// <summary>
    /// 物件晃動
    /// </summary>
    IEnumerator Shake()
    {
        float elapsed = 0.0f;
        while (elapsed < shakeDuration)
        {
            Vector3 randomPoint = originalPosition + Random.insideUnitSphere * shakeAmount;
            _tf.localPosition = randomPoint;
            elapsed += Time.deltaTime;
            yield return null; // 等待下一幀
        }
        _tf.localPosition = originalPosition; // 恢復到原始位置
    }
}
