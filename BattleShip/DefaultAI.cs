using System.Drawing;

namespace BattleShipp
{
    class DefaultAI : IArtificialIntelligence
    {
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

        private Point GetRandomPoint(CellType[,] field)
        {
            //var horizontalCells = field.GetLength(1);
            //var verticalCells = field.GetLength(0);
            var freeSpace = GetFreeSpace(field);
            var randomValue = FieldGenerator.Random.Next(freeSpace);
            return RandomToPoint(field, randomValue);
            //randomValue += horizontalCells*verticalCells - freeSpace;
            //var y = randomValue / horizontalCells;
            //var x = randomValue % horizontalCells;
            //return new Point(x, y);
        }

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
            //Возвращать рекурсивно RandomToPoint?
            return new Point(horizontalCells - 1, verticalCells - 1);
        }

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

        private Point GetPoint(CellType[,] field, bool horizontalDirection, Point damagedPoint)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            var shift = 0;
            while ((horizontalDirection ? damagedPoint.X : damagedPoint.Y) + shift >= 0)
            {
                shift--;
                if ((horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift) < 0 || (horizontalDirection ? damagedPoint.X + shift : damagedPoint.X) < 0)
                {
                    break;
                }
                var cell = field[(horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift), (horizontalDirection ? damagedPoint.X + shift : damagedPoint.X)];
                if (cell == CellType.EmptyWater)
                {
                    break;
                }
                if (cell != CellType.DamagedShip)
                {
                    return new Point((horizontalDirection ? damagedPoint.X + shift : damagedPoint.X), (horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift));
                }
            }
            shift = 0;
            while ((horizontalDirection ? damagedPoint.X : damagedPoint.Y) + shift < (horizontalDirection ? horizontalCells : verticalCells))
            {
                shift++;
                if ((horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift) >= verticalCells || (horizontalDirection ? damagedPoint.X + shift : damagedPoint.X) >= horizontalCells)
                {
                    break;
                }
                var cell = field[(horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift), (horizontalDirection ? damagedPoint.X + shift : damagedPoint.X)];
                if (cell == CellType.EmptyWater)
                {
                    break;
                }
                if (cell != CellType.DamagedShip)
                {
                    return new Point((horizontalDirection ? damagedPoint.X + shift : damagedPoint.X), (horizontalDirection ? damagedPoint.Y : damagedPoint.Y + shift));
                }
            }
            return new Point(-1, -1);
        }

        private Point GetDamagedPoint(CellType[,] field)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for (var row = 0; row < verticalCells; row++)
            {
                for (var column = 0; column < horizontalCells; column++)
                {
                    if (field[row, column] == CellType.DamagedShip)
                    {
                        return new Point(column, row);
                    }
                }
            }
            return new Point(-1, -1);
        }

        /// <summary>
        /// Получает направление для поиска корабля
        /// </summary>
        /// <param name="field"></param>
        /// <param name="damagedPoint"></param>
        /// <returns>true - горизонтальное, false - вертикальное</returns>
        private bool GetDirection(CellType[,] field, Point damagedPoint)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            var weight = 0;
            if (damagedPoint.Y != 0)
            {
                if (field[damagedPoint.Y - 1, damagedPoint.X] == CellType.DamagedShip)
                {
                    return false;
                }
                if (field[damagedPoint.Y - 1, damagedPoint.X] == CellType.EmptyWater)
                {
                    weight--;
                }
            }
            else
            {
                weight--;
            }
            if (damagedPoint.Y < verticalCells - 1)
            {
                if (field[damagedPoint.Y + 1, damagedPoint.X] == CellType.DamagedShip)
                {
                    return false;
                }
                if (field[damagedPoint.Y + 1, damagedPoint.X] == CellType.EmptyWater)
                {
                    weight--;
                }
            }
            else
            {
                weight--;
            }
            if (damagedPoint.X != 0)
            {
                if (field[damagedPoint.Y, damagedPoint.X - 1] == CellType.DamagedShip)
                {
                    return true;
                }
                if (field[damagedPoint.Y, damagedPoint.X - 1] == CellType.EmptyWater)
                {
                    weight++;
                }
            }
            else
            {
                weight++;
            }
            if (damagedPoint.X < horizontalCells - 1)
            {
                if (field[damagedPoint.Y, damagedPoint.X + 1] == CellType.DamagedShip)
                {
                    return true;
                }
                if (field[damagedPoint.Y, damagedPoint.X + 1] == CellType.EmptyWater)
                {
                    weight++;
                }
            }
            else
            {
                weight++;
            }
            return weight <= 0;
        }
    }
}
