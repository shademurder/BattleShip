using System;
using System.Windows.Forms;
using BattleShipp;

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
            var ai = new SmartAI();
            battleShip1.ArtificialIntelligence = ai;
            battleShip1.NewGame();
            battleShip1.EndGame += BattleShip1_EndGame;
        }

        private void BattleShip1_EndGame(PlayerType obj)
        {
            battleShip1.NewGame();
        }
    }
}
