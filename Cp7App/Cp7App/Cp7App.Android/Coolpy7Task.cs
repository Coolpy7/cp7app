using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Net.Wifi;
using Android.Widget;
using Com.Espressif.Iot.Esptouch;
using Cp7MultiPlatformLibrary;

[assembly: Xamarin.Forms.Dependency(typeof(Cp7App.Droid.Coolpy7Task))]
namespace Cp7App.Droid
{
    public class Coolpy7Task: ICoolpy7Task
    {
        static EsptouchTask mEsptouchTask;

        public List<string> GetWifiInfoTask()  
        {
            var list = new List<string>();
            WifiManager wifiManager = (WifiManager)(Application.Context.GetSystemService(Context.WifiService));
            if (wifiManager == null)
            {
                list.Add("get wifi info errors");
                return list;
            }
            list.Add(wifiManager.ConnectionInfo.SSID.Replace("\"",""));
            list.Add(wifiManager.ConnectionInfo.BSSID);
            return  list;
        }

        public async Task<List<SmartConfigResult>> SetSmartConfigTask(string ssid, string bssid, string passphrase, bool isBroadcast, int mcucount)
        {
            mEsptouchTask = new EsptouchTask(ssid, bssid, passphrase, Application.Context);
            mEsptouchTask.SetPackageBroadcast(isBroadcast);
            var res = new List<SmartConfigResult>();
            await Task.Run(() => {
                var results = mEsptouchTask.ExecuteForResults(mcucount);
                for (int i = 0; i < results.Count; i++)
                {
                    var scr = new SmartConfigResult();
                    scr.IsSus = results[i].IsSuc;
                    scr.IsCancel = results[i].IsCancelled;
                    if (results[i].InetAddress != null)
                    {
                        scr.InetAddress = results[i].InetAddress.HostAddress;
                    }
                    res.Add(scr);
                }
                return res;
            });
            return res;
        }

        public async void CancelSmartConfigTask()
        {
            if (mEsptouchTask != null)
            {
                await Task.Run(() => {
                    mEsptouchTask.Interrupt();
                });
            }
        }
    }
}