namespace Reversi
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        // the number of squers in the board. must be even >= 4
        const int BOARD_SIZE = 16;
        // the size of a square
        const int SQUARE_SIZE = 30;
        // the color of an empty square
        readonly Color defaultColor = new Button().BackColor;

        // the board
        readonly Button[,] board = new Button[BOARD_SIZE, BOARD_SIZE];

        // the colors of the players
        readonly Color[] colors = { Color.Red, Color.Blue };
        // the point of the playes
        readonly int[] points = { 2, 2 };


        int turn = 0;

        private void MainForm_Load(object sender, EventArgs e)
        {
            // The form client area
            ClientSize = new Size(BOARD_SIZE * SQUARE_SIZE, BOARD_SIZE * SQUARE_SIZE);
            Size buttonSize = new(SQUARE_SIZE, SQUARE_SIZE);

            for (int i = 0; i < BOARD_SIZE; ++i)
            {
                for (int j = 0; j < BOARD_SIZE; ++j)
                {
                    board[i, j] = new Button
                    {
                        Size = buttonSize,
                        Location = new Point(SQUARE_SIZE * i, SQUARE_SIZE * j),
                        TabStop = false
                    };
                    board[i, j].Click += Button_Click;
                    Controls.Add(board[i, j]);
                }
            }

            // Initialize the 4 center squres
            board[BOARD_SIZE / 2 - 1, BOARD_SIZE / 2 - 1].BackColor = colors[0];
            board[BOARD_SIZE / 2 - 1, BOARD_SIZE / 2].BackColor = colors[1];
            board[BOARD_SIZE / 2, BOARD_SIZE / 2 - 1].BackColor = colors[1];
            board[BOARD_SIZE / 2, BOARD_SIZE / 2].BackColor = colors[0];

            // Initialize the MainForm text
            MainFormText();

        }

        private void Button_Click(object sender, EventArgs e)
        {
            Button? button = sender as Button ?? new Button();

            int x = (button.Bounds.X) / SQUARE_SIZE;
            int y = (button.Bounds.Y) / SQUARE_SIZE;

            if (IsLegale(x, y))
            {
                Reverse(x, y);
                turn = (turn + 1) % 2;
                MainFormText();
            }
        }

        bool IsLegale(int x, int y)
        {
            if (!board[x, y].BackColor.Equals(defaultColor))
                return false;

            for (int i = -1; i <= 1; ++i)
            {
                for (int j = -1; j <= 1; ++j)
                {
                    if (IsLegaleDirection(x, y, i, j))
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        private bool IsLegaleDirection(int x, int y, int i, int j)
        {
            if (i == 0 && j == 0)
            {
                return false;
            }
            if (x + i < 0 || x + i >= BOARD_SIZE || y + j < 0 || y + j >= BOARD_SIZE)
            {
                return false;
            }
            if (!board[x + i, y + j].BackColor.Equals(colors[(turn + 1) % 2]))
            {
                return false;
            }
            int u = x + i, v = y + j;
            while (u + i >= 0 && u + i < BOARD_SIZE && v + j >= 0 && v + j < BOARD_SIZE && board[u + i, v + j].BackColor.Equals(colors[(turn + 1) % 2]))
            {
                u += i;
                v += j;
            }
            if (u + i >= 0 && u + i < BOARD_SIZE && v + j >= 0 && v + j < BOARD_SIZE && board[u + i, v + j].BackColor.Equals(colors[turn]))
                return true;
            return false;
        }

        private void Reverse(int x, int y)
        {
            board[x, y].BackColor = colors[turn];
            ++points[turn];
            for (int i = -1; i <= 1; ++i)
            {
                for (int j = -1; j <= 1; ++j)
                {
                    if (IsLegaleDirection(x, y, i, j))
                    {
                        Reverse(x, y, i, j);
                    }
                }
            }
        }

        private void Reverse(int x, int y, int i, int j)
        {
            int u = x + i, v = y + j;
            while (board[u, v].BackColor.Equals(colors[(turn + 1) % 2]))
            {
                board[u, v].BackColor = colors[turn];
                ++points[turn];
                --points[(turn + 1) % 2];
                u += i;
                v += j;
            }
        }

        private bool IsActive()
        {
            for (int x = 0; x < BOARD_SIZE; ++x)
            {
                for (int y = 0; y < BOARD_SIZE; ++y)
                {
                    if (IsLegale(x, y))
                        return true;
                }
            }
            return false;
        }

        void MainFormText()
        {
            Text =
                "Reversi " + colors[0].Name + ": " + points[0] + " " + colors[1].Name + ": " + points[1] +
                " turn: " + colors[turn].Name +
                " status: " + (IsActive() ? "Active" : "Finished");
        }
    }
}