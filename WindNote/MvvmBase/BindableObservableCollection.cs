using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace WindNote.MvvmBase
{
    public interface IBindableObservableCollection : IList
    {
        void Move(int oldIndex, int newIndex);
    }

    public class BindableObservableCollection<T> : ObservableCollection<T>, IBindableObservableCollection
    {
        public BindableObservableCollection() : base() { }
        public BindableObservableCollection(List<T> list) : base(list) { }
    }

}
