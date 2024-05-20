using Microsoft.EntityFrameworkCore;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;

namespace WPFUsefullThings
{
    public class ObjectView<T> : INotifyPropertyChanged
        where T : ProjectModel, new()
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        public T Edit { get; set; }
        public DynamicIsValid IsPropertyValid => _validation.IsValid;
        public bool IsValid => _validation.Validate();

        private readonly ObservableCollection<T> _collection;
        private readonly T _original;
        private readonly bool _isNew = false;
        private readonly Validation<T> _validation;


        public ObjectView(T? original, ObservableCollection<T> collection)
        {
            if (original == null)
            {
                _isNew = true;
                _original = new T();
            }
            else
            {
                _original = original.GetDeepSetForObj();
            }

            Edit = (T)_original.Clone();
            _collection = collection;
            _validation = new Validation<T>(Edit);
        }

        public void Save()
        {
            if (!IsValid)
            {
                ValidationRules.ShowErrorMessage();
                return;
            }

            Edit.SaveItem(_original, _isNew);
            
            if (!_isNew)
            {
                _collection.Remove(_original);
            }
            _collection.Add(_original);
        }
    }
}
