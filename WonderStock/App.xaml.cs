using System;
using System.Threading.Tasks;
using System.Windows;
using NLog;

namespace WonderStock
{
    public partial class App : Application
    {
        private Logger logger = LogManager.GetCurrentClassLogger();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            InitUnhandledException();

            StockManager.Init();
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
