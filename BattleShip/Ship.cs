using System.Linq;

namespace BattleShipp
{
    class Ship
    {
        /// <summary>
        /// Класс корабля
        /// </summary>
        /// <param name="owner">Владелец корабля</param>
        /// <param name="decks">Количество палуб корабля</param>
        public Ship(PlayerType owner, int decks)
        {
            Owner = owner;
            Decks = new Deck[decks];
        }

        /// <summary>
        /// Владелец корабля
        /// </summary>
        public PlayerType Owner { get; set; }
        /// <summary>
        /// Палубы корабля
        /// </summary>
        public Deck[] Decks { get; set; }
        /// <summary>
        /// Факт разрушения корабля
        /// </summary>
        /// <returns></returns>
        public bool Destroyed()
        {
            if (Decks != null)
            {
                if (Decks.Any(deck => !deck.Destroyed))
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Определяет, есть ли в этом корабле палуба с указанными координатами
        /// </summary>
        /// <param name="row">Координата по y</param>
        /// <param name="column">Координата по x</param>
        /// <returns>true, если такая палуба есть в корабле</returns>
        public bool ContainsDeck(int row, int column)
        {
            return Decks != null && (from deck in Decks where deck.Location == new System.Drawing.Point(column, row) select deck).ToArray().Length != 0;
        }

        /// <summary>
        /// Получает направление корабля
        /// </summary>
        /// <returns>true - горизонтальное, false - вертикальное</returns>
        public bool GetDirection()
        {
            if (Decks.Length == 1) return true;
            return Decks[0].Location.X != Decks[1].Location.X;
        }
    }
}
