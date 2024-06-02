using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
public class ReminderScript : MonoBehaviour
{
    [Header("提醒文字")]
    public TextMeshProUGUI ReminderText;
    [Header("場景名稱")]
    public string SceneName;

    private void Start()
    {
        StartCoroutine(GotoOpening());
    }
    IEnumerator GotoOpening()
    {
        float _alpha=0;
        TMP_Text text = ReminderText.GetComponent<TMP_Text>();
        while (text.color.a < 1)
        {
            _alpha += 0.1f;
            text.color = new Color(1, 1, 1, _alpha);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(2.5f);
        while (text.color.a > 0)
        {
            _alpha -= 0.1f;
            text.color = new Color(1, 1, 1, _alpha);
            yield return new WaitForSeconds(.1f);
        }
        yield return new WaitForSeconds(2.5f);
        SceneManager.LoadScene(SceneName);
    }

}
