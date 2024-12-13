using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SQLite;

namespace _20241206_D3_GUI
{
    public partial class Form1 : Form
    {
        int index = 1;

        public class DBConfig
        {
            //log.db要放在【bin\Debug底下】      
            public static string dbFile = Application.StartupPath + @"\log.db";

            public static string dbPath = "Data source=" + dbFile;

            public static SQLiteConnection sqlite_connect;
            public static SQLiteCommand sqlite_cmd;
            public static SQLiteDataReader sqlite_datareader;
        }

        private void Load_DB()
        {
            DBConfig.sqlite_connect = new SQLiteConnection(DBConfig.dbPath);
            DBConfig.sqlite_connect.Open();// Open

        }




        private void Show_DB()
        {
            this.dataGridView1.Rows.Clear();

            string sql = @"SELECT * from record;";
            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_datareader = DBConfig.sqlite_cmd.ExecuteReader();

            //把db資料讀進來
            if (DBConfig.sqlite_datareader.HasRows)
            {
                while (DBConfig.sqlite_datareader.Read()) //read every data
                {
                    int _serial = Convert.ToInt32(DBConfig.sqlite_datareader["serial"]);
                    int _date = Convert.ToInt32(DBConfig.sqlite_datareader["date"]);
                    int _type = Convert.ToInt32(DBConfig.sqlite_datareader["type"]);
                    string _name = Convert.ToString(DBConfig.sqlite_datareader["name"]);
                    double _price = Convert.ToDouble(DBConfig.sqlite_datareader["price"]);
                    double _number = Convert.ToDouble(DBConfig.sqlite_datareader["number"]);
                    double _total = _price * _number;

                    //轉換date格式
                    string _date_str = DateTimeOffset.FromUnixTimeSeconds(_date).ToString("yy-MM-dd hh:mm:ss");

                    string _type_str = "";

                    //數字轉換進出貨
                    if (_type == 0)
                    { _type_str = "進貨"; }
                    else { _type_str = "出貨"; }

                    index = _serial;
                    DataGridViewRowCollection rows = dataGridView1.Rows;
                    rows.Add(new Object[] { index, _date_str, _type_str, _name, _price, _number
                                               , _total });
                }
                DBConfig.sqlite_datareader.Close();
            }
        }

        public Form1()
        {
            InitializeComponent();

            Load_DB();
            Show_DB();
            this.label5.Text = index.ToString();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            string _name = "";
            long _date = 0;
            int _stock_type = 0;
            double _price = 0;
            double _number = 0;
            double _sum = 0;

            // 抓取textbox的資料
            _name = comboBox1.Text;
            _price = Convert.ToDouble(textBox1.Text);
            _number = Convert.ToDouble(textBox2.Text);

            _sum = _price * _number;
            _date = DateTimeOffset.Now.ToUnixTimeSeconds();
            if (radioButton1.Checked == true)
            {
                _stock_type = 0;
            }
            else
            {
                _stock_type = 1;
            }
            // update
            this.index = this.index + 1;

            // add item into database
            //以下需要改進改進建議：使用參數化查詢
            //參數化查詢可以避免 SQL 注入，並提高代碼的安全性和可讀性。
            //但總之以後再說
            string sql = @"INSERT INTO record (date, type, name,price,number)
                VALUES( "
                       + " '" + _date.ToString() + "' , "
                       + " '" + _stock_type.ToString() + "' , "
                       + " '" + _name.ToString() + "' , "
                       + " '" + _price.ToString() + "' , "
                       + " '" + _number.ToString() + "'   "
                      + ");";
            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_cmd.ExecuteNonQuery();

            // show database in the gui
            Show_DB();

            /* 換成結合DB
            string i_input_price = this.textBox1.Text;//儲存定價欄位
            string i_input_num = this.textBox2.Text;  //儲存數量欄位
            double _price = Convert.ToDouble(i_input_price);//將以下兩字串轉數字
            double _num = Convert.ToDouble(i_input_num);
            //richTextBox1.Text = (_price * _num).ToString(); //將轉完的數字進行運算，到richbox秀出結果

            String _radiobuttom_log = "";   //richbox秀是否進貨
            if (radioButton1.Checked == true)
            {
                _radiobuttom_log = "進貨";
            }
            else
            {
                _radiobuttom_log = "出貨";
            }
            //richTextBox1.Text = String.Format("{0} : {1}",(_price * _num),_radiobuttom_log); 進度到radiobuttom版

            String _combobox_log = comboBox1.SelectedItem.ToString();//儲存選單裡選擇的item

                richTextBox1.Text = String.Format("{0} : {1} {2}"
                    , (_price * _num), _radiobuttom_log, _combobox_log);

            DataGridViewRowCollection rows = dataGridView1.Rows;
            DateTime date = DateTime.Now;                                   //現在時間
            rows.Add(new Object[] { "", date.ToString("yyyy/MM/dd HH:mm:ss")//時間 進出貨 貨品 價格 數量 總價
                , _radiobuttom_log, _combobox_log, _price, _num, _price * _num });
            */
        }

        private void button3_Click(object sender, EventArgs e)
        {
            string _name = "";
            int _serial = 0;
            int _stock_type = 0;
            double _price = 0;
            double _number = 0;

            if (radioButton1.Checked == true)
            {
                _stock_type = 0;
            }
            else
            {
                _stock_type = 1;
            }

            // 抓取textbox的資料
            _name = comboBox1.Text;


            _price = Convert.ToDouble(textBox1.Text);
            _number = Convert.ToDouble(textBox2.Text);
            _serial = Convert.ToInt32(label5.Text);

            //以下需要改進改進建議：使用參數化查詢
            //參數化查詢可以避免 SQL 注入，並提高代碼的安全性和可讀性。
            //但總之以後再說

            string sql = @"UPDATE record " +
                      " SET name = '" + _name + "',"
                        + " type = '" + _stock_type.ToString() + "' , "
                        + " price = '" + _price.ToString() + "',"
                        + " number = '" + _number.ToString() + "' "
                        + "   where serial = " + _serial.ToString() + ";";


            DBConfig.sqlite_cmd = new SQLiteCommand(sql, DBConfig.sqlite_connect);
            DBConfig.sqlite_cmd.ExecuteNonQuery();
            Show_DB();

        }


        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("這是about");
        }

        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            DataGridViewCellCollection selRowData = dataGridView1.Rows[e.RowIndex].Cells;

            string _type = "";
            _type = Convert.ToString(selRowData[2].Value);

            if (_type.Equals("進貨"))
            {
                radioButton1.Checked = true;
            }
            else
            {
                radioButton2.Checked = true;
            }


            this.comboBox1.Text = Convert.ToString(selRowData[3].Value);
            this.textBox1.Text = Convert.ToString(selRowData[4].Value);
            this.textBox2.Text = Convert.ToString(selRowData[5].Value);
            this.label5.Text = Convert.ToString(selRowData[0].Value);

        }

        
    }
}
