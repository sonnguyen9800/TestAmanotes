using System;
using System.Collections;
using UnityCommunity.UnitySingleton;
using UnityEngine;

namespace TestAmanotes
{
    public class GameManager : MonoSingleton<GameManager>
    {    [SerializeField] private bool _runInFixedTime;

        [SerializeField] GameConfigSO _gameConfig;
        private void Start()
        {
            DataHubManager.Instance.Setup();
            NoteSpawnerManager.Instance.Setup();
            SongManager.Instance.Setup();
        }
        
        public int CalculateScore(Vector3 tapPos, float time)
        {
            int BonusScore = 0;
            if (time < _gameConfig.TimeBonus)
            {
                BonusScore = _gameConfig.BonusScore;
            }
            var distance = Math.Abs(tapPos.y -
                NoteSpawnerManager.Instance.TapLineTransform.position.y);
            Debug.Log("Tap Pos:" + tapPos + " Tap Line Pos:" + NoteSpawnerManager.Instance.TapLineTransform.position + " Distance:" + distance);
            if (distance < _gameConfig.ThresholdPerfect)
                return _gameConfig.PerfectScore + BonusScore;
            if (distance < _gameConfig.ThresholdNormal)
                return _gameConfig.NormalScore + BonusScore;
            return _gameConfig.BadScore + BonusScore;
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
            YourFunction();
        }

        private void YourFunction()
        {
            SongManager.Instance.StopSong();
            NoteSpawnerManager.Instance.StopSpawn();
        }
    }
}