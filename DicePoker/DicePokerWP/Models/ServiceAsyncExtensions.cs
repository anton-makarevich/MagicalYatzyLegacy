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
        /// <summary>
        /// Task wrapper for GetArtifactsAsync() method
        /// </summary>
        public static Task<GetPlayersMagicsResponse> GetPlayersMagicsTaskAsync(this KniffelServiceSoapClient client,
            string username, string pass, int rolls, int manuals, int resets)
        {
            var tcs = new TaskCompletionSource<GetPlayersMagicsResponse>();
            client.GetPlayersMagicsCompleted += (s, e) =>
            {
                if (e.Error != null) tcs.SetException(e.Error);
                else if (e.Cancelled) tcs.SetCanceled();
                else tcs.SetResult(new GetPlayersMagicsResponse(new GetPlayersMagicsResponseBody(e.Result,e.rolls,e.manuals,e.resets)));
            };
            client.GetPlayersMagicsAsync(username, pass, rolls, manuals, resets);
            return tcs.Task;
        }
        /// <summary>
        /// Task wrapper for AddArtifactsAsync() method
        /// </summary>
        public static Task<AddPlayersMagicsResponse> AddPlayersMagicsTaskAsync(this KniffelServiceSoapClient client,
            string username, string pass, string rolls, string manuals, string resets)
        {
            var tcs = new TaskCompletionSource<AddPlayersMagicsResponse>();
            client.AddPlayersMagicsCompleted += (s, e) =>
            {
                if (e.Error != null) tcs.SetException(e.Error);
                else if (e.Cancelled) tcs.SetCanceled();
                else tcs.SetResult(new AddPlayersMagicsResponse(new AddPlayersMagicsResponseBody(e.Result)));
            };
            client.AddPlayersMagicsAsync(username, pass, rolls, manuals, resets);
            return tcs.Task;
        }

        /// <summary>
        /// Task wrapper for PutScoreAsync() method
        /// </summary>
        public static Task<PutScoreIntoTableWithPicPureNameResponse> PutScoreIntoTableWithPicPureNameTaskAsync(this KniffelServiceSoapClient client,
            string username, string pass, string score, string table, string picurl)
        {
            var tcs = new TaskCompletionSource<PutScoreIntoTableWithPicPureNameResponse>();
            client.AddPlayersMagicsCompleted += (s, e) =>
            {
                if (e.Error != null) tcs.SetException(e.Error);
                else if (e.Cancelled) tcs.SetCanceled();
                else tcs.SetResult(new PutScoreIntoTableWithPicPureNameResponse(new PutScoreIntoTableWithPicPureNameResponseBody(e.Result)));
            };
            client.PutScoreIntoTableWithPicPureNameAsync(username, pass, score, table, picurl);
            return tcs.Task;
        }
    }
}
