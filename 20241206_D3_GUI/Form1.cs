using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _20241206_D3_GUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
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
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("這是about");
        }
    }
}
