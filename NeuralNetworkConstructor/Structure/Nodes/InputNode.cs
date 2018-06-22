﻿using System;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace NeuralNetwork.Structure.Nodes
{

    [DataContract]
    public class InputNode : IInputNode
    {

        [DataMember]
        private double _data;

        public event Action<double> OnOutput;
        public event Action<double> OnInput;

        public void Input(double input)
        {
            OnInput?.Invoke(input);
            _data = input;
        }

        public double Output()
        {
            OnOutput?.Invoke(_data);
            return _data;
        }

        public async Task<double> OutputAsync()
        {
            return await Task.Run(() =>
            {
                OnOutput?.Invoke(_data);
                return _data;
            }).ConfigureAwait(false);
        }
    }
}