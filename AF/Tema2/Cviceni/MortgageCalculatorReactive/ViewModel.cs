using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.MortgageCalculatorReactive;

public partial class ViewModel : ObservableObject
{
    [ObservableProperty] private double _loanAmount = 8000000.0;
    [ObservableProperty] private double _interestRate = 6.0;
    [ObservableProperty] private int _loanTerm = 30;
    [ObservableProperty] private double _monthlyPayment;

    partial void OnLoanAmountChanged(double value) => Calculate(); // expression body syntax
    partial void OnInterestRateChanged(double value) => Calculate(); 
    partial void OnLoanTermChanged(int value) => Calculate();
 
    public ViewModel()
    {
        Calculate();
    }
    
    private void Calculate()
    {
        int frequency = 12;
        int n = LoanTerm * frequency;
        
        double i = InterestRate / (100.0 * frequency);
        double v = 1 / (1 + i);
        MonthlyPayment = (LoanAmount * i) / (1 - Math.Pow(v, n));
    }
}