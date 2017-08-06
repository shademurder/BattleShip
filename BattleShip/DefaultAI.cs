using System.Drawing;

namespace BattleShipp
{
    class DefaultAI : IArtificialIntelligence 
    {
        public Point GetNextPoint(Player player)
        {
            var damagedPoint = GetDamagedPoint(player.Field);
            if(damagedPoint.X == -1)
            {
                //random
            }
            else
            {

            }

            return damagedPoint;
        }

        private Point GetDamagedPoint(CellType[,] field)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            for(int row = 0; row < verticalCells; row++)
            {
                for(int column = 0; column < horizontalCells; column++)
                {
                    if(field[row, column] == CellType.DamagedShip)
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
            bool top = true;
            if (damagedPoint.Y != 0)
            {
                if(field[damagedPoint.Y - 1, damagedPoint.X] == CellType.DamagedShip)
                {
                    return false;
                }
                if(field[damagedPoint.Y - 1, damagedPoint.X] == CellType.EmptyWater)
                {
                    top = false;
                }
            }
        }
    }
}
