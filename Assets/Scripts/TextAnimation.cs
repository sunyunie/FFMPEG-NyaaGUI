using TMPro;
using UnityEngine;

namespace Sunyunie.FFMPEGNyaa
{
    /// <summary>
    /// 간단한 텍스트 애니메이션을 위한 클래스
    /// </summary>
    public class TextAnimation : MonoBehaviour
    {
        [Header("String 사이클")]
        [SerializeField] private string[] strings;
        [Header("애니메이션 속도")]
        [SerializeField] private float animationSpeed = 0.5f;
        [Header("재생할 텍스트")]
        [SerializeField] private TextMeshProUGUI textToAnimate;

        private void FixedUpdate()
        {
            if (strings.Length == 0 || textToAnimate == null) return;

            // 현재 시간에 따라 인덱스 계산
            int index = Mathf.FloorToInt(Time.time / animationSpeed) % strings.Length;

            // 텍스트 업데이트
            textToAnimate.text = strings[index];
        }
    }
}