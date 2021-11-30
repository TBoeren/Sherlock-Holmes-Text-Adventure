using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sherlock_Holmes_Text_Adventure
{
    internal interface IExternalFilesManager
    {
        DataTable ConvertCSVToDatatable(string FileName);
        string ReadTextFile(string FileName);
        string GetFileLocation(string FileName);
    }
}
