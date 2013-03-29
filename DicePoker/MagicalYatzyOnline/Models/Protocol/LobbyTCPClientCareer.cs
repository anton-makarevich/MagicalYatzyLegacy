using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Sanet.Kniffel.Protocol
{
    public class LobbyTCPClientCareer : LobbyTCPClient
    {

        private UserInfo m_User;

        public UserInfo User { get { return m_User; } }

        public LobbyTCPClientCareer()
            : base()
        {
        }

        protected override int GetJoinedSeat(ref int p_noPort, string player)
        {
            return base.GetJoinedSeat(ref p_noPort, m_User.DisplayName);
        }

        /// <summary>
        /// Ttying to remove same user if already authed
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        public bool ClearServerUser(string username)
        {
            Send(new ClearUserCommand(username));
            StringTokenizer token = ReceiveCommand(ClearUserResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return false;
            ClearUserResponse response = new ClearUserResponse(token);
            return response.OK;
        }

        public bool CheckUsernameAvailable(string username)
        {
            Send(new CheckUserExistCommand(username));
            StringTokenizer token = ReceiveCommand(CheckUserExistResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return false;
            CheckUserExistResponse response = new CheckUserExistResponse(token);
            return !response.Exist;
        }

        public bool CheckDisplayNameAvailable(string display)
        {
            Send(new CheckDisplayExistCommand(display));
            StringTokenizer token = ReceiveCommand(CheckDisplayExistResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return false;
            CheckDisplayExistResponse response = new CheckDisplayExistResponse(token);
            return !response.Exist;
        }
        public bool CreateUser(string username, string password, string email, string displayname)
        {
            Send(new CreateUserCommand(username, password, email, displayname));
            StringTokenizer token = ReceiveCommand(CreateUserResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return false;
            CreateUserResponse response = new CreateUserResponse(token);
            return response.Success;
        }

        public bool Authenticate(string username, string password)
        {
            Send(new AuthenticateUserCommand(username, password));
            StringTokenizer token = ReceiveCommand(AuthenticateUserResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return false;
            AuthenticateUserResponse response = new AuthenticateUserResponse(token);
            return response.Success;
        }

        public void RefreshUserInfo(string username, string password="na")
        {
            LogManager.Log(LogLevel.Message, "LobyTCPClient.RefreshUserInfo", "refreshing user {0}", username);
            Send(new GetUserCommand(username,password));
            StringTokenizer token = ReceiveCommand(GetUserResponse.COMMAND_NAME);
            if (!token.HasMoreTokens())
                return;
            GetUserResponse response = new GetUserResponse(token);
            m_PlayerName = response.DisplayName;
            m_User = new UserInfo(response.DisplayName, "na", response.Email, response.Money);
        }
    }
}
