using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sanet.Kniffel.Models
{
    public static class Messages
    {
        public const string NETWORK_GAME_IS_NOT_READY= "NetworkGameNotReadyMessage";
        public const string NEW_GAME_START = "StartNewGameMessage";
        public const string NEW_GAME_PLAYERS = "PlayersLabel";
        public const string NEW_GAME_RULES = "RulesLabel";
        public const string NEW_GAME_ADD_HUMAN = "AddPlayerLabel";
        public const string NEW_GAME_ADD_BOT = "AddBotLabel";
        public const string NEW_GAME_START_GAME = "StartGameButton";

        public const string PLAYER_NAME = "PlayerNameLabel";
        public const string PLAYER_PASSWORD = "PlayerPasswordLabel";
        public const string PLAYER_TYPE = "PlayerTypeLabel";
        public const string PLAYER_NAME_DEFAULT = "PlayerNameDefault";
        public const string PLAYER_BOTNAME_DEFAULT = "BotNameDefault";
        public const string PLAYER_PASSWORD_REMEMBER = "PlayerRememberLabelLocalized";
        public const string PLAYER_HUMAN = "PlayerHumanLabelLocalized";
        public const string PLAYER_BOT = "PlayerBotLabelLocalized";
    }
}
