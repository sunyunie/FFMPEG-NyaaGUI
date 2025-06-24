using SFB;
using UnityEngine;

namespace Sunyunie.FFMPEGNyaa
{
    /// <summary>
    /// FFMPEGNyaa의 메인 클래스
    /// </summary>
    public class FFMPEGNyaa : MonoBehaviour
    {
        [SerializeField] private UserSetting userSetting;

        private bool isFFMPEGPathReady = false; // FFMPEG 경로가 설정되었는지 확인용
        private bool isInputPathReady = false; // 입력 소스 경로가 설정되었는지 확인용
        private bool isOutputPathReady = false; // 출력 경로가 설정되었는지 확인용

        private bool isInputFileNameReady = false; // 입력 비디오 파일 이름(형식)이 설정되었는지 확인용
        private bool isOutputFileNameReady = false; // 출력 비디오 파일 이름이 설정되었는지 확인용

        private bool isWidthReady = false; // 비디오 너비가 설정되었는지 확인용
        private bool isHeightReady = false; // 비디오 높이가 설정되었는지 확인용
        private bool isFramerateReady = false; // 비디오 프레임 레이트가 설정되었는지 확인용

        private bool isCodecReady = false; // 비디오 코덱이 설정되었는지 확인용
        private bool isSpeedReady = false; // 비디오 인코딩 속도가 설정되었는지 확인용

        private bool IsStringHasError(string _value, bool _isOnlyNumber) // 문자열에 오류가 있는지 확인용
        {
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
        }

        public void Button_FFMPEG_FindPath()
        {
            var paths = StandaloneFileBrowser.OpenFilePanel("FFmpeg 실행파일을 골라줘냥~", "", "exe", false);

            if (paths.Length > 0)
            {
                userSetting.ffmpegPath = paths[0];
            }
            else
            {
                Debug.LogWarning("FFmpeg 실행파일을 찾지 못했다냥~");
            }
        }

        public void Button_InputLocation_FindPath()
        {
            var paths = StandaloneFileBrowser.OpenFolderPanel("입력 비디오 파일의 경로를 골라줘냥~", "", false);

            if (paths.Length > 0)
            {
                userSetting.inputLocation = paths[0];
            }
            else
            {
                Debug.LogWarning("입력 비디오 파일의 경로를 찾지 못했다냥~");
            }
        }

        public void InputField_FFMPEG_Changed(string _value)
        {
            userSetting.ffmpegPath = _value;

            if (string.IsNullOrEmpty(userSetting.ffmpegPath))
            {
                Debug.LogWarning("FFmpeg 실행파일 경로가 비어있다냥~");
            }
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