using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Common.Base.Interfaces;

namespace Common.Base.Converters
{
    public static class FileToFormat
    {
        public static string ToCsvFormat(this ICoordinatesCollection data)
        {
            return $"{data.PointX},{data.PointY}";
        }
    }
}
