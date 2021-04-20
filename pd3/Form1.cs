using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace pd3
{
    public partial class Form1 : Form
    {
        List<string> pozymiai = new List<string>();
        List<int> pazymetiIndex = new List<int>();
        string[] fileArray = new string[10000];



        public Form1()
        {
            InitializeComponent();

        }

        private void button1_Click(object sender, EventArgs e)
        {
            //gija surast kataloga ir parodyti galimus pozymius:
            Thread th = new Thread(() => { ieskok(); });
            th.Start();
        }
        private void ieskok()
        {


            _ = this.Invoke((Action)delegate
            {

                //paspaudziam"koduot" ir ieskom ka uzkoduot
                FolderBrowserDialog FBD = new FolderBrowserDialog();

                if (FBD.ShowDialog() == DialogResult.OK)
                {

                    //failai pasirinktame kataloge
                    fileArray = Directory.GetFiles(FBD.SelectedPath);


                }

            });

            //gauti galimus pozymius
            var f = 0;

                    //count how many attributes
                    int att = 0;
                foreach (string file in fileArray)
                {
                    if (f == 1)
                    {
                        break;
                    }


                    var lines = System.IO.File.ReadLines(file);
                    foreach (var thisLine in lines)
                    {
                        if (thisLine.Contains("@attribute"))
                        {
                            pozymiai.Add(thisLine);
                            att++;


                        }

                    }
                    f++;
                }

                if (att ==6555)
                {
                    Console.WriteLine("teisinga struktura");
                }
                else
                {
                    return;
                }

            //uzpildyti checklistbox

            this.Invoke((Action)delegate
            {
                foreach (string pozymis in pozymiai)
                {
                    checkedListBox1.Items.Add(pozymis);
                }

            });


        }

        private void button2_Click(object sender, EventArgs e)
        {
            Thread th1 = new Thread(() => { pazymek(); });
            th1.Start();
        }

        private void pazymek()
        {
            _ = this.Invoke((Action)delegate
            {

                //isvalyti boxlista
                listBox1.Items.Clear();
                //uzpildyti boxlista su pazymetais
                foreach (object Item in checkedListBox1.CheckedItems)
                {
                    listBox1.Items.Add(Item);

                }
            });


            //indexai pazymetu "pozymiai liste"
            foreach (string s in listBox1.Items)
            {
                foreach (string p in pozymiai)
                {
                    if (s == p)
                    {
                        pazymetiIndex.Add(pozymiai.IndexOf(s));


                    }

                }

            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            Thread th2 = new Thread(() => { irasyti(); });
            th2.Start();
        }

        private void irasyti()
        {

            string[] lines = new string[100000];

            List<string> results = new List<string>();
            foreach (string file in fileArray)
            {
                lines = File.ReadAllLines(file);
                //rezultatu eilute visu bandymu 
                results.Add(lines[6560]);


            }

  
            List<List<string>> outlist = new List<List<string>>();

             List<string> values = new List<string>();
                foreach (string result in results)
                {
                    //istrinam viska is values
                    values.Clear();
                    values.AddRange(result.Split(','));
                outlist.Add(new List<string>(values));

            }
   

            StreamWriter sw = new StreamWriter("C:\\Users\\inga3\\OneDrive\\Stalinis kompiuteris\\inga\\test.arff");
            //pirma eilute
            sw.WriteLine("@relation SMILEfeatures");
            //tuscia eilute
            sw.WriteLine(" ");

            //suzymeti pozymiai
            foreach (int poz in pazymetiIndex)
            {
                sw.WriteLine(pozymiai[poz]);
            }
       
            sw.WriteLine(" ");
            sw.WriteLine("@data");
            sw.WriteLine(" ");
            for (int i = 0; i < outlist.Count; i++)
            {
                foreach (int paz in pazymetiIndex)
                {
  
                    //rezultatu eilute i faila
                    sw.Write(outlist[i][paz] + ",");


                }

            }
            sw.Close();

        }

    }
    
}