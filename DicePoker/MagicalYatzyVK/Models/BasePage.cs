using Sanet.Models;
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MagicalYatzyVK.Views
{
    public abstract class BasePage:Page
    {

        public abstract void NavigateTo();

        public abstract void NavigateFrom();

        #region ViewModel
        public void SetViewModel<T>() where T : BaseViewModel
        {
            DataContext = ViewModelProvider.GetViewModel<T>();

        }

        public T GetViewModel<T>() where T : BaseViewModel
        {
            if (DataContext == null)
                SetViewModel<T>();
            return (T)DataContext;
        }

        #endregion
    }
}
