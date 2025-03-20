using SisyphusFramework.Data;
using UnityCommunity.UnitySingleton;

namespace TestAmanotes
{
    public class DataHubManager : MonoSingleton<DataHubManager>
    {
        private TileDataHub _hubTile;
        public TileDataHub TileDataHub
        {
            get => _hubTile;
        }

        public void Setup()
        {
            _hubTile = new TileDataHub();
            VisualizerRegistry.Register(_hubTile);
        }
    }
}