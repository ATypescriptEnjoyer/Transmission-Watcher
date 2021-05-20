using System;
using System.IO;
using System.Threading.Tasks;
using System.Linq;
using Newtonsoft.Json;
using transmission_watcher.Models;
using Transmission.API.RPC;
using System.Collections.Generic;

namespace transmission_watcher
{
    public class TransmissionWatcher
    {
        private string[] fields = new string[] { "id", "isPrivate", "name", "status", "uploadRatio", "downloadDir", "peersConnected" };


        private Client TransmissionClient = null;

        public async Task Start()
        {
            Console.WriteLine("Starting Transmission Watcher...");
            var config = await File.ReadAllTextAsync("./config.json");
            ConfigModel configJson = JsonConvert.DeserializeObject<ConfigModel>(config);
            var url = $"http://{configJson.transmission.hostname}:{configJson.transmission.port}{configJson.transmission.path}";
            this.TransmissionClient = new Client(url);
            await CheckTorrents();
        }

        public async Task CheckTorrents()
        {
            while (true)
            {
                var torrents = await this.TransmissionClient.TorrentGetAsync(fields);
                var privateTorrents = torrents.Torrents.Where(x => x.IsPrivate).ToArray();
                var publicTorrents = torrents.Torrents.Where(x => !x.IsPrivate).ToArray();
                Console.WriteLine($"Total Torrents: {torrents.Torrents.Length}");
                Console.WriteLine($"Private Torrents: {privateTorrents.Length}");
                Console.WriteLine($"Public Torrents: {publicTorrents.Length}");
                var toStopList = new List<object>();
                foreach (var torrent in publicTorrents)
                {
                    if (torrent.Status == 6) //seeding
                    {
                        Console.WriteLine($"Public torrent: {torrent.Name} is finished, it's seed ratio is {torrent.uploadRatio}");
                        if (torrent.uploadRatio >= 1.3 && (torrent.PeersConnected > 2 || torrent.PeersConnected == 0))
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
                await Task.Delay(60 * 1000); // every 60 seconds
            }
        }
    }
}