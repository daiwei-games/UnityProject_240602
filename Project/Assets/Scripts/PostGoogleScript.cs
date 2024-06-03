using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class PostGoogleScript : MonoBehaviour
{

    [SerializeField]
    [Header("UI GameOver 物件")]
    private GameOver gameOver;
    [Header("輸入Name")]
    public InputField inputN;
    [Header("輸入IG")]
    public InputField inputI;
    [Header("輸入手機")]
    public InputField inputP;
    [Header("音效播放")]
    public AudioSource AudioSource;
    [Header("音效清單")]
    public AudioObjectList AudioObjectList;
    [Header("送出資料的按鈕")]
    public Button SubmitButton;
    [SerializeField]
    [Header("送出資料的按鈕文字")]
    private Text SubmitButText;
    private int FontSize;
    private string ButtonText;
    private void Start()
    {
        if(SubmitButton != null)
        {
            Transform SubmitButTextTf = SubmitButton.transform.Find("Text (Legacy)");
            SubmitButText = SubmitButTextTf.GetComponent<Text>();

            FontSize = SubmitButText.fontSize;
            ButtonText = SubmitButText.text;
        }
    }
    private void ButtonReset()
    {
        SubmitButton.interactable = true;
        SubmitButText.fontSize = FontSize;
        SubmitButText.text = ButtonText;
    }
    public void PostDataToExcel()
    {
        SubmitButton.interactable = false;
        SubmitButText.fontSize = 22;
        SubmitButText.text = "Loading...";
        StartCoroutine(PostData());
    }

    IEnumerator PostData()
    {
        WWWForm form = new WWWForm();
        form.AddField("name", inputN.text);
        form.AddField("id", inputI.text);
        form.AddField("phone", inputP.text);
        //form.AddField("欄位命名", "欄位資料"); 要幾種資料就要輸入幾行，並且命名，命名是為了給Google Apps Script讀取

        using (UnityWebRequest www = UnityWebRequest.Post("https://script.google.com/macros/s/AKfycbyEeIuTNhGZHwuzjOSLK2t9z0UWx98A0Yqkz_lJLZF5DxJTUUfIKQPu2u54JKNCCR-L/exec", form))
        {
            yield return www.SendWebRequest();
            ButtonReset();
            gameOver.BackScene();
        }
    }
}
