using System;
using UnityCommunity.UnitySingleton;

namespace TestAmanotes
{
    public class GameManager : MonoSingleton<GameManager>
    {
        private void Start()
        {
            DataHubManager.Instance.Setup();
            NoteSpawnerManager.Instance.Setup();
        }
    }
}