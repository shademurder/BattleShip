using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using BattleShip;

namespace BattleShipp
{
    class SmartAI : TypicalArtificialIntelligence, IArtificialIntelligence
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
                return GetRandomPoint(player);
            }
            var horizontalDirection = GetDirection(player.Field, damagedPoint);
            var nextPoint = GetPoint(player.Field, horizontalDirection, damagedPoint);
            return nextPoint.X == -1 ? GetRandomPoint(player) : nextPoint;
        }

        /// <summary>
        /// Получает точку на поле для выстрела, опираясь на веса всех ячеек на поле и рандом
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns>Точка для выстрела</returns>
        private Point GetRandomPoint(Player player)
        {
            var weightField = GetWeightField(player);
            var maxWeight = weightField.Cast<int>().Max();
            var maxWeightCount = weightField.Cast<int>().Count(value => value == maxWeight);
            return RandomToPoint(weightField, maxWeight, FieldGenerator.Random.Next(maxWeightCount));
        }

        /// <summary>
        /// Преобразовывает рандомное значение в точку на поле
        /// </summary>
        /// <param name="weightField">Массив весов ячеек поля</param>
        /// <param name="maxWeight">Максимальный вес ячейки на поле</param>
        /// <param name="randomValue">Рандомное значение</param>
        /// <returns>Результирующая точка</returns>
        private Point RandomToPoint(int[,] weightField, int maxWeight, int randomValue)
        {
            var horizontalCells = weightField.GetLength(1);
            var verticalCells = weightField.GetLength(0);
            for (var y = 0; y < verticalCells; y++)
            {
                for (var x = 0; x < horizontalCells; x++)
                {
                    if (weightField[y, x] == maxWeight)
                    {
                        if (randomValue == 0) return new Point(x, y);
                        randomValue--;
                    }
                }
            }
            return new Point(horizontalCells - 1, verticalCells - 1);
        }

        /// <summary>
        /// Вычисляет вес всех ячеек на поле
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns>Массив весов ячеек поля</returns>
        private int[,] GetWeightField(Player player)
        {
            var shipSizes = GetShipSizes(player);
            var horizontalCells = player.Field.GetLength(1);
            var verticalCells = player.Field.GetLength(0);
            int[,] weightField = new int[verticalCells, horizontalCells];
            for (var row = 0; row < verticalCells; row++)
            {
                for (var column = 0; column < horizontalCells; column++)
                {
                    weightField[row, column] = GetCellWeight(player.Field, shipSizes, new Point(column, row));
                }
            }
            return weightField;
        }

        /// <summary>
        /// Вычисляет вес указанной ячейки на поле
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="shipSizes">Размеры всех кораблей</param>
        /// <param name="selectedPoint">Выбранная ячейка</param>
        /// <returns>Вес ячейки от 0 и выше (0, если ячейка занята)</returns>
        private int GetCellWeight(CellType[,] field, int[] shipSizes, Point selectedPoint)
        {
            var cellWeight = -3;
            var maxShipSize = shipSizes.Max();
            var top = true;
            var bot = true;
            var left = true;
            var right = true;
            for (var step = 0; step < maxShipSize; step++)
            {
                if (right)
                {
                    var newWeight = CorrectWeight(field, selectedPoint, shipSizes, step, Direction.Right, cellWeight);
                    right = newWeight != cellWeight;
                    cellWeight = newWeight;
                }
                if (left)
                {
                    var newWeight = CorrectWeight(field, selectedPoint, shipSizes, step, Direction.Left, cellWeight);
                    left = newWeight != cellWeight;
                    cellWeight = newWeight;
                }
                if (bot)
                {
                    var newWeight = CorrectWeight(field, selectedPoint, shipSizes, step, Direction.Bot, cellWeight);
                    bot = newWeight != cellWeight;
                    cellWeight = newWeight;
                }
                if (top)
                {
                    var newWeight = CorrectWeight(field, selectedPoint, shipSizes, step, Direction.Top, cellWeight);
                    top = newWeight != cellWeight;
                    cellWeight = newWeight;
                }
            }
            return cellWeight == -3 ? 0 : cellWeight;
        }

        /// <summary>
        /// Корректирует вес ячейки
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="selectedPoint">Выбранная точка для проверки</param>
        /// <param name="shipSizes">Размеры всех кораблей</param>
        /// <param name="step">Шаг</param>
        /// <param name="direction">Направление</param>
        /// <param name="cellWeight">Вес ячейки</param>
        /// <returns></returns>
        private int CorrectWeight(CellType[,] field, Point selectedPoint, int[] shipSizes, int step, Direction direction, int cellWeight)
        {
            if (CellFree(field, selectedPoint, step, direction))
            {
                if (shipSizes.Contains(step + 1))
                {
                    cellWeight++;
                }
            }
            return cellWeight;
        }

        /// <summary>
        /// Проверяет, свободна ли клетка на поле в шаге от выбранной в указанном направлении
        /// </summary>
        /// <param name="field">Игровое поле</param>
        /// <param name="selectedPoint">Выбранная точка для проверки</param>
        /// <param name="step">Шаг</param>
        /// <param name="direction">Направление</param>
        /// <returns>true, если клетка в границах поля и ни чем не занята</returns>
        private bool CellFree(CellType[,] field, Point selectedPoint, int step, Direction direction)
        {
            var horizontalCells = field.GetLength(1);
            var verticalCells = field.GetLength(0);
            var x = selectedPoint.X + (direction == Direction.Right ? step : (direction == Direction.Left ? -step : 0));
            var y = selectedPoint.Y + (direction == Direction.Bot ? step : (direction == Direction.Top ? -step : 0));
            return
                FieldGenerator.CellInField(horizontalCells, verticalCells, x, y) &&
                field[y, x] != CellType.DamagedShip &&
                field[y, x] != CellType.DestroyedShip &&
                field[y, x] != CellType.EmptyWater;
        }

        /// <summary>
        /// Получает размеры всех не разрушенных кораблей
        /// </summary>
        /// <param name="player">Игрок</param>
        /// <returns>Массив размеров оставшихся на поле кораблей</returns>
        private int[] GetShipSizes(Player player)
        {
            var ships = player.OneDeckeds.Union(player.DoubleDecks)
                .Union(player.ThreeDecks)
                .Union(player.FourDecks)
                .Union(player.FiveDecks);
            var result = new List<int>();
            foreach (var ship in ships.Where(ship => !ship.Destroyed() && !result.Contains(ship.Decks.Length)))
            {
                result.Add(ship.Decks.Length);
            }
            return result.ToArray();
        }
    }
}
