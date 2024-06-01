using UnityEngine;

namespace Xian
{
    /// <summary>
    /// 移動系統：場景及物件橫移
    /// </summary>
    public class MovementSystem : MonoBehaviour
    {
        [Header("場景移動速度"), Range(0, 10)]
        public float movementSpeed;
        [Header("場景重生位置"), Tooltip("場景消失後重新生成的位置")]
        public float startPosition;
        [Header("場景消失位置"), Tooltip("場景移動後卸載的位置")]
        public float endPosition;

        void Update()
        {
            //判定場景物件x軸移動座標
            transform.position = new Vector2(transform.position.x - movementSpeed * Time.deltaTime, transform.position.y);

            if (transform.position.x <= endPosition)
            {
                //判定小於x值時將tag名為Mon的物件卸載
                if (gameObject.tag == "Mon")
                {
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = new Vector2(startPosition, transform.position.y);
                }

            }
        }
    }
}

