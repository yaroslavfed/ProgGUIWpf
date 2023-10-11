using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Base.Converters
{
    public static class FileNameCutter
    {
        public static string ToShortFileName(this string fileName)
        {
            var newLineSplit = fileName.Split('\\');

            return newLineSplit.LastOrDefault()!.Trim();
        }
    }
}
