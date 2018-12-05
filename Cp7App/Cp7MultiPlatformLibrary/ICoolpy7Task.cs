using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Cp7MultiPlatformLibrary
{
    public interface ICoolpy7Task
    {
        List<string> GetWifiInfoTask();

        void CancelSmartConfigTask();

        Task<List<SmartConfigResult>> SetSmartConfigTask(string ssid, string bssid, string passphrase, bool isBroadcast, int mcucount);
    }

    public class SmartConfigResult
    {
        public bool IsSus { get; set; }
        public bool IsCancel { get; set; }
        public string InetAddress { get; set; }
    }
}
