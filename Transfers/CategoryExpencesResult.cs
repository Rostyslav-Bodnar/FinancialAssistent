namespace FinancialAssistent.Transfers
{
    public class CategoryExpencesResult
    {
        public IEnumerable<object> Labels { get; }
        public IEnumerable<decimal> Values { get; }

        public CategoryExpencesResult(IEnumerable<object> labels, IEnumerable<decimal> values)
        {
            Labels = labels;
            Values = values;
        }
    }
}
