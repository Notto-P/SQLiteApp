using Microsoft.Data.Sqlite;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SQLiteApp
{
    class DataAccess
    {
        //method สำหรับสร้างฐานข้อมูลและตาราง
        public static void InitializeDatabase()  //method แบบ static จะสามารถเรียกใช้งานได้เลย ไม่ต้องสร้าง object ขึ้นมาก่อน
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db")) //สร้าง object ชื่อ db เพื่อสร้างเส้นทางติดต่อไปยังฐานข้อมูลที่ต้องการ (ในที่นี้ฐานข้อมูลชื่อว่า "sqliteSample"), .db คือชื่อสกุลไฟล์ของ database
            {
                db.Open();  //เปิดเส้นทางการเชื่อมต่อฐานข้อมูล

                //โค้ดส่วนของการเขียนเพื่อสร้าง tabale
                String tableCommand = "CREATE TABLE IF NOT " +   // มีการตรวจสอบว่ามี table ชื่อนี้อยู่แล้วหรือไม่(Customers)  
                    "EXISTS Customers (uid INTEGER PRIMARY KEY, " +  //ถ้ายังไม่มีให้สร้างขึ้น และมี field ชื่อ uid เป็น integer และเป็น primary key
                    "first_Name NVARCHAR(50) NOT NULL," + // field ชื่อ first_Name เก็บข้อมูลประเภท ตัวอักษร(จำกัด 50 ตัว) และต้องไม่เป็นค่าว่าง ...
                    "last_Name NVARCHAR(50) NOT NULL," +
                    "email NVARCHAR(50) NULL)";

                SqliteCommand createTable = new SqliteCommand(tableCommand, db); //สร้าง object ชื่อ createTable ขึ้นเพื่อเก็บคำสั่ง SQL และ conection

                createTable.ExecuteReader(); //ทำการ Execute 
            }
        }

        // method สำหรับเพิ่มข้อมูลลงในตาราง
        public static void AddData(string UserID,string firstName, string lastName, string email)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand insertCommand = new SqliteCommand();
                insertCommand.Connection = db;


                // Use parameterized query to prevent SQL injection attacks - ป้องกันการโจมตีฐานข้อมูล โดยให้ input ที่รับเข้ามาต้องไม่เป็น SQL command
                //insertCommand.CommandText = "INSERT INTO MyTable VALUES (NULL, @Entry);"; // การป้องกันทำได้โดยใส่ @ตัวแปรข้อมูล
                // insertCommand.Parameters.AddWithValue("@Entry", inputText);  // บรรทัดนี้จะเพิ่มเข้ามา , ตรง inputText คือข้อมูลที่จะใส่(ในที่นี้คือ input ที่รับเข้ามาใน method)

                insertCommand.CommandText = "INSERT INTO Customers VALUES ('" + UserID + "', @firstName, @lastName, @email);";
                insertCommand.Parameters.AddWithValue("@firstName", firstName);
                insertCommand.Parameters.AddWithValue("@lastName", lastName);
                insertCommand.Parameters.AddWithValue("@email", email);

                insertCommand.ExecuteReader();

                db.Close();
            }

        }

        // method สำหรับดึงข้อมูลในตารางมาใช้
        public static ArrayList GetData()  //method นี้ก็ return ค่าออกไปเป็น list
        {
            ArrayList entries = new ArrayList();  //สร้างตัวแปร list ขึ้นมา 1 ตัว เพื่อรับค่าที่ดึงออกมาจาก database โดยตัวอย่างนี้ list ถูกกำหนดให้รับแค่ข้อมูลประเภท String เท่านั้น

            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db"))
            {
                db.Open();

                SqliteCommand selectcommand = new SqliteCommand("SELECT uid, first_Name, last_Name, email from Customers", db);
                SqliteDataReader query = selectcommand.ExecuteReader();


                while (query.Read())
                {
                    entries.Add(query.GetString(0) + " : " + query.GetString(1) + " " + query.GetString(2) );
                    entries.Add(" Email : " + query.GetString(3) + "\n");
                }

                db.Close();
            }

            return entries;
        }


        public static void RemoveAllData (string UserId)
        {
            using (SqliteConnection db = new SqliteConnection($"Filename=sqliteSample.db")) //สร้าง object ชื่อ db เพื่อสร้างเส้นทางติดต่อไปยังฐานข้อมูลที่ต้องการ (ในที่นี้ฐานข้อมูลชื่อว่า "sqliteSample"), .db คือชื่อสกุลไฟล์ของ database
            {
                db.Open();  //เปิดเส้นทางการเชื่อมต่อฐานข้อมูล


                String removeCommand = "delete from Customers where uid = '" + UserId + "' ";

                SqliteCommand removeData = new SqliteCommand(removeCommand, db); //สร้าง object ชื่อ createTable ขึ้นเพื่อเก็บคำสั่ง SQL และ conection

                removeData.ExecuteReader(); //ทำการ Execute 

                db.Close();
            }
        }
    }
}
