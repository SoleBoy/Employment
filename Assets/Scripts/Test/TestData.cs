using RefactoringGuru.DesignPatterns.Builder.Conceptual;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class TestData : MonoBehaviour
{
    class Class1
    {
        internal static int count = 0;
        static Class1()
        {
            count++;
            Debug.Log("static" + count);
        }
        public Class1()
        {
            count++;
            Debug.Log(count);
        }
    }


    public class A
    {
        public A()
        {
            PrintFields();
        }
        public virtual void PrintFields() { }
    }
    public class B : A
    {
        int x = 1;
        int y;
        public B()
        {
            y = -1;
            Debug.Log("wozhix:"+y);
        }
        public override void PrintFields()
        {
            Debug.Log(string.Format("x={0},y={1}", x, y));
        }
    }

    Material material;

    void Start()
    {
        //for (int i = 0; i < 30; i++)
        //{
        //   //Debug.Log(GetNumberAtPos(i));
        //   Debug.Log(getNumber(i));
        //}
        Class1 class1 = new Class1();
        Class1 class2 = new Class1();
        B aaaa = new B();
        

        string strTmp = "a1某某某";
        var asda = System.Text.Encoding.Default.GetBytes(strTmp);
        for (int i = 0; i < asda.Length; i++)
        {
            Debug.Log(asda[i].ToString());
        }
       
        Debug.Log(strTmp.Length);

        //int? a = 1;
        //material = GetComponent<Renderer>().material;
        //material.DOFloat(-0.5f, "_DisappearOffset", 1);
        //Debug.Log(GetR());

        //var director = new Director();
        //var builder = new ConcreteBuilder();
        //director.Builder = builder;

        //Debug.Log("Standard basic product:");
        //director.BuildMinimalViableProduct();
        //Debug.Log(builder.GetProduct().ListParts());

        //Debug.Log("Standard full featured product:");
        //director.BuildFullFeaturedProduct();
        //Debug.Log(builder.GetProduct().ListParts());

        //Debug.Log("Custom product:");
        //builder.BuildPartA();
        //builder.BuildPartC();
        //Debug.Log(builder.GetProduct().ListParts());

        //Button button=GetComponent<Button>();
        //button.onClick.AddListener(()=> { });
    }
    public int GetNumberAtPos(int pos)
    {
        if (pos == 0 || pos == 1)
        {
            return 1;
        }
        int res = GetNumberAtPos(pos - 1) + GetNumberAtPos(pos - 2);
        return res;
    }

    public int getNumber(int pos)
    {
        int one = 1;
        int two = 1;
        if (pos == 0 || pos == 1)
        {
            return 1;
        }
        int i = 3;
        int sum = 1;
        while (i <= pos)
        {
            sum = one + two;
            one = two;
            two = sum;
            i++;
        }
        return sum;
    }
    /// <summary>
    /// 获取一年中的周
    /// </summary>
    /// <param name="dt">日期</param>
    /// <returns></returns>
    public static int GetWeekOfYear(DateTime dt)
    {
        System.Globalization.GregorianCalendar gc = new System.Globalization.GregorianCalendar();
        int weekOfYear = gc.GetWeekOfYear(dt, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

        return weekOfYear;
    }
    /// <summary>
    /// 根据一年中的第几周获取该周的开始日期与结束日期
    /// </summary>
    /// <param name="year"></param>
    /// <param name="weekNumber"></param>
    /// <param name="culture"></param>
    /// <returns></returns>
    public static Tuple<DateTime, DateTime> GetFirstEndDayOfWeek(int year, int weekNumber, System.Globalization.CultureInfo culture)
    {
        System.Globalization.Calendar calendar = culture.Calendar;
        DateTime firstOfYear = new DateTime(year, 1, 1, calendar);
        DateTime targetDay = calendar.AddWeeks(firstOfYear, weekNumber - 1);
        DayOfWeek firstDayOfWeek = culture.DateTimeFormat.FirstDayOfWeek;

        while (targetDay.DayOfWeek != firstDayOfWeek)
        {
            targetDay = targetDay.AddDays(-1);
        }

        return Tuple.Create<DateTime, DateTime>(targetDay, targetDay.AddDays(6));
    }

    public bool GetR()
    {
        string strA = "Hello ";

        string strB = "Hello ";

        strA = strA + "C";

        strB = strB + "C";

        if (System.Object.ReferenceEquals(strA, strB))
        {
            return true;
        }
        Debug.Log("strA:" + strA+ "strB:" + strB);
        if (strA == strB)
        {
            return false;
        }

        return true;
    }
}



namespace RefactoringGuru.DesignPatterns.Builder.Conceptual
{
    // The Builder interface specifies methods for creating the different parts
    // of the Product objects.
    public interface IBuilder
    {
        void BuildPartA();

        void BuildPartB();

        void BuildPartC();
    }

    // The Concrete Builder classes follow the Builder interface and provide
    // specific implementations of the building steps. Your program may have
    // several variations of Builders, implemented differently.
    public class ConcreteBuilder : IBuilder
    {
        private Product _product = new Product();

        // A fresh builder instance should contain a blank product object, which
        // is used in further assembly.
        public ConcreteBuilder()
        {
            this.Reset();
        }

        public void Reset()
        {
            this._product = new Product();
        }

        // All production steps work with the same product instance.
        public void BuildPartA()
        {
            this._product.Add("PartA1");
        }

        public void BuildPartB()
        {
            this._product.Add("PartB1");
        }

        public void BuildPartC()
        {
            this._product.Add("PartC1");
        }

        // Concrete Builders are supposed to provide their own methods for
        // retrieving results. That's because various types of builders may
        // create entirely different products that don't follow the same
        // interface. Therefore, such methods cannot be declared in the base
        // Builder interface (at least in a statically typed programming
        // language).
        //
        // Usually, after returning the end result to the client, a builder
        // instance is expected to be ready to start producing another product.
        // That's why it's a usual practice to call the reset method at the end
        // of the `GetProduct` method body. However, this behavior is not
        // mandatory, and you can make your builders wait for an explicit reset
        // call from the client code before disposing of the previous result.
        public Product GetProduct()
        {
            Product result = this._product;

            this.Reset();

            return result;
        }
    }

    // It makes sense to use the Builder pattern only when your products are
    // quite complex and require extensive configuration.
    //
    // Unlike in other creational patterns, different concrete builders can
    // produce unrelated products. In other words, results of various builders
    // may not always follow the same interface.
    public class Product
    {
        private List<object> _parts = new List<object>();

        public void Add(string part)
        {
            this._parts.Add(part);
        }

        public string ListParts()
        {
            string str = string.Empty;

            for (int i = 0; i < this._parts.Count; i++)
            {
                str += this._parts[i] + ", ";
            }

            str = str.Remove(str.Length - 2); // removing last ",c"

            return "Product parts: " + str + "\n";
        }
    }

    // The Director is only responsible for executing the building steps in a
    // particular sequence. It is helpful when producing products according to a
    // specific order or configuration. Strictly speaking, the Director class is
    // optional, since the client can control builders directly.
    public class Director
    {
        private IBuilder _builder;

        public IBuilder Builder
        {
            set { _builder = value; }
        }

        // The Director can construct several product variations using the same
        // building steps.
        public void BuildMinimalViableProduct()
        {
            this._builder.BuildPartA();
        }

        public void BuildFullFeaturedProduct()
        {
            this._builder.BuildPartA();
            this._builder.BuildPartB();
            this._builder.BuildPartC();
        }
    }
}
