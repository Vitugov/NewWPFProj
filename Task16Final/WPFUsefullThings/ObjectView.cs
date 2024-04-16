using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public class ObjectView<T>
        where T : class, IUpdateable<T>, INotifyPropertyChanged, new()
    {
        public readonly T Edit;
        public DynamicIsValid IsPropertyValid => _validation.IsValid;
        public bool IsValid => _validation.Validate();

        private readonly ObservableCollection<T> _collection;
        private readonly Type _contextType;
        private readonly T _original;
        private readonly bool _isNew;
        private readonly Validation<T> _validation;

        public ObjectView(T original, ObservableCollection<T> collection, Type contextType)
        {
            IfNotADbContextThrowExeption(contextType);
            _isNew = false;
            _original = original;
            Edit = (T)original.Clone();
            _collection = collection;
            _contextType = contextType;
            _validation = new Validation<T>(Edit);
        }

        public ObjectView(ObservableCollection<T> collection, Type contextType)
            : this(new T(), collection, contextType)
        {
            _isNew = true;
        }

        public void Save()
        {
            if (!IsValid)
            {
                ValidationRules.ShowErrorMessage();
                return;
            }

            _original.UpdateFrom(Edit);
            
            using (var context = GetContext())
            {
                var dbSet = context.Set<T>();
                dbSet.Update(_original);
                context.SaveChanges();
            }
            
            if (_isNew)
            {
                _collection.Add(_original);
            }
        }

        private void IfNotADbContextThrowExeption(Type contextType)
        {
            if (contextType != typeof(DbContext) && !contextType.IsSubclassOf(typeof(DbContext)))
                throw new ArgumentException("Type of the context don't belong to class DbContext");
        }

        private DbContext GetContext() => (DbContext)Activator.CreateInstance(_contextType);
    }
}
