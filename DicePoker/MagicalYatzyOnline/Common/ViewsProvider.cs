using Sanet.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public static class ViewsProvider
    {
        static List<BasePage> _views = new List<BasePage>();

        public static T GetPage<T>() where T : BasePage
        {
            T v = (T)_views.Where(f=>f is T).FirstOrDefault();
            if (v == null)
            {
                v=(T)Activator.CreateInstance(typeof(T));
                _views.Add(v);
            }
            return v;
        }
        
    }
}
