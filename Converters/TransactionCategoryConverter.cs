namespace FinancialAssistent.Converters
{
    public static class TransactionCategoryConverter
    {
        public static string GetExpenseCategory(int mcc)
        {
            if (mcc is 4900 or 1520)
                return "Mandatory Expenses";

            if (mcc is 5812 or 5912 or 5814 or 5411 or 5462 or 5441 or 5499 or 5311 or 5200 or 5300 or 5310 or 5399) // Їжа та супермаркети
                return "Food";

            if (mcc is 5541)
                return "Transportation";

            if (mcc is 8099 or 5122 or 5921 or 5697 or 7941)
                return "Sport and Health";

            if (mcc is 7832 or 5942 or 5977 or 4812) 
                return "Entertainment";

            return "Others";
        }

    }
}
