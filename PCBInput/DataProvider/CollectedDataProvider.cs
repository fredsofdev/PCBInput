using DBLib;
using DBLib.Record;
using DBLib.Record.Entities;
using PCBInput;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.DataProvider
{
    public class CollectedDataProvider : RecordRepoBase<ItemRecordRepository>, IDataProvider<Item>
    {

        public CollectedDataProvider(IUnitOfWork<ItemRecordRepository> work) =>
            this.work = work;

        public List<Item> GetData(DateTime date)
        {
            var dbFile = AppDomain.CurrentDomain.BaseDirectory + $"\\DATA\\"
                + $"DAT_{date.AddMinutes(-5).ToString("yyyyMMddHH")}.db";

            if (!File.Exists(dbFile)) return new();

            RenewUnitOfWork<RecordDataContext>(date.AddMinutes(-5));
            return work!.Repo.GetDurationalRecord(date);
        }
    }
}
