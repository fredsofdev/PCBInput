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
                int rptState = GetMergedRptState(items.ToList());
                Item dominantItem = items.Where(item => item.RptState == rptState).First();
                dominantItem.RptState = rptState;
                ItemDetail itemDetail = GetSensors().Single(i => i.Id == dominantItem.ItemId);

                sendItems.Add(new SendItem()
                {
                    Date = dateStamp,
                    isSent = false,
                    ItemId = dominantItem.ItemId,
                    FacilityCode = dominantItem.FacilityCode,
                    ItemCode = dominantItem.ItemCode,
                    RptValue = dominantItem.RptValue,
                    RptState = dominantItem.RptState,
                    OprState = GetMergedOprState(itemDetail, dominantItem),
                    PFState = dominantItem.FacilityCode.Contains("E") ? 1 : 3,
                });
            }

            return GetPFStates(sendItems);
        }

        public static int GetMergedOprState(ItemDetail itemDetail, Item item)
        {
            if (item.RptState < 1 && item.RptValue >= itemDetail.DefaultValue)
                return 1;
            else return 0;
        }

        public static List<SendItem> GetPFStates(List<SendItem> itemsToSend)
        {
            itemsToSend.ForEach(item => {

                if (item.FacilityCode.Contains("E") && item.ItemCode == "A")
                {
                    var items = itemsToSend.Where(item => !item.FacilityCode.Contains("E") && item.ItemCode == "A")
                            .OrderBy(fac => fac.FacilityCode.Substring(0, 1));
                    SendItem? pfitem = null;
                    if (items.Any())
                    {
                        pfitem = items.FirstOrDefault();
                    }

                    item.PFState = 1;

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
                else item.PFState = 3;
            });
            
            return itemsToSend;
        }

        public static int GetMergedRptState(List<Item> items)
        {
            var rpt = items.GroupBy(item => item.RptState).ToList();
            int totalCount = items.Count();

            var listRpt = rpt.OrderByDescending(item => item.Key).ToList();

            var moreThan50 = rpt.Where(item => item.Count() > totalCount / 2).ToList();

            return moreThan50.Any() ? moreThan50.OrderByDescending(i => i.Key).First().Key : 
                listRpt.First().Key;
        }

        private List<ItemDetail> GetSensors() => _work.Repo.GetAll().ToList();
    }
}
