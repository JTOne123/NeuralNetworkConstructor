﻿using NeuralNetworkConstructor.Network.Layer;
using NeuralNetworkConstructor.Network.Node;
using NeuralNetworkConstructor.Network.Node.Synapse;
using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;

namespace NeuralNetworkConstructor.Network
{
    public class KohonenNetwork : Network
    {

        /// <summary>
        /// Create Kohonen network
        /// </summary>
        /// <remarks>
        /// Network can contain only two layers - input and output(one-layer-network in classic view)
        /// </remarks>
        /// <param name="inputLayer"></param>
        /// <param name="outputLayer"></param>
        public KohonenNetwork(ILayer inputLayer, ILayer outputLayer)
            : base(new List<ILayer> { inputLayer, outputLayer})
        {
            Contract.Requires(inputLayer != null, nameof(inputLayer));
            Contract.Requires(outputLayer != null, nameof(outputLayer));
        }

        /// <summary>
        /// Class for learning network
        /// </summary>
        public class SelfLearning
        {

            internal KohonenNetwork _network;

            public SelfLearning(KohonenNetwork network)
            {
                Contract.Requires(network != null, nameof(network));

                _network = network;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input">Input data for learning</param>
            /// <param name="force">Force of learning</param>
            public void Learn(ICollection<double> input, double force)
            {
                Contract.Requires(force > 0, nameof(force));
                Contract.Requires(force <= 1, nameof(force));

                _network.Input(input);
                var output = _network.Output();

                var winnerIndex = Array.IndexOf(output.ToArray(), output.Max());
                var winner = _network.Layers.Last().Nodes[winnerIndex];

                foreach (var synapse in (winner as ISlaveNode).Synapses)
                {
                    synapse.ChangeWeight(force * (synapse.MasterNode.Output() - synapse.Weight));
                }
            }

        }

        /// <summary>
        /// Class for self-organizing of network
        /// </summary>
        public class SelfOrganizing
        {

            private readonly SelfLearning _learning;
            private readonly double _criticalRange;

            /// <summary>
            /// 
            /// </summary>
            /// <param name="learning"></param>
            /// <param name="criticalRange">Critical range for decide to start training or add a new neuron</param>
            public SelfOrganizing(SelfLearning learning, double criticalRange)
            {
                Contract.Requires(learning != null, nameof(learning));
                Contract.Requires(criticalRange > 0, nameof(criticalRange));

                _learning = learning;
                _criticalRange = criticalRange;
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="input">Input data for learning</param>
            /// <param name="creator">
            /// Callback for creation new node if Euclid distance more then critical range <see cref="_criticalRange"/>
            /// Note: if distance less then critical range - neural network will be trained
            /// </param>
            /// <param name="force">Force of learning</param>
            public void Organize(ICollection<double> input, Func<ISlaveNode> creator, double force)
            {
                Contract.Requires(creator != null, nameof(creator));

                var outputLayerNodes = _learning._network.Layers.Last().Nodes;
                foreach (var neuron in outputLayerNodes)
                {
                    var euclidRange = GetEuclidRange(input.ToArray(), (neuron as ISlaveNode).Synapses);
                    if(euclidRange < _criticalRange)
                    {
                        _learning.Learn(input, force);
                        return;
                    }
                }

                var networkLayers = _learning._network.Layers;
                var newNode = creator();
                networkLayers.Last().Nodes.Add(newNode);
                _learning._network.Input(input); //write input data to nodes for read in loop
                foreach (var inputNode in networkLayers.First().Nodes)
                {
                    newNode.AddSynapse(new Synapse(inputNode, inputNode.Output()));
                }
            }

            private static double GetEuclidRange(double[] input, ICollection<ISynapse> synapses)
            {
                double subSqrt = 0;
                var index = 0;

                foreach (var synapse in synapses)
                {
                    subSqrt += Math.Pow(input[index++] - synapse.Weight, 2);
                }

                return Math.Sqrt(subSqrt);
            }

        }

    }
}
