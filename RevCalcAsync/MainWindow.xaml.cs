using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
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

namespace RevCalcAsync
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        /** Переменные для подсчёта расходов/доходов */
        decimal totalIncome = 0;
        decimal totalOutcome = 0;

        /** Полные имена файлов расходов/доходов */
        string incomeFile = @"D:\FilesToRead\income.txt";
        string outcomeFile = @"D:\FilesToRead\outcome.txt";

        /** Кнопка для выполнения расчёта прибыли в асинхронном режиме
         *  с параллельным считыванием файлов расходов/доходов */
        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            // Пуск таймера для проверки времени работы программы
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Получение результатов расчёта
            var total = await CalcTotalAsync(incomeFile, outcomeFile);

            // Вывод результата на экран
            textBoxTotal.Text = $"Прибыль: {(total.Item1 - total.Item2).ToString()}";

            // Останов таймера
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            textBoxTime.Text = $"Время вычисления: {ts.Milliseconds.ToString()} мс.";
        }

        /** Асинхронный метод
         *  с параллельным считыванием файлов расходов/доходов */
        async Task<(decimal,decimal)> CalcTotalAsync(string incomeFile, string outcomeFile)
        {
            decimal tempTotalIncome = 0;
            decimal tempTotalOutncome = 0;
            await Task.Run(() =>
            {
                Parallel.Invoke(
                () => tempTotalIncome = CalcTotal(incomeFile),
                () => tempTotalOutncome = CalcTotal(outcomeFile));

            });
            return (tempTotalIncome, tempTotalOutncome);
        }

        /** Метод возвращающий сумму чисел в строках в файле */
        decimal CalcTotal(string fileName)
        {
            decimal temp = 0;
            string[] Lines = File.ReadAllLines(fileName);
            foreach (string line in Lines)
            {
                temp += decimal.Parse(line);
            }
            return temp;
        }

        /** Кнопка для выполнения расчёта прибыли в асинхронном режиме
         *  с последовательным считыванием файлов расходов/доходов в асинхронном режиме */
        private async void buttonReadAllLynesAsync_Click(object sender, RoutedEventArgs e)
        {
            // Пуск таймера для проверки времени работы программы
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            // Последовательное получение результатов расчёта
            totalIncome = await CalcTotalAsync(incomeFile);
            totalOutcome = await CalcTotalAsync(outcomeFile);
            textBoxTotal.Text = $"Прибыль: {(totalIncome - totalOutcome).ToString()}";

            // Останов таймера
            stopWatch.Stop();
            TimeSpan ts = stopWatch.Elapsed;
            textBoxTime.Text = $"Время вычисления: {ts.Milliseconds.ToString()} мс.";
        }

        /** Асинхронный метод возвращающий сумму чисел в строках в файле с асинхронным считыванием файла */
        async Task<decimal> CalcTotalAsync(string fileName)
        {
            decimal tempTotal = 0;
            await Task.Run(async () =>
            {
                string[] Lines = await File.ReadAllLinesAsync(fileName);
                foreach (string line in Lines)
                {
                    tempTotal += decimal.Parse(line);
                }
            });
            return tempTotal;
        }
    }
}
