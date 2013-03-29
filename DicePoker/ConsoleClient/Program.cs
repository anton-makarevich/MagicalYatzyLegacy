using Sanet.Kniffel.Models;
using Sanet.Kniffel.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.ConsoleKlient
{
    class Program
    {
        private static LobbyTCPClient m_Server;
        string m_Username = string.Empty;
        string m_Password = string.Empty;
        KniffelGameClient m_ConsoleGame;
        

        public void Start(string[] args)
        {
            Console.WriteLine("Hi! Welcom to Yatzy game");
            bool isManual = false;
            
            var tbl = tables.FirstOrDefault(f => f.NoPort == int.Parse(input));
            if (tbl != null)
                JoinTable(tbl, evt);
            else
                JoinTable(input, evt);
            
            evt.WaitOne();

        }
        /// <summary>
        /// Register user method
        /// </summary>
        /// <returns></returns>
        public bool RegisterNew()
        {
            m_Server = new LobbyTCPClientCareer();
            // Reaching the server ...
            if (m_Server.Connect())
            {
                m_Server.Start();
                
                // Availability of Username ...
                bool step2OK;
                do
                {
                    if (!string.IsNullOrEmpty(m_Username))
                        Console.WriteLine(m_Username +" is not available, please try another one.");
                    Console.WriteLine("enter your desired username:");
                    m_Username = Console.ReadLine();
                    step2OK = m_Server.CheckUsernameAvailable(m_Username);
                }while (!step2OK);
                if (step2OK)
                {
                    Console.WriteLine("enter your desired password:");
                    m_Password = Console.ReadLine();
                     if (m_Server.CreateUser(m_Username, m_Password, "test@console.ap", m_Username))
                        {
                            Console.WriteLine("Logging in...");
                            // Authenticating Player ...
                            if (m_Server.Authenticate(m_Username, m_Password))
                            {
                                
                                // Retrieving User Info ...
                                m_Server.RefreshUserInfo(m_Username);
                                Console.WriteLine("Ok");
                                return true;
                            }
                            else
                            {
                                Console.WriteLine("Cant' auth");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Can't create user");
                        }
                    
                }
                else
                {
                    Console.WriteLine("Can't connect");
                }
            }

            return false;
        }

        /// <summary>
        /// Log in user method
        /// </summary>
        /// <returns></returns>
        public bool Connect()
        {
            m_Server = new LobbyTCPClient();
            // Reaching the server ...
            if (m_Server.Connect())
            {
                m_Server.Start();
                    //Console.Write("enter your username:");
                    m_Username = m_inputMode.GetRequiredValue("enter your username:", new string[] { }, ManualActions.EnterLogin);
                    //Console.Write("enter your password:");
                    m_Password = m_inputMode.GetRequiredValue("enter your password:", new string[] { }, ManualActions.EnterPassword); 
                     Console.WriteLine("Logging in...");
                        // Authenticating Player ...
                        if (m_Server.Authenticate(m_Username, m_Password))
                        {

                            // Retrieving User Info ...
                            m_Server.RefreshUserInfo(m_Username,m_Password);
                            m_Username = m_Server.User.DisplayName;
                            Console.WriteLine(m_Username+" logged in");
                            return true;
                        }
                        else
                        {
                            Console.WriteLine("Cant' auth");
                        }

                    }
                    else
                    {
                        Console.WriteLine("Can't connect");
                    }
            

            return false;
        }

        /// <summary>
        /// Method to join table
        /// </summary>
        /// <param name="tbl"></param>
        public async void JoinTable(TupleTableInfo tbl, ManualResetEvent resetEvent)
        {


            if (tbl != null)
            {
                if (!m_Server.IsConnected)
                {
                    await m_Server.ConnectAsync();
                    m_Server.RefreshUserInfo(m_Username);
                }

                m_ConsoleGame = new PokerConsoleGame(m_Server, m_inputMode, resetEvent);
                GameClient game = m_Server.JoinTable(tbl.NoPort, tbl.TableName, m_ConsoleGame);
                //if (game != null)
                //{
                //    return true;
                //}

            }
            //return false;
        }
        public async void JoinTable(string tableno, ManualResetEvent resetEvent)
        {
                if (!m_Server.IsConnected)
                {
                    await m_Server.ConnectAsync();
                    m_Server.RefreshUserInfo(m_Username);
                }

                m_ConsoleGame = new PokerConsoleGame(m_Server, m_inputMode, resetEvent);
                GameClient game = m_Server.JoinTable(int.Parse(tableno), "auto", m_ConsoleGame);

            
        }
    }
}
