using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Linez
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Colors = new List<string>();
            Colors.Add("#FFFF0000");
            Colors.Add("#FFFFFF00");
            Colors.Add("#FF008000");
            Colors.Add("#FF0000FF");
            Colors.Add("#FFADD8E6");
            Colors.Add("#FFFFA500");
            Colors.Add("#FF800080");
            ButtonStatus = "NothingClicked";
            StartCoord = new Coords
            {
                x = 0,
                y = 0
            };
            TargetCoord = new Coords
            {
                x = 0,
                y = 0
            };
            BallSelectedClassId = "00";
            BallSelectedColor = new Color();
            HasRemovedRow = false;
            HasRemovedColumn = false;
            HasRemovedDiagL = false;
            HasRemovedDiagR = false;
            NextColors = new List<string> { null, null, null };
            Score = 0;
            DefenderScore = 100;
            InitializeScore();
            InitializeMaze();
            InitializeColorMaze();
            CreateGrid();
            GenerateNextColors();
            DisplayNextColors();
            PlaceThreeBalls();
        }
        private void CreateGrid()
        {
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });
            MainGrid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) });


            for (int rowIndex = 0; rowIndex < 9; rowIndex++)
            {
                for (int columnIndex = 0; columnIndex < 9; columnIndex++)
                {

                    var stackLayout = new StackLayout
                    {
                        BackgroundColor = Color.AliceBlue,
                        HorizontalOptions = LayoutOptions.FillAndExpand,
                        VerticalOptions = LayoutOptions.FillAndExpand,
                        ClassId = rowIndex.ToString() + columnIndex.ToString()
                    };
                    var tapGestureRecognizer = new TapGestureRecognizer();
                    tapGestureRecognizer.Tapped += (s, e) => {
                        if (ButtonStatus == "BallSelected")
                        {
                            TargetCoord = new Coords
                            {
                                x = Int32.Parse(stackLayout.ClassId[0].ToString()),
                                y = Int32.Parse(stackLayout.ClassId[1].ToString())
                            };
                            
                            
                            if (Algorithm.Astar(Maze, StartCoord, TargetCoord))
                            {
                                VisualizeBallAtTarget(TargetCoord, BallSelectedColor);
                                Processing.UpdateMazeToOne(Maze, TargetCoord.x, TargetCoord.y);
                                Processing.UpdateColorMazeToColor(ColorMaze, TargetCoord.x, TargetCoord.y, BallSelectedColor.ToHex());
                                Processing.UpdateMazeToZero(Maze, StartCoord.x, StartCoord.y);
                                Processing.UpdateColorMazeToNull(ColorMaze, StartCoord.x, StartCoord.y);
                                RemoveBallAtStart(StartCoord);
                                RemoveBackgroundColor();
                                ButtonStatus = "NothingClicked";
                                CheckColorRow();
                                CheckColorColumn();
                                CheckColorDiagonalLeft();
                                CheckColorDiagonalRight();
                                RemoveBalls();
                                //TrueFalseList.Text = HasRemovedRow.ToString() + HasRemovedColumn.ToString() + HasRemovedDiagL.ToString() + HasRemovedDiagR.ToString();
                                if (!(HasRemovedRow == true || HasRemovedColumn == true || HasRemovedDiagL == true || HasRemovedDiagR == true))
                                {
                                    NextRound();
                                }
                                else
                                {
                                    GetScore();
                                }
                            }
                            else
                            {
                                stackLayout.BackgroundColor = Color.Black;
                            }
                        }
                    };
                    stackLayout.GestureRecognizers.Add(tapGestureRecognizer);
                    MainGrid.Children.Add(stackLayout, columnIndex, rowIndex);
                }
            }
        }
        private void NextRound()
        {
            PlaceOneBallWithColor(NextColors[0]);
            PlaceOneBallWithColor(NextColors[1]);
            PlaceOneBallWithColor(NextColors[2]);
            GenerateNextColors();
            DisplayNextColors();
            CheckColorRow();
            CheckColorColumn();
            CheckColorDiagonalLeft();
            CheckColorDiagonalRight();
            RemoveBalls();
            if (HasRemovedRow == true || HasRemovedColumn == true || HasRemovedDiagL == true || HasRemovedDiagR == true)
            {
                GetScore();
            }
        }
        public void BoardChangeColor(int x, int y)
        {
            var items = MainGrid.Children.Cast<StackLayout>().Where(i => Grid.GetRow(i) == x && Grid.GetColumn(i) == y);
            foreach (var item in items)
            {
                if (ButtonStatus == "NothingClicked")
                {
                    item.BackgroundColor = Color.Black;
                }
                else
                {
                    item.BackgroundColor = Color.AliceBlue;
                }
            }
        }
        public void ManipulateTest1(object sender, EventArgs args)
        {
            PlaceThreeBalls();
        }
        private void InitializeMaze()
        {
            Maze = new List<List<int>>();
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            Maze.Add(new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0 });
        }
        private void InitializeColorMaze()
        {
            ColorMaze = new List<List<string>>();
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
            ColorMaze.Add(new List<string> { null, null, null, null, null, null, null, null, null });
        }
        private List<List<int>> Maze { get; set; }
        private List<List<string>> ColorMaze { get; set; }
        private List<string> Colors { get; set; }
        private bool HasRemovedRow { get; set; }
        private bool HasRemovedColumn { get; set; }
        private bool HasRemovedDiagL { get; set; }
        private bool HasRemovedDiagR { get; set; }
        private List<string> NextColors { get; set; }
        private int Score { get; set; }
        private int DefenderScore { get; set; }
        private void PlaceOneBall()
        {
            if (Processing.CheckRoom(Maze) > 2)
            {
                var color = Processing.GetRandomColor(Colors);
                var tempCoord = Processing.RandomPlaceBall(Maze, ColorMaze, color);
                VisualizeBall(tempCoord, color);
                Processing.UpdateColorMazeToColor(ColorMaze, tempCoord.x, tempCoord.y, color);
                Processing.UpdateMazeToOne(Maze, tempCoord.x, tempCoord.y);
            } 
        }
        private void PlaceOneBallWithColor(string color)
        {
            if (Processing.CheckRoom(Maze) > 2)
            {
                var tempCoord = Processing.RandomPlaceBall(Maze, ColorMaze, color);
                VisualizeBall(tempCoord, color);
                Processing.UpdateColorMazeToColor(ColorMaze, tempCoord.x, tempCoord.y, color);
                Processing.UpdateMazeToOne(Maze, tempCoord.x, tempCoord.y);
            }
            else
            {
                //TrueFalseList.Text = "Game Over!";
                DisplayAlert($"Game Over! Your Score is {Score}", "Start a new game", "OK");
                RestartGame();
            }
        }
        private void GetScore()
        {
            Score += 10;
            if (Score > DefenderScore)
            {
                Challenger.Progress = 1;
                Defender.Progress = (double)DefenderScore / Score;
            }
            else
            {
                Defender.Progress = 1;
                Challenger.Progress = (double)Score / DefenderScore;
            }
            DefenderText.Text = "Defender   " + DefenderScore.ToString();
            ChallengerText.Text = "Challenger " + Score.ToString();
        }
        private void InitializeScore()
        {
            DefenderText.Text = "Defender   " + DefenderScore.ToString();
            ChallengerText.Text = "Challenger " + Score.ToString();
        }
        private void DisplayNextColors()
        {

            switch (NextColors[0])
            {
                case "#FFFF0000":
                    P1.BackgroundColor = Color.Red;
                    break;
                case "#FF0000FF":
                    P1.BackgroundColor = Color.Blue;
                    break;
                case "#FFADD8E6":
                    P1.BackgroundColor = Color.LightBlue;
                    break;
                case "#FFFFFF00":
                    P1.BackgroundColor = Color.Yellow;
                    break;
                case "#FF008000":
                    P1.BackgroundColor = Color.Green;
                    break;
                case "#FFFFA500":
                    P1.BackgroundColor = Color.Orange;
                    break;
                case "#FF800080":
                    P1.BackgroundColor = Color.Purple;
                    break;
                default:
                    break;
            }
            switch (NextColors[1])
            {
                case "#FFFF0000":
                    P2.BackgroundColor = Color.Red;
                    break;
                case "#FF0000FF":
                    P2.BackgroundColor = Color.Blue;
                    break;
                case "#FFADD8E6":
                    P2.BackgroundColor = Color.LightBlue;
                    break;
                case "#FFFFFF00":
                    P2.BackgroundColor = Color.Yellow;
                    break;
                case "#FF008000":
                    P2.BackgroundColor = Color.Green;
                    break;
                case "#FFFFA500":
                    P2.BackgroundColor = Color.Orange;
                    break;
                case "#FF800080":
                    P2.BackgroundColor = Color.Purple;
                    break;
                default:
                    break;
            }
            switch (NextColors[2])
            {
                case "#FFFF0000":
                    P3.BackgroundColor = Color.Red;
                    break;
                case "#FF0000FF":
                    P3.BackgroundColor = Color.Blue;
                    break;
                case "#FFADD8E6":
                    P3.BackgroundColor = Color.LightBlue;
                    break;
                case "#FFFFFF00":
                    P3.BackgroundColor = Color.Yellow;
                    break;
                case "#FF008000":
                    P3.BackgroundColor = Color.Green;
                    break;
                case "#FFFFA500":
                    P3.BackgroundColor = Color.Orange;
                    break;
                case "#FF800080":
                    P3.BackgroundColor = Color.Purple;
                    break;
                default:
                    break;
            }
        }
        private void GenerateNextColors()
        {
            for (var i = 0; i < 3; i++)
            {
                NextColors[i] = (Processing.GetRandomColor(Colors));
            }
        }

        private void PlaceThreeBalls()
        {
            for (var i = 0; i < 3; i++)
            {
                PlaceOneBall();
            }
        }

        public void VisualizeBall(Coords coord, string color)
        {
            var items = MainGrid.Children.Cast<StackLayout>().Where(i => Grid.GetRow(i) == coord.x && Grid.GetColumn(i) == coord.y);
            foreach (var item in items)
            {
                var button = new Button
                {
                    CornerRadius = 12,
                    WidthRequest = 24,
                    HeightRequest = 24,
                    BorderWidth = 0,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    ClassId = coord.x.ToString() + coord.y.ToString()
                };
                switch(color)
                { 
                    case "#FFFF0000":
                        button.BackgroundColor = Color.Red;
                        break;
                    case "#FF0000FF":
                        button.BackgroundColor = Color.Blue;
                        break;
                    case "#FFADD8E6":
                        button.BackgroundColor = Color.LightBlue;
                        break;
                    case "#FFFFFF00":
                        button.BackgroundColor = Color.Yellow;
                        break;
                    case "#FF008000":
                        button.BackgroundColor = Color.Green;
                        break;
                    case "#FFFFA500":
                        button.BackgroundColor = Color.Orange;
                        break;
                    case "#FF800080":
                        button.BackgroundColor = Color.Purple;
                        break;
                    default:
                        break;
                }
                button.Clicked += OnBallClick;
                item.Children.Add(button);
            }
        }
        public void VisualizeBallAtTarget(Coords coord, Color color)
        {
            var items = MainGrid.Children.Cast<StackLayout>().Where(i => Grid.GetRow(i) == coord.x && Grid.GetColumn(i) == coord.y);
            foreach (var item in items)
            {
                var button = new Button
                {
                    CornerRadius = 12,
                    WidthRequest = 24,
                    HeightRequest = 24,
                    BorderWidth = 0,
                    HorizontalOptions = LayoutOptions.Center,
                    VerticalOptions = LayoutOptions.CenterAndExpand,
                    ClassId = coord.x.ToString() + coord.y.ToString(),
                    BackgroundColor = color
                };
                
                button.Clicked += OnBallClick;
                item.Children.Add(button);
            }
        }
        public void RemoveBallAtStart(Coords coord)
        {
            var items = MainGrid.Children.Cast<StackLayout>().Where(i => Grid.GetRow(i) == coord.x && Grid.GetColumn(i) == coord.y);
            foreach (var item in items)
            {
                item.Children.Clear();
            }
        }
        private string ButtonStatus { get; set; }
        private Coords StartCoord { get; set; }
        private Coords TargetCoord { get; set; }
        private string BallSelectedClassId { get; set; }
        private Color BallSelectedColor { get; set; }
        private void OnBallClick(object sender, EventArgs args)
        {
            var btn = (Button)sender;
            if (ButtonStatus == "NothingClicked")
            {
                StartCoord = new Coords
                {
                    x = Int32.Parse(btn.ClassId[0].ToString()),
                    y = Int32.Parse(btn.ClassId[1].ToString())
                };
                BallSelectedClassId = btn.ClassId;
                BoardChangeColor(Int32.Parse(btn.ClassId[0].ToString()), Int32.Parse(btn.ClassId[1].ToString()));
                ButtonStatus = "BallSelected";
                
                BallSelectedColor = btn.BackgroundColor;
                
            }
            else if (ButtonStatus == "BallSelected" && btn.ClassId == BallSelectedClassId)
            {
                BoardChangeColor(Int32.Parse(btn.ClassId[0].ToString()), Int32.Parse(btn.ClassId[1].ToString()));
                ButtonStatus = "NothingClicked";
                
            }
            
        }
        private void RemoveBackgroundColor()
        { 
            var items = MainGrid.Children.Cast<StackLayout>().Where(s => Grid.GetRow(s) > -1);
            foreach (var item in items)
            {
                item.BackgroundColor = Color.AliceBlue;
            } 
        }

        private void RemoveBalls()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 9; j++)
                {
                    if (Maze[i][j] == 0)
                    {
                        var items = MainGrid.Children.Cast<StackLayout>().Where(s => Grid.GetRow(s) == i && Grid.GetColumn(s) == j);
                        foreach (var item in items)
                        {
                            item.Children.Clear();
                        }
                    }
                }
            }
            
        }
        public void CheckColorRow()
        {
            var num = 0;
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (ColorMaze[i][j] != null && ColorMaze[i][j + 1] == ColorMaze[i][j] && ColorMaze[i][j + 2] == ColorMaze[i][j] && ColorMaze[i][j + 3] == ColorMaze[i][j] && ColorMaze[i][j + 4] == ColorMaze[i][j])
                    {
                        Maze[i][j] = 0;
                        Maze[i][j + 1] = 0;
                        Maze[i][j + 2] = 0;
                        Maze[i][j + 3] = 0;
                        Maze[i][j + 4] = 0;
                        ColorMaze[i][j] = null;
                        ColorMaze[i][j + 1] = null;
                        ColorMaze[i][j + 2] = null;
                        ColorMaze[i][j + 3] = null;
                        ColorMaze[i][j + 4] = null;
                        num++;
                    }
                }
            }
            if (num == 0)
            {
                HasRemovedRow = false;
            }
            else
            {
                HasRemovedRow = true;
            }
        }
        public void CheckColorColumn()
        {
            var num = 0;
            for (var i = 0; i < 9; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (ColorMaze[j][i] != null && ColorMaze[j + 1][i] == ColorMaze[j][i] && ColorMaze[j + 2][i] == ColorMaze[j][i] && ColorMaze[j + 3][i] == ColorMaze[j][i] && ColorMaze[j + 4][i] == ColorMaze[j][i])
                    {
                        Maze[j][i] = 0;
                        Maze[j + 1][i] = 0;
                        Maze[j + 2][i] = 0;
                        Maze[j + 3][i] = 0;
                        Maze[j + 4][i] = 0;
                        ColorMaze[j][i] = null;
                        ColorMaze[j + 1][i] = null;
                        ColorMaze[j + 2][i] = null;
                        ColorMaze[j + 3][i] = null;
                        ColorMaze[j + 4][i] = null;
                        num++;
                    }
                }
            }
            if (num == 0)
            {
                HasRemovedColumn = false;
            }
            else
            {
                HasRemovedColumn = true;
            }
        }
        public void CheckColorDiagonalLeft()
        {
            var num = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    if (ColorMaze[i][j] != null && ColorMaze[i + 1][j + 1] == ColorMaze[i][j] && ColorMaze[i + 2][j + 2] == ColorMaze[i][j] && ColorMaze[i + 3][j + 3] == ColorMaze[i][j] && ColorMaze[i + 4][j + 4] == ColorMaze[i][j])
                    {
                        Maze[i][j] = 0;
                        Maze[i + 1][j + 1] = 0;
                        Maze[i + 2][j + 2] = 0;
                        Maze[i + 3][j + 3] = 0;
                        Maze[i + 4][j + 4] = 0;
                        ColorMaze[i][j] = null;
                        ColorMaze[i + 1][j + 1] = null;
                        ColorMaze[i + 2][j + 2] = null;
                        ColorMaze[i + 3][j + 3] = null;
                        ColorMaze[i + 4][j + 4] = null;
                        num++;
                    }
                }
            }
            if (num == 0)
            {
                HasRemovedDiagL = false;
            }
            else
            {
                HasRemovedDiagL = true;
            }
        }
        public void CheckColorDiagonalRight()
        {
            var num = 0;
            for (var i = 0; i < 5; i++)
            {
                for (var j = 8; j > 3; j--)
                {
                    if (ColorMaze[i][j] != null && ColorMaze[i + 1][j - 1] == ColorMaze[i][j] && ColorMaze[i + 2][j - 2] == ColorMaze[i][j] && ColorMaze[i + 3][j - 3] == ColorMaze[i][j] && ColorMaze[i + 4][j - 4] == ColorMaze[i][j])
                    {
                        Maze[i][j] = 0;
                        Maze[i + 1][j - 1] = 0;
                        Maze[i + 2][j - 2] = 0;
                        Maze[i + 3][j - 3] = 0;
                        Maze[i + 4][j - 4] = 0;
                        ColorMaze[i][j] = null;
                        ColorMaze[i + 1][j - 1] = null;
                        ColorMaze[i + 2][j - 2] = null;
                        ColorMaze[i + 3][j - 3] = null;
                        ColorMaze[i + 4][j - 4] = null;
                        num++;
                    }
                }
            }
            if (num == 0)
            {
                HasRemovedDiagR = false;
            }
            else
            {
                HasRemovedDiagR = true;
            }
        }
        private void RestartGame()
        {
            for (var i = 0; i < 9; i++)
            {
                for (var j =0; j < 9; j++)
                {
                    Maze[i][j] = 0;
                    ColorMaze[i][j] = null;
                }
            }
            RemoveBalls();
            Score = 0;
            DefenderScore = 100;
            Defender.Progress = 1;
            Challenger.Progress = 0;
            InitializeScore();
            PlaceThreeBalls();
            GenerateNextColors();
            DisplayNextColors();
            ButtonStatus = "NothingClicked";
            HasRemovedRow = false;
            HasRemovedColumn = false;
            HasRemovedDiagL = false;
            HasRemovedDiagR = false;
        }
    }
}
