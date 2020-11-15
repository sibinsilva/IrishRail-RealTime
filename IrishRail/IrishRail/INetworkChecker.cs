using System;
using System.Collections.Generic;
using System.Text;

namespace IrishRail
{
    public interface INetworkChecker
    {
        bool IsConnected { get; }
        void CheckInternetConnection();
    }
}
