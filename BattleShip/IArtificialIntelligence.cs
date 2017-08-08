using System.Drawing;

namespace BattleShipp
{
    /// <summary>
    /// Интерфейс для создания ИИ
    /// </summary>
    interface IArtificialIntelligence
    {
        Point GetNextPoint(Player player);
    }
}
