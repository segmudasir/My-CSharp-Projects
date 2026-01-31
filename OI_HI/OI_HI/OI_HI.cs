using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace My_Project
{
    public partial class Form34 : Form
    {
        int formheight;
        int formwidth;
        int counter=0;       
        int title_position;     
        List<float> X = new List<float>();
        List<float> Y = new List<float>();
        List<string> wellname = new List<string>();
        List<string> wellname2 = new List<string>();
        int temp2=0; 
        float x1, y1, x2, y2;
        float required_xmax, required_xmin, required_ymax, required_ymin;
        float e_xmax, e_xmin, e_ymax, e_ymin;

        string line_name;
        int series_count = 1;
        
        public Chart Chart3;
        public Series Series1;
        public Series Series2;
                      
        //   public Title Title1;        
        public Form34()
        {
            InitializeComponent();
            formheight = this.Height;
            formwidth = this.Width;
                    
            //textBox1.Text = formheight.ToString();
            //textBox2.Text = formwidth.ToString();
            dataGridView1.Columns[0].HeaderText = "X";
            dataGridView1.Columns[1].HeaderText = "Y";
            dataGridView1.Columns[2].HeaderText = "Well Name";

            chart1.ChartAreas[0].BorderDashStyle = ChartDashStyle.Solid;
            chart1.ChartAreas[0].BorderWidth = 3;
            chart1.ChartAreas[0].BorderColor = Color.Green;
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;

            //chart1.ChartAreas[0].AxisX.IsLogarithmic = true;
            chart1.ChartAreas[0].AxisX.Minimum = 0;
            chart1.ChartAreas[0].AxisX.Maximum = 300;
            chart1.ChartAreas[0].AxisX.Interval = 50;           
            //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0.#}";
            //chart1.ChartAreas[0].AxisX.MinorTickMark.Enabled = true;  

            //chart1.ChartAreas[0].AxisY.IsLogarithmic = true;
            chart1.ChartAreas[0].AxisY.Minimum = 0;
            chart1.ChartAreas[0].AxisY.Maximum = 1000;
            chart1.ChartAreas[0].AxisY.Interval = 100;                               
           //chart1.ChartAreas[0].AxisY.LabelStyle.Format = "{0.#}";
            //chart1.ChartAreas[0].AxisY.MinorTickMark.Enabled = true;
            //chart1.ChartAreas[0].AxisY.MinorTickMark.Interval = 20;

            
            Set_default_series2();

            GetChartTitle("Oxygen Index", 52, 98, 14, 0);
            GetChartTitle("Hydrogen Index", 2.5F, 50, 14, 2);
        }
   

        ///////////////////////////////Copy and Paste from Clip board///////////////////////////////////////////////
        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            //if user clicked Shift+Ins or Ctrl+V (paste from clipboard)
            if ((e.Shift && e.KeyCode == Keys.Insert) || (e.Control && e.KeyCode == Keys.V))
            {
                char[] rowSplitter = { '\r', '\n' };
                char[] columnSplitter = { '\t' };
                //get the text from clipboard
                IDataObject dataInClipboard = Clipboard.GetDataObject();
                string stringInClipboard = (string)dataInClipboard.GetData(DataFormats.Text);
                //split it into lines
                string[] rowsInClipboard = stringInClipboard.Split(rowSplitter, StringSplitOptions.RemoveEmptyEntries);
                //get the row and column of selected cell in grid
                int r = dataGridView1.SelectedCells[0].RowIndex;
                int c = dataGridView1.SelectedCells[0].ColumnIndex;
                //add rows into grid to fit clipboard lines
                if (dataGridView1.Rows.Count < (r + rowsInClipboard.Length))
                {
                    dataGridView1.Rows.Add(r + rowsInClipboard.Length - dataGridView1.Rows.Count + 1);
                }
                // loop through the lines, split them into cells and place the values in the corresponding cell.
                for (int iRow = 0; iRow < rowsInClipboard.Length; iRow++)
                {
                    //split row into cell values
                    string[] valuesInRow = rowsInClipboard[iRow].Split(columnSplitter);
                    //cycle through cell values
                    for (int iCol = 0; iCol < valuesInRow.Length; iCol++)
                    {
                        //assign cell value, only if it within columns of the grid
                        if (dataGridView1.ColumnCount - 1 >= c + iCol)
                        {
                            dataGridView1.Rows[r + iRow].Cells[c + iCol].Value = valuesInRow[iCol];
                        }
                    }
                }
            }
        }         
        /////////////////////////////////////Main Ends - Chart General////////////////////////////////////////////////////////
        //default series for making straight dash lines on loading
        
        private Series Get_default_series2(Chart chart, string lineName,  List<float> X8, List<float> Y8, int ct)
        {
            var series = new Series();
            chart.Series.Add(lineName);

            chart.Series[lineName].IsValueShownAsLabel = false;
            chart.Series[lineName].ChartType = SeriesChartType.Spline;
            //chart.Series[lineName].BorderDashStyle = ChartDashStyle.Dash;
            //chart.Series[lineName].Color = Color.DarkOrange;
            chart.Series[lineName].BorderWidth = 2;
            //chart.Series["Series"].MarkerBorderColor = System.Drawing.Color.Fuchsia;
            //chart.Series["Series"].MarkerColor = System.Drawing.Color.Blue;
            //chart.Series["Series"].MarkerSize = 6;

            //adding points to the
            for (int i = 0; i < ct; i++ )
            {
                chart.Series[lineName].Points.AddXY(X8[i], Y8[i]);
            }
                            
            chart.Series[lineName].IsVisibleInLegend = false;
            return series;
        }
        private Series GetSeries_points(Chart chart, string well_name, int nlp, int k)
        {
            var series = new Series();

            chart.Series.Add(well_name);
            chart.Series[well_name].IsValueShownAsLabel = false;
            chart.Series[well_name].ChartType = SeriesChartType.Point;

            chart.Series[well_name].BorderWidth = 3;
            //chart.Series["Series"].MarkerBorderColor = System.Drawing.Color.Fuchsia;
            //chart.Series[seri_name].MarkerColor = Color.Orchid;
            chart.Series[well_name].MarkerSize = 12;
            chart.Series[well_name].MarkerBorderColor = Color.Purple;
            //marker shapes
            if (k == 0 || k == 5 || k == 10 || k == 15 || k == 20)
            {
                chart.Series[well_name].MarkerStyle = MarkerStyle.Circle;
            }
            else if (k == 1 || k == 6 || k == 11 || k == 16 || k == 21)
            {
                chart.Series[well_name].MarkerStyle = MarkerStyle.Square;
            }
            else if (k == 2 || k == 7 || k == 12 || k == 17 || k == 22)
            {
                chart.Series[well_name].MarkerStyle = MarkerStyle.Star5;
            }
            else if (k == 3 || k == 8 || k == 13 || k == 18 || k == 23)
            {
                chart.Series[well_name].MarkerStyle = MarkerStyle.Triangle;
            }
            else if (k == 4 || k == 9 || k == 14 || k == 19 || k == 24)
            {
                chart.Series[well_name].MarkerStyle = MarkerStyle.Star4;
            }
            

            //marker color
            if (k == 0 || k == 9 || k == 13 || k == 17 || k == 21)
            {
                chart.Series[well_name].MarkerColor = Color.Fuchsia;
            }
            else if (k == 1 || k == 5 || k == 14 || k == 18 || k == 22)
            {
                chart.Series[well_name].MarkerColor = Color.Blue;
            }
            else if (k == 2 || k == 6 || k == 10 || k == 19 || k == 23)
            {
                chart.Series[well_name].MarkerColor = Color.Yellow;
            }
            else if (k == 3 || k == 7 || k == 11 || k == 15 || k == 24)
            {
                chart.Series[well_name].MarkerColor = Color.Green;
            }

            else if (k == 4 || k == 8 || k == 12 || k == 16 || k == 20)
            {
                chart.Series[well_name].MarkerColor = Color.Cyan;
            }
            
            else 
            {
                chart.Series[well_name].MarkerColor = Color.Black;
                
            }
      
            for (int i = temp2; i < nlp; i++ )
            {
                chart.Series[well_name].Points.AddXY(X[i], Y[i]);
            }
            temp2 = nlp;

            return series;
        }
        
        //////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////       
        private void button1_Click(object sender, EventArgs e)
        {
            temp2 = 0;
            button5.Enabled = true;
            button4.Enabled = true;
            clear_list_series_legend();           
            Set_default_series2();
            GetChartTitle("Oxygen Index", 48, 97.5F, 14, 0);
            GetChartTitle("Hydrogen Index", 3, 50, 14, 2);
            GetChartTitle("Legend", 92.5F, 30, 14, 0);
            List<float> tempX = new List<float>();
            List<float> tempY = new List<float>();
            List<int> nlp = new List<int>();
            title_position = 3;
            
            counter = 0;

            while (dataGridView1.Rows[counter].Cells[0].Value != null)
            {
                X.Add(float.Parse((dataGridView1.Rows[counter].Cells[0].Value).ToString()));
                Y.Add(float.Parse((dataGridView1.Rows[counter].Cells[1].Value).ToString()));
                wellname.Add((dataGridView1.Rows[counter].Cells[2].Value).ToString());
                counter++;
            }
            
            String temp = wellname[0];
            wellname2.Add(temp);

            for (int i = 1; i < counter; i++)
            {
                if (wellname[i] != temp)
                {
                    wellname2.Add(wellname[i]);
                   
                    nlp.Add(i);                   
                }
                temp = wellname[i];        
            }

            //dataGridView2.Rows.Add(wellname2[0]);

            for (int i = 1; i < wellname2.Count; i++ )
            {
               // dataGridView2.Rows.Add(wellname2[i], nlp[i-1]);
            }

            //this will start from 0. because we sent for first well name series starting from zero to nexline position (nlp).
            for (int i = 0; i < wellname2.Count; i++)
            {
                if(i==wellname2.Count-1)
                {
                    Series2 = GetSeries_points(chart1, wellname2[i], counter, i);
                }
                else 
                    Series2 = GetSeries_points(chart1, wellname2[i], nlp[i], i);
            }

            // Add Chart Legends
            Legend Legend2 = new Legend();
            //Legend2.Docking = Docking.Right;
            Legend2.Alignment = StringAlignment.Center;
            // Legend2.Name = well_name;
            //Legend2.Title = "Legend";
            Legend2.BorderDashStyle = ChartDashStyle.Solid;
            Legend2.BorderWidth = 2;
            Legend2.BorderColor = Color.Black;
            chart1.Legends.Add(Legend2);

            textBox4.Text = 14.ToString();
            textBox2.Text = 20.ToString();
            textBox3.Text = 20.ToString();
            comboBox1.SelectedIndex = 0;
            textBox1.Text = "Test".ToString();
        }
        private void clear_list_series_legend()
        {
            X.Clear();
            Y.Clear();
            wellname.Clear();
            wellname2.Clear();
           
            chart1.Series.Clear();
            chart1.Legends.Clear();
            chart1.Titles.Clear();

        }
        

        
        //  this default series is for solid curved lines       
        private void Set_default_series2()
        {
            List<float> X5 = new List<float>();
            List<float> Y5 = new List<float>();
            //adding vertical and horizontal dash lines
            for (int i = 0; i <5; i++)
            {
                X5.Clear();
                Y5.Clear();
                if (i == 0)
                {
                    string xline = "7.83	9.3	13.04	17.33	23.33	29.88	40.13	50.08	64";
                    string yline = "724.53	772.36	817.91	847.5	880.89	903.64	927.14	942.29	949.07";
                    
                    char[] delimiterChars = { ' ', '\t' };
                    string[] xwords = xline.Split(delimiterChars);
                    string[] ywords = yline.Split(delimiterChars);
                    //Ricker Wavelet
                    for (int j = 0; j < xwords.Length; j++)
                    {
                        X5.Add(float.Parse(xwords[j]));
                        Y5.Add(float.Parse(ywords[j]));
                    }
                }
                else if (i == 1)
                {
                    string xline = "1.73	10.86	27.36	42.42	58.9	84.75	108.05	133.32	153.48";
                    string yline = "56.37	98.86	129.17	147.33	163.98	178.3	190.36	196.34	199.3";
                    char[] delimiterChars = { ' ', '\t' };
                    string[] xwords = xline.Split(delimiterChars);
                    string[] ywords = yline.Split(delimiterChars);
                    //Ricker Wavelet
                    for (int j = 0; j < xwords.Length; j++)
                    {
                        X5.Add(float.Parse(xwords[j]));
                        Y5.Add(float.Parse(ywords[j]));
                    }
                }
                else if (i == 2)
                {
                    string xline = "8.03	8.95	11.84	17.57	24.43	31.84	45.23	59.74	79.07";
                    string yline = "376.77	440.55	488.38	534.67	577.93	612.07	655.29	684.85	703.76";
                    char[] delimiterChars = { ' ', '\t' };
                    string[] xwords = xline.Split(delimiterChars);
                    string[] ywords = yline.Split(delimiterChars);
                    //Ricker Wavelet
                    for (int j = 0; j < xwords.Length; j++)
                    {
                        X5.Add(float.Parse(xwords[j]));
                        Y5.Add(float.Parse(ywords[j]));
                    }
                }
                else if (i == 3)
                {
                    string xline = "2.55	17.33	32.38	47.15	61.63	75.83	88.32	101.1	116.71	140.85	153.34	168.95";
                    string yline = "32.83	44.92	53.22	57.72	59.18	60.65	62.12	64.35	67.32	71.03	71.74	70.92";
                    char[] delimiterChars = { ' ', '\t' };
                    string[] xwords = xline.Split(delimiterChars);
                    string[] ywords = yline.Split(delimiterChars);
                    //Ricker Wavelet
                    for (int j = 0; j < xwords.Length; j++)
                    {
                        X5.Add(float.Parse(xwords[j]));
                        Y5.Add(float.Parse(ywords[j]));
                    }
                }

                else if (i == 4)
                {
                    string xline = "7.02	11.6	22.13	35.49	46.01	59.08	72.15	82.65	95.72	109.06	119.85";
                    string yline = "235	259.07	284.84	304.53	314.37	327.98	336.29	340.04	343.79	347.54	349.01";
                    char[] delimiterChars = { ' ', '\t' };
                    string[] xwords = xline.Split(delimiterChars);
                    string[] ywords = yline.Split(delimiterChars);
                    //Ricker Wavelet
                    for (int j = 0; j < xwords.Length; j++)
                    {
                        X5.Add(float.Parse(xwords[j]));
                        Y5.Add(float.Parse(ywords[j]));
                    }
                }
              

                line_name = series_count.ToString();
                Series1 = Get_default_series2(chart1, line_name, X5,Y5, X5.Count);
                series_count++;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            button3.Enabled = true;
            string label;
            label = textBox1.Text;
            float x = float.Parse(textBox2.Text);
            float y = float.Parse(textBox3.Text);            
            float font = float.Parse(textBox4.Text);
            int selectedindex = comboBox1.SelectedIndex;
            GetChartTitle(label, x, y,font, selectedindex);
        }

        private Title GetChartTitle(String titleName,float x,float y,float font, int selected_index)
        {
            var title1 = new Title();
            //title1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(colorR)))), ((int)(((byte)(colorG)))), ((int)(((byte)(colorB)))));
            //title1.BackHatchStyle = System.Windows.Forms.DataVisualization.Charting.ChartHatchStyle.DottedDiamond;
            title1.Font = new System.Drawing.Font("Microsoft Sans Serif", font);
            title1.Text = titleName;
            title1.Position.Auto = false;
            //title1.Position.Height = 12;
            //title1.Position.Width = 30;
           
           
            if(selected_index==0)
            {
                title1.TextOrientation = TextOrientation.Horizontal;
            }
            else if (selected_index == 1)
            {
                title1.TextOrientation = TextOrientation.Rotated90;
            }
            else
            {
                title1.TextOrientation = TextOrientation.Rotated270;
            }
            
            chart1.Titles.Add(title1);
            title1.Position.X = x;
            title1.Position.Y = y;
            title_position++;
            return title1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            chart1.Titles.RemoveAt(title_position-1);
            title_position--;
            if(title_position==3)
            {
                button3.Enabled = false;
            }
        }

       
        private void Chart1_MouseMove(object sender, MouseEventArgs e)
        {
            //it has been observed that chart label while adding at 0,0 adds label to the left corner while adding on 100,100 adds label to the right bottom corner.
            //Therefore i am setting these values for label - to be added.
            //Basically these are required values.

            required_xmin = 00;
            required_xmax = 100;
            required_ymin = 00;
            required_ymax = 100;

           
            //these values should be taken from runing the program after putting the data.
            //777 is maximum e.X value and 0 is minimum e.X value
            //755 is maximum e.Y value and 0 is minimum e.X value

            e_xmin = 0;
            e_xmax = chart1.Width;
            e_ymin = 0;
            e_ymax = chart1.Height;


            float xx = (((required_xmax - required_xmin) / (e_xmax - e_xmin)) * (e.X - e_xmin)) + required_xmin;
            float yy = (((required_ymax - required_ymin) / (e_ymax - e_ymin)) * (e.Y - e_ymin)) + required_ymin;
                       
            //toolStripStatusLabel1.Text = " X = " + e.X + "  " + "Y = " + e.Y;
            toolStripStatusLabel1.Text = " X = " + xx + "  " + "Y = " + yy;
            
        }
        
        private void chart1_MouseClick(object sender, MouseEventArgs e)
        {
            float xx = (((required_xmax - required_xmin) / (e_xmax - e_xmin)) * (e.X - e_xmin)) + required_xmin;
            float yy = (((required_ymax - required_ymin) / (e_ymax - e_ymin)) * (e.Y - e_ymin)) + required_ymin;
            
            textBox2.Text = xx.ToString();
            textBox3.Text = yy.ToString();         
        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            button4.Enabled = false;
            button5.Enabled = false;
            clear_list_series_legend();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            panel1.Enabled = true;
            button1.Enabled = false;
            button4.Enabled = false;
            button5.Enabled = false;
        }
        
    }
}
