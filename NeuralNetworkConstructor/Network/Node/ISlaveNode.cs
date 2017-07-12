﻿using NeuralNetworkConstructor.Network.Node.Synapse;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Network.Node
{

    /// <summary>
    /// Node that has synapses
    /// </summary>
    public interface ISlaveNode : INode
    {

        /// <summary>
        /// Collection of synapses to this node
        /// </summary>
        ICollection<ISynapse> Synapses { get; }

        /// <summary>
        /// Adding synapse from master node to this node
        /// </summary>
        /// <param name="synapse"></param>
        void AddSynapse(ISynapse synapse);

    }
}