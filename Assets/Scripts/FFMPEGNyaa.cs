using SFB;
using TMPro;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using System.Text.RegularExpressions;

namespace Sunyunie.FFMPEGNyaa
{
    /// <summary>
    /// FFMPEGNyaa의 메인 클래스
    /// </summary>
    public class FFMPEGNyaa : MonoBehaviour
    {
        [Header("색상")]
        [SerializeField] private Color errorColor = Color.red;      // 오류 발생 시 배경색
        [SerializeField] private Color normalColor = Color.green;   // 정상 상태일 때 배경색

        [Header("이미지")]
        [SerializeField] private Sprite badIcon;
        [SerializeField] private Sprite goodIcon;

        [Header("인풋필드")]
        [SerializeField] private TMP_InputField ffmpegPathText;         // FFMPEG 실행파일 경로를 표시할 텍스트
        [SerializeField] private TMP_InputField inputLocationText;      // 입력 비디오 파일 경로를 표시할 텍스트
        [SerializeField] private TMP_InputField inputFileNameText;      // 입력 비디오 파일 이름(형식)을 표시할 텍스트
        [SerializeField] private TMP_InputField outputLocationText;     // 출력 비디오 파일 경로를 표시할 텍스트

        [SerializeField] private TMP_InputField widthText;          // 비디오 너비를 표시할 텍스트
        [SerializeField] private TMP_InputField heightText;         // 비디오 높이를
        [SerializeField] private TMP_InputField framerateText;      // 비디오 프레임 레이트를 표시할 텍스트

        [SerializeField] private TMP_InputField outputFileNameText; // 출력 비디오 파일 이름을 표시할 텍스트

        [SerializeField] private TMP_InputField commandPreviewTest; // FFMPEG 명령어 미리보기를 표시할 텍스트

        [Header("드롭다운")]
        [SerializeField] private TMP_Dropdown codecDropdown;        // 비디오 코덱 선택 드롭다운
        [SerializeField] private TMP_Dropdown speedDropdown;        // 비디오 인코딩 속도 선택 드롭다운
        [SerializeField] private TMP_Dropdown pixelFormatDropdown;  // 비디오 픽셀 포맷 선택 드롭다운

        [Header("토글")]
        [SerializeField] private Toggle openFolderWhenDoneToggle; // 작업 완료 후 출력 폴더를 열지 여부를 선택하는 토글

        [Header("배경")]
        [SerializeField] private Image ffmpegPathBackgroundImage;       // FFMPEG 실행파일 경로 입력 필드의 배경 이미지
        [SerializeField] private Image inputLocationBackgroundImage;    // 입력 비디오 파일 경로 입력 필드의 배경 이미지
        [SerializeField] private Image inputFileNameBackgroundImage;    // 입력 비디오 파일 이름(형식) 입력 필드의 배경 이미지
        [SerializeField] private Image outputLocationBackgroundImage;   // 출력 비디오 파일 경로 입력 필드의 배경 이미지

        [SerializeField] private Image widthBackgroundImage;            // 비디오 너비 입력 필드의 배경 이미지
        [SerializeField] private Image heightBackgroundImage;           // 비디오 높이 입력 필드의 배경 이미지
        [SerializeField] private Image framerateBackgroundImage;        // 비디오 프레임 레이트 입력 필드의 배경 이미지
        [SerializeField] private Image codecBackgroundImage;            // 비디오 코덱 선택 필드의 배경 이미지
        [SerializeField] private Image speedBackgroundImage;            // 비디오 인코딩 속도 선택 필드의 배경 이미지
        [SerializeField] private Image pixelFormatBackgroundImage;      // 비디오 픽셀 포맷 선택 필드의 배경 이미지

        [SerializeField] private Image outputFileNameBackgroundImage;   // 출력 비디오 파일 이름 입력 필드의 배경 이미지

        [Header("아이콘")]
        [SerializeField] private Image ffmpegPathIcon;          // FFMPEG 실행파일 경로 입력 필드의 아이콘
        [SerializeField] private Image inputLocationIcon;       // 입력 비디오 파일 경로 입력 필드의 아이콘
        [SerializeField] private Image inputFileNameIcon;       // 입력 비디오 파일 이름(형식) 입력 필드의 아이콘
        [SerializeField] private Image outputLocationIcon;      // 출력 비디오 파일 경로 입력 필드의 아이콘

