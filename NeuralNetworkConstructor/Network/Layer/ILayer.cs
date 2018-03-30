﻿using NeuralNetworkConstructor.Common;
using NeuralNetworkConstructor.Network.Node;
using System.Collections.Generic;

namespace NeuralNetworkConstructor.Network.Layer
{

    public interface ILayer<TNode> : IRefreshable
        where TNode : INode
    {

        IList<TNode> Nodes { get; }

    }

}