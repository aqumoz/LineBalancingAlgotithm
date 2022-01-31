using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Data;
using System.Drawing;
using System.IO;
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
using Rectangle = System.Drawing.Rectangle;
using PointF = System.Drawing.PointF;

namespace LineBalancingAlgorithm
{
    public interface INode
    {
        public int Num { get; set; }
        public int Branch { get; set; }
        public List<int> PrecededByInts { get; set; }
        public List<Node> Next { get; set; }
        public List<Node> Prev { get; set; }
        public int Column { get; set; }
    }

    public class BaseNode : INode
    {
        public int Num { get; set; }
        public int Branch { get; set; }
        public List<int> PrecededByInts { get; set; } = new List<int>();
        public List<Node> Prev { get; set; } = new List<Node>();
        public List<Node> Next { get; set; } = new List<Node>();
        public int Column { get; set; }
    }

    public class Node : BaseNode
    {
        private string precededBy;

        public Node()
        {

        }

        public Node(int num, string description, float tek, string precededBy)
        {
            Num = num;
            Description = description;
            Tek = tek;
            PrecededBy = precededBy;
        }

        public string Description { get; set; }
        public float Tek { get; set; }
        public string PrecededBy { 
            get => precededBy;
            set {
                precededBy = value;
                PrecededByInts = precededBy.Split(',').Select(c =>
                {
                    if (c == "-" || c == string.Empty)
                        return 0;
                    else if (int.TryParse(c.Trim(), out int num))
                        return num;
                    else
                        throw new Exception("Could not parse numbers in \"Preceded By\" column number " + Num);
                }).OrderBy(n => n).ToList();
            }
        }
        public float RPW { get; set; }
        public List<Node> PrecededByAll { get; set; }

        //public void CalculatePrecededByIntsAll()
        //{
        //    PrecededByAll = CalculatePrecededByIntsAllReg(new List<Node>() { this }).Distinct().ToList();
        //}

        //private static List<Node> CalculatePrecededByIntsAllReg(List<Node> nodes)
        //{
        //    List<Node> tmpNode = nodes;
        //    foreach(Node n in tmpNode)
        //    {
        //        nodes.AddRange(n.Next);
        //    }
        //    return nodes;
        //}

    }



    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public ObservableCollection<Node> Rows { get; set; } = new ObservableCollection<Node>();
        public int PrecededByParseError { get; set; } = -1;