        [Header("앞배경 오브젝트")]
        [SerializeField] private GameObject forgroundObject; // UI 앞배경 오브젝트

        [Header("현 상태 추적 (ReadOnly)")]
        [SerializeField] private bool isFFMPEGPathReady = false;    // FFMPEG 경로가 설정되었는지 확인용
        [SerializeField] private bool isInputPathReady = false;     // 입력 소스 경로가 설정되었는지 확인용
        [SerializeField] private bool isOutputPathReady = false;    // 출력 경로가 설정되었는지 확인용

        [SerializeField] private bool isInputFileNameReady = false;     // 입력 비디오 파일 이름(형식)이 설정되었는지 확인용
        [SerializeField] private bool isOutputFileNameReady = false;    // 출력 비디오 파일 이름이 설정되었는지 확인용

        [SerializeField] private bool isWidthReady = false;         // 비디오 너비가 설정되었는지 확인용
        [SerializeField] private bool isHeightReady = false;        // 비디오 높이가 설정되었는지 확인용
        [SerializeField] private bool isFramerateReady = false;     // 비디오 프레임 레이트가 설정되었는지 확인용

        [SerializeField] private bool isCodecReady = false;         // 비디오 코덱이 설정되었는지 확인용
        [SerializeField] private bool isSpeedReady = false;         // 비디오 인코딩 속도가 설정되었는지 확인용
        [SerializeField] private bool isPixelFormatReady = false;   // 비디오 픽셀 포맷이 설정되었는지 확인용

        [SerializeField] private bool isProcessing = false; // 현재 FFMPEG 작업이 진행 중인지 확인용
        [SerializeField] private bool isOpenFolderWhenDone = false; // 작업 완료 후 출력 폴더를 열지 여부

        [Header("유저 데이터")]
        [SerializeField] private UserSetting userSetting;

        private void Start()
        {
            CheckAllErrors();
        }

        private void FixedUpdate()
        {
            if (!isProcessing && forgroundObject.activeSelf) // FFMPEG 작업이 진행 중이지 않고 UI 앞배경이 활성화되어 있다면
            {
                OnProcessEnded(); // 작업 완료 후 처리
            }
        }

        private void OnProcessEnded()
        {
            forgroundObject.SetActive(false); // UI 앞배경 비활성화

            if (isOpenFolderWhenDone)
            {
                Button_OpenOutputLocation(); // 작업 완료 후 출력 폴더 열기
            }
        }

