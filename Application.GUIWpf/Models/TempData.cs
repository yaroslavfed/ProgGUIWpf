using Common.Base.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.GUIWpf.Models
{
    internal static class TempData
    {
        public static List<List<Data>> datas = new List<List<Data>>();
        //{
        //    new List<Data> { new Data(1,1), new Data(1.8, 5), new Data(3, 3) },
        //    new List<Data> { new Data(1,3), new Data(2.2, 2), new Data(3, 0) },
        //    new List<Data> { new Data(1,4), new Data(2, 3), new Data(3, 4) },
        //};

        public static void CreateTempData(IList<IReaderSupport> lists)
        {
            datas = new();

            for (int i = 0; i < lists.Count; i++)
            {
                datas.Add(new List<Data>());
                for (int j = 0; j < lists[i].LocationsList.Count; j++)
                {
                    datas[i].Add(new(lists[i].LocationsList[j].PointX, lists[i].LocationsList[j].PointY));
                }
            }
        }
    }
}