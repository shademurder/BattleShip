using System;
using System.Drawing;

namespace BattleShipp
{
    static class FieldGenerator
    {
        public static readonly Random Random = new Random();
        /// <summary>
        /// Максимальное количество попыток рандомизации положения корабля на поле, прежде, чем он получит первое попавшееся положение
        /// </summary>
        public static int MaxAttempsShipGeneration = 30;
        /// <summary>
        /// Максимальное количество попыток генерации поля с указанными параметрами
        /// </summary>
        public static int MaxAttemptsFieldGeneration = 10;

        public static Player TryGenerateField(int horizontalCells, int verticalCells, int fiveDeckShips,
            int fourDeckShips, int threeDeckShips, int doubleDeckShips, int oneDeckShips, PlayerType playerType, int fieldRandomAttempts, int shipRandomAttempts)
        {
            for (var count = 0; count < fieldRandomAttempts; count++)
            {
                var result = GenerateField(horizontalCells, verticalCells, fiveDeckShips, fourDeckShips, threeDeckShips,
                    doubleDeckShips, oneDeckShips, playerType, shipRandomAttempts);
                if (result != null)
                {
                    return result;
                }
            }
            return null;
        }
        public static Player GenerateField(int horizontalCells, int verticalCells, int fiveDecks, int fourDecks, int threeDecks, int doubleDecks, int oneDecks, PlayerType playerType, int shipRandomAttempts)
        {
            var player = new Player(oneDecks, doubleDecks, threeDecks, fourDecks, fiveDecks, playerType, horizontalCells, verticalCells);
            var freeSpace = horizontalCells * verticalCells;
            if (!SetShips(player.FiveDecks, player.Field, ref freeSpace, 5, playerType, shipRandomAttempts))
            {
                return null;
            }
            if (!SetShips(player.FourDecks, player.Field, ref freeSpace, 4, playerType, shipRandomAttempts))
            {
                return null;
            }
            if (!SetShips(player.ThreeDecks, player.Field, ref freeSpace, 3, playerType, shipRandomAttempts))
            {
                return null;
            }
            if (!SetShips(player.DoubleDecks, player.Field, ref freeSpace, 2, playerType, shipRandomAttempts))
            {
                return null;
            }

            if (!SetShips(player.OneDeckeds, player.Field, ref freeSpace, 1, playerType, shipRandomAttempts))
            {
                return null;
            }

            return player;
        }

