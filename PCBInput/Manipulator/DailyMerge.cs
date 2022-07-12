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

            var groupedItems = data.GroupBy(i => i.ItemId);

            List<DayEndRecord> records = new List<DayEndRecord>();

            foreach (var group in groupedItems)
            {
                records.Add(new DayEndRecord()
                {
                    Date = group.Last().Date,
                    DataCount = data.GroupBy(i => i.Date).Count(),
                    TDATCount = data.Where(i => i.RptState != 4).Count(),
                    TOFFCount = data.Where(i => i.RptState == 4).Count(),
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
