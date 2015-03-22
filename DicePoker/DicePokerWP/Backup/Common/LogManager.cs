using System;
using System.Collections.Generic;
using System.Text;

namespace Sanet
{
    /// <summary>
    /// Degré donné au message
    /// </summary>
    public enum LogLevel
    {
        MessageVeryLow = 0,
        MessageLow = 10,
        Message = 20,
        MessageHigh = 30,
        MessageVeryHigh = 40,
        WarningLow = 50,
        Warning = 60,
        WarningHigh = 70,
        ErrorLow = 80,
        Error = 90,
        ErrorHigh = 100
    }

    /// <summary>
    /// Delegate d'un log
    /// </summary>
    /// <param name="from">Source du message</param>
    /// <param name="message">Le message</param>
    /// <param name="level">Le degré du message (Voir "LogLevel" pour des valeurs préféfinies) (plus il est élevé, plus il est important)</param>
    public delegate void LogDelegate(string from, string message, int level);

    public static class LogManager
    {
        /// <summary>
        /// Évenement déclenché lorsque quelqu'un log quelquechose
        /// </summary>
        public static event LogDelegate MessageLogged;

        /// <summary>
        /// Log d'un message formatté
        /// </summary>
        /// <param name="from">Source du message</param>
        /// <param name="format">Message formatté</param>
        /// <param name="args">Argument du message formaté</param>
        public static void Log(string from, string format, params object[] args)
        {
            Log((int)LogLevel.Message, from, format, args);
        }
        /// <summary>
        /// Log whole exception,
        /// Anton: added that as tired of formating every exception in every catch
        /// </summary>
        public static void Log(string from, Exception exception, MarkedUpExceptionType type = MarkedUpExceptionType.Trace)
        {
            Log(LogLevel.Error, from, from + " ex: {0}, trace: {1}", exception.Message, exception.StackTrace);

        }
        /// <summary>
        /// Log d'un message formatté
        /// </summary>
        /// <param name="level">Élément LogLevel déterminant le degré du message</param>
        /// <param name="from">Source du message</param>
        /// <param name="format">Message formatté</param>
        /// <param name="args">Argument du message formaté</param>
        public static void Log(LogLevel level, string from, string format, params object[] args)
        {
            Log((int)level, from, format, args);
        }

        /// <summary>
        /// Log d'un message formatté
        /// </summary>
        /// <param name="level">Nombre déterminant le degré du message (plus il est élevé, plus il est important)</param>
        /// <param name="from">Source du message</param>
        /// <param name="format">Message formatté</param>
        /// <param name="args">Argument du message formaté</param>
        public static void Log(int level, string from, string format, params object[] args)
        {
            if (MessageLogged != null)
                MessageLogged(from, String.Format(format, args), level);
        }
        /// <summary>
        /// Level of messages to log, messageverylow=log everything
        /// </summary>
        public static LogLevel LoggingLevel=LogLevel.MessageVeryLow;

        

    }

    public enum MarkedUpExceptionType
    {
        Crash,
        Exception,
        Trace,
        Information,
        Debug
    }

    public interface ILogConsole
    {
        void WriteLine(string line);
        void Write(string text);
    }

    public class DebugLogConsole : ILogConsole
    {
        public void WriteLine(string line)
        {
            System.Diagnostics.Debug.WriteLine(line);
        }
        public void Write(string text)
        {

        }
    }
}
