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

        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            
            string incomeFile = @"D:\FilesToRead\income.txt";
            string outcomeFile = @"D:\FilesToRead\outcome.txt";
            //Parallel.Invoke(() => CalcTotalIncome(incomeFile), () => CalcTotalOutcome(outcomeFile));
            totalIncome =  await CalcTotal(incomeFile);
            totalOutcome = await CalcTotal(outcomeFile);
            Dispatcher.Invoke(() => textBoxTotal.Text = $"Прибыль: {(totalIncome - totalOutcome).ToString()}");

            async Task<decimal> CalcTotal(string fileName)
            {
                decimal tempTotal = 0;
                await Task.Run(() =>
                {
                    string[] incomeLines = System.IO.File.ReadAllLines(fileName); // Read all lines сделать асинхронным
                    foreach (string line in incomeLines)
                    {
                        tempTotal += decimal.Parse(line);
                    }
                });
                return tempTotal;
            }

        }
    }
}
