//+--------------------------------------------------------------------------
//
// NightDriver.Net - (c) 2019 Dave Plummer.  All Rights Reserved.
//
// File:        Program.cs
//
// Description:
//
//   A WinForms app that hosts a server that can be used to control LED strips
//   via a network connection. 
//
// History:     Dec-23-2023        Davepl      Created
//
//---------------------------------------------------------------------------

namespace NightDriver
{
    internal static class Program
    {
        [STAThread]
        static void Main()
        {
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}