        public MainWindow()
        {
            InitializeComponent();
            Grid1.ItemsSource = Rows;

            // Test data
            Rows.Add(new Node(1, "Place frame in work holder and clamp", 0.2f, "-"));
            Rows.Add(new Node(2, "Assemble plug, grommet to power cord", 0.4f, "-"));
            Rows.Add(new Node(3, "Assemble brackets to frame", 0.7f, "1"));
            Rows.Add(new Node(4, "Wire power cord to monitor", 0.1f, "1,2"));
            Rows.Add(new Node(5, "Wire power cord to switch", 0.3f, "2"));
            Rows.Add(new Node(6, "Assemble mechanism plate to bracket", 0.11f, "3"));
            Rows.Add(new Node(7, "Assemble blade to bracket", 0.32f, "3"));
            Rows.Add(new Node(8, "Assemble motor to bracket", 0.6f, "3,4"));
            Rows.Add(new Node(9, "Align blade and attach to motor", 0.27f, "6,7,8"));
            Rows.Add(new Node(10, "Assemble switch to motor brackets", 0.38f, "5,8"));
            Rows.Add(new Node(11, "Attach cover, inspect, and test", 0.5f, "9,10"));
            Rows.Add(new Node(12, "Place in tote pan for packing", 0.12f, "11"));

            //Rows.Add(new Node(1, "test row 1", 9, "-"));
            //Rows.Add(new Node(2, "test row 2", 20, "-"));
            //Rows.Add(new Node(3, "test row 3", 11, "1"));
            //Rows.Add(new Node(4, "test row 4", 5, "2"));
            //Rows.Add(new Node(5, "test row 5", 12, "4"));
            //Rows.Add(new Node(6, "test row 6", 25, "5,8"));
            //Rows.Add(new Node(7, "test row 7", 8, "3,6"));
            //Rows.Add(new Node(8, "test row 8", 22, "4"));

            //BtnAdd.Click += new RoutedEventHandler((sender, args) => {
            //    Rows.Add(new RowModel((Rows.Count+1).ToString(), "", "", ""));
            //});

            BtnCopy.Click += new RoutedEventHandler((sender, args) =>
            {
                if(Diagram.Source is BitmapImage)
                    Clipboard.SetImage((BitmapImage)Diagram.Source);

            });

            BtnCalc.Click += new RoutedEventHandler((sender, args) => {

                foreach (Node row in Rows)
                {
                    row.Next.AddRange(Rows.Where(r => r.PrecededByInts.Contains(row.Num)));

                    foreach (Node r in Rows)
                    {
                        if (row.PrecededByInts.Contains(r.Num))
                            row.Prev.Add(r);
                    }
                }

                Node startNode = new Node() {
                    Next = Rows.Where(r => r.PrecededBy == "-" || r.PrecededBy == string.Empty).ToList(),
                    Branch = 1
                };

                //IEnumerable<INode> endNodes = Rows.Where(r => r.Next.Count == 0);
                //INode endNode = endNodes.Where(node => endNodes.Any(e => !node.PrecededByInts.Contains(e.Num))).First();


                CalculteColumnAndBranchNums(startNode);

                //CalculateRPW(Rows);

                int elWidth = Rows.Max(e => e.Column);
                int elHeight = Rows.Max(e => e.Branch);

                int cSpacing = 50;
                int rSpacing = 20;

                int imgWidth = elWidth * 40 + elWidth * cSpacing + cSpacing;
                int imgHeight = elHeight * 40 + elHeight * rSpacing + rSpacing;
                Bitmap img = new Bitmap(imgWidth, imgHeight);
                Graphics imgG = Graphics.FromImage(img);
                imgG.FillRectangle(System.Drawing.Brushes.White, new Rectangle(0, 0, imgWidth, imgHeight));

                foreach(Node node in Rows)
                {
                    int x = cSpacing + (node.Column - 1) * 40 + (node.Column - 1) * cSpacing;
                    int y = rSpacing + (node.Branch - 1) * 40 + (node.Branch - 1) * rSpacing;

                    imgG.DrawEllipse(new System.Drawing.Pen(System.Drawing.Brushes.Red), 
                        new Rectangle(x,y , 40, 40));
                    imgG.DrawString(node.Num.ToString(), new Font("Arial", 14), System.Drawing.Brushes.Black, new PointF(x+11, y+11));
                }

                Diagram.Source = BitmapToImageSource(img);



                Console.WriteLine("Test");
            });
        }

        private static void CalculteColumnAndBranchNums(INode startNode)
        {
            CalculteColumnAndBranchNumsReg(startNode);
        }

        private static void CalculteColumnAndBranchNumsReg(INode node, int currentColNum = 0)
        {
            int tempColNum = currentColNum + 1;
            if(node.Column < currentColNum)
                node.Column = currentColNum;

            int i = 0;
            foreach (INode n in node.Next)
            {
                if(n.Branch == 0)
                    n.Branch += node.Branch + i;
                CalculteColumnAndBranchNumsReg(n, tempColNum);
                i++;
            }
        }

        //private static void CalculateRPW(IEnumerable<Node> nodes)
        //{
        //    foreach(Node node in nodes)
        //    {
        //        node.CalculatePrecededByIntsAll();
        //    }

        //    IOrderedEnumerable<Node> ol = nodes.OrderByDescending(n => n.Column);
        //    foreach (Node node in nodes.OrderByDescending(n => n.Column))
        //    {
        //        if (node.Next.Count < 1)
        //            node.RPW = node.Tek;
        //        else
        //            node.RPW = node.PrecededByAll.Sum(n => n.Tek) + node.Tek;
        //    }
        //}


        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }
    }
}
