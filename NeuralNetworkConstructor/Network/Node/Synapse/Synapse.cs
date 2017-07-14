﻿using System;
using System.Linq;
using NeuralNetworkConstructor.Network.Layer;
using System.Diagnostics.Contracts;

namespace NeuralNetworkConstructor.Network.Node.Synapse
{

    /// <summary>
    /// Synapse gets output from neuron-transmitter and convert the value via its weight.
    /// Result value gets neuron-reciever.
    /// </summary>
    public class Synapse : ISynapse
    {

        /// <summary>
        /// Node transmitter
        /// </summary>
        public INode MasterNode { get; }

        /// <summary>
        /// Current weight of synapse
        /// </summary>
        public double Weight { get; private set; }

        private static readonly Random Random = new Random();
        /// <summary>
        /// Random value from -1.0 to 1.0
        /// </summary>
        private static double RandomWeight => (Random.NextDouble() - 0.5) * 2;

        /// <summary>
        /// Change value of weight
        /// </summary>
        /// <param name="delta">Delta value for change</param>
        public void ChangeWeight(double delta)
        {
            Weight += delta;
        }

        /// <summary>
        /// Calculate output data from master node via weight
        /// </summary>
        /// <returns></returns>
        public double Output() => Weight * MasterNode.Output();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="masterNode">Node-transmitter</param>
        /// <param name="weight">Initial weight. Random [-1, 1] if null</param>
        public Synapse(INode masterNode, double? weight = null)
        {
            Contract.Requires(masterNode != null, nameof(masterNode));

            MasterNode = masterNode;
            Weight = weight ?? RandomWeight;
        }

        /// <summary>
        /// Helper for easy generating
        /// </summary>
        public static class Generator
        {

            /// <summary>
            /// Generates synapses each to each nodes between layers 
            /// </summary>
            /// <param name="master"></param>
            /// <param name="slave"></param>
            public static void EachToEach(ILayer master, ILayer slave, Func<double> weightGenerator = null)
            {
                foreach (var mNode in master.Nodes)
                {
                    foreach (var sNode in slave.Nodes.Where(n => n is Neuron))
                    {
                        var weight = weightGenerator?.Invoke();
                        var synapse = new Synapse(mNode, weight);
                        (sNode as Neuron)?.AddSynapse(synapse);
                    }
                }
            }

        }

    }
}
