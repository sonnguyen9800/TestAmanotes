using System;
using System.Collections;
using NTC.Pool;
using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;

namespace TestAmanotes
{
    public class GameManager : MonoSingleton<GameManager>
    {    [SerializeField] private bool _runInFixedTime;
        [SerializeField] private ObjectPool _hitTextPool = null;
        [SerializeField] GameConfigSO _gameConfig;
        [SerializeField] private Canvas _canvasUI;
        [SerializeField] private Transform _testObjSpawnTxt;

        [SerializeField] private bool _playAuto;
        private void Start()
        {
            DataHubManager.Instance.Setup();
            NoteSpawnerManager.Instance.Setup();
            SongManager.Instance.Setup(() =>
            {
                if (_playAuto)
                {
                    StartCoroutine(StartGameAfterDelay());
                }
            });

        }

        IEnumerator StartGameAfterDelay()
        {
            yield return new WaitForSeconds(2f);
            StartGame();
        }
        
        public (int, ScoreManager.ScoreType) CalculateScore(Vector3 tapPos, float time)
        {
            int BonusScore = 0;
            if (time < _gameConfig.TimeBonus)
            {
                BonusScore = _gameConfig.BonusScore;
            }
            var distance = Math.Abs(tapPos.y -
                NoteSpawnerManager.Instance.TapLineTransform.position.y);
            // Debug.Log("Tap Pos:" + tapPos + " Tap Line Pos:" + NoteSpawnerManager.Instance.TapLineTransform.position + " Distance:" + distance);
            if (distance < _gameConfig.ThresholdPerfect)
                return (_gameConfig.PerfectScore + BonusScore, ScoreManager.ScoreType.Perfect);
            if (distance < _gameConfig.ThresholdNormal)
                return (_gameConfig.NormalScore + BonusScore, ScoreManager.ScoreType.Good);
            return (_gameConfig.BadScore + BonusScore, ScoreManager.ScoreType.Bad);
        }

        public void StartGame()
        {
            SongManager.Instance.PlayGame();
            NoteSpawnerManager.Instance.StartSpawn();
            if (_runInFixedTime)
                StartCoroutine(WaitAndCallFunction());
        }
        
        private IEnumerator WaitAndCallFunction()
        {
            yield return new WaitForSeconds(_gameConfig.TimeSongPlayed);
            // Call your function here
            StopAuto();
        }

        private void StopAuto()
        {
            SongManager.Instance.StopSong();
            NoteSpawnerManager.Instance.StopSpawn();
        }
        public RectTransform canvasTransform; // Assign the canvas RectTransform
        public Camera mainCamera; // Assign the main camera

        private Vector2 ConvertWorldToCanvasPosition(Vector3 worldPosition)
        {
            Vector3 screenPosition = mainCamera.WorldToScreenPoint(worldPosition);
            // Step 2: Convert screen position to UI (anchoredPosition)
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _canvasUI.transform as RectTransform, screenPosition, GetComponent<Camera>(), out Vector2 uiPosition);
            return uiPosition;
        }
        public GameObject SpawnText(ScoreManager.ScoreType type, Vector3 worldPos, bool autoDespawn = true)
        {
            var canvasPos = ConvertWorldToCanvasPosition(worldPos);
            var text = _hitTextPool.GetPrefabByTag(type.ToString());
            var obj = NightPool.Spawn(text);
            obj.transform.SetParent(_canvasUI.transform);
            obj.transform.localPosition = canvasPos;
            
            obj.GetComponent<TextMeshProUGUI>();
            
            if (autoDespawn)
                NightPool.Despawn(obj, 1.3f);
            return obj;

        }
        public void SpawnTextTest()
        { 
            SpawnText(ScoreManager.ScoreType.Perfect, _testObjSpawnTxt.position, true);
        }

        public void SpawnTextMousePos(ScoreManager.ScoreType type, Vector3 mousePos, bool autoDespawn = true)
        {
            var text = _hitTextPool.GetPrefabByTag(type.ToString());
            var obj = NightPool.Spawn(text, mousePos, Quaternion.identity, _canvasUI.transform);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasUI.transform as RectTransform,
                mousePos, mainCamera, out Vector2 anchoredPos);
            
            var tm = obj.GetComponent<RectTransform>();
            tm.anchoredPosition = anchoredPos;
            if (autoDespawn)
                NightPool.Despawn(obj, 1.3f);
            
        }
    }
}