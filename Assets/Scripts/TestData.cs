using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestData : MonoBehaviour
{
    void Start()
    {
        int dayOfYear = GetWeekOfYear(DateTime.Today)-1;
        Tuple<DateTime, DateTime> tuple= GetFirstEndDayOfWeek(DateTime.Now.Year, dayOfYear, System.Globalization.CultureInfo.InvariantCulture);
        Debug.Log(tuple.Item1);
        Debug.Log(tuple.Item2);
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
}
