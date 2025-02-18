namespace FinancialAssistent.Transfers
{
    public class BudgetForecastResult
    {
        public IEnumerable<object> Labels { get; }
        public IEnumerable<decimal> Values { get; }

        public BudgetForecastResult(IEnumerable<object> labels, IEnumerable<decimal> values)
        {
            Labels = labels;
            Values = values;
        }
    }

}
