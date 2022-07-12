using DBLib;
using DBLib.Record.Entities;
using DBLib.Setup;
using DBLib.Setup.Entities;
using Moq;
using PCBInput.Manipulator;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace PCBInput.Test
{
    public class CollectionMergeTest
    {
        [Fact]
        public void TestMergeRptState50percent()
        {
            List<Item> list = new List<Item>();
            foreach (int i in Enumerable.Range(0, 31))
            {
                list.Add(new Item() {RptState = 0});
            }
            foreach (int i in Enumerable.Range(0, 29))
            {
                list.Add(new Item() { RptState = 4});
            }

            Assert.Equal(0, CollectionMerge.GetMergedRptState(list));
        }
        [Fact]
        public void TestMergeRptStateOrderAdvantage()
        {
            List<Item> list = new List<Item>();
            foreach (int i in Enumerable.Range(0, 30))
            {
                list.Add(new Item() { RptState = 0 });
            }
            foreach (int i in Enumerable.Range(0, 30))
            {
                list.Add(new Item() { RptState = 1 });
            }

            Assert.Equal(1, CollectionMerge.GetMergedRptState(list));
        }
        [Fact]
        public void TestMergeRptState50Advantage()
        {
            List<Item> list = new List<Item>();
            foreach (int _ in Enumerable.Range(0, 20))
            {
                list.Add(new Item() { RptState = 0 });
            }
            foreach (int _ in Enumerable.Range(0, 30))
            {
                list.Add(new Item() { RptState = 4 });
            }
            foreach (int _ in Enumerable.Range(0, 10))
            {
                list.Add(new Item() { RptState = 8 });
            }

            Assert.Equal(8, CollectionMerge.GetMergedRptState(list));
        }
        [Fact]
        public void TestMergeRptState3OderAdvantage()
        {
            List<Item> list = new List<Item>();
            foreach (int _ in Enumerable.Range(0, 20))
            {
                list.Add(new Item() { RptState = 0 });
            }
            foreach (int _ in Enumerable.Range(0, 10))
            {
                list.Add(new Item() { RptState = 4 });
            }
            foreach (int _ in Enumerable.Range(0, 30))
            {
                list.Add(new Item() { RptState = 1 });
            }

            Assert.Equal(4, CollectionMerge.GetMergedRptState(list));
        }
        [Fact]
        public void TestMergeRptState3OderAdvantage2()
        {
            List<Item> list = new List<Item>();
            foreach (int _ in Enumerable.Range(0, 29))
            {
                list.Add(new Item() { RptState = 0 });
            }
            foreach (int _ in Enumerable.Range(0, 2))
            {
                list.Add(new Item() { RptState = 2 });
            }
            foreach (int _ in Enumerable.Range(0, 29))
            {
                list.Add(new Item() { RptState = 1 });
            }

            Assert.Equal(2, CollectionMerge.GetMergedRptState(list));
        }


        [Fact]
        public void TestMergeOperatingNormal()
        {
            var item = new Item() 
            {
                ItemCode = "A",
                RptState = 0,
                RptValue = 4,
            };
            var itemDetail = new ItemDetail()
            {
                DefaultValue = 3
            };

            Assert.Equal(1, CollectionMerge.GetMergedOprState(itemDetail, item));
        }
        [Fact]
        public void TestMergeOperatingAbnormal()
        {
            var item = new Item()
            {
                ItemCode = "A",
                RptState = 0,
                RptValue = 2,
            };
            var itemDetail = new ItemDetail()
            {
                DefaultValue = 3
            };

            Assert.Equal(0, CollectionMerge.GetMergedOprState(itemDetail, item));
        }
        [Fact]
        public void TestMergeOperatingAbnormal2()
        {
            var item = new Item()
            {
                ItemCode = "A",
                RptState = 1,
                RptValue = 2,
            };
            var itemDetail = new ItemDetail()
            {
                DefaultValue = 3
            };

            Assert.Equal(0, CollectionMerge.GetMergedOprState(itemDetail, item));
        }


        [Fact]
        public void TestMergePFStatesNotApplied()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemCode = "A",
                    FacilityCode = "P0201",
                    RptState = 1,
                    OprState = 2,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem);

            Assert.Equal(3, send.First().PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal1()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 0,
                    OprState = 1,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    RptState = 0,
                    OprState = 1,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t=>t.ItemId == 1);
            
            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal2()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 0,
                    OprState = 0,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal3()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 1,
                    OprState = 0,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    OprState = 1,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal4()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 1,
                    OprState = 0,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    RptState = 1,
                    OprState = 0,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal5()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 1,
                    OprState = 0,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    RptState = 0,
                    OprState = 0,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesNormal6()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 0,
                    OprState = 1,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    RptState = 1,
                    OprState = 0,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(1, send.PFState);
        }
        [Fact]
        public void TestMergePFStatesAbnormal()
        {
            var sendItem = new List<SendItem>()
            {
                new SendItem()
                {
                    ItemId = 1,
                    ItemCode = "A",
                    FacilityCode = "E0201",
                    RptState = 0,
                    OprState = 1,
                },
                new SendItem()
                {
                    ItemId = 2,
                    ItemCode = "A",
                    FacilityCode = "F0201",
                    RptState = 0,
                    OprState = 0,
                },
            };

            var send = CollectionMerge.GetPFStates(sendItem).Single(t => t.ItemId == 1);

            Assert.Equal(0, send.PFState);
        }

        
    }
}
