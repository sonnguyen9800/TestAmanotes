using System;
using SisyphusFramework.ScriptableObject;
using SisyphusFramework.Utils;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace _TestAmanotes
{
    [Serializable]
    public class NoteSpawnerData
    {

        public Tile TileRef;
        
    }
    [Serializable]

    public class NoteSpawnerItem : ADataItem<NoteSpawnerData>
    {
        
    }
    [CreateAssetMenu(menuName = "Note Spawner DB")]
    public class NoteSpawnerDatabase : AScriptableDatabase<NoteSpawnerItem, NoteSpawnerData>
    {
        
    }
}