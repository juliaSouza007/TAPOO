using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using System;
using System.Collections.Generic;

namespace AvaloniaUI
{
    public partial class MainWindow : Window
    {
        private TextBox inputTextBox;
        private TextBox outputTextBox;
        private ListBox conversionListBox;
        private Button convertButton;

        private Dictionary<string, Func<double, double>> conversionFunctions = new()
        {
            //temperatura
            { "Celsius para Fahrenheit", c => (c * 1.8) + 32 },
            { "Fahrenheit para Celsius", f => (f - 32) / 1.8 },
            { "Celsius para Kelvin", c => c + 273.15 },
            { "Kelvin para Celsius", k => k - 273.15 },

            //comprimento
            { "Metros para Pes", m => m * 3.28084 },
            { "Pes para Metros", p => p * 0.3048 },
            { "Quilometros para Milhas", km => km * 0.621371 },
            { "Milhas para Quilometros", mi => mi * 1.60934 },

            //peso/massa
            { "Quilogramas para Libras", kg => kg * 2.20462 },
            { "Libras para Quilogramas", lb => lb * 0.453592 },
            { "Gramas para Oncas", g => g * 0.035274 },
            { "Oncas para Gramas", oz => oz * 28.3495 },

            //volume
            { "Litros para Galoes", l => l * 0.264172 },
            { "Galoes para Litros", gal => gal * 3.78541 },
            { "Mililitros para Oncas Fluidas", ml => ml * 0.033814 },
            { "Oncas Fluidas para Mililitros", ozf => ozf * 29.5735 }
        };

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
            inputTextBox = this.FindControl<TextBox>("InputTextBox");
            outputTextBox = this.FindControl<TextBox>("OutputTextBox");
            conversionListBox = this.FindControl<ListBox>("ConversionListBox");
            convertButton = this.FindControl<Button>("ConvertButton");
            
            conversionListBox.ItemsSource = conversionFunctions.Keys;
            convertButton.Click += ConvertButton_Click;
        }

        private void ConvertButton_Click(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
        {
            if (double.TryParse(inputTextBox.Text, out double inputValue) &&
                conversionListBox.SelectedItem is string selectedConversion &&
                conversionFunctions.TryGetValue(selectedConversion, out var conversion))
            {
                double result = conversion(inputValue);
                outputTextBox.Text = result.ToString("F2");
            }
            else
            {
                outputTextBox.Text = "Erro";
            }
        }
    }
}
