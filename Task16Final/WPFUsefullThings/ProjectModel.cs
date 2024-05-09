﻿using PropertyChanged;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace WPFUsefullThings
{
    public abstract partial class ProjectModel : ICloneable, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        
        [Key]
        [DisplayName("Id")]
        [Invisible]
        public Guid Id { get; set; } = Guid.NewGuid();

        private string displayName = "";
        [Invisible]
        public string DisplayName
        {
            get => displayName;
            set
            {
                if (value != displayName)
                {
                    displayName = value;
                    OnPropertyChanged();
                }
            }
        }
        public ProjectModel() {}
        public virtual void UpdateFrom(ProjectModel obj)
        {
            if (obj == null || this.GetType() != obj.GetType())
            {
                throw new ArgumentException("Object must be of the same type.", nameof(obj));
            }

            var properties = ClassOverview.Dic[this.GetType().Name].Properties;
            foreach (var prop in properties)
            {
                if (prop.CanRead && prop.CanWrite)
                {
                    var value = prop.GetValue(obj);
                    prop.SetValue(this, value);
                    OnPropertyChanged(prop.Name);
                }
            }
        }

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

            if (propertyName != nameof(DisplayName))
            {
                UpdateDisplayName();
            }
        }

        protected abstract void UpdateDisplayName();

        public override string ToString() => DisplayName;

        public virtual object Clone()
        {
            var classOverview = ClassOverview.Dic[this.GetType().Name];
            var clone = classOverview.CreateObject();

            foreach (var property in classOverview.Properties)
            {
                var value = property.GetValue(this);

                // Проверяем, является ли значение коллекцией
                if (property == classOverview.CollectionProperty)
                {
                    var itemType = classOverview.CollectionGenericParameter;
                    if (typeof(ICloneable).IsAssignableFrom(itemType))
                    {
                        // Создаем новую коллекцию и клонируем каждый элемент
                        var clonedList = (IList)Activator.CreateInstance(value.GetType());
                        foreach (var item in (IEnumerable)value)
                        {
                            clonedList.Add(((ICloneable)item).Clone());
                        }
                        property.SetValue(clone, clonedList);
                    }
                    else
                    {
                        // Просто копируем коллекцию, если элементы не реализуют ICloneable
                        property.SetValue(clone, value);
                        throw new Exception("Collection should be from ICloneable objects");
                    }
                }
                else
                {
                    if (property.CanWrite)
                    {
                        property.SetValue(clone, value);
                    }
                }
                clone.OnPropertyChanged(property.Name);
            }
            return clone;
        }

        public override bool Equals(object? obj)
        {
            if (obj == null || obj.GetType() != this.GetType())
                return false;

            ProjectModel other = (ProjectModel)obj;
            return this.Id == other.Id; // Сравниваем только по GUID
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(ProjectModel me, ProjectModel other)
        {
            return Equals(me, other);
        }

        public static bool operator !=(ProjectModel me, ProjectModel other)
        {
            return !Equals(me, other);
        }
    }
}
