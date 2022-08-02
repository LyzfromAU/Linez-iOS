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
            InitializeMaze();
            InitializeColorMaze();
            CreateGrid();
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
                            StatusText.Text = TargetCoord.x.ToString() + TargetCoord.y.ToString();
                            
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
                                PlaceThreeBalls();
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
        private void PlaceOneBall()
        {
            if (Processing.CheckRoom(Maze))
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
            if (Processing.CheckRoom(Maze))
            {
                var tempCoord = Processing.RandomPlaceBall(Maze, ColorMaze, color);
                VisualizeBall(tempCoord, color);
                Processing.UpdateColorMazeToColor(ColorMaze, tempCoord.x, tempCoord.y, color);
                Processing.UpdateMazeToOne(Maze, tempCoord.x, tempCoord.y);
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
                StatusText.Text = "BallSelected";
                BallSelectedColor = btn.BackgroundColor;
                ColorText.Text = btn.BackgroundColor.ToHex();
            }
            else if (ButtonStatus == "BallSelected" && btn.ClassId == BallSelectedClassId)
            {
                BoardChangeColor(Int32.Parse(btn.ClassId[0].ToString()), Int32.Parse(btn.ClassId[1].ToString()));
                ButtonStatus = "NothingClicked";
                StatusText.Text = "NothingClicked";
            }
            
        }
        private void RemoveBackgroundColor()
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
                            item.BackgroundColor = Color.AliceBlue;
                        }
                    }
                }
            }
        }
    }
}
