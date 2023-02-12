using System;
using System.Collections.Generic;
using UnityEngine;

namespace BeatsBang.Core.Data
{
    [CreateAssetMenu(menuName = "Data/MapSelectionData", fileName = "MapSelectionData")]
    public class MapSelectionData : ScriptableObject
    {
        public List<MapInfo> Maps;
    }
}

[Serializable]
public struct MapInfo
{
    public Color MapThunbnail;
    // Image로 치환
    public string MapName;
    public string SongName;
    public string SceneName;
}