using System;
using System.Data;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;
using Npgsql;

namespace AutoNova_Car_Showroom
{
    public static class AppExceptionHandler
    {
        public static void RegisterGlobalHandlers()
        {
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += (sender, args) => ShowError(args.Exception);
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                ShowError(args.ExceptionObject as Exception);
            };
        }

        public static void ShowError(Exception ex, string context = null)
        {
            string message = BuildUserMessage(ex, context);
            MessageBox.Show(message, "AutoNova Car Showroom", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowWarning(string message)
        {
            MessageBox.Show(message, "AutoNova Car Showroom", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }

        public static void ShowInfo(string message)
        {
            MessageBox.Show(message, "AutoNova Car Showroom", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static bool SafeExecute(Action action, string context)
        {
            try
            {
                action();
                return true;
            }
            catch (Exception ex)
            {
                ShowError(ex, context);
                return false;
            }
        }

        private static string BuildUserMessage(Exception ex, string context)
        {
            if (ex == null)
            {
                return string.IsNullOrWhiteSpace(context)
                    ? "An unexpected error occurred."
                    : context + " An unexpected error occurred.";
            }

            string prefix = string.IsNullOrWhiteSpace(context) ? string.Empty : context + Environment.NewLine + Environment.NewLine;

            if (ex is FormatException)
            {
                return prefix + "Invalid input format. Please check numbers and required fields.";
            }

            if (ex is OverflowException)
            {
                return prefix + "The number you entered is too large.";
            }

            if (ex is ArgumentException || ex is ArgumentNullException)
            {
                return prefix + "Invalid input: " + ex.Message;
            }

            if (ex is IOException)
            {
                return prefix + "File error: " + ex.Message;
            }

            if (ex is NpgsqlException)
            {
                return prefix + "Database error. Please check PostgreSQL is running and your connection settings." +
                       Environment.NewLine + Environment.NewLine + ex.Message;
            }

            if (ex is WebException)
            {
                return prefix + "Network error. Please check your internet connection.";
            }

            if (ex is SmtpException)
            {
                return prefix + "Email could not be sent. Please verify email settings.";
            }

            return prefix + ex.Message;
        }
    }
}
