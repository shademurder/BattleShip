using System.Drawing;

namespace BattleShipp
{
    interface IArtificialIntelligence
    {
        Point GetNextPoint(Player player);
    }
}
