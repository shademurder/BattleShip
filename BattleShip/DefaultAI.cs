using System.Drawing;

namespace BattleShipp
{
    class DefaultAI : TypicalArtificialIntelligence, IArtificialIntelligence
    {
        /// <summary>
        /// Анализирует игровое поле игрока и возвращает следующую точку для выстрела
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns>Точка для выстрела</returns>
        public Point GetNextPoint(Player player)
        {
            var damagedPoint = GetDamagedPoint(player.Field);
            if (damagedPoint.X == -1)
            {
                return GetRandomPoint(player.Field);
            }
            var horizontalDirection = GetDirection(player.Field, damagedPoint);
            var nextPoint = GetPoint(player.Field, horizontalDirection, damagedPoint);
            return nextPoint.X == -1 ? GetRandomPoint(player.Field) : nextPoint;
        }
        /// <summary>
        /// Получает точку для выстрела "наугад"
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <returns>Точка для выстрела</returns>
        private Point GetRandomPoint(CellType[,] field)
        {
            var freeSpace = GetFreeSpace(field);
            var randomValue = FieldGenerator.Random.Next(freeSpace);
            return RandomToPoint(field, randomValue);
        }

        /// <summary>
        /// Переводит число в точку на игровом поле
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="randomValue">Случайное число</param>
        /// <returns>Точка для выстрела</returns>
        private Point RandomToPoint(CellType[,] field, int randomValue)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    if (randomValue == 0 && field[y, x] != CellType.DamagedShip && field[y, x] != CellType.DestroyedShip && field[y, x] != CellType.EmptyWater)
                    {
                        return new Point(x, y);
                    }
                    if (field[y, x] == CellType.DamagedShip || field[y, x] == CellType.DestroyedShip || field[y, x] == CellType.EmptyWater) continue;
                    randomValue--;
                }
            }
            return new Point(horizontalCells - 1, verticalCells - 1);
        }

        /// <summary>
        /// Получает количество пустых клеток на поле
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <returns>Количество пустых клеток</returns>
        private int GetFreeSpace(CellType[,] field)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            var freeSpace = verticalCells * horizontalCells;
            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    if (field[y, x] == CellType.DamagedShip || field[y, x] == CellType.DestroyedShip || field[y, x] == CellType.EmptyWater)
                    {
                        freeSpace--;
                    }
                }
            }
            return freeSpace;
        }

        
    }
}
