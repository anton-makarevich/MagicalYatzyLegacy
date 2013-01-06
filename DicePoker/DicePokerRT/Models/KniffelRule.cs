using Sanet.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public class KniffelRule:BaseViewModel
    {
        public KniffelRule(Rules rule)
        {
            Rule = rule;
        }

        /// <summary>
        /// Rule
        /// </summary>
        private Rules _Rule;
        public Rules Rule
        {
            get { return _Rule; }
            set
            {
                if (_Rule != value)
                {
                    _Rule = value;
                    NotifyPropertyChanged("Rule");
                    NotifyPropertyChanged("RuleNameLocalized");
                }
            }
        }
        /// <summary>
        /// Localized rule name
        /// </summary>
        public string RuleNameLocalized
        {
            get
            {
                return Rule.ToString().Localize();
            }
        }

    }
}
