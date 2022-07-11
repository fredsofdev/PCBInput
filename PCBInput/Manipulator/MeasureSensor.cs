using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;


namespace PCBInput.Manipulator
{
    public class MeasureSensor : IManipulator<int, Item>
    {
        private readonly IUnitOfWork<ItemDetailRepository> _work;

        public MeasureSensor(IUnitOfWork<ItemDetailRepository> unitOfWork)
        {
            _work = unitOfWork;
        }

        public List<Item> Manipulate(List<int> data, DateTime date)
        {
            if(!data.Any()) return new List<Item> {};

            var sensors = GetSensors();

            sensors = isInspected(sensors);

            var items = sensors.ConvertAll(sensor => 
                ConvertRawToModel(sensor, data[sensor.InputCh - 1], date));

            return items;
        }


        private List<ItemDetail> isInspected(List<ItemDetail> data)
        {
            var fanItem = data.Where(x => x.ItemCode == "A" && x.Facility.FacCode != "E")
                        .OrderBy(x => x.Facility.FacCode);
            if(fanItem.Any() && fanItem.First().IsInspection)
            {
                data.ForEach(c => {
                    if (c.Facility.FacCode != "E") c.IsInspection = true;
                });
            }
            
            return data;
        }

        private List<ItemDetail> GetSensors() => _work.Repo.GetAll().ToList();

        private Item ConvertRawToModel(ItemDetail itemDetail, int rawInput, DateTime date)
        {
            Item item = new Item()
            {
                Date = date,
                ItemId = itemDetail.Id,
                FacilityCode = itemDetail.Facility.FacilityCode + itemDetail.FacilityNum.ToString().PadLeft(2, '0'),
                ItemCode = itemDetail.ItemCode,
                RptValue = itemDetail.ItemType.min,
                RptState = 0,
            };

            double ManResult = 0.00;
            if (rawInput < 800)
            {
                item.RptState = 2; // connection fail 통신불량
            }
            else
            {
                ManResult = MeasureRawData(itemDetail.ItemType.max, itemDetail.ItemType.min, rawInput);
                if (ManResult <= itemDetail.ItemMaxRange && ManResult >= itemDetail.ItemMinRange)
                {
                    item.RptState = 0; // normal 정상
                }
                else
                {
                    item.RptState = 1; // abnormal 비정상
                }
            }

            if (itemDetail.IsInspection)
            {
                item.RptState = 8; // inspection 점검중
            }

            if (itemDetail.ItemType.code != "T")
            {
                item.RptValue = ManResult > 0 ? Math.Round(ManResult, 2) : 0.00;
            }
            else
            {
                item.RptValue = Math.Round(ManResult, 2);
            }

            return item;
        }

        private double MeasureRawData(int Pmax, int Pmin, int currentRaw)
        {
            double PmaxR = 4095;
            double PminR = 818;

            return (Pmax - Pmin) / (PmaxR - PminR) * (currentRaw - PminR) + Pmin;
        }
    }
}
