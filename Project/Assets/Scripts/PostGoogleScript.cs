using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

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
    public void PostDataToExcel()
    {
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
            gameOver.BackScene();
        }
    }
}
