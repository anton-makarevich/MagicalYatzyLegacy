using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;
using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Sanet.Kniffel.ViewModels
{
    public class TableWrapper:BaseViewModel
    {
        TupleTableInfo _table;
        public TableWrapper(TupleTableInfo table)
        {
            _table = table;
        }

        public int Id
        {
            get
            {
                return _table.Id;
            }
        }

        public List<string> Players
        {
            get
            { return _table.Players; }
        }
        public Rules Rule { get { return _table.Rule; } }
        public string Name { get { return _table.Name; } }

        
        private bool _IsSelected;
        public bool IsSelected
        {
            get { return _IsSelected; }
            set
            {
                if (_IsSelected != value)
                {
                    _IsSelected = value;
                    NotifyPropertyChanged("IsSelected");
                }
            }
        }



    }
}
