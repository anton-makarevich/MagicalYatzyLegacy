
using Acr.UserDialogs;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Utils
{
    public static class DialogsHelper
    {
        private static IProgressDialog _loading;
        private static IUserDialogs _dialogs;

        public static void Init(IUserDialogs dialogs)
        {
            _dialogs = dialogs;
        }

        public static void ShowMessage(string title, string message)
        {
            _dialogs.Alert(message, title);
        }

        public static void ShowMessage(string message)
        {
            _dialogs.Alert(message, "");
        }

        public static async Task<bool> AskUser(string title, string message)
        {
            return await _dialogs.ConfirmAsync(message, title);
        }

        public static void ShowLoading()
        {
            _loading = _dialogs.Loading();
            _loading.Show();
        }

        public static void HideLoading()
        {
            if (_loading != null)
            {
                _loading.Hide();
            }
        }
    }
}
