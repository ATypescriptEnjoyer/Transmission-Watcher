namespace transmission_watcher
{
    class Program
    {
        static void Main(string[] args)
        {
            new TransmissionWatcher().Start().Wait(-1);
        }
    }
}
