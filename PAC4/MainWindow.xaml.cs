using System;
using System.Collections.Generic;
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

namespace PAC4
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string currentNumber = "";
        private string currentOperation = "";
        private double result = 0;
        private bool isNewNumber = true;
        private List<string> operationList = new List<string>();
        private List<double> numberList = new List<double>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void OnNumberClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var number = button.Content.ToString();

            if (isNewNumber)
            {
                currentNumber = number;
                isNewNumber = false;
            }
            else
            {
                currentNumber += number;
            }

            UpdateDisplay();
        }

        private void OnOperatorClick(object sender, RoutedEventArgs e)
        {
            var button = (Button)sender;
            var operation = button.Content.ToString();

            if (operation == "=")
            {
                CalculateResult();
                return;
            }

            if (string.IsNullOrEmpty(currentNumber) && numberList.Count == 0)
            {
                DisplayError();
                return;
            }

            if (!string.IsNullOrEmpty(currentNumber))
            {
                numberList.Add(double.Parse(currentNumber));
                currentNumber = "";
            }

            if (operationList.Count > 0 && isNewNumber)
            {
                operationList[operationList.Count - 1] = operation;
            }
            else
            {
                operationList.Add(operation);
            }

            isNewNumber = true;
            UpdateDisplay();
        }

        private void OnEqualsClick(object sender, RoutedEventArgs e)
        {
            CalculateResult();
        }

        private void OnClearClick(object sender, RoutedEventArgs e)
        {
            currentNumber = "";
            currentOperation = "";
            result = 0;
            isNewNumber = true;
            operationList.Clear();
            numberList.Clear();
            DisplayText.Text = "0";
        }

        private void CalculateResult()
        {
            if (!string.IsNullOrEmpty(currentNumber))
            {
                numberList.Add(double.Parse(currentNumber));
                currentNumber = "";
            }

            if (numberList.Count == 0 || numberList.Count <= operationList.Count)
            {
                DisplayError();
                return;
            }

            // Primero realizar multiplicaciones y divisiones
            for (int i = 0; i < operationList.Count; i++)
            {
                if (operationList[i] == "x" || operationList[i] == "/")
                {
                    var result = operationList[i] == "x" 
                        ? numberList[i] * numberList[i + 1]
                        : numberList[i] / numberList[i + 1];
                    
                    numberList[i] = result;
                    numberList.RemoveAt(i + 1);
                    operationList.RemoveAt(i);
                    i--;
                }
            }

            // Luego realizar sumas y restas
            result = numberList[0];
            for (int i = 0; i < operationList.Count; i++)
            {
                if (operationList[i] == "+")
                    result += numberList[i + 1];
                else if (operationList[i] == "-")
                    result -= numberList[i + 1];
            }

            DisplayText.Text = result.ToString();
            numberList.Clear();
            operationList.Clear();
            numberList.Add(result);
            isNewNumber = true;
        }

        private void UpdateDisplay()
        {
            var display = "";
            for (int i = 0; i < numberList.Count; i++)
            {
                display += numberList[i];
                if (i < operationList.Count)
                    display += " " + operationList[i] + " ";
            }
            display += currentNumber;
            DisplayText.Text = display;
        }

        private void DisplayError()
        {
            DisplayText.Text = "Error";
            currentNumber = "";
            operationList.Clear();
            numberList.Clear();
            isNewNumber = true;
        }
    }
}