using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SQLiteApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataAccess.InitializeDatabase();

            RefreshID();
            
        }




        private void Btn_AddData_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_firstName.Text))                   //ถ้าในกล่องข้อความว่างเปล่า
            {
                MessageBox.Show("Please input 'FIRSTNAME' ");
            }
            else if (String.IsNullOrEmpty(tb_lastName.Text))
            {
                MessageBox.Show("Please input 'LASTNAME' ");
            }
            else if(String.IsNullOrEmpty(tb_email.Text))
            {
                MessageBox.Show("Please input 'EMAIL' ");
            }
            else
            {
                int keyCheck = 1;
                foreach (string list in DataAccess.GetData())
                {

                    if (list == tb_Uid.Text)
                    {

                        keyCheck = 2;
                        MessageBox.Show("User ID : " + list + " already exist");
                        break;
                    }
                }
                if (keyCheck == 1)
                {
                    DataAccess.AddData(tb_Uid.Text, tb_firstName.Text, tb_lastName.Text, tb_email.Text);
                    MessageBox.Show("added ID " + tb_Uid.Text + " done");
                }
                tb_firstName.Clear();
                tb_lastName.Clear();
                tb_email.Clear();
                RefreshID();
            }

            
        }




        private void Btn_Show_Click(object sender, RoutedEventArgs e)
        {

            string data = "";
            int counter = 0;
            foreach (string list in DataAccess.GetData())
            {
                counter = counter + 1;
                data = data + list + "\n";
            }
            
            if(counter == 0)
            {
                MessageBox.Show("No member data found", "Member detail");
            }
            else
            {
                MessageBox.Show(data, "Member detail");
            }
            

            RefreshID();
        }




        private void Btn_remove_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(tb_idRemove.Text)) //ถ้าในกล่องข้อความว่างเปล่า
            {
                MessageBox.Show("Please input ID to remove");
            }
            else
            {
                DataAccess.RemoveAllData(tb_idRemove.Text);
                MessageBox.Show("Removed ID : " + tb_idRemove.Text);
                tb_idRemove.Clear();
            }
            
            RefreshID();
        }





        private void RefreshID()
        {
            int UidCounting = 0;
            foreach (string list in DataAccess.GetData())
            {
                UidCounting = UidCounting + 1;
            }
            UidCounting = (UidCounting/2) + 1;  // เพราะ 1 ข้อมูล ทำเป็น 2 บรรทัด มี ชื่อบรรทัดนึง อีเมลบรรทัดนึง
            tb_Uid.Text = UidCounting.ToString();
        }
    }
}
