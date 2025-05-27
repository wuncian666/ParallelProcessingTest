using BenchmarkDotNet.Attributes;
using Iced.Intel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BranchMark
{
    [MemoryDiagnoser]
    public class Write_VS_OptimizeWrite
    {
        private static readonly PropertyInfo[] properties = typeof(CSVModel).GetProperties();
        private static int Count = properties.Length;

        private delegate string GetterDelegate(object obj);

        private static readonly GetterDelegate[] _getters = properties.Select(x => CreateGetter(x)).ToArray();

        private static GetterDelegate CreateGetter(PropertyInfo property)
        {
            Type targetType = typeof(object);

            // 因為外部是允許傳入任何類型(所以製作object 可以讓任何類型傳入)
            ParameterExpression targetPram = Expression.Parameter(targetType, "obj");

            // 將類型轉換成他們自己骨子裡可以接收的真實函數
            UnaryExpression targetConvert = Expression.Convert(targetPram, property.DeclaringType);

            MethodCallExpression propertyGetter = Expression.Call(targetConvert, property.GetGetMethod());
            Expression<GetterDelegate> lambdaExpression = Expression.Lambda<GetterDelegate>(propertyGetter, targetPram);
            GetterDelegate getterDelegate = lambdaExpression.Compile();

            return getterDelegate;
        }

        private static StringBuilder builder = new StringBuilder(90);
        private static char[] chars = new char[90];

        [Benchmark]
        public void Write()
        {
            for (int j = 0; j < 3500000; j++)
            {
                CSVModel data = new CSVModel()
                {
                    ID = "18",
                    FirstName = "Josias",
                    LastName = "Consterdine",
                    Email = "jconsterdineh@scribd.com",
                    Gender = "Male",
                    IP = "149.190.102.195"
                };

                string output = "";
                PropertyInfo[] props = typeof(CSVModel).GetProperties();
                for (int i = 0; i < props.Length; i++)
                {
                    string value = props[i].GetValue(data).ToString();
                    output += value + ",";
                }

                output = output.TrimEnd(',');
            }
        }

        [Benchmark]
        public void OptimizeWrite()
        {
            for (int j = 0; j < 3500000; j++)
            {
                CSVModel data = new CSVModel()
                {
                    ID = "18",
                    FirstName = "Josias",
                    LastName = "Consterdine",
                    Email = "jconsterdineh@scribd.com",
                    Gender = "Male",
                    IP = "149.190.102.195"
                };

                for (int i = 0; i < Count; i++)
                {
                    builder.Append(_getters[i](data));
                    if (i < Count - 1)
                        builder.Append(",");
                }
                builder.CopyTo(0, chars, 0, builder.Length - 1);
                //string output = builder.ToString();
                builder.Clear();
            }
        }
    }
}