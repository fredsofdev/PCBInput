using EasyModbus;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.SerialProvider
{
    public class SerialModbusProvider : IDataProvider<int>, IDisposable
    {
        private ModbusClient? _modbusClient;

        private bool disposed = false;

        public SerialModbusProvider(int port)
        {
            _modbusClient = new ModbusClient($"COM{port}");
            _modbusClient.UnitIdentifier = 1;
            _modbusClient.Baudrate = 9600;
            _modbusClient.Parity = Parity.None;
            _modbusClient.StopBits = StopBits.One;
            _modbusClient!.Connect();
        }

        private int GetTargetPort()
        {
            //TODO search target serial port
            return 3;
        }

        public void PowerUpSensors()
        {
            try
            {
                _modbusClient!.WriteSingleRegister(500, 5000);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }

        public void Disconnect()
        {
            if (_modbusClient!.Connected) _modbusClient!.Disconnect();
        }

        public List<int> GetData(DateTime date)
        {
            try
            {
                var raw = _modbusClient!.ReadInputRegisters(0, 8);
                return raw.ToList();
            }
            catch { return new List<int> {0,0,0,0,0,0,0,0}; }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing && _modbusClient!.Connected)
                {
                    _modbusClient.Disconnect();
                    _modbusClient = null;
                }

                disposed = true;
            }
        }
    }
}
