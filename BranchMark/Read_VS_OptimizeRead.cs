using BenchmarkDotNet.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace BranchMark
{
    [MemoryDiagnoser]
    public class Read_VS_OptimizeRead
    {
        private static PropertyInfo[] properties = typeof(CSVModel).GetProperties();

        private static int propertiesLength = properties.Length;

        private delegate void SetterDelegate(object obj, object value);

        private static SetterDelegate[] Setters = properties.Select(x => CreateSetter(x)).ToArray();

        private static SetterDelegate CreateSetter(PropertyInfo property)
        {
            Type targetType = typeof(object);
            Type valueType = typeof(object);

            // 因為外部是允許傳入任何類型(所以製作object 可以讓任何類型傳入)
            ParameterExpression targetPram = Expression.Parameter(targetType, "obj");
            ParameterExpression valuePram = Expression.Parameter(valueType, "value");

            // 將類型轉換成他們自己骨子裡可以接收的真實函數
            UnaryExpression targetConvert = Expression.Convert(targetPram, property.DeclaringType);
            UnaryExpression valueConvert = Expression.Convert(valuePram, property.PropertyType);

            MethodCallExpression propertySetter = Expression.Call(targetConvert, property.GetSetMethod(), valueConvert);
            Expression<SetterDelegate> lambdaExpression = Expression.Lambda<SetterDelegate>(propertySetter, targetPram, valuePram);
            SetterDelegate setterDelegate = lambdaExpression.Compile();

            return setterDelegate;
        }

        [Benchmark]
        public void Read()
        {
            //for (int j = 0; j < 3500000; j++)
            //{
            //}

            List<CSVModel> list = new List<CSVModel>();

            string input = "18,Josias,Consterdine,jconsterdineh@scribd.com,Male,149.190.102.195";
            string[] data = input.Split(',');
            CSVModel model = new CSVModel();
            var properties = typeof(CSVModel).GetProperties();
            for (int i = 0; i < properties.Length; i++)
            {
                properties[i].SetValue(model, data[i]);
            }
            list.Add(model);
        }

        [Benchmark]
        public void OptimizeRead()
        {
            List<CSVModel> list = new List<CSVModel>();

            string input = "18,Josias,Consterdine,jconsterdineh@scribd.com,Male,149.190.102.195";
            ReadOnlySpan<char> span = input.AsSpan();
            int start = 0;
            int field = 0;
            CSVModel model = new CSVModel();

            while (true)
            {
                int commaIndex = span.Slice(start).IndexOf(',');
                if (commaIndex == -1)
                {
                    //var lastString =
                    Setters[field](model, span.Slice(start).ToString());
                    break;
                }
                else
                {
                    //var spanString = ;
                    Setters[field++](model, span.Slice(start, commaIndex).ToString());
                    //field++;
                    start += commaIndex + 1;// 下一筆的起點
                }
                //var index = field - 1;
                //Setters[index](model, datas[index]);
            }
            list.Add(model);
        }

        //private readonly Action<PropertyInfo, CSVModel, string> setter = (property, model, data) =>
        //{
        //    property.SetValue(model, data);
        //};

        //public CSVModel(string s1,string s2, string s3, string s4, string s5, string s6)
        //{
        // Action<object, object> setter = xxx.SetValue;
        // setter(model,datas[i])
        //CSVModel result = new CSVModel();
        //properties[0].SetValue(result, s1);
        //properties[0].SetValue(result, s1);
        //setter()
        //}
    }
}