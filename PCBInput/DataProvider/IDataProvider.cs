using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.DataProvider
{
    public interface IDataProvider<T>
    {
        List<T> GetData(DateTime date);
    }
}
