using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using NModbus;
using MetoLibrary.Utilities;



namespace MetoLibrary.Utilities.Logger
{
    public class PlcService
    {
        private readonly string _ipAddress;
        private readonly int _port;

        public PlcService(string ipAddress, int port)
        {
            _ipAddress = ipAddress;
            _port = port;
        }

        public async Task<bool> WriteRegisterAsync(ushort address, ushort value)
        {
            using (TcpClient client = new TcpClient(_ipAddress, _port))
            {
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);

                try
                {
                    await Task.Run(() => master.WriteSingleRegister(1, address, value));
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.Log(ex); // Logger.Log metodunu kullanın
                    return false;
                }
            }
        }

        public async Task<ushort> ReadRegisterAsync(ushort address)
        {
            using (TcpClient client = new TcpClient(_ipAddress, _port))
            {
                var factory = new ModbusFactory();
                var master = factory.CreateMaster(client);

                try
                {
                    ushort[] result = await Task.Run(() => master.ReadHoldingRegisters(1, address, 1));
                    return result[0];
                }
                catch (Exception ex)
                {
                    Logger.Log(ex); // Logger.Log metodunu kullanın
                    throw;
                }
            }
        }
    }
}
