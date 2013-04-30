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
        public const string NEW_ONLINE_GAME_START = "StartNewGameMessage";

        public const string GAME_ROLL = "roll";
        public const string GAME_CLEAR = "clear";
        public const string GAME_MOVE = "MoveLabel";
        public const string GAME_FINISHED = "GameFinishedLabel";
        public const string GAME_WIN = "WinLabel";
        public const string GAME_PLAY_AGAIN = "AgainLabel";
        public const string GAME_PLAY_READY = "ReadyLabel";
        public const string GAME_STATUS = "StatusLabel";

        public const string PLAYER_NAME = "PlayerNameLabel";
        public const string PLAYER_PASSWORD = "PlayerPasswordLabel";
        public const string PLAYER_NO_PASSWORD = "PlayerNoPasswordLabel";
        public const string PLAYER_TYPE = "PlayerTypeLabel";
        public const string PLAYER_NAME_DEFAULT = "PlayerNameDefault";
        public const string PLAYER_BOTNAME_DEFAULT = "BotNameDefault";
        public const string PLAYER_PASSWORD_REMEMBER = "PlayerRememberLabelLocalized";
        public const string PLAYER_HUMAN = "PlayerHumanLabelLocalized";
        public const string PLAYER_SAVE_SCORE = "SaveScoreLabel";
        public const string PLAYER_BOT = "PlayerBotLabelLocalized";

        public const string PLAYER_SCORE = "PlayerBestScoreLabel";
        public const string PLAYER_SCORE_TOTAL = "PlayerTotalScoreLabel";
        public const string PLAYER_SCORE_COUNT = "PlayerGamesLabel";

        public const string PLAYER_ARTIFACTS_BONUS = "ArtifactsBonusMessage";
        public const string PLAYER_ARTIFACTS_WINBUY = "ArtifactsWinBuyMessage";
        public const string PLAYER_ARTIFACTS_WRONGNP = "ArtifactsWrongMessage";
        
        public const string LEADERBOARD_ALL_RECORDS = "AllRecordsLabel";

        public const string APP_NAME="AppNameLabel";
        public const string APP_NAME_OTHER = "OtherAppsLabel";

        public const string MP_SERVER_OFFLINE = "ServerOfflineLabel";
        public const string MP_SERVER_ONLINE = "ServerOnlineLabel";
        public const string MP_SERVER = "ServerLabel";
        public const string MP_CLIENT = "ClientLabel";
        public const string MP_CLIENT_UPDATED = "ClientUpdatedLabel";
        public const string MP_CLIENT_OUTDATED = "ClientOutdatedLabel";

        public const string MP_CLIENT_OUTDATED_STATUS = "ClientOutdatedMessage";
        public const string MP_SERVER_OFFLINE_STATUS = "ServerOfflineMessage";
        
        public const string MP_SERVER_MAINTANANCE = "ServerMaintananceMessage";
    }
}
