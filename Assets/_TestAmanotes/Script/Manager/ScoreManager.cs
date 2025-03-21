using TMPro;
using UnityCommunity.UnitySingleton;
using UnityEngine;

namespace TestAmanotes
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        [SerializeField] private TextMeshProUGUI _scoreCurrent;
        [SerializeField] private AudioSource _hitSource;
        
        private float _score = 0;
        public enum ScoreType
        {
            None,
            Bad,
            Good,
            Perfect
        }

        protected override void Awake()
        {
            base.Awake();
            _scoreCurrent.text = "Score: " +_score;
        }

        public void AddScore(int score)
        {
            Debug.Log("Add score " + score);
            _score += score;
            _scoreCurrent.text = "Score: " +_score;
            _hitSource.Play();
        }
    }
}