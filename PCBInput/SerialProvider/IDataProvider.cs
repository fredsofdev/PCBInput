using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PCBInput.SerialProvider
{
    public interface IDataProvider<T>
    {
        List<T> GetData(DateTime date);
    }
}
