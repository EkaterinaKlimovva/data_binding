using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;
using System.Xml.Serialization;

// 0 вариант
// 2 вариант доп функциональности
// 2 вариант доп функциональности
namespace Практика_5
{
    public partial class Form1 : Form
    {
        BindingSource bs = new BindingSource();
        public Form1()
        {
            InitializeComponent();
            bs.DataSource = GetStartList();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = bs;

            var col1 = new DataGridViewTextBoxColumn
            {
                Width = 180,
                Name = "AutoName",
                HeaderText = "Модель",
                DataPropertyName = "AutoName"
            };
            dataGridView1.Columns.Add(col1);

            var col2 = new DataGridViewTextBoxColumn
            {
                Width = 100,
                Name = "Country",
                HeaderText = "Страна производства",
                DataPropertyName = "Country"
            };
            dataGridView1.Columns.Add(col2);

            var col3 = new DataGridViewTextBoxColumn
            {
                Width = 80,
                Name = "Year",
                HeaderText = "Год выпуска",
                DataPropertyName = "Year"
            };
            dataGridView1.Columns.Add(col3);

            var col4 = new DataGridViewTextBoxColumn
            {
                Width = 100,
                Name = "Cost",
                HeaderText = "Стоимость",
                DataPropertyName = "Cost",
            };
            dataGridView1.Columns.Add(col4);

            var col5 = new DataGridViewComboBoxColumn
            {
                Width = 100,
                Name = "Groups",
                HeaderText = "Категория",
                DataPropertyName = "Groups",
                DataSource = Enum.GetValues(typeof(eGroup))
            };
            dataGridView1.Columns.Add(col5);

            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.DataBindings.Add("ImageLocation", bs, "img", true);
            pictureBox1.DoubleClick += ClickPicture;
            bindingNavigator1.BindingSource = bs;

            chart1.DataSource = from w in bs.DataSource as List<Auto>
                                group w by w.StrGroup into g
                                select new { Groups = g.Key, Avg = g.Average(w => w.Cost) };
            chart1.Series[0].XValueMember = "Groups";
            chart1.Series[0].YValueMembers = "Avg";
            chart1.Legends.Clear();
            propertyGrid1.DataBindings.Add("SelectedObject", bs, "");
            DataBindings.Add("Text", bs, "AutoName");
            chart1.Titles.Add("Средняя стоимость по категории");
            bs.CurrentChanged += (o, e) => chart1.DataBind();
        }
        public void ClickPicture(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                InitialDirectory = Environment.CurrentDirectory,
                Filter = "Картинка в формате jpg|*.jpg|Картинка в формате jpeg|*.jpeg"
            };
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                (bs.Current as Auto).Img = ofd.FileName;
                bs.ResetBindings(false);
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch (ofd.FilterIndex)
                {
                    case 1:
                        BinaryFormatter bin = new BinaryFormatter();
                        using (Stream sw = new FileStream(ofd.FileName, FileMode.Open))
                        {
                            bs.DataSource = (List<Auto>)bin.Deserialize(sw);
                        }
                        break;
                    case 2:
                        XmlSerializer ser = new XmlSerializer(typeof(List<Auto>));
                        using (Stream sw = new FileStream(ofd.FileName, FileMode.Open))
                        {
                            bs.DataSource = (List<Auto>)ser.Deserialize(sw);
                        }
                        break;
                }
            }
        }

        private void BinarySerialize(string f)
        {
            BinaryFormatter b = new BinaryFormatter();
            Stream s = new FileStream(f, FileMode.Create);
            {
                b.Serialize(s, bs.DataSource);
            }
            s.Close();
        }

        private void SaveXml(string f)
        {
            XmlSerializer s = new XmlSerializer(typeof(List<Auto>));
            using (Stream sw = new FileStream(f, FileMode.Create))
            {
                s.Serialize(sw, bs.DataSource);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

            SaveFileDialog ofd = new SaveFileDialog();
            ofd.InitialDirectory = System.Environment.CurrentDirectory;
            ofd.Filter = "Файл в bin|*.bin|Файл в xml|*.xml";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                switch (ofd.FilterIndex)
                {
                    case 1:
                        BinarySerialize(ofd.FileName);
                        break;
                    case 2:
                        SaveXml(ofd.FileName);
                        break;
                }
            }
        }
        
        private void MoreCost(object sender, EventArgs e)
        {
            int n;

            if (toolStripTextBox1.Text != "")
            {
                if (!Int32.TryParse(toolStripTextBox1.Text, out n))
                {
                    foreach (var v in dataGridView1.Rows)
                    {
                        foreach (DataGridViewRow row in dataGridView1.Rows)
                        {
                            var f = (string)row.Cells[1].Value;

                            if (row.Cells[1].Value != null && (!toolStripTextBox1.Text.Contains((string)row.Cells[1].Value)))
                            {
                                dataGridView1.CurrentCell = null;
                                row.Visible = false;
                            }
                            if ((row.Cells[1].Value != null && (toolStripTextBox1.Text.Contains((string)row.Cells[1].Value))))
                                row.Visible = true;
                        }
                    }
                }
                else
                {
                    if (Int32.Parse(toolStripTextBox1.Text) < 2023)
                    {
                        foreach (var v in dataGridView1.Rows)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells[2].Value != null && (int)row.Cells[2].Value <= Int32.Parse(toolStripTextBox1.Text))
                                {
                                    dataGridView1.CurrentCell = null;
                                    row.Visible = false;
                                }
                                if (row.Cells[2].Value != null && (int)row.Cells[2].Value > Int32.Parse(toolStripTextBox1.Text))
                                    row.Visible = true;
                            }
                        }
                    }
                    else
                    {
                        foreach (var v in dataGridView1.Rows)
                        {
                            foreach (DataGridViewRow row in dataGridView1.Rows)
                            {
                                if (row.Cells[3].Value != null && (int)row.Cells[3].Value <= Int32.Parse(toolStripTextBox1.Text))
                                {
                                    dataGridView1.CurrentCell = null;
                                    row.Visible = false;
                                }
                                if (row.Cells[3].Value != null && (int)row.Cells[3].Value > Int32.Parse(toolStripTextBox1.Text))
                                    row.Visible = true;
                            }
                        }
                    }
                }
            }
        }

        private List<Auto> GetStartList() => new List<Auto>
        {
            new Auto("Renault Logan", "Румыния", 2004, 1200000, eGroup.Легковые, "../../img/logan.png"),
            new Auto("Toyota Corolla", "Япония", 1966, 2300000, eGroup.Легковые, "../../img/corolla.jpg"),
            new Auto("КамАЗ-6520", "Россия", 2003, 6600000, eGroup.Грузовые, "../../img/kamaz.png"),
            new Auto("Honda VFR800", "Япония", 1998, 990000, eGroup.Мотоциклы, "../../img/honda.jpeg"),
            new Auto("Volkswagen Polo", "Германия", 1975, 1900000, eGroup.Легковые, "../../img/polo.jpg"),
            new Auto("Audi A4", "Германия", 1994, 6600000, eGroup.Легковые, "../../img/audi.jpg"),
            new Auto("ГАЗ-51", "Россия", 1946, 300000, eGroup.Грузовые, "../../img/gaz.jpeg")
        };

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }
    }
}
