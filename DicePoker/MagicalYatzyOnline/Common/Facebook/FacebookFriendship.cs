using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Common
{
    /// <summary>
    /// Relashion between 2 facebook friends
    /// </summary>
    public class FacebookFriendship: IComparable<FacebookFriendship>
    {
        public FacebookFriendship() { }

        public FacebookFriendship(string id1, string id2)
        {
            Friend1FBId = id1;
            Friend2FBId = id2;
        }

        public String Friend1FBId { set; get; }
        public String Friend2FBId { set; get; }

        public int CompareTo(FacebookFriendship other)
        {
            if ((this.Friend1FBId == other.Friend1FBId || this.Friend1FBId == other.Friend2FBId) &&
                (this.Friend2FBId == other.Friend1FBId || this.Friend2FBId == other.Friend2FBId))
                return 0;
            return 1;
        }
    }
}
