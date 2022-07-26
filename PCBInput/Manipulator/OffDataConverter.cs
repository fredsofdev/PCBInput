using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.Manipulator
{
    public class OffDataConverter : IManipulator<DateTime, IGrouping<DateTime, SendItem>>
    {
        private readonly IUnitOfWork<ItemDetailRepository> _work;

        public OffDataConverter(IUnitOfWork<ItemDetailRepository> work)
        {
            _work = work;
        }

        public List<IGrouping<DateTime, SendItem>> Manipulate(List<DateTime> data, DateTime date)
        {
            var offItems = new List<SendItem>();
            if (!data.Any()) return offItems.GroupBy(i =>i.Date.Date).ToList();
            var itemDetails = _work.Repo.GetAll().ToList();
            foreach(var dataDate in data)
            {
                var sendItems = itemDetails.ConvertAll(i => new SendItem()
                {
                    Date = dataDate,
                    isSent = false,
                    ItemId = i.Id,
                    FacilityCode = i.FullFacilityCode,
                    ItemCode = i.ItemCode,
                    RptValue = 0,
                    RptState = 4,
                    OprState = 0,
                    PFState = 3,
                });
                offItems.AddRange(sendItems);
            }
            var grouped = offItems.GroupBy(i => i.Date.Date).ToList();
            return grouped;
        }
    }
}
