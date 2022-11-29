using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using Transmission.API.RPC;
using System.Collections.Generic;
using System.Threading;

namespace transmission_watcher
{
    public class TransmissionWatcher
    {
        private string[] fields = new string[] { "id", "isPrivate", "name", "status", "uploadRatio", "downloadDir", "peersConnected" };


        private Client TransmissionClient = null;

        public async Task Start()
        {
            Console.WriteLine("Starting Transmission Watcher...");
            var transmissionPath = Environment.GetEnvironmentVariable("TRANSMISSION_PATH");
            var url = $"http://{transmissionPath}";
            this.TransmissionClient = new Client(url);
            await CheckTorrents();
        }

        public async Task CheckTorrents()
        {
            var timer = new PeriodicTimer(TimeSpan.FromSeconds(60));

            while (await timer.WaitForNextTickAsync())
            {
                var torrents = await this.TransmissionClient.TorrentGetAsync(fields);
                Console.WriteLine($"Total Torrents: {torrents.Torrents.Length}");
                var toStopList = new List<object>();
                Double.TryParse(Environment.GetEnvironmentVariable("STOP_RATIO"), out double stopRatio);
                Boolean.TryParse(Environment.GetEnvironmentVariable("HELP_SOLO_PEERS"), out bool helpSoloPeers);
                foreach (var torrent in torrents.Torrents)
                {
                    if (torrent.Status == 6) //seeding
                    {
                        Console.WriteLine($"{torrent.Name} is finished, it's seed ratio is {torrent.uploadRatio}");
                        if (torrent.uploadRatio >= stopRatio && (!helpSoloPeers || torrent.PeersConnected > 2 || torrent.PeersConnected == 0))
                        {
                            Console.WriteLine($"Goodbye forever, {torrent.Name}. You won't be missed.");
                            toStopList.Add(torrent.ID);
                        }
                    }
                }
                if (toStopList.Count > 0)
                {
                    Console.WriteLine($"Stopping {toStopList.Count} torrents.");
                    this.TransmissionClient.TorrentStopAsync(toStopList.ToArray());
                }
                Console.WriteLine("Sleeping for 60 seconds.");
            }
        }
    }
}