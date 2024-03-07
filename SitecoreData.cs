﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PackageAnalyzerDesktop
{
    public class SitecoreData : INotifyPropertyChanged
    {
        private string _identifier;
        public string Identifier
        {
            get { return _identifier; }
            set
            {
                if (_identifier != value)
                {
                    _identifier = value;
                    OnPropertyChanged(nameof(Identifier));
                }
            }
        }

        private string _value;
        public string Value
        {
            get { return _value; }
            set
            {
                if (_value != value)
                {
                    _value = value;
                    OnPropertyChanged(nameof(Value));
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
