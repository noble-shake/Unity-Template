using System;
using UnityEngine;

namespace RottenNoble.ScriptableObjects
{
    /// <summary>
    /// 씬 이름 표. 씬 이름을 코드에 문자열로 박지 않고 여기를 거친다 — 이름이 바뀌면 이 에셋만 고친다.
    /// 프로젝트의 실제 씬만큼 필드를 늘린다.
    /// </summary>
    [Serializable]
    [CreateAssetMenu(fileName = "ScenePathSO", menuName = "RottenNoble/ScriptableObjects/Cores - Scene Path", order = 2000)]
    public class ScenePathSO : ScriptableObject
    {
        [SerializeField] private string main;
        public string Main => main;
    }
}
