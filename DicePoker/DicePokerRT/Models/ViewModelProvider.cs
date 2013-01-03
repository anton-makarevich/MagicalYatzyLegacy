using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Models
{
    public static class ViewModelProvider
    {
        static List<BaseViewModel> viewModels = new List<BaseViewModel>();

        public static T GetViewModel<T>() where T:BaseViewModel
        {
            T vm = (T)viewModels.Where(f=>f is T).FirstOrDefault();
            if (vm == null)
            {
                vm=(T)Activator.CreateInstance(typeof(T));
                viewModels.Add(vm);
            }
            return vm;
        }

    }
}
