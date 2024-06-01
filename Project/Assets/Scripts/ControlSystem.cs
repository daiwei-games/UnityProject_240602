using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
namespace Xian
{
    
    /// <summary>
    /// 角色控制系統：控制角色進行動作
    /// </summary>
    public class ControlSystem : MonoBehaviour
    {
        // 剛體組件
        private Rigidbody2D rbody;
        // 動畫組件
        private Animator ani;
        [Header("跳躍力度"), Range(0, 10)]
        public float jump;
        bool isJumping;
        bool isCrouching;
        [Header("結束畫面")]
        public GameObject gameOverScene;
        public GameObject upButton;
        public GameObject downButton;
        // 添加了 BoxCollider2D 引用，用於控制角色的碰撞器


        [Header("角色碰撞器")]
        public Collider2D RoleCollider2D;


        [Header("所有自動移動物件 MovementSystem 腳本")]
        public List<MovementSystem> movementSystemsList;
        [Header("生成系統")]
        public SpawnSystem spawnSystem;
        [Header("音效列表")]
        public AudioObjectList audioObjectList;
        [Header("音效撥放器")]
        public AudioSource audioSource;
        [Header("背景音樂撥放器")]
        public AudioSource audioSource_BGM;
        [Header("分數")]
        public TextMeshProUGUI Score;
        [Header("秒數")]
        private float Seconds;
        private void Awake()
        {
            // 在 Awake 中獲取組件，以確保按鈕事件能夠正確註冊
            rbody = GetComponent<Rigidbody2D>();
            ani = GetComponent<Animator>();
            isJumping = false;
            isCrouching = false;

            // 獲取初始的站立時 Collider 的大小和中心點偏移

            
            spawnSystem = FindObjectOfType<SpawnSystem>();

            Seconds = 0;
        }

        // Update is called once per frame
        void Update()
        {
            
            // 按下上、space按鍵即可跳躍並判斷是否落地
            if ((Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.Space)) && !isJumping)
            {
                Jump();
            }

            // 判斷 下 鍵值小於0時執行蹲下動畫、大於等於0時關閉蹲下動畫
            float v = Input.GetAxis("Vertical");
            if (isCrouching || v < 0) //蹲下
            {

                ani.SetBool("isCrouch", true);
                // 蹲下時設置 Collider 的大小和中心點偏移
            }
            else //站起
            {
 
                ani.SetBool("isCrouch", false);
                // 站起來時恢復 Collider 的大小和中心點偏移
            }
            if (!gameOverScene.activeSelf) // 遊戲未結束時，計算分數
            {
                Seconds += Time.deltaTime;
                Score.text = "Score: " + (int)Seconds;
            }
        }

        // 新增的函數，處理跳躍功能
        public void Jump()
        {
            if(!isJumping)
                rbody.velocity = new Vector2(0, jump);

            isJumping = true;
            ani.SetBool("isJump", true);

        }

        // 新增的函數，處理蹲下功能
        public void Crouch()
        {
            isCrouching = true;

        }

        // 新增的函數，處理站起來功能
        public void StandUp()
        {
            isCrouching = false;

        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            isJumping = false;
            ani.SetBool("isJump", false);
            // 判定當物件觸碰到tag為Mon的物件時
            if (collision.gameObject.CompareTag("Mon"))
            {
                RoleDie();
                // 將結束畫面顯示
                gameOverScene.SetActive(true);
                upButton.SetActive(false);
                downButton.SetActive(false);
            }
        }
        /// <summary>
        /// 角色死亡
        /// </summary>
        public void RoleDie()
        {
            ani.SetBool("isDie", true);
            audioSource.clip = audioObjectList.clipList[5];
            audioSource.Play();
            rbody.velocity = new Vector2(0, jump * 1.5f);
            spawnSystem.enabled = false;
            movementSystemsList = FindObjectsOfType<MovementSystem>().ToList();
            foreach (var msitem in movementSystemsList)
            {
                msitem.movementSpeed = 0;
            }

            // 關閉角色碰撞器
            RoleCollider2D.enabled = false;
            StartCoroutine(EndGame());
        }
        IEnumerator EndGame()
        {
            yield return new WaitForSecondsRealtime(.5f);
            audioSource_BGM.loop = false;
            audioSource_BGM.clip = audioObjectList.clipList[6];
            audioSource_BGM.Play();
            yield return new WaitForSecondsRealtime(3f);
            // 停止遊戲
            //Time.timeScale = 0;
        }
    }
}

