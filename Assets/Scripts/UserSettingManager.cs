using System.IO;
using UnityEngine;

namespace Sunyunie.FFMPEGNyaa
{
    public static class UserSettingManager
    {
        private static readonly string savePath = Path.Combine(Application.persistentDataPath, "usersetting.json");

        public static void Save(UserSetting setting)
        {
            string json = JsonUtility.ToJson(setting, true);
            File.WriteAllText(savePath, json);
            Debug.Log("설정을 저장했다냥: " + savePath);
        }

        public static UserSetting Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                return JsonUtility.FromJson<UserSetting>(json);
            }
            else
            {
                Debug.LogWarning("저장된 설정이 없어서 기본값으로 초기화한다냥~");
                return new UserSetting(); // 또는 기본값을 직접 지정해도 된다냥!
            }
        }
    }
}