using PCBInput.Manipulator;
using PCBInput.DataProvider;
using PCBInput.Helper;
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

        public async void Execute(DateTime dateTime)
        {
            await Task.Run(() => Executer(dateTime));
        }

        public Task Executer(DateTime date)
        {
            List<T> rawData = _dataProvider.GetData(date);
            if (!rawData.Any()) return Task.CompletedTask;
            List<V> saveItem = _manipulator.Manipulate(rawData, date);
            _writer.Write(saveItem, date);
            return Task.CompletedTask;
        }
    }
}
