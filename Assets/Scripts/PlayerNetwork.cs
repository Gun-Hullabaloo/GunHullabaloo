using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private LifeContainer Life;
    void Start()
    {
        Life = gameObject.GetComponent<LifeContainer>();
    }
    void Update()
    {
        if (IsOwner)
        {
            _netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
                Direction = transform.localScale,
                Life = Life.life,
            };
        }
        else
        {
            transform.position = _netState.Value.Position;
            transform.localScale = _netState.Value.Direction;
            Life.life = _netState.Value.Life;
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _PosX, _PosY, _PosZ;
        private float _ScaleX, _ScaleY, _ScaleZ;
        public int Life;


        internal Vector3 Position
        {
            get => new Vector3(_PosX, _PosY, _PosZ);
            set
            {
                _PosX = value.x;
                _PosY = value.y;
                _PosZ = value.z;
            }
        }

        internal Vector3 Direction
        {
            get => new Vector3(_ScaleX, _ScaleY, _ScaleZ);
            set
            {
                _ScaleX = value.x;
                _ScaleY = value.y;
                _ScaleZ = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _PosX);
            serializer.SerializeValue(ref _PosY);
            serializer.SerializeValue(ref _PosZ);

            serializer.SerializeValue(ref _ScaleX);
            serializer.SerializeValue(ref _ScaleY);
            serializer.SerializeValue(ref _ScaleZ);

            serializer.SerializeValue(ref Life);
        }
    }
}
