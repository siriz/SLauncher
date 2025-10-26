using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace SLauncher.Classes
{
    /// <summary>
    /// Represents a tab that contains a collection of launcher items
    /// </summary>
    public class TabItem : INotifyPropertyChanged
    {
        private string _name;
  private string _id;
        private ObservableCollection<UserControl> _items;

  public TabItem()
        {
          Id = Guid.NewGuid().ToString();
         Items = new ObservableCollection<UserControl>();
        }

        /// <summary>
   /// Unique identifier for the tab
        /// </summary>
        public string Id
        {
    get => _id;
  set
            {
       _id = value;
     OnPropertyChanged();
            }
        }

        /// <summary>
        /// Display name of the tab
        /// </summary>
      public string Name
        {
     get => _name;
        set
            {
     _name = value;
    OnPropertyChanged();
            }
        }

        /// <summary>
        /// Collection of items in this tab
      /// </summary>
        public ObservableCollection<UserControl> Items
        {
            get => _items;
   set
            {
         _items = value;
            OnPropertyChanged();
         }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
          PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
