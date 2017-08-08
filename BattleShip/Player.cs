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

        /// <summary>
        /// Однопалубные корабли
        /// </summary>
        public Ship[] OneDeckeds { get; set; }
        /// <summary>
        /// Двухпалубные корабли
        /// </summary>
        public Ship[] DoubleDecks { get; set; }
        /// <summary>
        /// Трёхпалубные корабли
        /// </summary>
        public Ship[] ThreeDecks { get; set; }
        /// <summary>
        /// Четырёхпалубные корабли
        /// </summary>
        public Ship[] FourDecks { get; set; }
        /// <summary>
        /// Пятипалубные корабли
        /// </summary>
        public Ship[] FiveDecks { get; set; }
        /// <summary>
        /// Поле игрока
        /// </summary>
        public CellType[,] Field { get; set; }
        /// <summary>
        /// Тип игрока
        /// </summary>
        public PlayerType PlayerType { get; set; }

        /// <summary>
        /// Начало новой игры
        /// Подписаться на событие Shot указанной игры
        /// </summary>
        /// <param name="parrent"></param>
        public void NewGame(BattleShip parrent)
        {
            parrent.Shot += Parrent_Shot;
        }

        /// <summary>
        /// Конец игры
        /// Отписаться от события Shot
        /// </summary>
        /// <param name="parrent"></param>
        public void EndGame(BattleShip parrent)
        {
            parrent.Shot -= Parrent_Shot;
        }

        /// <summary>
        /// Обработка выстрела по полю
        /// </summary>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        /// <param name="playerType">Тип игрока, по полю которого был совершён выстрел</param>
        private void Parrent_Shot(int x, int y, PlayerType playerType)
        {
            if (playerType != PlayerType) return;
            switch (Field[y, x])
            {
                case CellType.FogOverShip:
                case CellType.Ship:
                    Field[y, x] = CellType.DamagedShip;
                    CheckDestroy(y, x);
                    break;
                case CellType.FogOverWater:
                case CellType.Water:
                    Field[y, x] = CellType.EmptyWater;
                    break;
            }
        }

        /// <summary>
        /// Проверяет, был ли разрушен корабль после выстрела по указанным координатам
        /// и при необходимости помечает все точки вокруг него
        /// </summary>
        /// <param name="row">Координата по y</param>
        /// <param name="column">Координата по x</param>
        private void CheckDestroy(int row, int column)
        {
            var ships = OneDeckeds.Union(DoubleDecks).Union(ThreeDecks).Union(FourDecks).Union(FiveDecks);
            var damagedShips = (from ship in ships where ship.ContainsDeck(row, column) select ship).ToArray();
            if(damagedShips.Length != 0)
            {
                var damagedShip = damagedShips[0];
                (from deck in damagedShip.Decks where deck.Location == new Point(column, row) select deck).ToArray()[0].Destroyed = true;
                if (damagedShip.Destroyed())
                {
                    DestroyShip(damagedShip);
                }
            }
        }

        /// <summary>
        /// Помечает корабль на поле, как разрушенный и выделяет вокруг
        /// </summary>
        /// <param name="ship">Уничтоженный корабль</param>
        private void DestroyShip(Ship ship)
        {
            var length = ship.Decks.Length - 1;
            var horizontalDirection = ship.GetDirection();
            for (var row = ship.Decks[0].Location.Y - 1; row <= ship.Decks[0].Location.Y + 1 + (horizontalDirection ? 0 : length); row++)
            {
                for (var column = ship.Decks[0].Location.X - 1; column <= ship.Decks[0].Location.X + 1 + (horizontalDirection ? length : 0); column++)
                {
                    if (!FieldGenerator.CellInField(Field.GetLength(1), Field.GetLength(0), column, row)) continue;
                    switch (Field[row, column])
                    {
                        case CellType.DamagedShip:
                            Field[row, column] = CellType.DestroyedShip;
                            break;
                        case CellType.Water:
                        case CellType.FogOverWater:
                            Field[row, column] = CellType.EmptyWater;
                            break;
                    }
                }
            }
        }

        /// <summary>
        /// Проверяет, были ли разрушены все корабли игрока
        /// </summary>
        /// <returns>true, если все корабли были разрушены</returns>
        public bool Lose()
        {
            var ships = OneDeckeds.Union(DoubleDecks).Union(ThreeDecks).Union(FourDecks).Union(FiveDecks);
            return ships.All(ship => ship.Destroyed());
        }
        
        
    }
}
