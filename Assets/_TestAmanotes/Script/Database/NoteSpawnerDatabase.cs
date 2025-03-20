using System;
using _TestAmanotes.Script;
using SisyphusFramework.ScriptableObject;
using SisyphusFramework.Utils;
using UnityEngine;

namespace TestAmanotes
{
    [Serializable]
    public class NoteSpawnerData
    {

        public UnityEngine.Tilemaps.Tile TileRef;
        
    }
    [Serializable]

    public class NoteSpawnerItem : ADataItem<NoteSpawnerData>
    {
        
    }
    [CreateAssetMenu(menuName = "Note Spawner DB")]
    public class NoteSpawnerDatabase : AScriptableDatabase<NoteSpawnerItem, NoteSpawnerData>
    {

        public bool IsTileSpawner(UnityEngine.Tilemaps.Tile tile)
        {
            foreach (var data in _data)
            {
                if (data.Data.TileRef== null)
                    continue;
                if (data.Id != Define.AssestId.TileSpawnerId)
                    continue;
                if (data.Data.TileRef == tile)
                    return true;
            }
            return false;
        }
        public bool IsTileNormalNote(UnityEngine.Tilemaps.Tile tile)
        {
            foreach (var data in _data)
            {
                if (data.Data.TileRef== null)
                    continue;
                if (data.Id != Define.AssestId.TileNotePosition)
                    continue;
                if (data.Data.TileRef == tile)
                    return true;
            }
            return false;
        }
        
    }
}