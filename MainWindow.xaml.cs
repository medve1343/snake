using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Snake
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Dictionary<GridValue, ImageSource> gridVaToImage = new()
        {
            {GridValue.Empty, Images.Empty },
            {GridValue.Snake, Images.Body },
            {GridValue.Food, Images.Food }
        };

        private readonly int rows = 15, cols = 15;

        private readonly Image[,] gridImages;

        private GameState gamestate;

        public MainWindow()
        {
            InitializeComponent();
            gridImages = SetupGrid();
            gamestate = new GameState(rows, cols);
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Draw();
            await GameLoop();
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (gamestate.GameOver)
            {
                return;
            }

            switch(e.Key)
            {
                case Key.Left:
                    gamestate.ChangeDirection(Direction.left);
                    break;
                case Key.Right:
                    gamestate.ChangeDirection(Direction.right);
                    break;
                case Key.Up:
                    gamestate.ChangeDirection(Direction.up);
                    break;
                case Key.Down:
                    gamestate.ChangeDirection(Direction.down);
                    break;
            }
        }

        private async Task GameLoop()
        {
            while(!gamestate.GameOver)
            {
                await Task.Delay(200);
                gamestate.Move();
                Draw();
            }
        }

        private Image[,] SetupGrid()
        {
            Image[,] images = new Image[rows, cols];
            GameGrid.Rows = rows;
            GameGrid.Columns = cols;

            for (int r = 0; r < rows; r++)
            {
                for (int c = 0; c < cols; c++)
                {
                    Image image = new Image
                    {
                        Source = Images.Empty
                    };

                    images[r, c] = image;
                    GameGrid.Children.Add(image);
                }
            }
            return images;
        }

        private void Draw()
        {
            DrawGrid();
            ScoreText.Text = $"SCORE {gamestate.Score}";
        }
        private void DrawGrid()
        {
            for (int r = 0; r < rows; r++)
            {
                for(int c = 0; c < cols; c++)
                {
                    GridValue gridVal = gamestate.Grid[r, c];
                    gridImages[r, c].Source = gridVaToImage[gridVal];
                }
            }
        }
    }
}