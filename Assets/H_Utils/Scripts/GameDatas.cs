using UnityEngine;

namespace H_Utils
{
    public class ConstDatas
    {
        public const string CURRENT_LEVEL = "CURRENT_LEVEL";

        public const string PLAYER_PREFS_SOUND = "PLAYER_PREFS_SOUND";
        public const string PLAYER_PREFS_MUSIC = "PLAYER_PREFS_MUSIC";
        public const string PLAYER_PREFS_VIBRATION = "PLAYER_PREFS_VIBRATION";
    }

    public static class GameDatas
    {
        public static int CurrentLevel
        {
            get => PlayerPrefs.GetInt(ConstDatas.CURRENT_LEVEL, 0);
            set => PlayerPrefs.SetInt(ConstDatas.CURRENT_LEVEL, value);
        }

        public static bool IsSoundOn
        {
            get => PlayerPrefs.GetInt(ConstDatas.PLAYER_PREFS_SOUND, 1) == 1;
            set => PlayerPrefs.SetInt(ConstDatas.PLAYER_PREFS_SOUND, value ? 1 : 0);
        }   

        public static bool IsMusicOn
        {
            get => PlayerPrefs.GetInt(ConstDatas.PLAYER_PREFS_MUSIC, 1) == 1;
            set => PlayerPrefs.SetInt(ConstDatas.PLAYER_PREFS_MUSIC, value ? 1 : 0);
        }

        public static bool IsVibrationOn
        {
            get => PlayerPrefs.GetInt(ConstDatas.PLAYER_PREFS_VIBRATION, 1) == 1;
            set => PlayerPrefs.SetInt(ConstDatas.PLAYER_PREFS_VIBRATION, value ? 1 : 0);
        }
    }
}
