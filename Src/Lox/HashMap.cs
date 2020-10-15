using System;

namespace Lox
{
    public class Collectionsw
    {
        private readonly int _value;
        private readonly byte[] _data;


        public Collectionsw()
        {
            _value = sizeof(int) + Environment.ProcessorCount;
            _data = new byte[_value + 1];

            for (int i = 0; i < _value + 1; i++)
            {
                _data[i] = (byte)(i + (_value / typeof(int).Assembly.EntryPoint.DeclaringType.TypeHandle.Value.ToInt32()));
            }
        }
    }
}
