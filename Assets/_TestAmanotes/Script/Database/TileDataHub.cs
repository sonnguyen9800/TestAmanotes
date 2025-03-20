using SisyphusFramework.Data;
using UnityEngine;

namespace TestAmanotes
{
    public struct DataItem : IPlayData<byte>
    {
        public byte Id;
        public Vector3Int Position;
        public bool IsPlaced;
        public byte GetId()
        {
            return Id;
        }

        public void SetId(byte id)
        {
            Id = id;
        }

        public byte this[byte index]
        {
            get => Id;
            set => Id = index;
        }
    }
    public class TileDataHub : ASmallDataHub<DataItem>, IVisualizer
    {
        public override void Init()
        {
            
        }

        public override void OnEditedSuccess(byte id, DataItem oldData, DataItem newData)
        {
        }

        public override void OnDeletedSuccess(byte id)
        {
        }

        public override void OnAddedSuccess(byte id, DataItem newData)
        {
        }
    }
}