using Cp7MultiPlatformLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Cp7App.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SmarConfigPage : ContentPage
	{
        ICoolpy7Task EspTouch;
        bool bmode = true;
		public SmarConfigPage ()
		{
			InitializeComponent ();
            EspTouch = DependencyService.Get<ICoolpy7Task>();
            var ssids = EspTouch.GetWifiInfoTask();
            tb_ssid.Text = ssids[0];
            tb_bssid.Text = ssids[1];
            bt_start.Clicked += Bt_start_Clicked;
            bt_cancel.Clicked += Bt_cancel_Clicked;
            bt_refresh.Clicked += Bt_refresh_Clicked;
            pk_mode.SelectedIndexChanged += Pk_mode_SelectedIndexChanged;
		}

        private void Bt_refresh_Clicked(object sender, EventArgs e)
        {
            var ssids = EspTouch.GetWifiInfoTask();
            tb_ssid.Text = ssids[0];
            tb_bssid.Text = ssids[1];
        }

        private void Pk_mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            var picker = (Picker)sender;
            int selectedIndex = picker.SelectedIndex;
            if (selectedIndex != -1)
            {
                if (selectedIndex == 0)
                {
                    bmode = true;
                }
                else
                {
                    bmode = false;
                }
            }
        }

        private async void Bt_cancel_Clicked(object sender, EventArgs e)
        {
            if (ai.IsRunning)
            {
                await Task.Run(() => {
                    EspTouch.CancelSmartConfigTask();
                });
                ai.IsRunning = false;
            }
        }

        private async void Bt_start_Clicked(object sender, EventArgs e)
        {
            if (!ai.IsRunning)
            {
                ai.IsRunning = true;
                var ssids = EspTouch.GetWifiInfoTask();
                if (ssids.Count == 2)
                {
                    int dc =1;
                    if (!int.TryParse(et_dvcount.Text, out dc))
                    {
                        ai.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert("Coolpy7", "设备数量只允许填写数字", "OK");
                        return;
                    }
                    if (et_pwd.Text == null)
                    {
                        ai.IsRunning = false;
                        await Application.Current.MainPage.DisplayAlert("Coolpy7", "请填写当前Wifi密码", "OK");
                        return;
                    }

                    List<SmartConfigResult> res = await EspTouch.SetSmartConfigTask(ssids[0], ssids[1], et_pwd.Text, bmode, dc);
                    ai.IsRunning = false;

                }
            }
        }
    }
}