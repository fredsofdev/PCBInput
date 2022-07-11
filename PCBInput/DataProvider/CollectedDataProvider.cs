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
            RenewUnitOfWork<RecordDataContext>(date);
            return work!.Repo.GetDurationalRecord(date);
        }
    }
}
