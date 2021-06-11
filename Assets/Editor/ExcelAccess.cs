using UnityEngine;
using Excel;
using System.Data;
using System.IO;
using System.Collections.Generic;
using OfficeOpenXml;

public class ExcelAccess
{
    public static string Excel = "config";

    //查询数据表
    public static List<ItemData> SelectMenuLevel()
    {
        string excelName = Excel + ".xlsx";
        DataRowCollection collect = ExcelAccess.ReadExcel(excelName,0);

        List<ItemData> menuArray = new List<ItemData>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;

            ItemData level = new ItemData
            {
                id= collect[i][0].ToString(),
                type = collect[i][1].ToString(),
                realname = collect[i][2].ToString(),
                attack = collect[i][3].ToString(),
            };
            menuArray.Add(level);
        }
        return menuArray;
    }
    //查询角色数据
    public static List<RoleData> SelectRoleItem()
    {
        DataRowCollection collect = ExcelAccess.ReadExcel("GameValue.xlsx", 0);

        List<RoleData> menuArray = new List<RoleData>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;
            RoleData data = new RoleData
            {
                id = collect[i][0].ToString(),
                name = collect[i][1].ToString(),
                quality = collect[i][2].ToString(),
                exp = collect[i][3].ToString(),
                exp_upgrade = collect[i][4].ToString(),
                hp = collect[i][5].ToString(),
                hp_upgrade = collect[i][6].ToString(),
                attack = collect[i][7].ToString(),
                attack_upgrade = collect[i][8].ToString(),
                def = collect[i][9].ToString(),
                def_upgrade = collect[i][10].ToString(),
                talentId = collect[i][11].ToString(),
            };
            menuArray.Add(data);
        }
        return menuArray;
    }
    //查询物品种类
    public static List<TypeData> SelectTypeItem()
    {
        DataRowCollection collect = ExcelAccess.ReadExcel("GameValue.xlsx", 1);

        List<TypeData> menuArray = new List<TypeData>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;
            TypeData data = new TypeData
            {
                id = collect[i][0].ToString(),
                name = collect[i][1].ToString(),
                itemId = collect[i][2].ToString(),
                itemNum = collect[i][3].ToString(),
                itemInfo = collect[i][4].ToString(),
            };
            menuArray.Add(data);
        }
        return menuArray;
    }
    //查询天赋
    public static List<TalentData> SelectTalentItem()
    {
        DataRowCollection collect = ExcelAccess.ReadExcel("GameValue.xlsx", 2);

        List<TalentData> menuArray = new List<TalentData>();
        for (int i = 1; i < collect.Count; i++)
        {
            if (collect[i][1].ToString() == "") continue;
            TalentData data = new TalentData
            {
                id = collect[i][0].ToString(),
                name = collect[i][1].ToString(),
                probability = collect[i][2].ToString(),
                value = collect[i][3].ToString(),
                duration = collect[i][4].ToString(),
                info = collect[i][5].ToString(),
            };
            menuArray.Add(data);
        }
        return menuArray;
    }


    /// <summary>
    /// 读取 Excel ; 需要添加 Excel.dll; System.Data.dll;
    /// </summary>
    /// <param name="excelName">excel文件名</param>
    /// <param name="sheetName">sheet名称</param>
    /// <returns>DataRow的集合</returns>
    static DataRowCollection ReadExcel(string excelName,int sheetName)
    {
        string path= Application.dataPath + "/Excel/" + excelName;
        FileStream stream = File.Open(path, FileMode.Open, FileAccess.Read, FileShare.Read);
        IExcelDataReader excelReader = ExcelReaderFactory.CreateOpenXmlReader(stream);

        DataSet result = excelReader.AsDataSet();
        //int columns = result.Tables[0].Columns.Count;
        //int rows = result.Tables[0].Rows.Count;

        //tables可以按照sheet名获取，也可以按照sheet索引获取
       // return result.Tables[3].Rows;
        return result.Tables[sheetName].Rows;
    }

    /// <summary>
    /// 写入 Excel ; 需要添加 OfficeOpenXml.dll;
    /// </summary>
    /// <param name="excelName">excel文件名</param>
    /// <param name="sheetName">sheet名称</param>
    public static void WriteExcel(string excelName, string sheetName)
    {
        //通过面板设置excel路径
        //string outputDir = EditorUtility.SaveFilePanel("Save Excel", "", "New Resource", "xlsx");

        //自定义excel的路径
        string path = Application.dataPath + "/" + excelName;
        FileInfo newFile = new FileInfo(path);
        if (newFile.Exists)
        {
            //创建一个新的excel文件
            newFile.Delete();
            newFile = new FileInfo(path);
        }

        //通过ExcelPackage打开文件
        using (ExcelPackage package = new ExcelPackage(newFile))
        {
            //在excel空文件添加新sheet
            ExcelWorksheet worksheet = package.Workbook.Worksheets.Add(sheetName);
            //添加列名
            worksheet.Cells[1, 1].Value = "ID";
            worksheet.Cells[1, 2].Value = "Product";
            worksheet.Cells[1, 3].Value = "Quantity";
            worksheet.Cells[1, 4].Value = "Price";
            worksheet.Cells[1, 5].Value = "Value";

            //添加一行数据
            worksheet.Cells["A2"].Value = 12001;
            worksheet.Cells["B2"].Value = "Nails";
            worksheet.Cells["C2"].Value = 37;
            worksheet.Cells["D2"].Value = 3.99;
            //添加一行数据
            worksheet.Cells["A3"].Value = 12002;
            worksheet.Cells["B3"].Value = "Hammer";
            worksheet.Cells["C3"].Value = 5;
            worksheet.Cells["D3"].Value = 12.10;
            //添加一行数据
            worksheet.Cells["A4"].Value = 12003;
            worksheet.Cells["B4"].Value = "Saw";
            worksheet.Cells["C4"].Value = 12;
            worksheet.Cells["D4"].Value = 15.37;

            //保存excel
            package.Save();
        }
    }

}