        public static bool SetShips(Ship[] ships, CellType[,] field, ref int freeSpace, int shipSize, PlayerType playerType, int shipRandomAttempts)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var selectedShip = 0; selectedShip < ships.Length; selectedShip++)
            {
                ships[selectedShip] = new Ship(playerType, shipSize);
                var realPoint = CanPlace(field, horizontalCells, verticalCells, shipSize);
                if (realPoint.X == -1)
                {
                    return false;
                    //нельзя поставить корабль на карту
                }
                //ships[selectedShip].Decks = new Deck[shipSize];
                SetShip(ships[selectedShip], realPoint, field, horizontalCells, verticalCells, freeSpace, shipRandomAttempts);
                freeSpace -= shipSize;
            }
            return true;
        }

        public static void SetShip(Ship ship, Point realPoint, CellType[,] field, int horizontalCells, int verticalCells, int freeSpace, int attempts)
        {
            for (var attempt = 0; attempt < attempts; attempt++)
            {
                var randValue = Random.Next(freeSpace);
                randValue = GetShiftedRandomValue(field, randValue, horizontalCells, verticalCells);
                var y = randValue / horizontalCells;
                var x = randValue % horizontalCells;//-1
                if (PlaceFree(field, horizontalCells, verticalCells, x, y, ship.Decks.Length - 1, 0))
                {
                    SetShipLocation(ship, field, new Point(x, y), true);
                    return;
                }
                if (PlaceFree(field, horizontalCells, verticalCells, x, y, 0, ship.Decks.Length - 1))
                {
                    SetShipLocation(ship, field, new Point(x, y), false);
                    return;
                }
            }
            if (PlaceFree(field, horizontalCells, verticalCells, realPoint.X, realPoint.Y, ship.Decks.Length - 1, 0))
            {
                SetShipLocation(ship, field, realPoint, true);
                return;
            }
            if (PlaceFree(field, horizontalCells, verticalCells, realPoint.X, realPoint.Y, 0, ship.Decks.Length - 1))
            {
                SetShipLocation(ship, field, realPoint, false);
            }
        }

        private static int GetShiftedRandomValue(CellType[,] cellTypes, int randValue, int horizontalCells, int verticalCells)
        {
            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    if (cellTypes[y, x] != CellType.Water)
                    {
                        randValue++;
                    }
                }
            }
            return randValue;
        }

        private static void SetShipLocation(Ship ship, CellType[,] field, Point startPoint, bool horizontalDirection)
        {
            //MessageBox.Show($"startPoint = ({startPoint.X};{startPoint.Y})  horizontal = {horizontalDirection}  shipSize = {ship.Decks.Length}");
            for (var count = 0; count < ship.Decks.Length; count++)
            {
                ship.Decks[count] = new Deck { Location = new Point(startPoint.X + (horizontalDirection ? count : 0), startPoint.Y + (horizontalDirection ? 0 : count)) };
                field[startPoint.Y + (horizontalDirection ? 0 : count), startPoint.X + (horizontalDirection ? count : 0)] = CellType.Ship;
            }
        }

        /// <summary>
        /// Определяет наличие точки для установки корабля с заданным размером на поле
        /// </summary>
        /// <param name="field"></param>
        /// <param name="horizontalCells"></param>
        /// <param name="verticalCells"></param>
        /// <param name="shipSize"></param>
        /// <returns></returns>
        public static Point CanPlace(CellType[,] field, int horizontalCells, int verticalCells, int shipSize)
        {
            for (var x = 0; x < horizontalCells; x++)
            {
                for (var y = 0; y < verticalCells; y++)
                {
                    if (PlaceFree(field, horizontalCells, verticalCells, x, y, shipSize - 1, 0) || PlaceFree(field, horizontalCells, verticalCells, x, y, 0, shipSize - 1))
                    {
                        return new Point(x, y);
                    }
                }
            }
            return new Point(-1, -1);
        }

        /// <summary>
        /// Определяет, свободно ли место под корабль
        /// </summary>
        /// <param name="field"></param>
        /// <param name="horizontalCells"></param>
        /// <param name="verticalCells"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="horizontalShift"></param>
        /// <param name="verticalShift"></param>
        /// <returns></returns>
        public static bool PlaceFree(CellType[,] field, int horizontalCells, int verticalCells, int x, int y, int horizontalShift, int verticalShift)
        {
            if (x + horizontalShift >= horizontalCells || y + verticalShift >= verticalCells || x < 0 || y < 0)
            {
                return false;
            }
            //MessageBox.Show($"x = {x} y = {y}");
            //MessageBox.Show($"horizontalShift = {horizontalShift} verticalShift = {verticalShift}");
            for (var row = y - 1; row <= y + 1 + verticalShift; row++)
            {
                for (var column = x - 1; column <= x + 1 + horizontalShift; column++)
                {
                    if (!CellInField(horizontalCells, verticalCells, column, row)) continue;
                    if (field[row, column] != CellType.Water)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        /// <summary>
        /// Определяет, находится ли клетка в границах поля
        /// </summary>
        /// <param name="horizontalCells"></param>
        /// <param name="verticalCells"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static bool CellInField(int horizontalCells, int verticalCells, int x, int y)
        {
            return x >= 0 && x < horizontalCells && y >= 0 && y < verticalCells;
        }


        public static void HideField(CellType[,] field)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var x = 0; x < horizontalCells; x++)
            {
                for (var y = 0; y < verticalCells; y++)
                {
                    switch (field[y, x])
                    {
                        case CellType.Ship:
                            field[y, x] = CellType.FogOverShip;
                            break;
                        case CellType.Water:
                            field[y, x] = CellType.FogOverWater;
                            break;
                    }
                }
            }
        }
    }
}
