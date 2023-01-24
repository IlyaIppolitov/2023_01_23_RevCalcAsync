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

        decimal total = 0;
        decimal totalIncome = 0;
        decimal totalOutcome = 0;


        string incomeFile = @"D:\FilesToRead\income.txt";
        string outcomeFile = @"D:\FilesToRead\outcome.txt";

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            var total = await CalcTotalAsync(incomeFile, outcomeFile);
            Dispatcher.Invoke(() => textBoxTotal.Text = $"Прибыль: {(total.Item1 - total.Item2).ToString()}");
        }

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

        decimal CalcTotal(string fileName)
        {
            decimal temp = 0;
            string[] Lines = System.IO.File.ReadAllLines(fileName); // Read all lines сделать асинхронным
            foreach (string line in Lines)
            {
                temp += decimal.Parse(line);
            }
            return temp;
        }

        private async void buttonReadAllLynesAsync_Click(object sender, RoutedEventArgs e)
        {
            totalIncome = await CalcTotalAsync(incomeFile);
            totalOutcome = await CalcTotalAsync(outcomeFile);
            Dispatcher.Invoke(() => textBoxTotal.Text = $"Прибыль: {(totalIncome - totalOutcome).ToString()}");
        }

        async Task<decimal> CalcTotalAsync(string fileName)
        {
            decimal tempTotal = 0;
            await Task.Run(async () =>
            {
                string[] Lines = await ReadAllLinesAsync(fileName);
                foreach (string line in Lines)
                {
                    tempTotal += decimal.Parse(line);
                }
            });
            return tempTotal;
        }

        public static Task<string[]> ReadAllLinesAsync(string path)
        {
            return ReadAllLinesAsync(path);
        }
    }
}
