﻿using Avalonia;
using Avalonia.Controls;
using Avalonia.Media;
using Avalonia.Layout;
using Avalonia.Data;
using System.ComponentModel;
using Avalonia.Styling;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System;

namespace AvaloniaAplikace
{
    public class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? name = null )
        {
            ArgumentException.ThrowIfNullOrEmpty(name);

            if (EqualityComparer<T>.Default.Equals(field, newValue)) return false;

            field = newValue;
            OnPropertyChanged(name);

            return true;
        }
    }

    public class ViewModelObjednavka : ViewModelBase
    {

        public required string NazevProduktu { get; set; }
        public decimal Cena { get; set; }
        public int Pocet { get; set; }

        private decimal cenaCelkem;
        public decimal CenaCelkem 
        {
            get => cenaCelkem;
            set => SetProperty(ref cenaCelkem, value);
        }

        public void SpocitejCenuCelkem()
        {
            CenaCelkem = Pocet * Cena;
        }
    }

    public class MojeOkno : Window
    {
        public MojeOkno()
        {
            //DataContext = new ViewModelObjednavka() { NazevProduktu = "Tesla 3", Cena = 1100000.0m, Pocet = 1 };

            TextBlock textBlockNazevProduktu = new TextBlock();
            textBlockNazevProduktu.Bind(TextBlock.TextProperty, new Binding("NazevProduktu"));

            TextBlock textBlockCena = new TextBlock();
            textBlockCena.Bind(TextBlock.TextProperty, new Binding("Cena"));

            TextBlock textBlockCenaCelkem = new TextBlock()
            {
                Background = Brushes.LightBlue
            };

            textBlockCenaCelkem.Bind(TextBlock.TextProperty, new Binding("CenaCelkem"));

            NumericUpDown numericUpDown = new NumericUpDown()
            {
                HorizontalAlignment =  HorizontalAlignment.Left,
                Minimum = 1,
                Maximum = 10,
                Increment = 1
            };

            numericUpDown.Bind(NumericUpDown.ValueProperty, new Binding("Pocet", BindingMode.TwoWay));

            Button buttonSpocitej = new Button()
            {
                Content = "Cena celkem"
            };

            buttonSpocitej.Bind(Button.CommandProperty, new Binding("SpocitejCenuCelkem"));

            StackPanel panel = new StackPanel();

            panel.Children.Add(textBlockNazevProduktu);
            panel.Children.Add(textBlockCena);
            panel.Children.Add(textBlockCenaCelkem);
            panel.Children.Add(numericUpDown);
            panel.Children.Add(buttonSpocitej);

            Content = panel;
        }
    }
}
