//+--------------------------------------------------------------------------
//
// NightDriver.Net - (c) 2019 Dave Plummer.  All Rights Reserved.
//
// File:        LERServer.cs
//
// Description:
//
//   A server object that can load, start, and stop the LightStrips 
//
// History:     Dec-23-2023        Davepl      Created
//
//---------------------------------------------------------------------------

namespace NightDriver
{
    class LEDServer
    {
        private bool _bRunning = false;

        public Statistics Stats = new Statistics();

        // Main
        //
        // Application main loop - starts worker threads

        public List<Site> AllLocations = new List<Site>
        {
            new Tree()              { FramesPerSecond = 28 },

            new CeilingStrip        { FramesPerSecond = 30 },

            new Cabana()            { FramesPerSecond = 20 },

            new TV()                { FramesPerSecond = 20 },
            new ShopCupboards()     { FramesPerSecond = 20 },

            new ShopSouthWindows1() { FramesPerSecond = 2 },
            new ShopSouthWindows2() { FramesPerSecond = 2 },
            new ShopSouthWindows3() { FramesPerSecond = 2 },
        };

        public List<LightStrip> AllStrips = new List<LightStrip>();

        internal void LoadStrips()
        {
            AllStrips.Clear();
            foreach (var location in AllLocations)
            {
                foreach (var strip in location.LightStrips)
                {
                    strip.StripSite = location;
                    AllStrips.Add(strip);
                }
            }
        }

        private void StartCommunications()
        {
            foreach (var location in AllLocations)
                foreach (var strip in location.LightStrips)
                    strip.Start();
        }

        private void StopCommunications()
        {
            foreach (var location in AllLocations)
                foreach (var strip in location.LightStrips)
                    strip.Stop();
        }


        internal void Start(CancellationToken token)
        {
            _bRunning = true;
            foreach (var location in AllLocations)
                location.StartWorkerThread(token);
            StartCommunications();
        }

        internal void Stop(CancellationTokenSource cancelSource)
        {
            cancelSource.Cancel();
            StopCommunications();
            _bRunning = false;
        }

        public void RemoveStrip(LightStrip strip)
        {
            strip.Stop();
            var siteContainingStrip = AllLocations.FirstOrDefault(site => site.LightStrips.Contains(strip));
            siteContainingStrip.LightStrips.Remove(strip);
            AllStrips.Remove(strip);
        }

   }
}
