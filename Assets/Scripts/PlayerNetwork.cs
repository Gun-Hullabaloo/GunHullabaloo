using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerNetwork : NetworkBehaviour
{
    private readonly NetworkVariable<PlayerNetworkData> _netState = new(writePerm: NetworkVariableWritePermission.Owner);
    private Vector3 _velocity;
    [SerializeField] private float _interpTime = 0.1f;
    void Update()
    {
        if (IsOwner)
        {
            _netState.Value = new PlayerNetworkData()
            {
                Position = transform.position,
            };
        }
        else
        {
            // transform.position = Vector3.SmoothDamp(transform.position, _netState.Value.Position, ref _velocity, _interpTime);
            transform.position = _netState.Value.Position;
        }
    }

    struct PlayerNetworkData : INetworkSerializable
    {
        private float _x, _y, _z;

        internal Vector3 Position
        {
            get => new Vector3(_x, _y, _z);
            set
            {
                _x = value.x;
                _y = value.y;
                _z = value.z;
            }
        }

        public void NetworkSerialize<T>(BufferSerializer<T> serializer) where T : IReaderWriter
        {
            serializer.SerializeValue(ref _x);
            serializer.SerializeValue(ref _y);
            serializer.SerializeValue(ref _z);
        }
    }
}
