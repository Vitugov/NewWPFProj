using Microsoft.EntityFrameworkCore;
using Microsoft.SqlServer.Management.Sdk.Sfc;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Linq.Dynamic.Core;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Dynamic.Core;

namespace WPFUsefullThings
{
    public static class DbHandler
    {
        public static ObservableCollection<T> GetDeepData<T>(this DbContext dbContext)
            where T : ProjectModel
        {
            var classOverview = ClassOverview.Dic[typeof(T).Name];
            IQueryable<T> query = dbContext.Set<T>();
            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.Name);
            }
            if (classOverview.HaveCollection)
            {
                query = query.Include(classOverview.CollectionProperty.Name);
                foreach (var property in classOverview.CollectionGenericClassOverview.PropertiesOfCoreClass)
                {
                    
                    //Func<IQueryable<T>, IIncludableQueryable<T, object>> func = i => i.Include(i => i.prop1).ThenInclude(t => t.innerprop1);
                    string dynamicLambda = $"d => d.Include(k => k.{classOverview.CollectionProperty.Name}).ThenInclude(e => e.{property.Name})";
                    var path = $"d => d.{classOverview.CollectionProperty.Name}.Select(e => e.{property.Name})";
                    
                    var elementType = typeof(T);
                    var resultType = property.PropertyType;//typeof(ProjectModel);

                    // Парсинг динамического лямбда-выражения
                    var dynamicExpression = DynamicExpressionParser.ParseLambda(
                        new ParsingConfig(), true, elementType, resultType, path);
                    var expr = Expression.Lambda() // TODO!!!!!!!!!!!
                    var delegat = (Func<IQueryable<T>, IIncludableQueryable<T, object>>)dynamicExpression.Compile();
                    query = delegat(query);

                    //(Expression<Func<T, ProjectModel>>)dynamicExpression;
                    //query = query.Include($"d => d.{classOverview.CollectionProperty.Name}").ThenInclude($"e => e.{ property.Name})");
                    //var formatExpr = Expression.Call(
                    //formatStaticMethod,
                    //Expression.Constant(parsed.format, typeof(string)),
                    //formatParamsArrayExpr);

                    //var resultExpr = Expression.Lambda<Func<T, string>>(
                    //    formatExpr,
                    //    argumentExpression);
                }
            }
            using (var context = DbContextCreator.Create())
            {
                return new ObservableCollection<T>(query);
            }
        }

        public static IQueryable<ProjectModel> GetData(this DbContext dbContext, Type type)
        {
            var method = dbContext.GetType().GetMethods().Single(p =>
                p.Name == nameof(DbContext.Set) && p.ContainsGenericParameters && !p.GetParameters().Any());

            method = method.MakeGenericMethod(type);
            //using (dbContext)
            //{
                var result = (IQueryable<ProjectModel>)method.Invoke(dbContext, null);
                return result;
            //}
        }

        public static IQueryable<ProjectModel> GetDeepData(this DbContext dbContext, Type type)
        {
            var classOverview = ClassOverview.Dic[type.Name];
            var query = dbContext.GetData(type);

            foreach (var property in classOverview.PropertiesOfCoreClass)
            {
                query = query.Include(property.Name);
            }
            if (classOverview.HaveCollection)
            {
                query = query.Include(classOverview.CollectionProperty.Name);
                foreach (var property in classOverview.CollectionGenericClassOverview.PropertiesOfCoreClass)
                {
                    //var k = DynamicExpressionParser.ParseLambda()
                    //var path = $"d => d.{classOverview.CollectionProperty.Name}.Select(e => e.{property.Name})";
                    //query = query.Include(e => EF.Property<IEnumerable<ProjectModel>>(e, classOverview.CollectionProperty.Name)
                    //    .Select(x => EF.Property<ProjectModel>(x, property.Name)));

                    //query.Include(classOverview.CollectionProperty.Name).ThenInclude(property.Name);
                    // Строка лямбда-выражения с динамическими именами свойств
                    //string dynamicLambda = $"d => d.{classOverview.CollectionProperty.Name}.Select(e => e.{property.Name})";


                    //var elementType = type;
                    //var resultType = property.PropertyType;

                    //// Парсинг динамического лямбда-выражения
                    //var dynamicExpression = DynamicExpressionParser.ParseLambda(
                    //    new ParsingConfig(), true, elementType, resultType, dynamicLambda);
                    //var expression = dynamicExpression as Expression<Func<ProjectModel, ProjectModel>>;
                    //query = query.Include((Expression<Func<ProjectModel,ProjectModel>>)dynamicExpression);

                    // Дополнительно: использование полученного выражения (нужен контекст использования)
                    // Пример использования был бы с IQueryable<DataItem> или подобным
                }
            }
            return query;
        }

        ///////////////////////////////////////////////////////////////
        //public static Func<IQueryable<T>, IIncludableQueryable<T, object>> returnsomethig
        //{


        //}
        //Func<IQueryable<T>, IIncludableQueryable<T, object>> func = i => i.Include(i => i.prop1).ThenInclude(t => t.innerprop1);
        //string dynamicLambda = $"d => d.{classOverview.CollectionProperty.Name}.Select(e => e.{property.Name})";

        //var elementType = typeof(T);
        //var resultType = property.PropertyType;//typeof(ProjectModel);

        //// Парсинг динамического лямбда-выражения
        //var dynamicExpression = DynamicExpressionParser.ParseLambda(
        //    new ParsingConfig(), true, elementType, resultType, dynamicLambda);
        //var delegat = (Func<IQueryable<T>, IIncludableQueryable<T, object>>)dynamicExpression.Compile();
    }
}
