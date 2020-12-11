using Core.Utils;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Auth.Emu.Network
{
    public class Connection
    {
        private TcpServer _server;
        public async Task AsyncMain()
        {
            MessageTable.ScanAssembly();
            _server = new TcpServer("127.0.0.1", 8888);
            _server.Listen();
            var time = Stopwatch.StartNew();
            Logging.Info($"Application successfully started at {(time.ElapsedMilliseconds / 1000)} seconds");
            await Task.Delay(-1);
        }
    }
}