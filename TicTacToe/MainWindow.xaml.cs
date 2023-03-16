using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TicTacToe
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        enum typeField
        {
            empty,
            tic,
            tac
        };

        typeField[,] field = new typeField[3, 3];
        typeField step = typeField.tic;

        bool isGame = true;
        bool isTicPlayer = true;

        void initField()
        {
            for (uint i = 0; i < 3; ++i)
                for (uint j = 0; j < 3; ++j)
                    field[i, j] = typeField.empty;
        }

        int checkEndGame()
        {
            if (field[0, 0] == field[0, 1] && field[0, 1] == field[0, 2] && field[0, 0] == typeField.tic
                || field[1, 0] == field[1, 1] && field[1, 1] == field[1, 2] && field[1, 0] == typeField.tic
                || field[2, 0] == field[2, 1] && field[2, 1] == field[2, 2] && field[2, 0] == typeField.tic
                || field[0, 0] == field[1, 0] && field[1, 0] == field[2, 0] && field[0, 0] == typeField.tic
                || field[0, 1] == field[1, 1] && field[1, 1] == field[2, 1] && field[0, 1] == typeField.tic
                || field[0, 2] == field[1, 2] && field[1, 2] == field[2, 2] && field[0, 2] == typeField.tic
                || field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2] && field[0, 0] == typeField.tic
                || field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0] && field[0, 2] == typeField.tic)
                return isTicPlayer ? -10 : 10;
            else if (field[0, 0] == field[0, 1] && field[0, 1] == field[0, 2] && field[0, 0] == typeField.tac
                || field[1, 0] == field[1, 1] && field[1, 1] == field[1, 2] && field[1, 0] == typeField.tac
                || field[2, 0] == field[2, 1] && field[2, 1] == field[2, 2] && field[2, 0] == typeField.tac
                || field[0, 0] == field[1, 0] && field[1, 0] == field[2, 0] && field[0, 0] == typeField.tac
                || field[0, 1] == field[1, 1] && field[1, 1] == field[2, 1] && field[0, 1] == typeField.tac
                || field[0, 2] == field[1, 2] && field[1, 2] == field[2, 2] && field[0, 2] == typeField.tac
                || field[0, 0] == field[1, 1] && field[1, 1] == field[2, 2] && field[0, 0] == typeField.tac
                || field[0, 2] == field[1, 1] && field[1, 1] == field[2, 0] && field[0, 2] == typeField.tac)
                return isTicPlayer ? 10 : -10;
            bool draw = true;
            for (uint i = 0; i < 3; ++i)
                for (uint j = 0; j < 3; ++j)
                    if (field[i, j] == typeField.empty)
                        draw = false;
            return draw ? 0 : 1;
        }

        void outputField()
        {
            for (uint i = 0; i < 3; ++i)
                for (uint j = 0; j < 3; ++j)
                {
                    var cell = (Label)FindName("labelCell" + i.ToString() + j.ToString());
                    switch (field[i, j])
                    {
                        case typeField.empty:
                            cell.Content = "";
                            break;
                        case typeField.tic:
                            cell.Content = "X";
                            break;
                        case typeField.tac:
                            cell.Content = "O";
                            break;
                    }
                }
        }

        int minimax(typeField _step)
        {
            int res = checkEndGame();
            if (res != 1) return res;

            if (_step == typeField.tac)
            {
                int max_step = -100;
                for (uint i = 0; i < 3; ++i)
                    for (uint j = 0; j < 3; ++j)
                        if (field[i, j] == typeField.empty)
                        {
                            field[i, j] = typeField.tac;
                            int mmax = minimax(typeField.tic);
                            if (mmax > max_step)
                                max_step = mmax;
                            field[i, j] = typeField.empty;
                        }
                return max_step;
            }
            if (_step == typeField.tic)
            {
                int min_step = 100;
                for (uint i = 0; i < 3; ++i)
                    for (uint j = 0; j < 3; ++j)
                        if (field[i, j] == typeField.empty)
                        {
                            field[i, j] = typeField.tic;
                            int mmax = minimax(typeField.tac);
                            if (mmax < min_step)
                                min_step = mmax;
                            field[i, j] = typeField.empty;
                        }
                return min_step;
            }
            return 0; // ни на что не влияющий return, пишем его для того, чтобы компилятор точно знал, 
            // что мы вернем что-то из этой функции
        }

        void stepComputer()
        {
            if (isGame) return;
            int max_step = -100;
            uint max_i = 0, max_j = 0;
            for (uint i = 0; i < 3; ++i)
                for (uint j = 0; j < 3; ++j)
                    if (field[i, j] == typeField.empty)
                    {
                        field[i, j] = isTicPlayer ? typeField.tac : typeField.tic;
                        int mmax = minimax(isTicPlayer ? typeField.tic : typeField.tac);
                        if (mmax > max_step)
                        {
                            max_step = mmax;
                            max_i = i;
                            max_j = j;
                        }
                        field[i, j] = typeField.empty;
                    }
            field[max_i, max_j] = isTicPlayer ? typeField.tac : typeField.tic;
            step = step == typeField.tic ? typeField.tac : typeField.tic;
            isGame = true;
        }

        void endGame()
        {
            isGame = false;
            if (checkEndGame() == 10)
                labelResult.Content = "Вы проиграли";
            else if (checkEndGame() == -10)
                labelResult.Content = "Вы победили";
            else
                labelResult.Content = "Ничья";
        }

        public MainWindow()
        {
            InitializeComponent();

            initField();
            outputField();

            labelInfo.Content = "X";
        }

        private void buttonRestart_Click(object sender, RoutedEventArgs e)
        {
            isTicPlayer = !isTicPlayer;
            initField();

            if (isTicPlayer)
                labelInfo.Content = "X";
            else
            {
                labelInfo.Content = "O";
                stepComputer();
            }
            isGame = true;
            labelResult.Content = "";
            outputField();
        }

        private void buttonStep00_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[0, 0] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }    

        }

        private void buttonStep01_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[0, 1] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep02_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[0, 2] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep10_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[1, 0] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep11_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[1, 1] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep12_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[1, 2] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep20_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[2, 0] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep21_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[2, 1] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }

        private void buttonStep22_Click(object sender, RoutedEventArgs e)
        {
            if (isGame)
            {
                field[2, 2] = isTicPlayer ? typeField.tic : typeField.tac;
                outputField();
                isGame = false;
                stepComputer();
                outputField();
                if (checkEndGame() != 1) endGame();
            }
        }
    }
}
