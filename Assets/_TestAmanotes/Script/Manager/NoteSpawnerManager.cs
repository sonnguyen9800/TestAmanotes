using System;
using System.Collections.Generic;
using System.Linq;
using _TestAmanotes.Script;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using TestAmanotes.Core;
using Unity.VisualScripting;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestAmanotes
{
    public class NoteSpawnerManager : MonoSingleton<NoteSpawnerManager>
    {
        public Melanchall.DryWetMidi.MusicTheory.NoteName noteRestriction;

        public List<double> timeStamps = new List<double>();

        [SerializeField] private Transform _gridTransform = null;
        [SerializeField] private Tilemap _gameTileMap;

        [SerializeField] private NoteSpawnerDatabase _db;
        [SerializeField] private NoteDatabase _noteDb;

        [SerializeField] private bool _setupOnAwake;

        [SerializeField] private ObjectPool _notePools;
        [SerializeField] private NoteTapLine _tapLine;
        
        public Transform TapLineTransform {get => _tapLine.transform;}

        [SerializeField]
        public List<Vector3> SpawnerPos => _cachedSpawnerPos.ToList();

        private HashSet<Vector3> _cachedSpawnerPos = new();

        private bool _didSetup = false;
        private bool _start = false;

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
        
        public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array)
        {
            foreach (var note in array)
            {
                var metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                timeStamps.Add((double)metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds + (double)metricTimeSpan.Milliseconds / 1000f);
                //
                // if (note.NoteName == noteRestriction)
                // {
                //
                // }
         
            }
        }
        public void SpawnNote(Define.NoteType normal)
        {
            var pos = GetValidSpawnerPos(normal);
            if (pos == Vector3.zero)
            {
                Debug.LogError("No valid slot to spawn");
                return;
            }

            _notePools.SpawnFromPool(normal.ToString(), pos, _gridTransform, Quaternion.identity);
        }

        public void DestroyNote(GameObject otherGameObject)
        {
            _notePools.DespawnToPool(otherGameObject.tag, otherGameObject);
        }
        int spawnIndex = 0;

        private void Update()
        {
            if (!_start)
                return;
            if (spawnIndex < timeStamps.Count)
            {
                if (SongManager.GetAudioSourceTime() >= timeStamps[spawnIndex] - SongManager.Instance.noteTime)
                {
                    SpawnNote(Define.NoteType.Normal);
                    spawnIndex++;
                }
            }
        }

        public void StartSpawn()
        {
            _start = true;
        }

        public void StopSpawn()
        {
            _start = false;
        }
    }
}