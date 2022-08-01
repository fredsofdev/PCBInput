using DBLib.Record;
using DBLib.Helper;

using System.Diagnostics;

namespace PCBInput.DataProvider
{
    public class OffDataProvider : RecordRepoBase<SendRecordRepository>, IDataProvider<DateTime>
    {
        private IDbFileManager _dbFileManager;

        public OffDataProvider(IDbFileManager dbFileManager)
        {
            _dbFileManager = dbFileManager;
        }

        public List<DateTime> GetData(DateTime now)
        {
            var offDates = new List<DateTime>();
            var lastDbDate = _dbFileManager.GetLastFile("DAY");
            //Trace.WriteLine(lastDbDate);
            if (Math.Abs((lastDbDate - DateTime.Now).TotalMinutes) < 5) return offDates;
            RenewUnitOfWork<SendDataContext>(lastDbDate);
            if(!work!.Repo.GetLastRecord().Any()) return offDates;
            var lastItem = work!.Repo.GetLastRecord().LastOrDefault();
            
            for(var date = lastItem!.Date.AddMinutes(5); date <= now; date = date.AddMinutes(5))
            {
                offDates.Add(date);
            }
            return offDates;
        }
    }
}
