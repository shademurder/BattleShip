using System;
using System.Windows.Forms;

namespace BattleShip
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Task<int> task = Task<int>.Factory.StartNew(Method);
            //int a = task.Result;
            battleShip1.NewGame();
        }

        //private int Method()
        //{
        //    return 1;
        //}

        //private void Method2(params int[] a)
        //{
        //    TaskScheduler taskScheduler = TaskScheduler.FromCurrentSynchronizationContext();
        //    var task = Task<int>.Factory.StartNew(Method);
        //    task.ContinueWith((t, o) => Method3(task.Result), null, taskScheduler);
        //}

        //private void Method3(int c)
        //{

        //}

    }
}
