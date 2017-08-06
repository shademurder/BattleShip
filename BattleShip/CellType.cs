namespace BattleShipp
{
    enum CellType
    {
        FogOverWater = 0,//туман над водой
        FogOverShip = 1,//туман над кораблём
        Water = 2,//вода
        Ship = 3,//корабль
        DamagedShip = 4,//подбитый корабль
        DestroyedShip = 5,//разрушенный корабль
        EmptyWater = 6//Цвет воды на поле игрока, в которую был произведён высрел
    }
}
