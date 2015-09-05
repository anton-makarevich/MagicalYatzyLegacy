using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Windows.Controls;
using Microsoft.Phone.Tasks;

namespace Microsoft.Phone.Controls.Unofficial
{
    /// <summary>
    /// An extended HyperlinkButton control that uses the Tag property to
    /// open the web browser, compose an e-mail, text message, or make a call.
    /// </summary>
    public class PhoneHyperlinkButton : HyperlinkButton
    {
        /// <summary>
        /// Handles the click event.
        /// </summary>
        protected override void OnClick()
        {
            base.OnClick();

            Debug.Assert(Tag is string, "You need to set the Tag property!");
            string tag = Tag as string;
            if (tag == null)
            {
                // This should support data binding to Uri.
                tag = Tag.ToString();
            }

            IDictionary<string, string> d;

            if (tag.StartsWith("mailto:"))
            {
                Email(tag.Substring(7));
            }
            else if (tag.StartsWith("tel:"))
            {
                // RFC 2806 only defines the basics of a number component.
                // However, since the Windows Phone supports the concept of
                // sending a name to display as well, I have deviated.
                // tel:8005221212&displayname=Unknown%20Caller
                PhoneCallTask pct = new PhoneCallTask();
                pct.PhoneNumber = GetAddress(tag.Substring(4), out d);

                string name;
                if (d.TryGetValue("displayname", out name))
                {
                    pct.DisplayName = name;
                }

                pct.Show();
            }
            else if (tag.StartsWith("sms:"))
            {
                // Also not really an official syntax for SMS.
                // sms:8005551212&body=Hello%20there!
                SmsComposeTask sct = new SmsComposeTask();
                sct.To = GetAddress(tag.Substring(4), out d);

                string body;
                if (d.TryGetValue("body", out body))
                {
                    sct.Body = body;
                }

                sct.Show();
            }
            else
            {
                // Assume the web.
                WebBrowserTask wbt = new WebBrowserTask
                {
                    URL = (string)Tag,
                };
                wbt.Show();
            }
        }

        private void Email(string s)
        {
            IDictionary<string, string> d;
            string to = GetAddress(s, out d);

            EmailComposeTask ect = new EmailComposeTask
            {
                To = to,
            };

            string cc;
            if (d.TryGetValue("cc", out cc))
            {
                ect.Cc = cc;
            }

            string subject;
            if (d.TryGetValue("subject", out subject))
            {
                ect.Subject = subject;
            }

            string body;
            if (d.TryGetValue("body", out body))
            {
                ect.Body = body;
            }

            ect.Show();
        }

        private static string GetAddress(string input, out IDictionary<string, string> query)
        {
            query = new Dictionary<string, string>(StringComparer.Ordinal);
            int q = input.IndexOf('?');
            string address = input;
            if (q >= 0)
            {
                address = input.Substring(0, q);
                ParseQueryStringToDictionary(input.Substring(q + 1), query);
            }
            return address;
        }

        private static void ParseQueryStringToDictionary(string queryString, IDictionary<string, string> dictionary)
        {
            foreach (string str in queryString.Split("&".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
            {
                int index = str.IndexOf("=", StringComparison.Ordinal);
                if (index == -1)
                {
                    dictionary.Add(HttpUtility.UrlDecode(str), string.Empty);
                }
                else
                {
                    dictionary.Add(HttpUtility.UrlDecode(str.Substring(0, index)), HttpUtility.UrlDecode(str.Substring(index + 1)));
                }
            }
        }
    }
}