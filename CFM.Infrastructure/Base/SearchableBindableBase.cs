using System;
using System.Collections.Generic;
using CFM.Infrastructure.Interfaces;
using Prism.Mvvm;

namespace CFM.Infrastructure.Base
{
    public abstract class SearchableBindableBase<T> : BindableBase, ISearchable<T> where T: class 
    {
        private string _filter;

        public string Filter
        {
            get { return _filter; }
            set
            {
                SetProperty(ref _filter, value);
                ApplyFilter(_filter);
            }
        }

        private ICollection<T> _filteredCollection;

        public ICollection<T> FilteredCollection
        {
            get { return _filteredCollection; }
            set { SetProperty(ref _filteredCollection, value); }
        }

        protected abstract void ApplyFilter(string filter);
    }
}