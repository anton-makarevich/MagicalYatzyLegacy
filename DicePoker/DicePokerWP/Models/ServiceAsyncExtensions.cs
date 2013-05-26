using DicePokerWP.KniffelLeaderBoardService;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    /// <summary>
    /// Need this extensions as for some reason wp doesn't generate task based methods for service references
    /// </summary>
    public static class ServiceAsyncExtensions
    {
        /// <summary>
        /// Task wrapper for GetLastWeekChempionAsync() method
        /// </summary>
        public static Task<IEnumerable<string>> GetLastWeekChempionTaskAsync(this KniffelServiceSoapClient client, string rules)
        {
            var tcs = new TaskCompletionSource<IEnumerable<string>>();
            client.GetLastWeekChempionCompleted+= (s, e) =>
            {
                if (e.Error != null) tcs.SetException(e.Error);
                else if (e.Cancelled) tcs.SetCanceled();
                else tcs.SetResult(new List<string>(){e.Name,e.Score});
            };
            client.GetLastWeekChempionAsync(rules, "", "");
            return tcs.Task;
        }

        /// <summary>
        /// Task wrapper for GetLastWeekChempionAsync() method
        /// </summary>
        public static Task<GetTopPlayersResponse> GetTopPlayersTaskAsync(this KniffelServiceSoapClient client, string rules)
        {
            var tcs = new TaskCompletionSource<GetTopPlayersResponse>();
            client.GetTopPlayersCompleted += (s, e) =>
            {
                if (e.Error != null) tcs.SetException(e.Error);
                else if (e.Cancelled) tcs.SetCanceled();
                else tcs.SetResult(new GetTopPlayersResponse(new GetTopPlayersResponseBody(e.Result,e.Players)));
            };
            client.GetTopPlayersAsync(rules, null);
            return tcs.Task;
        }

    }
}
