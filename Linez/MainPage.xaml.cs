using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
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
            Colors.Add("Red");
            Colors.Add("Yellow");
            Colors.Add("Green");
            Colors.Add("Blue");
            Colors.Add("LightBlue");
            Colors.Add("Brown");
            Colors.Add("Purple");
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
                                ButtonStatus = "NothingClicked";
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
                    item.BackgroundColor = Color.White;
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
                VisualizeBall(Processing.RandomPlaceBall(Maze, ColorMaze, color), color);
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
                    case "Red":
                        button.BackgroundColor = Color.Red;
                        break;
                    case "Blue":
                        button.BackgroundColor = Color.Blue;
                        break;
                    case "LightBlue":
                        button.BackgroundColor = Color.LightBlue;
                        break;
                    case "Yellow":
                        button.BackgroundColor = Color.Yellow;
                        break;
                    case "Green":
                        button.BackgroundColor = Color.Green;
                        break;
                    case "Brown":
                        button.BackgroundColor = Color.Brown;
                        break;
                    case "Purple":
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
            }
            else if (ButtonStatus == "BallSelected" && btn.ClassId == BallSelectedClassId)
            {
                BoardChangeColor(Int32.Parse(btn.ClassId[0].ToString()), Int32.Parse(btn.ClassId[1].ToString()));
                ButtonStatus = "NothingClicked";
                StatusText.Text = "NothingClicked";
            }
            
        }
    }
}
