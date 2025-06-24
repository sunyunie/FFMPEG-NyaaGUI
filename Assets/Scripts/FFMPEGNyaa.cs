using SFB;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sunyunie.FFMPEGNyaa
{
    /// <summary>
    /// FFMPEGNyaa의 메인 클래스
    /// </summary>
    public class FFMPEGNyaa : MonoBehaviour
    {
        [Header("색상")]
        [SerializeField] private Color errorColor = Color.red; // 오류 발생 시 배경색
        [SerializeField] private Color normalColor = Color.green; // 정상 상태일 때 배경색

        [Header("이미지")]
        [SerializeField] private Sprite badIcon;
        [SerializeField] private Sprite goodIcon;

        [Header("인풋필드")]
        [SerializeField] private TMP_InputField ffmpegPathText; // FFMPEG 실행파일 경로를 표시할 텍스트
        [SerializeField] private TMP_InputField inputLocationText; // 입력 비디오 파일 경로를 표시할 텍스트
        [SerializeField] private TMP_InputField inputFileNameText; // 입력 비디오 파일 이름(형식)을 표시할 텍스트
        [SerializeField] private TMP_InputField outputLocationText; // 출력 비디오 파일 경로를 표시할 텍스트

        [SerializeField] private TMP_InputField widthText; // 비디오 너비를 표시할 텍스트
        [SerializeField] private TMP_InputField heightText; // 비디오 높이를
        [SerializeField] private TMP_InputField framerateText; // 비디오 프레임 레이트를 표시할 텍스트

        [SerializeField] private TMP_InputField outputFileNameText; // 출력 비디오 파일 이름을 표시할 텍스트

        [Header("배경")]
        [SerializeField] private Image ffmpegPathBackgroundImage; // FFMPEG 실행파일 경로 입력 필드의 배경 이미지
        [SerializeField] private Image inputLocationBackgroundImage; // 입력 비디오 파일 경로 입력 필드의 배경 이미지
        [SerializeField] private Image inputFileNameBackgroundImage; // 입력 비디오 파일 이름(형식) 입력 필드의 배경 이미지
        [SerializeField] private Image outputLocationBackgroundImage; // 출력 비디오 파일 경로 입력 필드의 배경 이미지

        [SerializeField] private Image widthBackgroundImage; // 비디오 너비 입력 필드의 배경 이미지
        [SerializeField] private Image heightBackgroundImage; // 비디오 높이 입력 필드의 배경 이미지
        [SerializeField] private Image framerateBackgroundImage; // 비디오 프레임 레이트 입력 필드의 배경 이미지
        [SerializeField] private Image codecBackgroundImage; // 비디오 코덱 선택 필드의 배경 이미지
        [SerializeField] private Image speedBackgroundImage; // 비디오 인코딩 속도 선택 필드의 배경 이미지

        [SerializeField] private Image outputFileNameBackgroundImage; // 출력 비디오 파일 이름 입력 필드의 배경 이미지

        [Header("아이콘")]
        [SerializeField] private Image ffmpegPathIcon; // FFMPEG 실행파일 경로 입력 필드의 아이콘
        [SerializeField] private Image inputLocationIcon; // 입력 비디오 파일 경로 입력 필드의 아이콘
        [SerializeField] private Image inputFileNameIcon; // 입력 비디오 파일 이름(형식) 입력 필드의 아이콘
        [SerializeField] private Image outputLocationIcon; // 출력 비디오 파일 경로 입력 필드의 아이콘

        [Header("현 상태 추적 (ReadOnly)")]
        [SerializeField] private bool isFFMPEGPathReady = false; // FFMPEG 경로가 설정되었는지 확인용
        [SerializeField] private bool isInputPathReady = false; // 입력 소스 경로가 설정되었는지 확인용
        [SerializeField] private bool isOutputPathReady = false; // 출력 경로가 설정되었는지 확인용

        [SerializeField] private bool isInputFileNameReady = false; // 입력 비디오 파일 이름(형식)이 설정되었는지 확인용
        [SerializeField] private bool isOutputFileNameReady = false; // 출력 비디오 파일 이름이 설정되었는지 확인용

        [SerializeField] private bool isWidthReady = false; // 비디오 너비가 설정되었는지 확인용
        [SerializeField] private bool isHeightReady = false; // 비디오 높이가 설정되었는지 확인용
        [SerializeField] private bool isFramerateReady = false; // 비디오 프레임 레이트가 설정되었는지 확인용

        [SerializeField] private bool isCodecReady = false; // 비디오 코덱이 설정되었는지 확인용
        [SerializeField] private bool isSpeedReady = false; // 비디오 인코딩 속도가 설정되었는지 확인용

        [Header("유저 데이터")]
        [SerializeField] private UserSetting userSetting;

        private void Start()
        {
            CheckAllErrors();
        }

        private bool IsStringHasError(string _value, bool _isOnlyNumber) // 문자열에 오류가 있는지 확인용
        {
            _value = CleanTMPInput(_value); // 입력값에서 제로폭 문자를 제거하고 앞뒤 공백을 제거

            // 빈 문자열인지 확인
            if (string.IsNullOrEmpty(_value))
            {
                Debug.LogWarning("빈 문자열은 입력할 수 없다냥~");
                return false;
            }

            if (_isOnlyNumber)
            {
                // 숫자만 입력되었는지 확인
                foreach (char c in _value)
                {
                    if (!char.IsDigit(c))
                    {
                        Debug.LogWarning("숫자만 입력되어야 한다냥~");
                        return false;
                    }
                }
            }

            return true;
        }

        private string CleanTMPInput(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            // 제로폭 문자들 제거
            return input
                .Replace("\u200B", "") // Zero Width Space
                .Replace("\u200C", "") // Zero Width Non-Joiner
                .Replace("\u200D", "") // Zero Width Joiner
                .Replace("\uFEFF", "") // Zero Width No-Break Space (BOM)
                .Trim();               // 앞뒤 공백 제거
        }

        private void CheckAllErrors()
        {
            isFFMPEGPathReady = IsStringHasError(userSetting.ffmpegPath, false);
            isInputPathReady = IsStringHasError(userSetting.inputLocation, false);
            isOutputPathReady = IsStringHasError(userSetting.outputLocation, false);

            isInputFileNameReady = IsStringHasError(userSetting.inputFileName, false);
            isOutputFileNameReady = IsStringHasError(userSetting.outputFileName, false);

            isWidthReady = IsStringHasError(userSetting.width, true);
            isHeightReady = IsStringHasError(userSetting.height, true);
            isFramerateReady = IsStringHasError(userSetting.framerate, true);

            UpdateUI();
        }

        private void UpdateUI()
        {
            if (isFFMPEGPathReady)
            {
                ffmpegPathBackgroundImage.color = normalColor;
                ffmpegPathIcon.sprite = goodIcon;
            }
            else
            {
                ffmpegPathBackgroundImage.color = errorColor;
                ffmpegPathIcon.sprite = badIcon;
            }

            if (isInputPathReady)
            {
                inputLocationBackgroundImage.color = normalColor;
                inputLocationIcon.sprite = goodIcon;
            }
            else
            {
                inputLocationBackgroundImage.color = errorColor;
                inputLocationIcon.sprite = badIcon;
            }

            if (isOutputPathReady)
            {
                outputLocationBackgroundImage.color = normalColor;
                outputLocationIcon.sprite = goodIcon;
            }
            else
            {
                outputLocationBackgroundImage.color = errorColor;
                outputLocationIcon.sprite = badIcon;
            }

            if (isInputFileNameReady)
            {
                inputFileNameBackgroundImage.color = normalColor;
                inputFileNameIcon.sprite = goodIcon;
            }
            else
            {
                inputFileNameBackgroundImage.color = errorColor;
                inputFileNameIcon.sprite = badIcon;
            }

            if (isOutputFileNameReady)
            {
                outputFileNameBackgroundImage.color = normalColor;
            }
            else
            {
                outputFileNameBackgroundImage.color = errorColor;
            }

            if (isWidthReady)
            {
                widthBackgroundImage.color = normalColor;
            }
            else
            {
                widthBackgroundImage.color = errorColor;
            }

            if (isHeightReady)
            {
                heightBackgroundImage.color = normalColor;
            }
            else
            {
                heightBackgroundImage.color = errorColor;
            }

            if (isFramerateReady)
            {
                framerateBackgroundImage.color = normalColor;
            }
            else
            {
                framerateBackgroundImage.color = errorColor;
            }

            if (isCodecReady)
            {
                codecBackgroundImage.color = normalColor;
            }
            else
            {
                codecBackgroundImage.color = errorColor;
            }

            if (isSpeedReady)
            {
                speedBackgroundImage.color = normalColor;
            }
            else
            {
                speedBackgroundImage.color = errorColor;
            }
        }

        public void Button_FFMPEG_FindPath()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("FFmpeg 실행파일을 골라줘냥~", "", "exe", false);

            if (paths.Length > 0)
            {
                userSetting.ffmpegPath = paths[0];

                ffmpegPathText.text = userSetting.ffmpegPath;
            }
            else
            {
                Debug.LogWarning("FFmpeg 실행파일을 찾지 못했다냥~");
            }

            CheckAllErrors();
        }

        public void Button_InputLocation_FindPath()
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("입력 비디오 파일의 경로를 골라줘냥~", "", false);

            if (paths.Length > 0)
            {
                userSetting.inputLocation = paths[0];

                inputLocationText.text = userSetting.inputLocation;
            }
            else
            {
                Debug.LogWarning("입력 비디오 파일의 경로를 찾지 못했다냥~");
            }

            CheckAllErrors();
        }

        public void Button_InputFileName_FindPath()
        {
            var extensions = new[]
            {
                new ExtensionFilter("Image Files", new[] { "png", "jpg", "jpeg", "bmp", "tga", "gif", "tif", "tiff", "exr", "webp" }),
                new ExtensionFilter("All Files", "*")
            };

            var paths = StandaloneFileBrowser.OpenFilePanel("입력 이미지 파일의 첫 프레임을 골라달라냥~", userSetting.inputLocation, extensions, false);


            if (paths.Length > 0)
            {
                userSetting.inputFileName = paths[0];

                inputFileNameText.text = userSetting.inputFileName;
            }
            else
            {
                Debug.LogWarning("입력 비디오 파일의 이름(형식)을 찾지 못했다냥~");
            }

            CheckAllErrors();
        }

        public void Button_OutputLocation_FindPath()
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("출력 비디오 파일의 경로를 골라줘냥~", "", false);

            if (paths.Length > 0)
            {
                userSetting.outputLocation = paths[0];

                outputLocationText.text = userSetting.outputLocation;
            }
            else
            {
                Debug.LogWarning("출력 비디오 파일의 경로를 찾지 못했다냥~");
            }

            CheckAllErrors();
        }

        public void InputField_FFMPEG_Changed()
        {
            userSetting.ffmpegPath = ffmpegPathText.text;

            CheckAllErrors();
        }

        public void InputField_InputLocation_Changed()
        {
            userSetting.inputLocation = inputLocationText.text;

            CheckAllErrors();
        }

        public void InputField_InputFileName_Changed()
        {
            userSetting.inputFileName = inputFileNameText.text;

            CheckAllErrors();
        }

        public void InputField_OutputLocation_Changed()
        {
            userSetting.outputLocation = outputLocationText.text;

            CheckAllErrors();
        }

        public void InputField_Width_Changed()
        {
            userSetting.width = widthText.text;

            CheckAllErrors();
        }

        public void InputField_Height_Changed()
        {
            userSetting.height = heightText.text;

            CheckAllErrors();
        }

        public void InputField_Framerate_Changed()
        {
            userSetting.framerate = framerateText.text;

            CheckAllErrors();
        }

        public void Dropdown_Codec_Changed()
        {
            isCodecReady = true;

            CheckAllErrors();
        }

        public void Dropdown_Speed_Changed()
        {
            isSpeedReady = true;

            CheckAllErrors();
        }

        public void InputField_OutputFileName_Changed()
        {
            userSetting.outputFileName = outputFileNameText.text;

            CheckAllErrors();
        }
    }

    /// <summary>
    /// 저장될 설정
    /// </summary>
    [System.Serializable]
    public struct UserSetting
    {
        public string       ffmpegPath;     // FFMPEG의 실행 파일 경로
        public string       inputLocation;  // 입력 비디오 파일의 경로
        public string       outputLocation; // 출력 비디오 파일의 경로

        public string       inputFileName;  // 입력 비디오 파일의 이름 (형식)
        public string       outputFileName; // 출력 비디오 파일의 이름

        public string       width;          // 비디오의 너비
        public string       height;         // 비디오의 높이

        public string       framerate;      // 비디오의 프레임 레이트

        public FFMPEGCodecs codec;          // 비디오 코덱
        public FFMPEGSpeed  speed;          // 비디오 인코딩 속도

    }

    /// <summary>
    /// 코덱
    /// </summary>
    public enum FFMPEGCodecs
    {
        H264,
        H265,
        WebM
    }

    /// <summary>
    /// 처리 속도
    /// </summary>
    public enum FFMPEGSpeed
    {
        ultrafast,
        superfast,
        veryfast,
        faster,
        fast,
        medium,
        slow,
        slower,
        veryslow,
        placebo
    }
}