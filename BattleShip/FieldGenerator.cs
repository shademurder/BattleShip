using System;
using System.Drawing;

namespace BattleShipp
{
    static class FieldGenerator
    {
        public static readonly Random Random = new Random();

        /// <summary>
        /// Создаёт игрока и генерирует все корабли на его поле, используя максимальное число попыток генерации поля
        /// </summary>
        /// <param name="horizontalCells">Количество ячеек на поле по горизонтали</param>
        /// <param name="verticalCells">Количество ячеек на поле по вертикали</param>
        /// <param name="fiveDeckShips">Количество пятипалубных кораблей</param>
        /// <param name="fourDeckShips">Количество четырёхпалубных кораблей</param>
        /// <param name="threeDeckShips">Количество трёхпалубных кораблей</param>
        /// <param name="doubleDeckShips">Количество двухпалубных кораблей</param>
        /// <param name="oneDeckShips">Количество однопалубных кораблей</param>
        /// <param name="playerType">Тип игрока</param>
        /// <param name="fieldRandomAttempts">Максимальное количество попыток генерации поля с указанными параметрами</param>
        /// <param name="shipRandomAttempts">Максимальное количество попыток рандомизации положения корабля на поле, прежде, чем он получит первое попавшееся положение</param>
        /// <returns></returns>
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

        /// <summary>
        /// Создаёт игрока и генерирует все корабли на его поле
        /// </summary>
        /// <param name="horizontalCells">Количество ячеек на поле по горизонтали</param>
        /// <param name="verticalCells">Количество ячеек на поле по вертикали</param>
        /// <param name="fiveDecks">Количество пятипалубных кораблей</param>
        /// <param name="fourDecks">Количество четырёхпалубных кораблей</param>
        /// <param name="threeDecks">Количество трёхпалубных кораблей</param>
        /// <param name="doubleDecks">Количество двухпалубных кораблей</param>
        /// <param name="oneDecks">Количество однопалубных кораблей</param>
        /// <param name="playerType">Тип игрока</param>
        /// <param name="shipRandomAttempts">Количество попыток для рандомизации местоположения каждого корабля</param>
        /// <returns></returns>
        public static Player GenerateField(int horizontalCells, int verticalCells, int fiveDecks, int fourDecks, int threeDecks, int doubleDecks, int oneDecks, PlayerType playerType, int shipRandomAttempts)
        {
            var player = new Player(oneDecks, doubleDecks, threeDecks, fourDecks, fiveDecks, playerType, horizontalCells, verticalCells);
            var freeSpace = horizontalCells * verticalCells;
            return !SetShips(player.FiveDecks, player.Field, ref freeSpace, 5, playerType, shipRandomAttempts) ||
                   !SetShips(player.FourDecks, player.Field, ref freeSpace, 4, playerType, shipRandomAttempts) ||
                   !SetShips(player.ThreeDecks, player.Field, ref freeSpace, 3, playerType, shipRandomAttempts) ||
                   !SetShips(player.DoubleDecks, player.Field, ref freeSpace, 2, playerType, shipRandomAttempts) ||
                   !SetShips(player.OneDeckeds, player.Field, ref freeSpace, 1, playerType, shipRandomAttempts)
                ? null : player;
        }

        /// <summary>
        /// Поочерёдно устанавливает корабли на поле
        /// </summary>
        /// <param name="ships">Устанавливаемые на поле корабли</param>
        /// <param name="field">Игровое поле</param>
        /// <param name="freeSpace">Свободное место на поле</param>
        /// <param name="shipSize">Размер кораблей</param>
        /// <param name="playerType">Тип игрока</param>
        /// <param name="shipRandomAttempts">Количество попыток для рандомизации местоположения корабля</param>
        /// <returns>true в случае удачного распределения всех кораблей</returns>
        public static bool SetShips(Ship[] ships, CellType[,] field, ref int freeSpace, int shipSize, PlayerType playerType, int shipRandomAttempts)
        {
            for (var selectedShip = 0; selectedShip < ships.Length; selectedShip++)
            {
                ships[selectedShip] = new Ship(playerType, shipSize);
                var realPoint = CanPlace(field, shipSize);
                if (realPoint.X == -1)
                {
                    return false;
                    //нельзя поставить корабль на карту
                }
                SetShip(ships[selectedShip], realPoint, field, freeSpace, shipRandomAttempts);
                freeSpace -= shipSize;
            }
            return true;
        }

