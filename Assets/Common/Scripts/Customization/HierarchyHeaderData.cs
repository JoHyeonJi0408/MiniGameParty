using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "HierarchyHeaderData", menuName = "Custom Hierarchy/Header Data")]
public class HierarchyHeaderData : ScriptableObject
{
    [System.Serializable]
    public struct HeaderInfo
    {
        public HEADER_TYPE type;
        public string text;
        public Color color;
    }

    public List<HeaderInfo> HeaderInfos = new();
}

public enum HEADER_TYPE
{
    ENVIRONMENT,
    GAMEMANAGER,
    GAMEOBJECTS,
    UI
}