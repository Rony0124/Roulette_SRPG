using Unity.Netcode;
using UnityEngine;

namespace HF.Utils
{
    public static class NetUtil
    {
        //Serialize an array using NetCode
        public static void NetSerializeArray<T, TS>(BufferSerializer<TS> serializer, ref T[] array) where T : INetworkSerializable, new() where TS : IReaderWriter
        {
            if (serializer.IsReader)
            {
                int size = 0;
                serializer.SerializeValue(ref size);
                array = new T[size];
                for(int i=0; i<size; i++)
                {
                    T val = new T();
                    serializer.SerializeValue(ref val);
                    array[i] = val;
                }
            }

            if (serializer.IsWriter)
            {
                int size = array.Length;
                serializer.SerializeValue(ref size);
                for (int i = 0; i < size; i++)
                    serializer.SerializeValue(ref array[i]);
            }
        }
    }
}
