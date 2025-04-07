using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Collections.Specialized;
using MarmotAp.Helpers;
// So we can call UI thread

namespace MarmotAp.Pages;

public partial class TestPage : ContentPage
{
    private String _ID = "";
    const int dataSize = 36;
    const int notifyCallBackMax = 20;  // Max Bytes from registered notiify callback
    private byte[] data = new byte[dataSize];
    public TestPage()
    {
        InitializeComponent();
        _ID = Preferences.Get("@string/DevKey", "");
        PageTitle.Text = "Test Page";
    }
    protected override void OnAppearing()                                               // When the page is called set button and icon in navbar. 
    {
        base.OnAppearing();

    //    try
    //    {
    //        if (App.g_Device == null)
    //        {
    //            if (App.Current.RequestedTheme == OSAppTheme.Dark)
    //            {
    //                NavBarImage.Source = "BT_Not_connected_Dark.png";

    //            }
    //            else
    //            {
    //                NavBarImage.Source = "BT_Not_connected_Light.png";

    //            }
    //            btnConnect.Text = "Connect";
    //        }
    //        else
    //        {
    //            if (App.Current.RequestedTheme == OSAppTheme.Dark)
    //            {
    //                NavBarImage.Source = "BT_connected_Dark.png";

    //            }
    //            else
    //            {
    //                NavBarImage.Source = "BT_connected_Light.png";

    //            }
    //            btnConnect.Text = "Connected";
    //            _ID = Preferences.Get("@string/DevKey", "");
    //        }
    //    }
    //    catch
    //    {
    //        DisplayAlert("Error", "OnAppearing Issue", "Cancel");
    //    }
    }
    private void WriteSignature_Clicked(object sender, EventArgs e)
    {
        if (App.g_Service == null)
        {
            DisplayAlert("Not connected", "Connect to device first.", "Cancel");
            return;
        }
        WriteTheSignature();
    }

    private async void WriteTheSignature()
    {
        var nameValue = CommandTxt.Text;
        var cmdHeader = nameValue;

        if (!String.IsNullOrEmpty(nameValue))
        {
            if (nameValue.ToString().Contains("Anteater"))
            {
                nameValue = nameValue.Replace("Anteater", "");
                nameValue = nameValue.TrimEnd('_');
            }
            else
            {
                nameValue = nameValue + "_Anteater";
            }

        }
        if (!nameValue.Contains("_Anteater"))
        {
            nameValue = nameValue + "_Anteater";
        }

        if (App.g_Service == null)
        {
            await DisplayAlert("Not connected", "Cannot save to device, please connect and resave.", "Cancel");
            return;
        }
        // Only one '_' allowed
        if (FindDuplicateChar.repeatcount(nameValue, '_') > 1)
        {
            await DisplayAlert("Error", "Invalid character in name, character '_' not allowed.", "Cancel");
            return;
        }
        Preferences.Set("@string/DevKey", (string)nameValue);
        Preferences.Set("@string/CmdHeader", (string)cmdHeader.Replace("_Anteater", "_"));
        CommandRecTxt.Text = (string)nameValue;
        //currentname.Text = (string)nameValue;
        WriteSignature();
        
    }
    private async void WriteSignature()
    {
        try
        {
            string sdev = Preferences.Get("@string/CmdHeader", "");
            byte[] array = Encoding.UTF8.GetBytes("dev!:" + sdev);                               // Set the new unique device name in the ESP32
            await App.g_Characteristic_2.WriteAsync(array);                                                 // Set the device signature (DevName)
            //Thread.Sleep(50);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Send Error", ex.Message, "Cancel");
        }
    }
    private void ReadSignature_Clicked(object sender, EventArgs e)
    {
        if (App.g_Service == null)
        {
            DisplayAlert("Not connected", "Connect to device first", "Cancel");
            return;
        }
        ReadSignature();
    }

    private async void ReadSignature()
    {
        try
        {
            string s = Preferences.Get("@string/CmdHeader", "") + "dev?:";
            byte[] array = Encoding.UTF8.GetBytes(s);
            await App.g_Characteristic_2.WriteAsync(array);                                                 // Query the device signature (DevName)
            //Thread.Sleep(50);
            var receivedBytes = await App.g_Characteristic_2.ReadAsync();
            _ID =  Encoding.UTF8.GetString(receivedBytes.data, 0, receivedBytes.data.Length);
            CommandRecTxt.Text = _ID;
        }
        catch (Exception ex)
        {
            await DisplayAlert("Get Error", ex.Message, "Cancel");
        }
    }

    private void WriteCmd_Clicked(object sender, EventArgs e)
    {
        if (App.g_Service == null)
        {
            DisplayAlert("Not connected", "Connect to device first", "Cancel");
            return;
        }
        WriteCmd();
    }
    private async void WriteCmd()
    {
        try
        {
            string s = Preferences.Get("@string/CmdHeader", "") + CommandTxt.Text;
            byte[] array = Encoding.UTF8.GetBytes(s);                                         // Get command from UI
            
            await App.g_Characteristic_2.WriteAsync(array);                                                 // Send to Device
            Thread.Sleep(50);

            var receivedBytes = await App.g_Characteristic_2.ReadAsync();

            string sval = Encoding.UTF8.GetString(receivedBytes.data, 0, receivedBytes.data.Length);
            CommandRecTxt.Text = sval;

        }
        catch (Exception ex)
        {
            await DisplayAlert("WriteCmd Error", ex.Message, "Cancel");
        }
    }

