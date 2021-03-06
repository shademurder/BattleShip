﻿using System;
using System.Windows.Forms;
using BattleShipp;

namespace BattleShip
{
    public partial class BattleShipForm : Form
    {
        public BattleShipForm()
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

        private void BattleShip1_EndGame(PlayerType playerType)
        {
            MessageBox.Show(playerType == PlayerType.Player ? "Вы победили!" : "Вы проиграли!");
            battleShip1.NewGame();
        }
    }
}
