using Mud.Engine.Runtime.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ServiceLocatorBuilder.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private Type selectedLoggingService;

        public MainWindowViewModel()
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies()
                .Where(assembly =>
                    !assembly.FullName.StartsWith("System") &&
                    !assembly.FullName.StartsWith("Microsoft") &&
                    !assembly.FullName.StartsWith("mscor"));
            var types = new List<Type>();

            foreach(Assembly assembly in assemblies)
            {
                types.AddRange(assembly.GetTypes().Where(t => t.GetInterface(typeof(IService).FullName) != null));
            }

            this.LoggingServices= types
                .Where(t => t.GetInterface(typeof(ILoggingService).FullName) != null);
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public IEnumerable<Type> LoggingServices { get; set; }

        public Type SelectedLoggingService
        {
            get
            {
                return this.selectedLoggingService;
            }

            set
            {
                this.selectedLoggingService = value;
                this.OnPropertyChanged();
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string property = "")
        {
            var handler = this.PropertyChanged;
            if (handler == null)
            {
                return;
            }

            handler(this, new PropertyChangedEventArgs(property));
        }
    }
}