    private void ReadCmd_Clicked(Object sender, EventArgs e)
    {
        if (App.g_Service == null)
        {
            DisplayAlert("Not connected", "Connect to device first", "Cancel");
            return;
        }
        ReadCmd();
    }

    private async void ReadCmd()
    {
        try
        {
            Thread.Sleep(50);
            var receivedBytes = await App.g_Characteristic_2.ReadAsync();
            string sval = Encoding.UTF8.GetString(receivedBytes.data, 0, receivedBytes.data.Length);
            CommandRecTxt.Text = sval;
        }
        catch (Exception ex)
        {
            await DisplayAlert("ReadCmd Error", ex.Message, "Cancel");
        }
    }

    private void RegisterCallback_Clicked(object sender, EventArgs e)
    {
        if (App.g_Service == null)
        {
            DisplayAlert("Not connected", "Connect to device first", "Cancel");
            return;
        }
        RegisterCallback();
    }

    private async void RegisterCallback()
    {
        try
        {
            if (App.g_Characteristic_1 != null)                                                                      // make sure the characteristic exists
            {
                if (App.g_Characteristic_1.CanUpdate)                                                                // check if characteristic supports notify
                {
                    bool bHeader = false;
                    App.g_Characteristic_1.ValueUpdated += (o, args) =>                                              // define a callback function
                    {
                        var receivedBytes = args.Characteristic.Value;                              // read in received bytes
                        //Console.WriteLine("byte array: " + BitConverter.ToString(receivedBytes));   // write to the console for debugging


                        string _charStr = "";                                                                           // in the following section the received bytes will be displayed in different ways (you can select the method you need)
                        if (receivedBytes != null)
                        {
                            // For DEBUG
                            //_charStr = "Bytes: " + BitConverter.ToString(receivedBytes);                                // by directly converting the bytes to strings we see the bytes themselves as they are received
                            //_charStr += " | UTF8: " + Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);  // This code interprets the bytes received as ASCII characters
                            string s = Preferences.Get("@string/CmdHeader", "");
                            _charStr = Encoding.UTF8.GetString(receivedBytes, 0, receivedBytes.Length);
                            if (_charStr == s && bHeader == false)
                            {
                                bHeader = true;
                            }
                        }

                        if ((receivedBytes[0] == 0x0A) && bHeader)
                        {
                            // Save first 20 bytes
                            receivedBytes.CopyTo(data, 0);
                        }
                        else
                        {
                            // Combine next 16 bytes with previously saved 20 bytes data for the full 36 bytes
                            if (data[0] == 0x0A)
                            {
                                receivedBytes.CopyTo(data, notifyCallBackMax);
                                // Parse into a Packet10hz
                                Packet10hz p = new Packet10hz();
                                p = Parse.ToPacket10hz(data);

                                // as this is a callback function, the "MainThread" needs to be invoked to update the GUI
                                //XamarinEssentials.MainThread.BeginInvokeOnMainThread(() =>                                      
                                //{
                                //    //Output.Text += _charStr;
                                //    MPH.Text = p.mph.ToString();
                                //});
                                Array.Clear(data, 0, dataSize);
                            }
                        }

                        // FFR
                        // If only 4 or less bytes were received than it could be that an INT was sent.
                        // The code here combines the 4 bytes back to an INT
                        //if (receivedBytes.Length <= 4)
                        //{                                                                                               
                        //    int char_val = 0;
                        //    for (int i = 0; i < receivedBytes.Length; i++)
                        //    {
                        //        char_val |= (receivedBytes[i] << i * 8);
                        //    }
                        //    _charStr += " | int: " + char_val.ToString();
                        //}
                        //_charStr += Environment.NewLine;                                                                // the NewLine command is added to go to the next line



                    };
                    await App.g_Characteristic_1.StartUpdatesAsync();

                    ErrorLabel.Text = GetTimeNow() + ": Notify callback function registered successfully.";
                }
                else
                {
                    ErrorLabel.Text = GetTimeNow() + ": Characteristic does not have a notify function.";
                }
            }
            else
            {
                ErrorLabel.Text = GetTimeNow() + ": No characteristic selected.";
            }
        }
        catch
        {
            ErrorLabel.Text = GetTimeNow() + ": Error initializing UART GATT service.";
        }
    }

    private string GetTimeNow()
    {
        var timestamp = DateTime.Now;
        return timestamp.Hour.ToString() + ":" + timestamp.Minute.ToString() + ":" + timestamp.Second.ToString();
    }

    private async void BtnConnect_Clicked(object sender, EventArgs e)
    {
        if (App.g_Service != null)
        {
            await DisplayAlert("Already connected", "Good to go", "Cancel");
            return;
        }
        
        await Shell.Current.GoToAsync("//HomePage");
    }
}