using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    class FileController
    {
        public List<rowItem> convert2list(string[] fileContent)
        {
            var list = new List<rowItem>();
            foreach (var item in fileContent)
            {
                string[] oneLine = item.Split(',');
                try
                {
                    list.Add(new rowItem() { rowNumber = oneLine[0], description = oneLine[1], value = float.Parse(oneLine[2]) });
                }
                catch { }
            }
            list = list
                   .GroupBy(i => i.description)
                   .Select(g => new rowItem
                   {
                       rowNumber = g.Key,
                       description = g.Key,
                       value = g.Average(i => i.value)
                   })
                    .ToList();
            return list;
        }



        public float getMax(string[] fileContent)
        {
            var list = new List<rowItem>();
            foreach (var item in fileContent)
            {
                string[] oneLine = item.Split(',');
                try
                {
                    list.Add(new rowItem() { rowNumber = oneLine[0], description = oneLine[1], value = float.Parse(oneLine[2]) });
                }
                catch { }
            }
            list = list
                   .GroupBy(i => i.description)
                   .Select(g => new rowItem
                   {
                       rowNumber = g.Key,
                       description = g.Key,
                       value = g.Average(i => i.value)
                   })
                    .ToList();

            var lmax = new List<float>();
            foreach (var m in list)
                lmax.Add(m.value);          

            return lmax.Max();
        }


        public List<rowItem> getPrecent(string[] fileContent)
        {
            var list = new List<rowItem>();
            foreach (var item in fileContent)
            {
                string[] oneLine = item.Split(',');
                try
                {
                    list.Add(new rowItem() { rowNumber = oneLine[0], description = oneLine[1], value = float.Parse(oneLine[2]) });
                }
                catch { }
            }
            list = list
                   .GroupBy(i => i.description)
                   .Select(g => new rowItem
                   {
                       rowNumber = g.Key,
                       description = g.Key,
                       value = g.Average(i => i.value)
                   })
                    .ToList();

            var lmax = new List<float>();
            foreach (var m in list)
                lmax.Add(m.value);
            var _max= lmax.Max();

            for (int curent_value = 0; curent_value < list.Count; curent_value++)
                list[curent_value].value = (list[curent_value].value)/_max;

                return list;
        }
    }
}
