using System;
using System.Collections.Generic;
using System.Linq;
using _TestAmanotes.Script;
using Melanchall.DryWetMidi.Interaction;
using Melanchall.DryWetMidi.MusicTheory;
using NTC.Pool;
using TestAmanotes.Core;
using UnityCommunity.UnitySingleton;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace TestAmanotes
{
    public class NoteSpawnerManager : MonoSingleton<NoteSpawnerManager>
    {
        [SerializeField] private GameConfigSO _configSO = null;
        public Dictionary<NoteName,List<double>> timeStamps = new();

        private HashSet<double> _cachedTimeStamp = new();
        [SerializeField] private Transform _gridTransform = null;
        [SerializeField] private Tilemap _gameTileMap;

        [SerializeField] private NoteSpawnerDatabase _db;
        [SerializeField] private NoteDatabase _noteDb;

        [SerializeField] private bool _setupOnAwake;

        [SerializeField] private ObjectPool _notePools;
        [SerializeField] private NoteTapLine _tapLine;
        
        HashSet<NoteName> _noteNames = new HashSet<NoteName>();
        public Transform TapLineTransform {get => _tapLine.transform;}

        [SerializeField]
        public List<Vector3> SpawnerPos => _cachedSpawnerPos;

        private List<Vector3> _cachedSpawnerPos = new();
        Dictionary<NoteName, Vector3> _noteLine = new();
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
            _spawnedIndex = new();
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
        
        public void SetTimeStamps(Melanchall.DryWetMidi.Interaction.Note[] array, List<NoteName> topFour)
        {
            _noteNames = new HashSet<NoteName>(topFour);
            int i = 0;
            foreach (var note in _noteNames)
            {
                timeStamps[note] = new List<double>();
                _spawnedIndex[note] = 0;
                _noteLine[note] = _cachedSpawnerPos[i];
                i++;
            }

            double time;
            TimeSpan metricTimeSpan;
            foreach (var note in array)
            {
                if (!_noteNames.Contains(note.NoteName))
                    continue;
                if (note.Length < _configSO.NoteLongFilter)
                    continue;
                metricTimeSpan = TimeConverter.ConvertTo<MetricTimeSpan>(note.Time, SongManager.midiFile.GetTempoMap());
                time = metricTimeSpan.Minutes * 60f + metricTimeSpan.Seconds +
                       (double)metricTimeSpan.Milliseconds / 1000f;
                if (_cachedTimeStamp.Contains(time))
                    continue;
                timeStamps[note.NoteName].Add(time);
                _cachedTimeStamp.Add(time);
                
            }
        }
        public void SpawnNote(Define.NoteType normal, NoteName name)
        {
            var pos = _noteLine[name];
            if (pos == Vector3.zero)
            {
                Debug.LogError("No valid slot to spawn");
                return;
            }

            var prefab = _notePools.GetPrefabByTag(normal.ToString());
            //Instantiate(prefab, pos, Quaternion.identity, _gridTransform);
            var go = NightPool.Spawn(prefab);
            go.transform.position = pos;
            go.transform.SetParent(_gridTransform);
        }

        public void DestroyNote(GameObject otherGameObject)
        {
            NightPool.Despawn(otherGameObject);
            
            //_notePools.DespawnToPool(otherGameObject.tag, otherGameObject);
        }

        private Dictionary<NoteName, int> _spawnedIndex= new();
        int spawnIndex = 0;

        private void Update()
        {
            if (!_start)
                return;
            foreach (var iter in timeStamps)
            {
                if (_spawnedIndex[iter.Key] < iter.Value.Count)
                {
                    if (SongManager.GetAudioSourceTime() >= iter.Value[_spawnedIndex[iter.Key]] - SongManager.Instance.noteTime)
                    {
                        SpawnNote(Define.NoteType.Normal, iter.Key);
                        _spawnedIndex[iter.Key]++;
                    }
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
            _spawnedIndex.Clear();
            foreach (var name in _noteNames)
            {
                _spawnedIndex[name] = 0;
            }
        }

        public void SpawnNoteDefault(Define.NoteType large)
        {
            var note = _noteNames.First();
            SpawnNote(large, note);
        }
    }
}