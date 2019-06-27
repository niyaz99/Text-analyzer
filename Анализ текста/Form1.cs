using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace Анализ_текста
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //диаграмма
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = true; // по оси X
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = true; //сеткa по оси Y
            openFileDialog1.Filter = " Текстовые файлы(*.txt)|*.txt|Microsoft Word(*.doc;*docx)|*.doc;*.docx|Все файлы(*)|*.*";
        }
        

        List<char> letters = new List<char>(); //массив символов
        List<int> count = new List<int>();// массив количества символов
        List<string> words = new List<string>(); //массив слов
        List<int> cw = new List<int>();// массив количества слов
        int m = 0;
        int k = 0;
        public void sort()// сортировка по количеству (символы)
        {
            for (int i = 0; i < count.Count-1; i++)
            {
                for (int j = 0; j < count.Count - i - 1; j++)
                {
                    if (count[j] < count[j + 1])
                    {
                        int t1 = count[j];
                        count[j] = count[j+1];
                        count[j + 1] = t1;
                        char t2 = letters[j];
                        letters[j] = letters[j + 1];
                        letters[j + 1] = t2;
                    }
                }
            }
        }
        public void sortw()// сортировка по количеству (слова)
        {
            for (int i = 0; i < cw.Count - 1; i++)
            {
                for (int j = 0; j < cw.Count - i - 1; j++)
                {
                    if (cw[j] < cw[j + 1])
                    {
                        int t1 = cw[j];
                        cw[j] = cw[j + 1];
                        cw[j + 1] = t1;
                        string t2 = words[j];
                        words[j] = words[j + 1];
                        words[j + 1] = t2;
                    }
                }
            }
        }
        //кнопка "Анализ текста"
        private void button2_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
                label3.Text = "Нет текста для анализа";
            else
                label3.Text = "";
            //анализируем символы
            letters.Clear();
            count.Clear();
            string s = textBox1.Text;
            label1.Text = "Количество символов: " + s.Length;
            s = s.ToLower();
            for (int i = 0; i < s.Length; i++)
            {
                if (letters.Exists(x => x.Equals(s[i])))
                {
                    m = letters.IndexOf(s[i]);
                }
                else
                {
                    m = -1;
                }
                if (m >= 0)
                {
                    count[m] = count[m] + 1;
                }
                else
                {
                    letters.Add(s[i]);
                    count.Add(1);
                }
            }
            textBox2.Text = "";
            sort();
            for (int i = 0; i < letters.Count; i++)
            {
                textBox2.AppendText(letters[i] + " встречается " + count[i] + " раз \r\n");
            }
            //анализируем слова
            string[] w = s.Split(new[] { ' ', ',', ':', '?', '!', '—', '—', '\n', '-' }, StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < w.Length; i++)
            {
                if (words.Exists(x => x.Equals(w[i])))
                {
                    k = words.IndexOf(w[i]);
                }
                else
                {
                    k = -1;
                }
                if (k >= 0)
                {
                    cw[k] = cw[k] + 1;
                }
                else
                {
                    words.Add(w[i]);
                    cw.Add(1);
                }
            }
            textBox5.Text = "";
            sortw();
            for (int i = 0; i < words.Count; i++)
            {
                textBox5.AppendText(words[i] + " встречается " + cw[i] + " раз \r\n");
            }
            button5_Click(sender, e);
        }
        //кнопка "Очистить"
        private void button3_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            letters.Clear();
            count.Clear();
            m = 0;
            textBox5.Text = "";
            words.Clear();
            cw.Clear();
            k = 0;
        }
        //кнопка "Очистить текст"
        private void button4_Click(object sender, EventArgs e)
        {
            textBox1.Text = "";
            letters.Clear();
            count.Clear();
            m = 0;
        }
        //кнопка "Построить график"
        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox1.Text == "")
            {
                label3.Text = "Нет текста для построения графика";
                return;
            }
            else
                label3.Text = "";

            chart1.ChartAreas[0].AxisX.Interval = 1;
            chart1.ChartAreas[0].CursorX.IsUserEnabled = true;
            chart1.ChartAreas[0].CursorX.IsUserSelectionEnabled = true;
            chart1.ChartAreas[0].AxisX.ScaleView.Zoomable = true;
            chart1.ChartAreas[0].AxisX.ScrollBar.IsPositionedInside = false;

            if (checkBox2.Checked)
            {
                chart1.Series["test"].Points.Clear();
                chart1.Series["words"].Points.Clear();
                for (int i = 0; i < words.Count; i++)
                {
                    if ((textBox3.Text == "" || i < Convert.ToInt32(textBox3.Text)) && (textBox4.Text == "" || cw[i] > Convert.ToInt32(textBox4.Text)))
                    {
                        chart1.Series["words"].Points.AddXY("'" + words[i] + "'", cw[i]);
                        chart1.Series["words"].Points.ElementAt(i).ToolTip = "'" + Convert.ToString(words[i]) + "'";
                    }
                    else
                    {
                        break;
                    }
                }
                chart1.ChartAreas[0].AxisX.ScaleView.Size = 10;
            }
            else
            {
                if (count.Any())
                {
                    chart1.ChartAreas[0].AxisY.Minimum = 0; // настройка минимума и максимума оси X
                                                            //chart1.ChartAreas[0].AxisY.Maximum = count[0];
                    chart1.Series["test"].Points.Clear();
                    chart1.Series["words"].Points.Clear();
                    int u = 0;
                    for (int i = 0; i < letters.Count; i++)
                    {
                        if ((textBox3.Text == "" || i - u < Convert.ToInt32(textBox3.Text)) && (textBox4.Text == "" || count[i] > Convert.ToInt32(textBox4.Text)))
                        {
                            if (!checkBox1.Checked)
                            {
                                chart1.Series["test"].Points.AddXY("'" + letters[i] + "'", count[i]);
                                chart1.Series["test"].Points.ElementAt(i).ToolTip = "'" + Convert.ToString(letters[i]) + "'";
                            }
                            else
                            {
                                if (Char.IsLetterOrDigit(letters[i]))
                                {

                                    chart1.Series["test"].Points.AddXY("'" + letters[i] + "'", count[i]);

                                    chart1.Series["test"].Points.ElementAtOrDefault(i - u).ToolTip = "'" + Convert.ToString(letters[i]) + "'";
                                }
                                else
                                    u++;
                            }
                        }
                        else
                            break;
                    }                   
                }
                chart1.ChartAreas[0].AxisX.ScaleView.Size = 20;
            }          
        }
        //кнопка "Открыть файл"
        private void button1_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                string filename = openFileDialog1.FileName;
                string fileText = System.IO.File.ReadAllText(filename);
                textBox1.Text = fileText;
            }
            if (textBox1.Text == "")
            {
                label3.Text = "Не удалось открыть файл";
                return;
            }
            else
                label3.Text = "";
        }

        private void checkBox2_CheckedChanged(object sender, EventArgs e)
        {
            //если выбран checkBox "Только слова", отключаем checkBox "Исключить знаки препинания"
            if (checkBox2.Checked)
            {
                checkBox1.Enabled = false;
            }
            else
                checkBox1.Enabled = true;
        }

    }
}
