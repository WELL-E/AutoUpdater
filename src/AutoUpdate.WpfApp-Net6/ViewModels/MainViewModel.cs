using GeneralUpdate.Common.Models;
using GeneralUpdate.Core;
using GeneralUpdate.Core.Strategys;
using GeneralUpdate.Core.Update;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace AutoUpdate.WpfApp_Net6.ViewModels
{
    internal class MainViewModel : INotifyPropertyChanged
    {
        private string _prameter;
        private double _progressValue, _progressMaxValue, _progressMinValue;
        private ICommand _launchUpdate;

        public ICommand LaunchUpdate { get => _launchUpdate ?? (_launchUpdate = new RelayCommand(LaunchUpdateAction)); }

        public double ProgressValue 
        {
            get => _progressValue;
            set 
            {
                _progressValue = value;
                OnPropertyChanged("ProgressValue");
            }
        }

        public double ProgressMaxValue 
        { 
            get => _progressMaxValue;
            set 
            {
                _progressMinValue = value;
                OnPropertyChanged("ProgressMaxValue");
            }
        }

        public double ProgressMinValue 
        {
            get => _progressMinValue;
            set 
            {
                _progressMinValue = value;
                OnPropertyChanged("ProgressMinValue");
            }
        }

        public MainViewModel(string prameter) 
        {
            _prameter = prameter;
            ProgressMinValue = 0;
        }

        private void LaunchUpdateAction()
        {
            var bootstrap = new GeneralUpdateBootstrap();
            bootstrap.Exception += OnException;
            bootstrap.MutiDownloadError += OnMutiDownloadError;
            bootstrap.MutiDownloadCompleted += OnMutiDownloadCompleted;
            bootstrap.MutiDownloadStatistics += OnMutiDownloadStatistics;
            bootstrap.MutiDownloadProgressChanged += OnMutiDownloadProgressChanged;
            bootstrap.MutiAllDownloadCompleted += OnMutiAllDownloadCompleted;
            bootstrap.Strategy<DefaultStrategy>().
                Option(UpdateOption.DownloadTimeOut, 60).
                RemoteAddressBase64(_prameter).
                LaunchAsync();
        }


        private void OnMutiDownloadStatistics(object sender, MutiDownloadStatisticsEventArgs e)
        {
            Console.WriteLine($"{ e.Remaining },{ e.Speed },{ e.Version.Name }.");
        }

        private void OnException(object sender, ExceptionEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private void OnMutiDownloadProgressChanged(object sender, MutiDownloadProgressChangedEventArgs e)
        {
            switch (e.Type)
            {
                case ProgressType.Check:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Donwload:
                    ProgressValue = e.ProgressValue;
                    ProgressMaxValue = e.TotalBytesToReceive;
                    Console.WriteLine($"{ e.Version.Name },{ e.ProgressValue },{ e.ProgressPercentage },{ e.TotalBytesToReceive }");
                    break;
                case ProgressType.Updatefile:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Done:
                    Console.WriteLine($"{ e.Message }");
                    break;
                case ProgressType.Fail:
                    Console.WriteLine($"{ e.Message }");
                    break;
            }
        }

        private void OnMutiDownloadError(object sender, MutiDownloadErrorEventArgs e)
        {
            Console.WriteLine($"{ e.Version.Name } ,{ e.Exception.Message }");
        }

        private void OnMutiDownloadCompleted(object sender, MutiDownloadCompletedEventArgs e)
        {
            Console.WriteLine($"{ e.Version.Name },{ e.Version.Version },{ e.Version.Url }.");
        }

        private void OnMutiAllDownloadCompleted(object sender, MutiAllDownloadCompletedEventArgs e)
        {
            //e.FailedVersions; 如果出现下载失败则会把下载错误的版本、错误原因统计到该集合当中。

            //是否下载成功
            if (e.IsAllDownloadCompleted)
            {
                Console.WriteLine("All download completed.");
            }
            else
            {
                Console.WriteLine("download failed!");
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) 
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }

    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand<T> : ICommand
    {
        #region Constructors

        public RelayCommand(Action<T> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action<T> execute, Predicate<T> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute((T)parameter);
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute((T)parameter);
        }

        #endregion // ICommand Members

        #region Fields

        readonly Action<T> _execute = null;
        readonly Predicate<T> _canExecute = null;

        #endregion // Fields
    }

    /// <summary>
    /// A command whose sole purpose is to 
    /// relay its functionality to other
    /// objects by invoking delegates. The
    /// default return value for the CanExecute
    /// method is 'true'.
    /// </summary>
    public class RelayCommand : ICommand
    {
        #region Constructors

        /// <summary>
        /// Creates a new command that can always execute.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        public RelayCommand(Action execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Creates a new command.
        /// </summary>
        /// <param name="execute">The execution logic.</param>
        /// <param name="canExecute">The execution status logic.</param>
        public RelayCommand(Action execute, Func<bool> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        #endregion // Constructors

        #region ICommand Members

        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null || _canExecute();
        }

        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested += value;
            }
            remove
            {
                if (_canExecute != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        public void Execute(object parameter)
        {
            _execute();
        }

        #endregion // ICommand Members

        #region Fields

        readonly Action _execute;
        readonly Func<bool> _canExecute;

        #endregion // Fields
    }
}
