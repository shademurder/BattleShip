using System.Drawing;

namespace BattleShipp
{
    /// <summary>
    /// Класс палубы корабля
    /// </summary>
    class Deck
    {
        /// <summary>
        /// Рарушена ли эта палуба
        /// </summary>
        public bool Destroyed { get; set; } = false;
        /// <summary>
        /// Положение палубы на карте
        /// </summary>
        public Point Location { get; set; } = new Point(0, 0);
    }
}
