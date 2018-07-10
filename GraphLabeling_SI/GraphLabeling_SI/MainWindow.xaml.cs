using Microsoft.Glee.Drawing;
using Microsoft.Glee.Splines;
using Microsoft.Glee.GraphViewerGdi;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
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
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;

namespace GraphLabeling_SI
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //private static string graphstring = "[[1,2],[1,3],[2,3]],3,3";
        //private static string fulleargstring = "-g \"load('gracefulgraph.pl'), go(" + graphstring+ "), halt\" ";


        public Graph graph;
        public ObservableCollection<string> NodeList { get; set; }
        private string edgestring;
        private int EdgeCounter = 1;
        private int NodeCounter = 1;

        public MainWindow()
        {
            graph = new Graph("graph");
            NodeList = new ObservableCollection<string>();
            edgestring = string.Empty;
            InitializeComponent();
            DataContext = this;
            this.gViewer.Graph = graph;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var edge = EdgeCounter-1;
            var node = NodeCounter-1;
            if (edgestring != string.Empty)
            {
                edgestring = edgestring.Remove(edgestring.Length - 1);
                string fullstring = "[" + edgestring + "],";
                fullstring += edge.ToString() + "," + node.ToString(); ;
                string fullargstring = "-g \"load('gracefulgraph.pl'), go(" + fullstring + "), halt\" ";
                string output = GetOutput(fullargstring);
                string[] lines = output.Split(new char[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
                //lines[2] = nodes value
                //lines[3] = edges value
                if (lines.Length < 4)
                    MessageBox.Show("Graph can not be graceful");
                else
                {
                    List<string> nodes = GetNumbers(lines[2]);
                    List<string> edges = GetNumbers(lines[3]);
                    for (int i = 0; i < NodeCounter - 1; i++)
                        graph.FindNode((i + 1).ToString()).Attr.Label = nodes[i];
                    int j = 0;
                    foreach (var edg in graph.Edges)
                    {
                        edg.EdgeAttr.Label = edges[j];
                        j++;
                    }
                    this.gViewer.Graph = graph;
                    MessageBox.Show("Graph laballed graceful");
                }
            }
            else
                return;

        }
        /* output looks like:
        nodes:[0,1,2]
        edges:[1,2,3]
        */
        private string GetOutput(string fullstring)
        {
            string msg = string.Empty;
            Process process = new Process();
            ProcessStartInfo startInfo = new ProcessStartInfo("bp.exe");
            startInfo.Arguments = fullstring;
            startInfo.RedirectStandardOutput = true;
            startInfo.RedirectStandardInput = true;
            startInfo.RedirectStandardError = true;
            startInfo.UseShellExecute = false;
            process.StartInfo = startInfo;
            process.OutputDataReceived += (s, e) => msg += "\n" + e.Data;
            process.Start();
            process.BeginOutputReadLine();
            process.WaitForExit();
            return msg;
        }
        private List<String> GetNumbers(string msg)
        {
            List<string> ret = new List<string>();
            Match match = Regex.Match(msg, @"\d+");
            while (match.Success)
            {
                ret.Add(match.Value);
                match = match.NextMatch();
            }
            return ret;
        }

        private void NewNode_Click(object sender, RoutedEventArgs e)
        {
            graph.AddNode(NodeCounter.ToString()).Attr.Label = NodeCounter.ToString();
            NodeList.Add(NodeCounter.ToString());
            NodeCounter++;
            this.gViewer.Graph = graph;
        }
        private void NewEdge_Click(object sender, RoutedEventArgs e)
        {
            if (NodeA.SelectedItem == null)
                return;
            if (NodeB.SelectedItem == null)
                return;
            string nodeA = NodeA.SelectedItem.ToString();
            string nodeB = NodeB.SelectedItem.ToString();
            string edgeID = EdgeCounter.ToString();
            graph.AddEdge(nodeA, nodeB).Attr.ArrowHeadAtTarget = ArrowStyle.None;
            edgestring += "[" + nodeA + "," + nodeB + "],";
            EdgeCounter++;
            this.gViewer.Graph = graph;

        }

        private void Button1_Click(object sender, RoutedEventArgs e)
        {
            graph = new Graph("graph");
            EdgeCounter = 1;
            NodeCounter = 1;
            NodeList.Clear();
            edgestring = string.Empty;
            this.gViewer.Graph = graph;
        }
    }
}
