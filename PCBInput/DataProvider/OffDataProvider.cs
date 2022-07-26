using DBLib.Record;
using DBLib.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            if (Math.Abs((lastDbDate - DateTime.Now).TotalMinutes) < 5) return offDates;
            RenewUnitOfWork<SendDataContext>(lastDbDate);
            var lastItem = work!.Repo.GetLastRecord().FirstOrDefault();
            for(var date = lastItem.Date.AddMinutes(5); date <= now; date = date.AddMinutes(5))
            {
                offDates.Add(date);
            }
            return offDates;
        }
    }
}
