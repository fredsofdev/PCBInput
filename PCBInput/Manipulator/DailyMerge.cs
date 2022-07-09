using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;

namespace PCBInput.Manipulator
{
    public class DailyMerge : IManipulator<SendItem, DayEndRecord>
    {
        public List<DayEndRecord> Manipulate(List<SendItem> data, DateTime date)
        {
            if(!data.Any()) return new List<DayEndRecord> {};

            var tdatCount = data.Where(i => i.isSent == true && i.RptState != 4).Count();
            var toffCount = data.Where(i => i.isSent == true && i.RptState == 4).Count();

            var totalDataCount = data.GroupBy(i => i.Date).Count();
            var groupedItems = data.GroupBy(i => i.ItemId);

            List<DayEndRecord> records = new List<DayEndRecord>();

            foreach (var group in groupedItems)
            {
                records.Add(new DayEndRecord()
                {
                    Date = group.Last().Date,
                    DataCount = totalDataCount,
                    TDATCount = tdatCount,
                    TOFFCount = toffCount,
                    ItemCount = groupedItems.Count(),
                    FacCode = group.Last().FacilityCode,
                    ItemCode = group.Last().ItemCode!,
                    NormalCount = group.Where(i => i.RptState == 0).Count(),
                    AbnormalCount = group.Where(i => i.RptState == 1).Count(),
                    ConnectionCount = group.Where(i => i.RptState == 2).Count(),
                    PowerOffCount = group.Where(i => i.RptState == 4).Count(),
                    ExaminCount = group.Where(i => i.RptState == 8).Count(),
                });
            }
            return records;
        }
    }
}
