using System.Drawing;

namespace BattleShipp
{
    abstract class TypicalArtificialIntelligence
    {
        /// <summary>
        /// Ищет точку для добивания корабля
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="horizontalDirection">true, если предположительное направление корабля горизонтальное</param>
        /// <param name="damagedPoint">Точка с повреждённой палубой корабля</param>
        /// <returns>точка с координатами (-1;-1), если ничего не удалось найти</returns>
        protected Point GetPoint(CellType[,] field, bool horizontalDirection, Point damagedPoint)
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
        /// <summary>
        /// Находит на поле точку с повреждённой палубой корабля
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <returns>точка с координатами (-1;-1), если ничего не удалось найти</returns>
        protected Point GetDamagedPoint(CellType[,] field)
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
        /// <param name="field">Игровое поле</param>
        /// <param name="damagedPoint">Точка с повреждённой палубой корабля</param>
        /// <returns>true - горизонтальное, false - вертикальное</returns>
        protected bool GetDirection(CellType[,] field, Point damagedPoint)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            var weight = 0;
            if (damagedPoint.Y != 0)
            {
                switch (field[damagedPoint.Y - 1, damagedPoint.X])
                {
                    case CellType.DamagedShip:
                        return false;
                    case CellType.EmptyWater:
                        weight--;
                        break;
                }
            }
            else
            {
                weight--;
            }
            if (damagedPoint.Y < verticalCells - 1)
            {
                switch (field[damagedPoint.Y + 1, damagedPoint.X])
                {
                    case CellType.DamagedShip:
                        return false;
                    case CellType.EmptyWater:
                        weight--;
                        break;
                }
            }
            else
            {
                weight--;
            }
            if (damagedPoint.X != 0)
            {
                switch (field[damagedPoint.Y, damagedPoint.X - 1])
                {
                    case CellType.DamagedShip:
                        return true;
                    case CellType.EmptyWater:
                        weight++;
                        break;
                }
            }
            else
            {
                weight++;
            }
            if (damagedPoint.X < horizontalCells - 1)
            {
                switch (field[damagedPoint.Y, damagedPoint.X + 1])
                {
                    case CellType.DamagedShip:
                        return true;
                    case CellType.EmptyWater:
                        weight++;
                        break;
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
