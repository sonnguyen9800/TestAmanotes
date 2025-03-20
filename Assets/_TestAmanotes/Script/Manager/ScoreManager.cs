using UnityCommunity.UnitySingleton;
using UnityEngine;

namespace TestAmanotes
{
    public class ScoreManager : MonoSingleton<ScoreManager>
    {
        public void AddScore(int score)
        {
            Debug.Log("Add score");
        }
    }
}