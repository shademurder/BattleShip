using System.Linq;
using System.Drawing;

namespace BattleShipp
{
    class Player
    {
        public Player(int oneDeckeds, int doubleDecks, int threeDecks, int fourDecks, int fiveDecks, PlayerType playerType, int fieldWidth, int fieldHeight)
        {
            OneDeckeds = new Ship[oneDeckeds];
            DoubleDecks = new Ship[doubleDecks];
            ThreeDecks = new Ship[threeDecks];
            FourDecks = new Ship[fourDecks];
            FiveDecks = new Ship[fiveDecks];
            Field = new CellType[fieldHeight, fieldWidth];
            for (var x = 0; x < fieldWidth; x++)
            {
                for (var y = 0; y < fieldHeight; y++)
                {
                    Field[y, x] = CellType.Water;
                }
            }
            PlayerType = playerType;
        }

        public void NewGame(BattleShip parrent)
        {
            parrent.Shot += Parrent_Shot;
        }

        public void EndGame(BattleShip parrent)
        {
            parrent.Shot -= Parrent_Shot;
        }

        private void Parrent_Shot(int x, int y, PlayerType playerType)
        {
            if(playerType == PlayerType)
            {
                if(Field[y, x] == CellType.FogOverShip || Field[y, x] == CellType.Ship)
                {
                    Field[y, x] = CellType.DamagedShip;
                    CheckDestroy(y, x);
                }
                if(Field[y, x] == CellType.FogOverWater || Field[y, x] == CellType.Water)
                {
                    Field[y, x] = CellType.EmptyWater;
                }
                //if(Field[y, x] == CellType.)
            }
        }

        private void CheckDestroy(int row, int column)
        {
            var ships = OneDeckeds.Union(DoubleDecks).Union(ThreeDecks).Union(FourDecks).Union(FiveDecks);
            var damagedShips = (from ship in ships where ship.ContainsDeck(row, column) select ship).ToArray();
            if(damagedShips. Length != 0)
            {
                var damagedShip = damagedShips[0];
                (from deck in damagedShip.Decks where deck.Location == new Point(column, row) select deck).ToArray()[0].Destroyed = true;
                //destroyedDeck.Destroyed = true;
                if (damagedShip.Destroyed())
                {
                    DestroyShip(damagedShip);
                }
                
            }
        }

        private void DestroyShip(Ship ship)
        {
            var length = ship.Decks.Length - 1;
            var horizontalDirection = ship.GetDirection();
            for (var row = ship.Decks[0].Location.Y - 1; row <= ship.Decks[0].Location.Y + 1 + (horizontalDirection ? 0 : length); row++)
            {
                for (var column = ship.Decks[0].Location.X - 1; column <= ship.Decks[0].Location.X + 1 + (horizontalDirection ? length : 0); column++)
                {
                    if (!FieldGenerator.CellInField(Field.GetLength(1), Field.GetLength(0), column, row)) continue;
                    if(Field[row, column] == CellType.DamagedShip)
                    {
                        Field[row, column] = CellType.DestroyedShip;
                    }
                    if(Field[row, column] == CellType.Water || Field[row, column] == CellType.FogOverWater)
                    {
                        Field[row, column] = CellType.EmptyWater;
                    }
                }
            }
        }

        public Ship[] OneDeckeds { get; set; }

        public Ship[] DoubleDecks { get; set; }

        public Ship[] ThreeDecks { get; set; }

        public Ship[] FourDecks { get; set; }

        public Ship[] FiveDecks { get; set; }

        public CellType[,] Field { get; set; }

        internal PlayerType PlayerType { get; set; }
    }
}
