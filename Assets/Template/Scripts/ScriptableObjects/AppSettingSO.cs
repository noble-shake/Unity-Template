using System;
using UnityEngine;

using RottenNoble.Cores.Enum;

namespace RottenNoble.ScriptableObjects
{
    [Serializable]
    [CreateAssetMenu(fileName = "AppSettingSO", menuName = "RottenNoble/ScriptableObjects/Cores - App Setting", order = 2000)]
    public class AppSettingSO : ScriptableObject
    {
        private readonly string Tag = $"[{nameof(AppSettingSO)}]";

        [Header("Scene Management")]
        [SerializeField] private string startingSceneName;
        public string StartingSceneName => startingSceneName;

        [Header("Mobile Graphics")]
        [SerializeField] private int targetFrameRate = 60;
        public int TargetFrameRate => targetFrameRate;

        [SerializeField] private int vSyncCount = 0;
        public int VSyncCount => vSyncCount;

        [Range(0.5f, 2.0f)]
        [SerializeField] private float defaultRenderScale = 1.0f;
        public float DefaultRenderScale => defaultRenderScale;

        [Header("Resolution")]
        [SerializeField] private int targetWidth = 1080;
        public int TargetWidth => targetWidth;

        [SerializeField] private int targetHeight = 1920;
        public int TargetHeight => targetHeight;

        [Header("Sound")]
        [Range(0f, 1f)]
        [SerializeField] private float defaultBGMVolume = 0.7f;
        public float DefaultBGMVolume => defaultBGMVolume;

        [Range(0f, 1f)]
        [SerializeField] private float defaultSFXVolume = 1.0f;
        public float DefaultSFXVolume => defaultSFXVolume;

        [Header("Network")]
        [Tooltip("기본값은 Offline. 서버가 준비된 프로젝트에서만 Online으로 올린다.")]
        [SerializeField] private NetworkMode networkMode = NetworkMode.Offline;
        public NetworkMode NetworkMode => networkMode;

        [Header("Addressables CDN")]
        [SerializeField] private string addressablesCDNUrl = "";
        public string AddressablesCDNUrl => addressablesCDNUrl;

        [Header("Debug")]
        [SerializeField] private bool enableLogs = true;
        public bool EnableLogs => enableLogs;

        public void ApplyAppSettings()
        {
            QualitySettings.vSyncCount = vSyncCount;
            Application.targetFrameRate = targetFrameRate;

            Screen.orientation = ScreenOrientation.Portrait;
            Screen.SetResolution(targetWidth, targetHeight, FullScreenMode.FullScreenWindow);

            RNDebug.Log(Tag + " App Setting Applied");
        }
    }
}
