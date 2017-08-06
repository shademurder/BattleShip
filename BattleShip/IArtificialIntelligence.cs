using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using System.Threading.Tasks;

namespace BattleShipp
{
    interface IArtificialIntelligence
    {
        Point GetNextPoint(Player player);
    }
}
