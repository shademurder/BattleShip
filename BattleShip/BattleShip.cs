using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace BattleShipp
{
    class BattleShip : UserControl
    {
        public BattleShip()
        {
            DoubleBuffered = true;
            SetFieldsSize();
            ResizeControl();
           // NewGame();
        }

        //размер поля от 5х5 до 25х25
        private int _verticalCells = 10;
        private int _horizontalCells = 10;
        private const int BorderSize = 1;
        private Size _cellSize = new Size(20, 20);
        private int _verticalSpace = 30;
        private int _horizontalSpace = 40;
        private int _spaceBetweenFields = 15;
        private Color _borderColor = Color.Black;
        private int _oneDeckeds = 4;
        private int _doubleDecks = 3;
        private int _threeDecks = 2;
        private int _fourDecks = 1;
        private int _fiveDecks = 0;
        private Player _player;
        private Player _pc;
        private IArtificialIntelligence _artificialIntelligence = new DefaultAI();
        private bool _aiMove = false;

        /// <summary>
        /// Количество ячеек по вертикали
        /// </summary>
        public int AVerticalCells
        {
            get
            {
                return _verticalCells;
            }
            set
            {
                if (value <= 4 || value >= 26) return;
                var oldValue = _verticalCells;
                _verticalCells = value;
                if (!CheckFieldSize())
                {
                    _verticalCells = oldValue;
                }
                SetFieldsSize();
                ResizeControl();
            }
        }
        /// <summary>
        /// Количество ячеек по горизонтали
        /// </summary>
        public int AHorizontalCells
        {
            get
            {
                return _horizontalCells;
            }
            set
            {
                if (value <= 4 || value >= 26) return;
                var oldValue = _horizontalCells;
                _horizontalCells = value;
                if (!CheckFieldSize())
                {
                    _horizontalCells = oldValue;
                }
                SetFieldsSize();
                ResizeControl();
            }
        }

        /// <summary>
        /// Цвет границ ячеек
        /// </summary>
        public Color BorderColor
        {
            get { return _borderColor; }
            set
            {
                _borderColor = value;
            }
        }

        /// <summary>
        /// Размер ячейки поля
        /// </summary>
        public Size CellSize
        {
            get { return _cellSize; }
            set
            {
                if (value.Height <= 0 || value.Width <= 0) return;
                _cellSize = value;
                ResizeControl();
            }
        }

        /// <summary>
        /// Цвет фона ячеек
        /// </summary>
        public Color CellBackground { get; set; } = Color.White;

        /// <summary>
        /// Флаг автоматической генерации местоположения кораблей на поле
        /// </summary>
        public bool RandomField { get; set; } = true;

        /// <summary>
        /// Отступы сверху и снизу от полей
        /// </summary>
        public int VerticalSpace
        {
            get { return _verticalSpace; }
            set
            {
                if (value < 0) return;
                _verticalSpace = value;
                ResizeControl();
            }
        }

        /// <summary>
        /// Отступы слева и справа от полей
        /// </summary>
        public int HorizontalSpace
        {
            get
            {
                return _horizontalSpace;
            }

            set
            {
                if (value < 0) return;
                _horizontalSpace = value;
                ResizeControl();
            }
        }

        /// <summary>
        /// Расстояние между двумя полями
        /// </summary>
        public int SpaceBetweenFields
        {
            get
            {
                return _spaceBetweenFields;
            }

            set
            {
                if (value < 0) return;
                _spaceBetweenFields = value;
                ResizeControl();
            }
        }
        /// <summary>
        /// Цвет тумана
        /// </summary>
        public Color FogColor { get; set; } = Color.Gray;
        /// <summary>
        /// Цвет воды
        /// </summary>
        public Color WaterColor { get; set; } = Color.LightSkyBlue;
        /// <summary>
        /// Цвет корабля на карте игрока
        /// </summary>
        public Color ShipColor { get; set; } = Color.LimeGreen;
        /// <summary>
        /// Цвет повреждённой палубы корабля
        /// </summary>
        public Color DamagedShipColor { get; set; } = Color.Red;
        /// <summary>
        /// Цвет разрушенного корабля
        /// </summary>
        public Color DestroedShipColor { get; set; } = Color.DarkRed;
        /// <summary>
        /// Цвет воды на поле игрока, в которую был произведён высрел
        /// </summary>
        public Color EmptyWaterColor { get; set; } = Color.Aquamarine;
        /// <summary>
        /// Количество однопалубных кораблей
        /// </summary>
        [Category("Корабли"), Description("Позволяет задать количество однопалубных кораблей")]
        public int OneDeckShips
        {
            get { return _oneDeckeds; }
            set
            {
                if (value < 0) return;
                var oldValue = _oneDeckeds;
                _oneDeckeds = value;
                if (!CheckFieldSize())
                {
                    _oneDeckeds = oldValue;
                }
                SetFieldsSize();
            }
        }
        /// <summary>
        /// Количество двухпалубных кораблей
        /// </summary>
        [Category("Корабли"), Description("Позволяет задать количество двухпалубных кораблей")]
        public int DoubleDeckShips
        {
            get { return _doubleDecks; }
            set
            {
                if (value < 0) return;
                var oldValue = _doubleDecks;
                _doubleDecks = value;
                if (!CheckFieldSize())
                {
                    _doubleDecks = oldValue;
                }
                SetFieldsSize();
            }
        }
        /// <summary>
        /// Количество трёхпалубных кораблей
        /// </summary>
        [Category("Корабли"), Description("Позволяет задать количество трёхпалубных кораблей")]
        public int ThreeDeckShips
        {
            get { return _threeDecks; }
            set
            {
                if (value < 0) return;
                var oldValue = _threeDecks;
                _threeDecks = value;
                if (!CheckFieldSize())
                {
                    _threeDecks = oldValue;
                }
                SetFieldsSize();
            }
        }
        /// <summary>
        /// Количество четырёхпалубных кораблей
        /// </summary>
        [Category("Корабли"), Description("Позволяет задать количество четырёхпалубных кораблей")]
        public int FourDeckShips
        {
            get { return _fourDecks; }
            set
            {
                if (value < 0) return;
                var oldValue = _fourDecks;
                _fourDecks = value;
                if (!CheckFieldSize())
                {
                    _fourDecks = oldValue;
                }
                SetFieldsSize();
            }
        }

        /// <summary>
        /// Количество пятипалубных кораблей
        /// </summary>
        [Category("Корабли"), Description("Позволяет задать количество пятипалубных кораблей")]
        public int FiveDeckShips
        {
            get { return _fiveDecks; }
            set
            {
                if (value < 0) return;
                var oldValue = _fiveDecks;
                _fiveDecks = value;
                if (!CheckFieldSize())
                {
                    _fiveDecks = oldValue;
                }
                SetFieldsSize();
            }
        }

        internal IArtificialIntelligence ArtificialIntelligence
        {
            get
            {
                return _artificialIntelligence;
            }

            set
            {
                if (value != null)
                {
                    _artificialIntelligence = value;
                }
            }
        }

        public bool UseDefaultCheckingFieldSize { get; set; } = true;


        /// <summary>
        /// Выстрел по полю
        /// </summary>
        public event Action<int, int, PlayerType> Shot;


        /// <summary>
        /// Проверка на возможность поля вместить все корабли
        /// </summary>
        /// <returns></returns>
        private bool CheckFieldSize()
        {
            if (!UseDefaultCheckingFieldSize) return true;
            //Для установки каждого корабля берётся дополнительно 1 клетка, чтобы избежать полного заполнения поля
            //Но лучше дополнительно из количества ячеек игрового поля вычитать 10%
            var fieldSpace = AVerticalCells * AHorizontalCells;
            var shipsSpace = OneDeckShips * 6 + DoubleDeckShips * 8 + ThreeDeckShips * 10 + FourDeckShips * 12 +
                             FiveDeckShips * 14;//130 клеток для 5\4\3\2\1 и 80 для 4\3\2\1\0
            if (fieldSpace < shipsSpace)
            {
                //MessageBox.Show(@"Мало места для выбранного количества кораблей");
            }
            return fieldSpace >= shipsSpace;
        }

        /// <summary>
        /// Пересоздание массивов с данными полей
        /// </summary>
        private void SetFieldsSize()
        {
            _player = new Player(OneDeckShips, DoubleDeckShips, ThreeDeckShips, FourDeckShips, FiveDeckShips, PlayerType.Player, AHorizontalCells, AVerticalCells);
            _pc = new Player(OneDeckShips, DoubleDeckShips, ThreeDeckShips, FourDeckShips, FiveDeckShips, PlayerType.Pc, AHorizontalCells, AVerticalCells);
        }

        /// <summary>
        /// Перерассчёт размера контрола
        /// </summary>
        private void ResizeControl()
        {
            var fieldSize = new Size((AHorizontalCells * (CellSize.Width + BorderSize) + BorderSize), AVerticalCells * (CellSize.Height + BorderSize) + BorderSize);
            Size = new Size((fieldSize.Width + HorizontalSpace) * 2 + SpaceBetweenFields, fieldSize.Height + VerticalSpace * 2);
        }

        /// <summary>
        /// Перерисовка сеток полей
        /// </summary>
        /// <param name="paintGraphics"></param>
        private void RepaintFields(Graphics paintGraphics)
        {
            var pen = new Pen(_borderColor);
            var fieldWidth = (CellSize.Width + BorderSize) * AHorizontalCells + BorderSize;
            var fieldHeight = (CellSize.Height + BorderSize) * AVerticalCells + BorderSize;
            paintGraphics.FillRectangle(new SolidBrush(CellBackground), HorizontalSpace, VerticalSpace, fieldWidth, fieldHeight);
            paintGraphics.FillRectangle(new SolidBrush(CellBackground), fieldWidth + HorizontalSpace + SpaceBetweenFields, VerticalSpace, fieldWidth, fieldHeight);
            for (var border = 0; border < AHorizontalCells + 1; border++)//отрисовка вертикальных линий поля
            {
                var shift = HorizontalSpace + (CellSize.Width + BorderSize) * border;
                paintGraphics.DrawLine(pen, new Point(shift, VerticalSpace), new Point(shift, VerticalSpace + fieldHeight - BorderSize));
                paintGraphics.DrawLine(pen, new Point(fieldWidth + shift + SpaceBetweenFields, VerticalSpace), new Point(fieldWidth + shift + SpaceBetweenFields, VerticalSpace + fieldHeight - BorderSize));
            }
            for (var border = 0; border < AVerticalCells + 1; border++)//отрисовка горизонтальных линий поля
            {
                var shift = VerticalSpace + (CellSize.Height + BorderSize) * border;
                paintGraphics.DrawLine(pen, new Point(HorizontalSpace, shift), new Point(HorizontalSpace + fieldWidth - BorderSize, shift));
                paintGraphics.DrawLine(pen, new Point(HorizontalSpace + fieldWidth + SpaceBetweenFields, shift), new Point(HorizontalSpace + fieldWidth + SpaceBetweenFields + fieldWidth - BorderSize, shift));
            }
        }

        /// <summary>
        /// Получение цвета в зависимости от типа ячейки
        /// </summary>
        /// <param name="cellType">Тип ячейки</param>
        /// <param name="playerType">Тип игрока</param>
        /// <returns></returns>
        private Color GetCellColor(CellType cellType, PlayerType playerType)
        {
            switch (cellType)
            {
                case CellType.DamagedShip:
                    return DamagedShipColor;
                case CellType.DestroyedShip:
                    return DestroedShipColor;
                case CellType.Water:
                    return WaterColor;
                case CellType.Ship:
                    return ShipColor;
                case CellType.FogOverShip:
                    return FogColor;
                case CellType.FogOverWater:
                    return FogColor;
                case CellType.EmptyWater:
                    return EmptyWaterColor;
                default:
                    return CellBackground;
            }
        }

        /// <summary>
        /// Заполнение определённой клетки поля
        /// </summary>
        /// <param name="paintGraphics"></param>
        /// <param name="x">Координата поля по горизонтали от 0</param>
        /// <param name="y">Координата поля по вертикали от 0</param>
        /// <param name="cellType">Тип закрашиваемой ячейки</param>
        /// <param name="playerType">Тип игрока</param>
        /// <param name="shift"></param>
        private void FillGameCell(Graphics paintGraphics, int x, int y, CellType cellType, PlayerType playerType, int shift)
        {
            paintGraphics.FillRectangle(new SolidBrush(GetCellColor(cellType, playerType)), shift + x * (CellSize.Width + BorderSize), VerticalSpace + y * (CellSize.Height + BorderSize) + BorderSize, CellSize.Width, CellSize.Height);
        }

        /// <summary>
        /// Перерисовка всех ячеек полей
        /// </summary>
        /// <param name="paintGraphics"></param>
        private void RepaintGameField(Graphics paintGraphics)
        {
            if (_player == null || _pc == null) return;
            var playerShift = HorizontalSpace + BorderSize;
            var pcShift = HorizontalSpace + SpaceBetweenFields + (CellSize.Width + BorderSize) * AHorizontalCells + BorderSize + BorderSize;
            for (var y = 0; y < AVerticalCells; y++)
            {
                for (var x = 0; x < AHorizontalCells; x++)
                {
                    FillGameCell(paintGraphics, x, y, _player.Field[y, x], _player.PlayerType, playerShift);
                    FillGameCell(paintGraphics, x, y, _pc.Field[y, x], _pc.PlayerType, pcShift);
                }
            }
        }

        private void BotMove()
        {
            var nextPoint = ArtificialIntelligence.GetNextPoint(_player);
            MessageBox.Show($"({nextPoint.X};{nextPoint.Y})");
            if (_player.Field[nextPoint.Y, nextPoint.X] == CellType.Ship)
            {
                OnShot(nextPoint.X, nextPoint.Y, PlayerType.Player);
                if (_player.Lose())
                {
                    EndGame();
                    MessageBox.Show(@"Вы проиграли!");
                    return;
                }
                BotMove();
            }
            else
            {
                OnShot(nextPoint.X, nextPoint.Y, PlayerType.Player);
                _aiMove = false;
            }
        }

        /// <summary>
        /// Генерация игровых полей
        /// </summary>
        public bool NewGame()
        {
            _player = FieldGenerator.TryGenerateField(AHorizontalCells, AVerticalCells, _fiveDecks, _fourDecks, _threeDecks, _doubleDecks, _oneDeckeds, PlayerType.Player, 10, 10);
            _pc = FieldGenerator.TryGenerateField(AHorizontalCells, AVerticalCells, _fiveDecks, _fourDecks, _threeDecks, _doubleDecks, _oneDeckeds, PlayerType.Pc, 10, 10);
            if (_player == null || _pc == null) return false;
            _player.NewGame(this);
            _pc.NewGame(this);
            FieldGenerator.HideField(_pc.Field);
            return true;
        }

        private void EndGame()
        {
            _player.EndGame(this);
            _pc.EndGame(this);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            RepaintFields(e.Graphics);
            //NewGame();
            //OnShot(2, 3, PlayerType.Player);
            RepaintGameField(e.Graphics);
            base.OnPaint(e);
        }

        private Point GetClickLocation(Point location)
        {
            var horizontalStep = BorderSize + CellSize.Width;
            var verticalStep = BorderSize + CellSize.Height;
            var x = location.X / horizontalStep + (location.X % horizontalStep > 0 ? 1 : 0) - 1;
            var y = location.Y / verticalStep + (location.Y % verticalStep > 0 ? 1 : 0) - 1;
            return new Point(x, y);
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            if (!_aiMove)
            {
                var fieldWidth = (BorderSize + CellSize.Width)*AHorizontalCells + BorderSize;
                var fieldHeight = (BorderSize + CellSize.Height)*AVerticalCells + BorderSize;
                var leftShift = HorizontalSpace + SpaceBetweenFields + fieldWidth;
                if (e.X > leftShift && e.X <= leftShift + fieldWidth && e.Y > VerticalSpace &&
                    e.Y <= VerticalSpace + fieldHeight)
                {
                    var point = GetClickLocation(new Point(e.X - leftShift, e.Y - VerticalSpace));
                    if (_pc.Field[point.Y, point.X] == CellType.FogOverShip)
                    {
                        OnShot(point.X, point.Y, PlayerType.Pc);
                        if (_pc.Lose())
                        {
                            _aiMove = true;
                            EndGame();
                            MessageBox.Show(@"Вы победили!");
                        }
                    }
                    else if (_pc.Field[point.Y, point.X] == CellType.FogOverWater)
                    {
                        OnShot(point.X, point.Y, PlayerType.Pc);
                        _aiMove = true;
                        BotMove();
                    }
                    else
                    {
                        MessageBox.Show(@"WTF?");
                    }
                }
            }
            base.OnMouseClick(e);
        }

        protected void OnShot(int x, int y, PlayerType playerType)
        {
            Shot?.Invoke(x, y, playerType);
            Refresh();
        }
    }
}
