﻿using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using NeuralNetwork.Structure.Nodes;

namespace NeuralNetwork.Structure.Summators
{

    [DataContract]
    public class Summator : ISummator
    {

        public double GetSum(ISlaveNode node)
        {
            return node.Synapses.Sum(x => x.Output());
        }

        public async Task<double> GetSumAsync(ISlaveNode node)
        {
            var tasks = node.Synapses.Select(async x => await x.OutputAsync());
            return (await Task.WhenAll(tasks).ConfigureAwait(false)).Sum();
        }

    }
}