using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using NLog;
using WonderStock.Database;

namespace WonderStock
{
    public partial class App : Application
    {
        static StockDatabase database;
        public static StockDatabase Database
        {
            get
            {
                return database ?? (database = new StockDatabase(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "Stocks.db")));
            }
        }

        private Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitUnhandledException();
        }

        private void InitUnhandledException()
        {
            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
            LoggingUnhandledException((Exception)e.ExceptionObject, "AppDomain.CurrentDomain.UnhandledException");

            DispatcherUnhandledException += (s, e) =>
            {
                LoggingUnhandledException(e.Exception, "Application.Current.DispatcherUnhandledException");
                e.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (s, e) =>
            {
                LoggingUnhandledException(e.Exception, "TaskScheduler.UnobservedTaskException");
                e.SetObserved();
            };
        }

        private void LoggingUnhandledException(Exception exception, string source)
        {
            string message = $"Unhandled exception ({source})";

            try
            {
                System.Reflection.AssemblyName assemblyName = System.Reflection.Assembly.GetExecutingAssembly().GetName();
                message = string.Format("Unhandled exception in {0} v{1}", assemblyName.Name, assemblyName.Version);
            }
            catch (Exception ex)
            {
                logger.Error(ex, "Exception in LoggingUnhandledException");
            }
            finally
            {
                logger.Error(exception, message);
            }
        }
    }
}