        /// <summary>
        /// Устанавливает корабль на поле, используя рандомизацию указанное количество раз
        /// При неудаче ставит корабль в указанную точку
        /// </summary>
        /// <param name="ship">Устанавливаемый корабль</param>
        /// <param name="realPoint">Точка, где корабль точно можно установить</param>
        /// <param name="field">Игровое поле</param>
        /// <param name="freeSpace">Свободное место на поле</param>
        /// <param name="attempts">Количество попыток для рандомизации местоположения корабля</param>
        public static void SetShip(Ship ship, Point realPoint, CellType[,] field, int freeSpace, int attempts)
        {
            var horizontalCells = field.GetLength(1);
            for (var attempt = 0; attempt < attempts; attempt++)
            {
                var randValue = Random.Next(freeSpace);
                randValue = GetShiftedRandomValue(field, randValue);
                var y = randValue / horizontalCells;
                var x = randValue % horizontalCells;
                if (PlaceFree(field, x, y, ship.Decks.Length - 1, 0))
                {
                    SetShipLocation(ship, field, new Point(x, y), true);
                    return;
                }
                if (!PlaceFree(field, x, y, 0, ship.Decks.Length - 1)) continue;
                SetShipLocation(ship, field, new Point(x, y), false);
                return;
            }
            if (PlaceFree(field, realPoint.X, realPoint.Y, ship.Decks.Length - 1, 0))
            {
                SetShipLocation(ship, field, realPoint, true);
                return;
            }
            if (PlaceFree(field, realPoint.X, realPoint.Y, 0, ship.Decks.Length - 1))
            {
                SetShipLocation(ship, field, realPoint, false);
            }
        }

        /// <summary>
        /// Корректирует рандомное число
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="randValue">Изменяемое число</param>
        /// <returns>Откорректированное значение</returns>
        private static int GetShiftedRandomValue(CellType[,] field, int randValue)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    if (field[y, x] != CellType.Water)
                    {
                        randValue++;
                    }
                }
            }
            return randValue;
        }

        /// <summary>
        /// Помещает корабль в указанное местоположение
        /// </summary>
        /// <param name="ship">Устанавливаемый корабль</param>
        /// <param name="field">Игровое поле</param>
        /// <param name="startPoint">Точка для установки корабля</param>
        /// <param name="horizontalDirection">Напрвление корабля (true - горизонтальное)</param>
        private static void SetShipLocation(Ship ship, CellType[,] field, Point startPoint, bool horizontalDirection)
        {
            for (var count = 0; count < ship.Decks.Length; count++)
            {
                ship.Decks[count] = new Deck { Location = new Point(startPoint.X + (horizontalDirection ? count : 0), startPoint.Y + (horizontalDirection ? 0 : count)) };
                field[startPoint.Y + (horizontalDirection ? 0 : count), startPoint.X + (horizontalDirection ? count : 0)] = CellType.Ship;
            }
        }

        /// <summary>
        /// Определяет наличие точки для установки корабля с заданным размером на поле
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="shipSize">Размер корабля</param>
        /// <returns>Точка, в которой точно можно установить корабль</returns>
        public static Point CanPlace(CellType[,] field, int shipSize)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var x = 0; x < horizontalCells; x++)
            {
                for (var y = 0; y < verticalCells; y++)
                {
                    if (PlaceFree(field, x, y, shipSize - 1, 0) || PlaceFree(field, x, y, 0, shipSize - 1))
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
        /// <param name="field">Поле для поиска места</param>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        /// <param name="horizontalShift">Максимальное смещение по горизонтали</param>
        /// <param name="verticalShift">Максимальное смещение по вертикали</param>
        /// <returns></returns>
        public static bool PlaceFree(CellType[,] field, int x, int y, int horizontalShift, int verticalShift)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            if (x + horizontalShift >= horizontalCells || y + verticalShift >= verticalCells || x < 0 || y < 0)
            {
                return false;
            }
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
        /// <param name="horizontalCells">Количество ячеек на поле по горизонтали</param>
        /// <param name="verticalCells">Количество ячеек на поле по вертикали</param>
        /// <param name="x">Координата по оси X</param>
        /// <param name="y">Координата по оси Y</param>
        /// <returns>true, если клетка в границах поля</returns>
        public static bool CellInField(int horizontalCells, int verticalCells, int x, int y)
        {
            return x >= 0 && x < horizontalCells && y >= 0 && y < verticalCells;
        }

        /// <summary>
        /// Скрывает поле игрока (добавляет туман)
        /// </summary>
        /// <param name="field">Игровое поле</param>
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
