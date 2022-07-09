using PCBInput.Manipulator;
using PCBInput.SerialProvider;
using PCBInput.TimeCaller;
using PCBInput.Writer;

namespace PCBInput
{
    public class TimedTask<T, V>
    {
        private readonly IDataProvider<T> _dataProvider;
        private readonly IManipulator<T, V> _manipulator;
        private readonly IDataWriter<V> _writer;

        public TimedTask(IDataProvider<T> dataProvider, 
            IManipulator<T, V> manipulator, 
            IDataWriter<V> writer)
        {
            _dataProvider = dataProvider;
            _manipulator = manipulator;
            _writer = writer;
        }

        public async void Execute(TimeScheduleEventArgs arg)
        {
            await Task.Run(() => executer(arg.DateTime));
        }

        private Task executer(DateTime date)
        {
            List<T> rawData = _dataProvider.GetData(date);
            List<V> saveItem = _manipulator.Manipulate(rawData, date);
            _writer.Write(saveItem, date);
            return Task.CompletedTask;
        }
    }
}
