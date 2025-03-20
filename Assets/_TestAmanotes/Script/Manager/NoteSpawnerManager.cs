using System.Collections.Generic;
using _TestAmanotes.Script;
using TestAmanotes.Core;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestAmanotes
{
    public class NoteSpawnerManager : MonoSingleton<NoteSpawnerManager>
    {
        [SerializeField] private Tilemap _gameTileMap;

        [SerializeField] private NoteSpawnerDatabase _db;
        [SerializeField] private NoteDatabase _noteDb;

        [SerializeField] private bool _setupOnAwake;

        [SerializeField] private ObjectPool _notePools;
        private HashSet<Vector3> _cachedSpawnerPos;

        private bool _didSetup = false;

        protected override void Awake()
        {
            base.Awake();
            if (_setupOnAwake)
                Setup();
        }

        public void Setup()
        {
            if (_didSetup)
                return;
            SetupSpawnerPosition();
            _didSetup = true;
        }

        public Vector3 GetValidSpawnerPos(Define.NoteType type)
        {
            foreach (var pos in _cachedSpawnerPos)
            {
                return pos;
            }
            return Vector3.zero;
        }

        private bool IsPositionFitNote(Note note, Vector3 pos)
        {
            HashSet<Vector3> allPositionOfNote = note.GetAllPosition();
            
            return false;
        }
        private void SetupSpawnerPosition()
        {
             _cachedSpawnerPos = new();
            // Get bounds of the tilemap
            BoundsInt bounds = _gameTileMap.cellBounds;

            // Loop through all the cells in the tilemap
            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);
                    UnityEngine.Tilemaps.Tile tile = (UnityEngine.Tilemaps.Tile)_gameTileMap.GetTile(cellPosition);

                    if (tile == null)
                        continue;
                    if (_db.IsTileSpawner(tile))
                    {
                        _cachedSpawnerPos.Add(_gameTileMap.GetCellCenterWorld(cellPosition));
                        SpawnTileNote(tile, cellPosition);

                    }
                    else if (_db.IsTileNormalNote(tile))
                    {
                        SpawnTileNote(tile, cellPosition);
                    }
                }
            }
            
        }

        private void SpawnTileNote(Tile tile, Vector3Int cellPosition)
        {
            var noteObj = Instantiate(_noteDb.GetObjectById(Define.AssestId.TileNotePosition), _gameTileMap.transform, true);
            var worldPos = _gameTileMap.GetCellCenterWorld(cellPosition);
            noteObj.transform.position = worldPos;
            TileNote tileNoteBehavior = noteObj.GetComponent<TileNote>();
            tileNoteBehavior.Setup(cellPosition);
        }
        
        
        public void SpawnNote(Define.NoteType normal)
        {
            var pos = GetValidSpawnerPos(normal);
            if (pos == Vector3.zero)
            {
                Debug.LogError("No valid slot to spawn");
                return;
            }

            _notePools.SpawnFromPool(normal.ToString(), pos, Quaternion.identity);
        }

        public void DestroyNote(GameObject otherGameObject)
        {
            _notePools.DespawnToPool(otherGameObject.tag, otherGameObject);
        }
    }
}