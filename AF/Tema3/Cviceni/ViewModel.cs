using System;
using CommunityToolkit.Mvvm.ComponentModel;

namespace Avalonia.NETCoreApp1;

public partial class ViewModel : ObservableObject
{
    [ObservableProperty] private double _loanAmount = 7200000.0;
    [ObservableProperty] private double _propertyValue = 8000000.0;
    [ObservableProperty] private double _interestRate = 6.0;
    [ObservableProperty] private int _loanTerm = 30;
    [ObservableProperty] private double _monthlyNetIncome = 45000;
    [ObservableProperty] private double _otherMonthlyPayments = 0;
    [ObservableProperty] private bool _entitledToBenefits = false;
    
    [ObservableProperty] private double _monthlyPayment;
    
    [ObservableProperty] private double _DTILimit;
    [ObservableProperty] private double _DSTILimit;
    [ObservableProperty] private double _LTVLimit;

    [ObservableProperty] private double _DTI;
    [ObservableProperty] private double _DSTI;
    [ObservableProperty] private double _LTV;

    partial void OnLoanAmountChanged(double value) => Calculate();
    partial void OnPropertyValueChanged(double value) => Calculate();
    partial void OnInterestRateChanged(double value) => Calculate();
    partial void OnLoanTermChanged(int value) => Calculate();
    partial void OnMonthlyNetIncomeChanged(double value) => Calculate();
    partial void OnOtherMonthlyPaymentsChanged(double value) => Calculate();
    partial void OnEntitledToBenefitsChanged(bool value) => Calculate();

    public ViewModel()
    {
        Calculate();
    }
    private void Calculate()
    {
        int frequency = 12;
        int n = LoanTerm * frequency;
        double i = InterestRate / (100 * frequency);
        double v = 1 / (1 + i);
        MonthlyPayment = (i * LoanAmount) / (1 - Math.Pow(v, n));

        DTILimit = EntitledToBenefits ? 9.5 : 8.5;
        DSTILimit = EntitledToBenefits ? 50.0 : 40.0;
        LTVLimit = EntitledToBenefits ? 90.0 : 80.0;
        
        DTI = LoanAmount / (MonthlyNetIncome * frequency);
        DSTI = 100 * (MonthlyPayment + OtherMonthlyPayments) / MonthlyNetIncome;
        LTV = 100 * LoanAmount / PropertyValue;
    }
}