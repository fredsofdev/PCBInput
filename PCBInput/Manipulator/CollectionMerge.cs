using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;

namespace PCBInput.Manipulator
{
    public class CollectionMerge : IManipulator<Item, SendItem>
    {
        private readonly IUnitOfWork<ItemDetailRepository> _work;

        public CollectionMerge(IUnitOfWork<ItemDetailRepository> work)
        {
            _work = work;
        }

        public List<SendItem> Manipulate(List<Item> data, DateTime date)
        {
            if(!data.Any()) return new List<SendItem>{};

            var dateStamp = date.AddMinutes(-5);
            var sendItems = new List<SendItem>();

            foreach(var items in data.GroupBy(c => c.ItemId))
            {
                int rptState = GetMergedRptState(items);
                Item dominantItem = items.Where(item => item.RptState == rptState).First();

                sendItems.Add(new SendItem()
                {
                    Date = dateStamp,
                    isSent = false,
                    ItemId = dominantItem.ItemId,
                    FacilityCode = dominantItem.FacilityCode,
                    ItemCode = dominantItem.ItemCode,
                    RptValue = dominantItem.RptValue,
                    RptState = rptState,
                    OprState = 0,
                    PFState = dominantItem.FacilityCode.Contains("E") ? 1 : 3,
                });
            }

            return GetOprPFStates(sendItems);
        }

        private List<SendItem> GetOprPFStates(List<SendItem> itemsToSend)
        {
            itemsToSend.ForEach(item => {
                var defaultValue = GetSensors().Single(i=>i.Id == item.ItemId).DefaultValue;
                if (item.RptState < 1 && item.RptValue > defaultValue)
                    item.OprState = 1;

                if (item.FacilityCode.Contains("E") && item.ItemCode == "A")
                {
                    var pfitem = itemsToSend.Where(item => item.ItemCode == "A" && !item.FacilityCode.Contains("E"))
                            .OrderBy(fac => fac.FacilityCode.Substring(0, 1))
                            .FirstOrDefault();
                    if ((item.RptState == 0 && item.OprState == 1) && (pfitem!.RptState == 0 && pfitem.OprState == 0))
                    {
                        item.PFState = 0; //비정상
                    }
                    else if ((item.RptState > 0 && item.OprState == 0) && (pfitem!.RptState == 0 && pfitem.OprState == 0))
                    {
                        // TODO Alert 정상1
                    }
                    else if ((item.RptState == 0 && item.OprState == 1) && (pfitem!.RptState > 0 && pfitem.OprState == 0))
                    {
                        // TODO Alert 정상2
                    }
                }
            });
            
            return itemsToSend;
        }

        private int GetMergedRptState(IGrouping<int, Item> items)
        {
            var rpt = items.GroupBy(item => item.RptState).ToList();
            int totalCount = items.Count();

            var listRpt = rpt.OrderByDescending(item => item.Key).ToList();

            var moreThan50 = rpt.Where(item => item.Count() > totalCount / 2).ToList();

            return moreThan50.Count() > 0 ? moreThan50.OrderByDescending(i => i.Key).ToList()[0].Key : listRpt[0].Key;
        }

        private List<ItemDetail> GetSensors() => _work.Repo.GetAll().ToList();
    }
}