        private bool IsStringHasError(string _value, bool _isOnlyNumber) // 문자열에 오류가 있는지 확인용
        {
            _value = CleanTMPInput(_value); // 입력값에서 제로폭 문자를 제거하고 앞뒤 공백을 제거

            // 빈 문자열인지 확인
            if (string.IsNullOrEmpty(_value))
            {
                UnityEngine.Debug.LogWarning("빈 문자열은 입력할 수 없다냥~");
                return false;
            }

            if (_isOnlyNumber)
            {
                // 숫자만 입력되었는지 확인
                foreach (char c in _value)
                {
                    if (!char.IsDigit(c))
                    {
                        UnityEngine.Debug.LogWarning("숫자만 입력되어야 한다냥~");
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

            if (isPixelFormatReady)
            {
                pixelFormatBackgroundImage.color = normalColor;
            }
            else
            {
                pixelFormatBackgroundImage.color = errorColor;
            }

            UpdatePreview();
        }

        private void UpdatePreview()
        {
            if (!isFFMPEGPathReady || !isInputPathReady || !isOutputPathReady ||
                !isInputFileNameReady || !isOutputFileNameReady || !isWidthReady ||
                !isHeightReady || !isFramerateReady || !isCodecReady || !isSpeedReady || !isPixelFormatReady)
            {
                commandPreviewTest.text = "모든 필드를 올바르게 입력해야 한다냥~";
                return;
            }

            // FFMPEG 명령어 미리보기 업데이트
            string codec = userSetting.codec switch
            {
                FFMPEGCodecs.H264 => "libx264",
                FFMPEGCodecs.H265 => "libx265",
                FFMPEGCodecs.WebM => "libvpx-vp9",
                _ => "libx264"
            };

            string speed = userSetting.speed switch
            {
                FFMPEGSpeed.ultrafast => "ultrafast",
                FFMPEGSpeed.superfast => "superfast",
                FFMPEGSpeed.veryfast => "veryfast",
                FFMPEGSpeed.faster => "faster",
                FFMPEGSpeed.fast => "fast",
                FFMPEGSpeed.medium => "medium",
                FFMPEGSpeed.slow => "slow",
                FFMPEGSpeed.slower => "slower",
                FFMPEGSpeed.veryslow => "veryslow",
                FFMPEGSpeed.placebo => "placebo",
                _ => "medium"
            };

            string pixelFormat = userSetting.pixelFormat switch
            {
                FFMPEGPixelFormat.yuv420p => "yuv420p",
                FFMPEGPixelFormat.yuv444p => "yuv444p",
                FFMPEGPixelFormat.yuva420p => "yuva420p",
                _ => "yuv420p"
            };

            commandPreviewTest.text = $"\"{userSetting.ffmpegPath}\" -i \"{userSetting.inputLocation}/{userSetting.inputFileName}\" " +
                                      $"-vf \"scale={userSetting.width}:{userSetting.height}\" " +
                                      $"-r {userSetting.framerate} " +
                                      $"-c:v {codec} -preset {speed} " +
                                      $"-pix_fmt {pixelFormat} " +
                                      $"\"{userSetting.outputLocation}/{userSetting.outputFileName}\"";

            /*
            commandPreviewTest.text = $"\"{userSetting.ffmpegPath}\" -i \"{userSetting.inputLocation}/{userSetting.inputFileName}\" " +
                          $"-vf \"scale={userSetting.width}:{userSetting.height}:force_original_aspect_ratio=decrease,pad={userSetting.width}:{userSetting.height}:(ow-iw)/2:(oh-ih)/2\" " +
                          $"-r {userSetting.framerate} " +
                          $"-c:v {codec} -preset {speed} " +
                          $"-pix_fmt {pixelFormat} " +
                          $"\"{userSetting.outputLocation}/{userSetting.outputFileName}\"";
            */

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
                UnityEngine.Debug.LogWarning("FFmpeg 실행파일을 찾지 못했다냥~");
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
                UnityEngine.Debug.LogWarning("입력 비디오 파일의 경로를 찾지 못했다냥~");
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
                string fullPath = paths[0];
                string fileName = System.IO.Path.GetFileName(fullPath);

                // 확장자 분리
                string extension = System.IO.Path.GetExtension(fileName);
                string nameWithoutExtension = System.IO.Path.GetFileNameWithoutExtension(fileName);

                // 뒤쪽의 연속된 숫자 찾기 (예: 0000, 00123, 42 등)
                Match match = Regex.Match(nameWithoutExtension, @"^(.*?)(\d+)$");
                if (match.Success)
                {
                    string prefix = match.Groups[1].Value;
                    string number = match.Groups[2].Value;
                    int digitCount = number.Length;

                    // 포맷팅: %04d, %05d 등
                    string pattern = $"{prefix}%0{digitCount}d{extension}";

                    userSetting.inputFileName = pattern;
                    inputFileNameText.text = pattern;
                }
                else
                {
                    // 숫자가 없으면 그냥 이름 그대로 표시
                    userSetting.inputFileName = fileName;
                    inputFileNameText.text = fileName;

                    UnityEngine.Debug.LogWarning("프레임 넘버 형식을 감지하지 못했으니 자동 치환 안 했댕...냥냥펀치! (งΦㅅΦ)ง");
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("입력 이미지 파일을 선택하지 않았댕~");
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
                UnityEngine.Debug.LogWarning("출력 비디오 파일의 경로를 찾지 못했다냥~");
            }

            CheckAllErrors();
        }

        public void Button_SamplePreset()
        {
            userSetting.ffmpegPath = "C:/ffmpeg/bin/ffmpeg.exe"; // FFMPEG 실행파일 경로
            userSetting.inputLocation = "C:/Videos/Input"; // 입력 비디오 파일 경로
            userSetting.outputLocation = "C:/Videos/Output"; // 출력 비디오 파일 경로

            userSetting.inputFileName = "sample_%04d.png"; // 입력 비디오 파일 이름(형식)
            userSetting.outputFileName = "output.mp4"; // 출력 비디오 파일 이름

            userSetting.width = "1920"; // 비디오 너비
            userSetting.height = "1080"; // 비디오 높이

            userSetting.framerate = "30"; // 비디오 프레임 레이트

            userSetting.codec = FFMPEGCodecs.H264; // 비디오 코덱
            userSetting.speed = FFMPEGSpeed.slow; // 비디오 인코딩 속도

            userSetting.pixelFormat = FFMPEGPixelFormat.yuv420p; // 비디오 픽셀 포맷

            ffmpegPathText.text = userSetting.ffmpegPath;
            inputLocationText.text = userSetting.inputLocation;
            inputFileNameText.text = userSetting.inputFileName;
            outputLocationText.text = userSetting.outputLocation;
            outputFileNameText.text = userSetting.outputFileName;
            widthText.text = userSetting.width;
            heightText.text = userSetting.height;
            framerateText.text = userSetting.framerate;
            codecDropdown.value = userSetting.codec switch
            {
                FFMPEGCodecs.H264 => 1,
                FFMPEGCodecs.H265 => 2,
                FFMPEGCodecs.WebM => 3,
                _ => 0
            };
            speedDropdown.value = userSetting.speed switch
            {
                FFMPEGSpeed.ultrafast => 1,
                FFMPEGSpeed.superfast => 2,
                FFMPEGSpeed.veryfast => 3,
                FFMPEGSpeed.faster => 4,
                FFMPEGSpeed.fast => 5,
                FFMPEGSpeed.medium => 6,
                FFMPEGSpeed.slow => 7,
                FFMPEGSpeed.slower => 8,
                FFMPEGSpeed.veryslow => 9,
                FFMPEGSpeed.placebo => 10,
                _ => 0
            };
            pixelFormatDropdown.value = userSetting.pixelFormat switch
            {
                FFMPEGPixelFormat.yuv420p => 1,
                FFMPEGPixelFormat.yuv444p => 2,
                FFMPEGPixelFormat.yuva420p => 3,
                _ => 0
            };

            UpdateUI();
        }

        public void Button_Process()
        {
            if (!isFFMPEGPathReady || !isInputPathReady || !isOutputPathReady ||
                !isInputFileNameReady || !isOutputFileNameReady || !isWidthReady ||
                !isHeightReady || !isFramerateReady || !isCodecReady || !isSpeedReady || !isPixelFormatReady)
            {
                UnityEngine.Debug.LogWarning("모든 필드를 올바르게 입력해야 한다냥~");
                return;
            }

            isProcessing = true; // FFMPEG 작업 시작

            forgroundObject.SetActive(true); // UI 앞배경 활성화

            string codec = userSetting.codec.ToString().ToLower();

            switch (codec)
            {
                case "h265":
                case "hevc":
                    codec = "libx265";
                    break;
                case "h264":
                case "mp4":
                    codec = "libx264";
                    break;
                case "webm":
                    codec = "libvpx"; // 또는 libvpx-vp9
                    break;
            }

            System.Threading.Tasks.Task.Run(() =>
            {
                ProcessStartInfo startInfo = new ProcessStartInfo
                {
                    FileName = userSetting.ffmpegPath,
                    Arguments = $"-i \"{userSetting.inputLocation}/{userSetting.inputFileName}\" " +
                                $"-vf \"scale={userSetting.width}:{userSetting.height}\" " +
                                $"-r {userSetting.framerate} " +
                                $"-c:v {codec.ToString().ToLower()} -preset {userSetting.speed.ToString().ToLower()} " +
                                $"\"{userSetting.outputLocation}/{userSetting.outputFileName}\"",
                    RedirectStandardOutput = false,
                    RedirectStandardError = true, // 🔥 에러 로그 수집
                    UseShellExecute = false,       // 🐾 리디렉션 위해 false로
                    CreateNoWindow = true          // 🪟 콘솔창 감춤 (원하면 false)
                };

                using (Process process = new Process())
                {
                    process.StartInfo = startInfo;
                    process.Start();

                    UnityEngine.Debug.Log("FFMPEG 작업을 시작했다냥~");

                    string errorLog = process.StandardError.ReadToEnd(); // 🔍 stderr 읽기

                    process.WaitForExit();

                    if (process.ExitCode == 0)
                    {
                        UnityEngine.Debug.Log("FFMPEG 작업이 성공적으로 완료되었다냥~");
                    }
                    else
                    {
                        UnityEngine.Debug.LogError($"FFMPEG 종료 코드: {process.ExitCode}다냥~");
                        UnityEngine.Debug.LogError($"FFMPEG 오류 로그다냥:\n{errorLog}");
                    }

                    isProcessing = false;
                }
            });


            /*
            // FFMPEG 명령어 실행
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = userSetting.ffmpegPath,
                Arguments = $"-i \"{userSetting.inputLocation}/{userSetting.inputFileName}\" " +
                            $"-vf \"scale={userSetting.width}:{userSetting.height}\" " +
                            $"-r {userSetting.framerate} " +
                            $"-c:v {userSetting.codec.ToString().ToLower()} -preset {userSetting.speed.ToString().ToLower()} " +
                            $"\"{userSetting.outputLocation}/{userSetting.outputFileName}\"",
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            using (Process process = new Process())
            {
                process.StartInfo = startInfo;
                process.Start();

                // 표준 출력과 오류 출력 읽기
                string output = process.StandardOutput.ReadToEnd();
                string error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (process.ExitCode == 0)
                {
                    UnityEngine.Debug.Log("FFMPEG 작업이 성공적으로 완료되었다냥~");
                    UnityEngine.Debug.Log(output);
                }
                else
                {
                    UnityEngine.Debug.LogError("FFMPEG 작업 중 오류가 발생했다냥~");
                    UnityEngine.Debug.LogError(error);
                }
            }
            */
        }

        public void Button_SaveSettings()
        {
            // 유저 설정 저장
            UserSettingManager.Save(userSetting);
            UnityEngine.Debug.Log("설정을 저장했다냥~");

            // UI 업데이트
            CheckAllErrors();
        }

        public void Button_LoadSettings()
        {
            // 유저 설정 불러오기
            userSetting = UserSettingManager.Load();
            UnityEngine.Debug.Log("설정을 불러왔다냥~");

            // UI 업데이트
            ffmpegPathText.text = userSetting.ffmpegPath;
            inputLocationText.text = userSetting.inputLocation;
            inputFileNameText.text = userSetting.inputFileName;
            outputLocationText.text = userSetting.outputLocation;
            outputFileNameText.text = userSetting.outputFileName;
            widthText.text = userSetting.width;
            heightText.text = userSetting.height;
            framerateText.text = userSetting.framerate;

            codecDropdown.value = userSetting.codec switch
            {
                FFMPEGCodecs.H264 => 1,
                FFMPEGCodecs.H265 => 2,
                FFMPEGCodecs.WebM => 3,
                _ => 0
            };

            speedDropdown.value = userSetting.speed switch
            {
                FFMPEGSpeed.ultrafast => 1,
                FFMPEGSpeed.superfast => 2,
                FFMPEGSpeed.veryfast => 3,
                FFMPEGSpeed.faster => 4,
                FFMPEGSpeed.fast => 5,
                FFMPEGSpeed.medium => 6,
                FFMPEGSpeed.slow => 7,
                FFMPEGSpeed.slower => 8,
                FFMPEGSpeed.veryslow => 9,
                FFMPEGSpeed.placebo => 10,
                _ => 0
            };

            pixelFormatDropdown.value = userSetting.pixelFormat switch
            {
                FFMPEGPixelFormat.yuv420p => 1,
                FFMPEGPixelFormat.yuv444p => 2,
                FFMPEGPixelFormat.yuva420p => 3,
                _ => 0
            };

            isOpenFolderWhenDone = userSetting.isOpenFolderWhenDone;

            CheckAllErrors();
        }

        public void Button_OpenOutputLocation()
        {
            if (!isOutputPathReady)
            {
                UnityEngine.Debug.LogWarning("출력 경로가 설정되지 않았다냥~");
                return;
            }

            // 출력 경로 열기
            System.Diagnostics.Process.Start("explorer.exe", userSetting.outputLocation);
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
            if (codecDropdown.value == 0)
            {
                isCodecReady = false;
                UnityEngine.Debug.LogWarning("코덱을 선택해야 한다냥~");
                return;
            }
            else
            {
                isCodecReady = true;

                userSetting.codec = codecDropdown.value switch
                {
                    1 => FFMPEGCodecs.H264,
                    2 => FFMPEGCodecs.H265,
                    3 => FFMPEGCodecs.WebM,
                };
            }

            CheckAllErrors();
        }

        public void Dropdown_Speed_Changed()
        {
            if (speedDropdown.value == 0)
            {
                isSpeedReady = false;
                UnityEngine.Debug.LogWarning("속도를 선택해야 한다냥~");
                return;
            }
            else
            {
                isSpeedReady = true;

                userSetting.speed = speedDropdown.value switch
                {
                    1 => FFMPEGSpeed.ultrafast,
                    2 => FFMPEGSpeed.superfast,
                    3 => FFMPEGSpeed.veryfast,
                    4 => FFMPEGSpeed.faster,
                    5 => FFMPEGSpeed.fast,
                    6 => FFMPEGSpeed.medium,
                    7 => FFMPEGSpeed.slow,
                    8 => FFMPEGSpeed.slower,
                    9 => FFMPEGSpeed.veryslow,
                    10 => FFMPEGSpeed.placebo,
                };
            }

            CheckAllErrors();
        }

        public void Dropdown_PixelFormat_Changed()
        {
            if (pixelFormatDropdown.value == 0)
            {
                isPixelFormatReady = false;
                UnityEngine.Debug.LogWarning("픽셀 포맷을 선택해야 한다냥~");
                return;
            }
            else
            {
                isPixelFormatReady = true;

                userSetting.pixelFormat = pixelFormatDropdown.value switch
                {
                    1 => FFMPEGPixelFormat.yuv420p,
                    2 => FFMPEGPixelFormat.yuv444p,
                    3 => FFMPEGPixelFormat.yuva420p,
                };
            }

            CheckAllErrors();
        }

        public void InputField_OutputFileName_Changed()
        {
            userSetting.outputFileName = outputFileNameText.text;

            CheckAllErrors();
        }

        public void Toggle_OpenFolderWhenDone()
        {
            bool _isOn = openFolderWhenDoneToggle.isOn;

            isOpenFolderWhenDone                = _isOn;
            userSetting.isOpenFolderWhenDone    = _isOn;
        }
    }

    /// <summary>
    /// 저장될 설정
    /// </summary>
    [System.Serializable]
    public struct UserSetting
    {
        public string ffmpegPath;               // FFMPEG의 실행 파일 경로
        public string inputLocation;            // 입력 비디오 파일의 경로
        public string outputLocation;           // 출력 비디오 파일의 경로

        public string inputFileName;            // 입력 비디오 파일의 이름 (형식)
        public string outputFileName;           // 출력 비디오 파일의 이름

        public string width;                    // 비디오의 너비
        public string height;                   // 비디오의 높이

        public string framerate;                // 비디오의 프레임 레이트

        public FFMPEGCodecs codec;              // 비디오 코덱
        public FFMPEGSpeed speed;               // 비디오 인코딩 속도
        public FFMPEGPixelFormat pixelFormat;   // 비디오 픽셀 포맷

        public bool isOpenFolderWhenDone; // 작업 완료 후 출력 폴더를 열지 여부
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

    /// <summary>
    /// 픽셀 포맷
    /// </summary>
    public enum FFMPEGPixelFormat
    {
        yuv420p,
        yuv444p,
        yuva420p
    }
}