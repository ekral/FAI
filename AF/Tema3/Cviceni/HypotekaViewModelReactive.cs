using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.HypotecniKalkulackaReaktivni;

public partial class HypotekaViewModelReactive : ObservableObject
{
    [ObservableProperty]
    private double _loanAmount = 7000000.0;
    
    [ObservableProperty]
    private double _propertyValue = 80000000.0;
    
    [ObservableProperty]
    private double _interestRate = 6.0;
    
    [ObservableProperty]
    private int _loanTerm = 30;

    [ObservableProperty]
    private double _monthlyNetIncome = 45000.0;

    [ObservableProperty]
    private double _otherMonthlyPayments = 0.0;
    
    [ObservableProperty]
    private bool _discountEligibility = true;
    
    [ObservableProperty]
    private double _monthlyPayment;

    [ObservableProperty]
    private double _DSTI;

    [ObservableProperty]
    private double _LTV;
    
    [ObservableProperty]
    private double _DTI;
    
    partial void OnLoanAmountChanged(double value) => Calculate();
    partial void OnInterestRateChanged(double value) => Calculate(); 
    partial void OnLoanTermChanged(int value) => Calculate();
    partial void OnMonthlyNetIncomeChanged(double value) => Calculate();


    public HypotekaViewModelReactive()
    {
        Calculate();
    }
    
    private void Calculate()
    {
        const int frequency = 12;
        int n = LoanTerm * frequency;
        double i = InterestRate / 100.0 / 12;
        double v = 1 / (1 + i);
        MonthlyPayment = (i * LoanAmount) / (1 - Math.Pow(v,n));

        DTI = LoanAmount / (MonthlyNetIncome * frequency);
        DSTI = 100 * (OtherMonthlyPayments + MonthlyPayment) / MonthlyNetIncome;
        LTV = 100 * LoanAmount / PropertyValue;
    }
}