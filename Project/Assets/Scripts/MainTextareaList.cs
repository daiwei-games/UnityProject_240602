
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Assets.Scripts
{
    [System.Serializable]
    public class MainTextareaList
    {
        [Header("中文")]
        public List<string> Ch;
        [Header("英文")]
        public List<string> En;
    }
}
