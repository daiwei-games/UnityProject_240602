using UnityEngine;

namespace Xian
{
    /// <summary>
    /// 生成系統：預製物生成
    /// </summary>
    public class SpawnSystem : MonoBehaviour
    {
        //怪物陣列
        public GameObject[] monster;
        [Header("預製物生成位置")]
        public GameObject spawnPosition;
        [Header("怪物生成時間"),Tooltip("當遊戲計時大於生成時間會生成一隻怪物")]
        public float spawnTime;
        //計時
        float timer;

        void Update()
        {
            timer += Time.deltaTime;
            //隨機生成怪物
            int random = Random.Range(0, monster.Length);
            if (timer >= spawnTime)
            {

                Instantiate(monster[random], spawnPosition.transform);
                timer = 0;
            }
        }
    }
}

