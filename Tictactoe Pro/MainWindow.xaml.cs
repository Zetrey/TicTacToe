using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Tictactoe_Pro
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool gameover = false;

        private bool isMouseDown = false;
        private bool isMouseEnter = false;

        private int score_you = 0;
        private int score_AI = 0;

        private bool youFirst = true; //定义是否是你的先手
        private bool youRed = false; //定义你的棋子颜色，你是否是红X

        private bool yourTurn = true; //定义当前是否是你的回合
        private int playerTurns = 0; //定义当前回合数

        private bool mode = false; //定义当前模式 0-人机 1-双人
        private bool hard = false; //定义当前难度 0-随机 1-AI觉醒{}

        private Border[,] borders_O = new Border[3, 3];
        private Border[,] borders_X = new Border[3, 3];

        struct array_2_
        {
            public int i; //谜.row
            public int j; //谜.column
            public int state; //记录每个位置的落子信息 1为AI的落子 0为空位 -1为用户的落子
        }
        private array_2_[] _2_ = new array_2_[9]; //谜之操作

        string customColorRed = "#FFAAAA";
        string customColorGreen = "#AAFFAA";
        string customColorBlue = "#AAFFFF";
        string customColorViolet = "#AAAAFF";
        string customColorGold = "#FFFFAA";
        string customColorPink = "#FFAAFF";
        string customColorGray = "#AAAAAA";
        string customColorWhite = "#DDDDDD";
        string customColorRed_Full = "#FF1111";

        /// <summary>
        /// 实现窗口基本功能
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            _O_();
            _X_();
            arrayed_2_();
        }
        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left)
            {
                this.DragMove();
            }
        }

        private void CloseButton_Down(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                border.Background = new SolidColorBrush(Colors.Red);
                isMouseDown = true;
            }
        }
        private void MinimizeButton_Down(object sender, RoutedEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#20FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }

        private void MinimizeButton_Up(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if(border != null)
                {
                    // 定义自定义颜色字符串
                    string customColor = "#00000000";

                    // 将自定义颜色字符串转换为Color对象
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);

                    // 创建SolidColorBrush并设置为Border的背景颜色
                    border.Background = new SolidColorBrush(color);

                    this.WindowState = WindowState.Minimized;
                }
            }
        }

        private void CloseButton_Up(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if( border != null)
                {
                    this.Close();
                }
            }
        }

        /// <summary>
        /// 动画实现
        /// </summary>
        private void StartTextAin(string targetName)
        {
            Storyboard storyboard = (Storyboard)FindResource("TextAni");
            Storyboard clonedStoryboard = storyboard.Clone();

            foreach (var animation in clonedStoryboard.Children)
            {
                Storyboard.SetTargetName(animation, targetName);
            }

            clonedStoryboard.Begin(this);
        }
        //private void StartHighlightAin(string targetName)
        //{
        //    Storyboard storyboard = (Storyboard)FindResource("HighlightAni");
        //    Storyboard clonedStoryboard = storyboard.Clone();

        //    foreach (var animation in clonedStoryboard.Children)
        //    {
        //        Storyboard.SetTargetName(animation, targetName);
        //    }

        //    clonedStoryboard.Begin(this);
        //}

        /// <summary>
        /// 棋盘实现
        /// </summary>

        private void _O_()
        {
            borders_O[0, 0] = O_1;
            borders_O[0, 1] = O_2;
            borders_O[0, 2] = O_3;
            borders_O[1, 0] = O_4;
            borders_O[1, 1] = O_5;
            borders_O[1, 2] = O_6;
            borders_O[2, 0] = O_7;
            borders_O[2, 1] = O_8;
            borders_O[2, 2] = O_9;
        }
        private void _X_()
        {
            borders_X[0, 0] = X_1;
            borders_X[0, 1] = X_2;
            borders_X[0, 2] = X_3;
            borders_X[1, 0] = X_4;
            borders_X[1, 1] = X_5;
            borders_X[1, 2] = X_6;
            borders_X[2, 0] = X_7;
            borders_X[2, 1] = X_8;
            borders_X[2, 2] = X_9;
        }
        private void arrayed_2_() //谜之操作
        {
            for (int i = 0; i < 3; i++) 
            {
                for (int j = 0; j < 3; j++)
                {
                    _2_[3 * i + j].i = i;
                    _2_[3 * i + j].j = j;
                    _2_[3 * i + j].state = 0;
                }
            }
        }
        //显示棋子
        private void Show(int tag, int state, double opacity)
        {
            if (state == -1)
            {
                if (youRed)
                {
                    borders_X[_2_[tag].i, _2_[tag].j].Opacity = opacity;
                }
                else
                {
                    borders_O[_2_[tag].i, _2_[tag].j].Opacity = opacity;
                }
            }
            else
            {
                if (!youRed)
                {
                    borders_X[_2_[tag].i, _2_[tag].j].Opacity = opacity;
                }
                else
                {
                    borders_O[_2_[tag].i, _2_[tag].j].Opacity = opacity;
                }
            }
        }
        private void Add_Show(int tag, int state, double opacity, bool isAdd = false)
        {
            Show(tag, state, opacity);
            if (isAdd) 
            {
                _2_[tag].state = state;
            }
            else
                return;
        }
        //private void Remove(int tag, int state, double opacity, bool isRemove = false)
        //{
        //    Show(tag, state, opacity);
        //    if (isRemove)
        //    {
        //        _2_[tag].state = 0;
        //    }
        //}
        //完全透明隐藏所有棋子
        private void HideAll()
        {
            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    borders_O[i, j].Opacity = 0;
                    borders_X[i, j].Opacity = 0;
                }
            }
            for (int i = 0; i < 9; i++)//把格子置为可放置状态
            {
                _2_[i].state = 0;
                //if(i<7)
                //{
                //    times[i] = -1;
                //}
            }
            playerTurns = 0;

        }
        //绘制获胜一方棋子连线
        private void DrawWinningLine(int row1, int col1, int row2, int col2, int who)
        {
            if (who == -1)
            {
                Line line = new Line
                {
                    Stroke = (Brush)new BrushConverter().ConvertFromString(customColorGold),
                    StrokeThickness = 15,
                    X1 = col1 * (400 / 3) + (400 / 6),
                    Y1 = row1 * (400 / 3) + (400 / 6),
                    X2 = col2 * (400 / 3) + (400 / 6),
                    Y2 = row2 * (400 / 3) + (400 / 6)
                };
                WinningLineCanvas.Children.Add(line);
            }
            else
            {
                Line line = new Line
                {
                    Stroke = (Brush)new BrushConverter().ConvertFromString(customColorRed_Full),
                    StrokeThickness = 10,
                    X1 = col1 * (400 / 3) + (400 / 6),
                    Y1 = row1 * (400 / 3) + (400 / 6),
                    X2 = col2 * (400 / 3) + (400 / 6),
                    Y2 = row2 * (400 / 3) + (400 / 6)
                };
                WinningLineCanvas.Children.Add(line);
            }
        }
        //清除获胜者线条
        private void ClearWinningLine()
        {
            WinningLineCanvas.Children.Clear();
        }

        /// <summary>
        /// 功能实现
        /// </summary>
        //交换棋子
        private void ChangePiecesVis()
        {
            youRed = !youRed;

            Color colorGreen = (Color)ColorConverter.ConvertFromString(customColorGreen);
            Color colorRed = (Color)ColorConverter.ConvertFromString(customColorRed);

            if (youRed)
            {
                Score_AI.Foreground = new SolidColorBrush(colorGreen);
                Score_you.Foreground = new SolidColorBrush(colorRed);
                if (youFirst)
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorRed);
                }
                else
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorGreen);
                }
                for (int i = 0; i < 9; i++)
                {
                    if (_2_[i].state == -1)
                    {
                        borders_O[_2_[i].i, _2_[i].j].Opacity = 0;
                        borders_X[_2_[i].i, _2_[i].j].Opacity = 1;
                    }
                    if (_2_[i].state == 1)
                    {
                        borders_O[_2_[i].i, _2_[i].j].Opacity = 1;
                        borders_X[_2_[i].i, _2_[i].j].Opacity = 0;
                    }
                }
            }
            else
            {
                Score_AI.Foreground = new SolidColorBrush(colorRed);
                Score_you.Foreground = new SolidColorBrush(colorGreen);
                if (youFirst)
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorGreen);
                }
                else
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorRed);
                }
                for (int i = 0; i < 9; i++)
                {
                    if (_2_[i].state == -1)
                    {
                        borders_O[_2_[i].i, _2_[i].j].Opacity = 1;
                        borders_X[_2_[i].i, _2_[i].j].Opacity = 0;
                    }
                    if (_2_[i].state == 1)
                    {
                        borders_O[_2_[i].i, _2_[i].j].Opacity = 0;
                        borders_X[_2_[i].i, _2_[i].j].Opacity = 1;
                    }
                }
            }
        }

        //交换先手
        private void ChangeFirstVis()
        {
            youFirst = !youFirst;

            Color colorGreen = (Color)ColorConverter.ConvertFromString(customColorGreen);
            Color colorRed = (Color)ColorConverter.ConvertFromString(customColorRed);

            if (youFirst)
            {
                WhoFirst.Text = "你";
                if (youRed)
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorRed);
                }
                else
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorGreen);
                }
            }
            else
            {
                WhoFirst.Text = "AI";
                if (youRed)
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorGreen);
                }
                else
                {
                    WhoFirst.Foreground = new SolidColorBrush(colorRed);
                }
            }
        }
        //更改模式
        private void ChangeMode()
        {
            mode = !mode;

            if (mode)
            {
                ModeTextBlock.Text = "模式：调试";
            }
            else
            {
                ModeTextBlock.Text = "模式：正常";
            }
        }
        //更改难度
        private void ChangeDifficulty()
        {
            hard = !hard;

            Color colorGreen = (Color)ColorConverter.ConvertFromString(customColorGreen);
            Color colorRed = (Color)ColorConverter.ConvertFromString(customColorRed);

            if (hard)
            {
                DifficultyTextBlock.Text = "困难";
                DifficultyTextBlock.Foreground = new SolidColorBrush(colorRed);
            }
            else
            {
                DifficultyTextBlock.Text = "简单";
                DifficultyTextBlock.Foreground = new SolidColorBrush(colorGreen);
            }
        }
        //检查获胜者
        private bool CheckForWinner(out int who)
        {
            for (int i = 0; i < 9; i += 3)
            {
                if (_2_[0 + i].state == -1 && _2_[1 + i].state == -1 && _2_[2 + i].state == -1)
                {
                    who = -1;
                    DrawWinningLine(i / 3, 0, i / 3, 2, who);
                    return true;
                }
                if (_2_[0 + i].state == 1 && _2_[1 + i].state == 1 && _2_[2 + i].state == 1)
                {
                    who = 1;
                    DrawWinningLine(i / 3, 0, i / 3, 2, who);
                    return true;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (_2_[0 + i].state == -1 && _2_[3 + i].state == -1 && _2_[6 + i].state == -1)
                {
                    who = -1;
                    DrawWinningLine(0, i, 2, i, who);
                    return true;
                }
                if (_2_[0 + i].state == 1 && _2_[3 + i].state == 1 && _2_[6 + i].state == 1)
                {
                    who = 1;
                    DrawWinningLine(0, i, 2, i, who);
                    return true;
                }
            }
            if (_2_[0].state == -1 && _2_[4].state == -1 && _2_[8].state == -1)
            {
                who = -1;
                DrawWinningLine(0, 0, 2, 2, who);
                return true;
            }
            if (_2_[2].state == -1 && _2_[4].state == -1 && _2_[6].state == -1)
            {
                who = -1;
                DrawWinningLine(0, 2, 2, 0, who);
                return true;
            }
            if (_2_[0].state == 1 && _2_[4].state == 1 && _2_[8].state == 1)
            {
                who = 1;
                DrawWinningLine(0, 0, 2, 2, who);
                return true;
            }
            if (_2_[2].state == 1 && _2_[4].state == 1 && _2_[6].state == 1)
            {
                who = 1;
                DrawWinningLine(0, 2, 2, 0, who);
                return true;
            }
            who = 0;
            return false;
        }
        private void CheckFirstMove()
        {
            if (!mode)
            {
                if (youFirst)
                {
                    yourTurn = true;
                }
                else
                {
                    if (hard) AIMove();
                    else ComputerMove();
                }
            }
            else
            {
                if (youFirst)
                {
                    yourTurn = true;
                }
                else
                {
                    yourTurn = false;
                }
            }
        }

        /// <summary>
        /// 事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //棋盘鼠标移入移出事件
        private void _1_MouseEnter(object sender, MouseEventArgs e)
        {
            Border border = sender as Border;
            if (border != null) 
            {
                int tag = int.Parse(border.Tag.ToString());
                isMouseEnter = true;
                if (_2_[tag].state == 0)  //检查格子状态，如果被标记为0以外的值就不会再次落子
                {
                    if (!mode && yourTurn)
                    {
                        Add_Show(tag, -1, 0.5);
                    }
                    else if(mode && !gameover)
                    {
                        if(yourTurn)
                        {
                            Add_Show(tag, -1, 0.5);
                        }
                        else
                        {
                            Add_Show(tag, 1, 0.5);
                        }
                    }
                }
            }
        }
        private void _1_MouseLeave(object sender, MouseEventArgs e)
        {
            if (isMouseEnter)
            {
                isMouseEnter = false;
                Border border = sender as Border;
                if (border != null)
                {
                    int tag = int.Parse(border.Tag.ToString());
                    if (_2_[tag].state == 0)  //检查格子状态，如果被标记为0以外的值就不会再次落子
                    {
                        if (!mode && yourTurn)
                        {
                            Add_Show(tag, -1, 0);
                        }
                        else if(mode && !gameover)
                        {
                            if (yourTurn)
                            {
                                Add_Show(tag, -1, 0);
                            }
                            else
                            {
                                Add_Show(tag, 1, 0);
                            }
                        }
                    }
                }
            }
        }
        //棋盘鼠标点击事件
        private void _1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                isMouseDown = true;

            }
        }
        private void _1_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    int tag = int.Parse(border.Tag.ToString());

                    if (_2_[tag].state == 0)  //检查格子状态，如果被标记为0以外的值就不会再次落子
                    {
                        if (!mode)
                        {
                            if (yourTurn)
                            {
                                yourTurn = false;
                                playerTurns++;

                                Add_Show(tag, -1, 1.0, true);

                                if (CheckForWinner(out int who))
                                {
                                    if (who == -1)
                                    {
                                        YourPerformance.Text = "胜！";
                                        Color color = (Color)ColorConverter.ConvertFromString(customColorGold);
                                        YourPerformance.Foreground = new SolidColorBrush(color);

                                        score_you++;
                                        Score_you.Text = score_you.ToString();
                                    }
                                    else
                                    {
                                        YourPerformance.Text = "负！";
                                        Color color = (Color)ColorConverter.ConvertFromString(customColorPink);
                                        YourPerformance.Foreground = new SolidColorBrush(color);

                                        score_AI++;
                                        Score_AI.Text = score_AI.ToString();
                                    }
                                    StartTextAin("YourPerformance");
                                }
                                else if (playerTurns == 9)
                                {
                                    YourPerformance.Text = "平";
                                    Color color = (Color)ColorConverter.ConvertFromString(customColorWhite);
                                    YourPerformance.Foreground = new SolidColorBrush(color);

                                    StartTextAin("YourPerformance");
                                }
                                else if (!yourTurn)
                                {
                                    if (hard) AIMove();
                                    else ComputerMove();
                                }
                            }
                        }
                        else if(!gameover)
                        {
                            if (yourTurn)
                            {
                                yourTurn = false;
                                playerTurns++;

                                Add_Show(tag, -1, 1.0, true);
                            }
                            else
                            {
                                yourTurn = true;
                                playerTurns++;

                                Add_Show(tag, 1, 1.0, true);
                            }
                            if (CheckForWinner(out int who))
                            {
                                gameover = true;

                                if (who == -1)
                                {
                                    YourPerformance.Text = "胜！";
                                    Color color = (Color)ColorConverter.ConvertFromString(customColorGold);
                                    YourPerformance.Foreground = new SolidColorBrush(color);

                                    score_you++;
                                    Score_you.Text = score_you.ToString();
                                }
                                else
                                {
                                    YourPerformance.Text = "负！";
                                    Color color = (Color)ColorConverter.ConvertFromString(customColorPink);
                                    YourPerformance.Foreground = new SolidColorBrush(color);

                                    score_AI++;
                                    Score_AI.Text = score_AI.ToString();
                                }
                                StartTextAin("YourPerformance");
                            }
                            else if (playerTurns == 9)
                            {
                                gameover = true ;

                                YourPerformance.Text = "平";
                                Color color = (Color)ColorConverter.ConvertFromString(customColorWhite);
                                YourPerformance.Foreground = new SolidColorBrush(color);

                                StartTextAin("YourPerformance");
                            }
                        }
                    }
                }
            }
        }
        //交换先手按钮事件
        private void ChangeFirst_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void ChangeFirst_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    ChangeFirstVis();
                    gameover = false ;
                    HideAll(); //清空棋子
                    ClearWinningLine(); //清除划线
                    CheckFirstMove();
                    StartTextAin("WhoFirst");
                    StartTextAin("ChgFirstTextBlock");

                }
            }
        }
        //交换棋子按钮事件
        private void ChangePieces_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void ChangePieces_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    ChangePiecesVis();

                    StartTextAin("Score_AI");
                    StartTextAin("Score_you");
                    StartTextAin("WhoFirst");
                    StartTextAin("ChgPiecesTextBlock");
                }
            }
        }
        //清空分数按钮事件
        private void Clear_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void Clear_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    score_you = 0;
                    score_AI = 0;
                    Score_AI.Text = "0";
                    Score_you.Text = "0";

                    StartTextAin("Score_AI");
                    StartTextAin("Score_you");
                    StartTextAin("ClearTextBlock");
                }
            }
        }
        //开始按钮事件
        private void Start_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void Start_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    HideAll(); //清空棋子
                    ClearWinningLine(); //清除划线
                    gameover = false;
                    CheckFirstMove();
                    StartTextAin("StartTextBlock");
                }
            }
        }
        //难度按钮事件
        private void Difficulty_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void Difficulty_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    ChangeDifficulty();
                    StartTextAin("DifficultyTextBlock");
                }
            }
        }
        //模式按钮事件
        private void Mode_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Border border = sender as Border;
            if (border != null)
            {
                string customColor = "#30FFFFFF";
                Color color = (Color)ColorConverter.ConvertFromString(customColor);
                border.Background = new SolidColorBrush(color);
                isMouseDown = true;
            }
        }
        private void Mode_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (isMouseDown)
            {
                isMouseDown = false;
                Border border = sender as Border;
                if (border != null)
                {
                    string customColor = "#20FFFFFF";
                    Color color = (Color)ColorConverter.ConvertFromString(customColor);
                    border.Background = new SolidColorBrush(color);

                    ChangeMode();
                    gameover = false;
                    HideAll(); //清空棋子
                    ClearWinningLine(); //清除划线
                    CheckFirstMove();
                    StartTextAin("ModeTextBlock");
                }
            }
        }

        /// <summary>
        /// AI实现
        /// </summary>
        /// <returns></returns>
        private int EvaluateBoard()
        {
            for (int i = 0; i < 9; i += 3)
            {
                if (_2_[0 + i].state == -1 && _2_[1 + i].state == -1 && _2_[2 + i].state == -1)
                {
                    return -10;
                }
                if (_2_[0 + i].state == 1 && _2_[1 + i].state == 1 && _2_[2 + i].state == 1)
                {
                    return 10;
                }
            }
            for (int i = 0; i < 3; i++)
            {
                if (_2_[0 + i].state == -1 && _2_[3 + i].state == -1 && _2_[6 + i].state == -1)
                {
                    return -10;
                }
                if (_2_[0 + i].state == 1 && _2_[3 + i].state == 1 && _2_[6 + i].state == 1)
                {
                    return 10;
                }
            }
            if (_2_[0].state == -1 && _2_[4].state == -1 && _2_[8].state == -1)
            {
                return -10;
            }
            if (_2_[2].state == -1 && _2_[4].state == -1 && _2_[6].state == -1)
            {
                return -10;
            }
            if (_2_[0].state == 1 && _2_[4].state == 1 && _2_[8].state == 1)
            {
                return 10;
            }
            if (_2_[2].state == 1 && _2_[4].state == 1 && _2_[6].state == 1)
            {
                return 10;
            }

            // 如果没有人赢
            return 0;
        }
        private int Minimax(int depth, bool isMax)
        {
            int score = EvaluateBoard();

            if (score == 10)
                return score;

            if (score == -10)
                return score;

            if (playerTurns == 9)
                return 0;

            if (isMax)
            {
                int best = int.MinValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (_2_[3 * i + j].state == 0)
                        {
                            _2_[3 * i + j].state = 1;
                            playerTurns++;

                            best = Math.Max(best, Minimax(depth + 1, !isMax));

                            _2_[3 * i + j].state = 0;
                            playerTurns--;
                        }
                    }
                }
                return best;
            }
            else
            {
                int best = int.MaxValue;

                for (int i = 0; i < 3; i++)
                {
                    for (int j = 0; j < 3; j++)
                    {
                        if (_2_[3 * i + j].state == 0)
                        {
                            _2_[3 * i + j].state = -1;
                            playerTurns++;

                            best = Math.Min(best, Minimax(depth + 1, !isMax));

                            _2_[3 * i + j].state = 0;
                            playerTurns--;
                        }
                    }
                }
                return best;
            }
        }
        private void AIMove()
        {

            int bestVal = int.MinValue;
            int bestMoveRow = -1;
            int bestMoveCol = -1;

            for (int i = 0; i < 3; i++)
            {
                for (int j = 0; j < 3; j++)
                {
                    if (_2_[3 * i + j].state == 0) 
                    {
                        _2_[3 * i + j].state = 1;
                        playerTurns++;

                        int moveVal = Minimax(0, false);

                        _2_[3 * i + j].state = 0;
                        playerTurns--;

                        if (moveVal > bestVal)
                        {
                            bestMoveRow = i;
                            bestMoveCol = j;
                            bestVal = moveVal;
                        }
                    }
                }
            }

            _2_[bestMoveRow * 3 + bestMoveCol].state = 1;

            Add_Show((bestMoveRow * 3 + bestMoveCol), 1, 1.0, true);
            
            if (CheckForWinner(out int who))
            {
                if (who == -1)
                {
                    YourPerformance.Text = "胜！";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorGold);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    score_you++;
                    Score_you.Text = score_you.ToString();
                }
                else
                {
                    YourPerformance.Text = "负！";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorPink);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    score_AI++;
                    Score_AI.Text = score_AI.ToString();
                }
                StartTextAin("YourPerformance");
            }
            else if (!mode)
            {
                playerTurns++;

                if (playerTurns == 9)
                {
                    YourPerformance.Text = "平";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorWhite);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    StartTextAin("YourPerformance");
                }
                else
                {
                    yourTurn = true;
                }
            }
            else
            {
                yourTurn = true;
            }
        }
        //随机（智障模式）
        private void ComputerMove()
        {
            // Simple AI for computer move
            Random rand = new Random();
            int i, j;
            do
            {
                i = rand.Next(3);
                j = rand.Next(3);
            }
            while (_2_[i * 3 + j].state != 0);

            _2_[i * 3 + j].state = 1;

            Add_Show((i* 3 + j), 1, 1.0, true);

            if (CheckForWinner(out int who))
            {
                if (who == -1)
                {
                    YourPerformance.Text = "胜！";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorGold);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    score_you++;
                    Score_you.Text = score_you.ToString();
                }
                else
                {
                    YourPerformance.Text = "负！";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorPink);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    score_AI++;
                    Score_AI.Text = score_AI.ToString();
                }
                StartTextAin("YourPerformance");
            }
            else if (!mode)
            {
                playerTurns++;
                if (playerTurns == 9)
                {
                    YourPerformance.Text = "平";
                    Color color = (Color)ColorConverter.ConvertFromString(customColorWhite);
                    YourPerformance.Foreground = new SolidColorBrush(color);

                    StartTextAin("YourPerformance");
                }
                else
                {
                    yourTurn = true;
                }
            }
            else
            {
                yourTurn = true;
            }
        }
    }
}
