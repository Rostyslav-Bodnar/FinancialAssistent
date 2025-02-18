namespace FinancialAssistent.Converters
{
    public static class TransactionCategoryConverter
    {
        private static readonly Dictionary<int, string> MccCategories = new()
        {
            { 5812, "Ресторани та громадське харчування" },
            { 5912, "Аптеки" },
            { 5814, "Ресторани-закусочні" },
            { 5411, "Супермаркети та бакалія" },
            { 5462, "Булочні" },
            { 7832, "Кінотеатри" },
            { 7941, "Спортивні заходи" },
            { 5441, "Кондитерські" },
            { 5977, "Магазини косметики" },
            { 5211, "Будівельні матеріали" },
            { 5499, "Продовольчі магазини" },
            { 5942, "Книжкові магазини" },
            { 5541, "Станції техобслуговування" },
            { 5995, "Зоомагазини" },
            { 8099, "Медичні послуги" },
            { 5311, "Універмаги" },
            { 5200, "Товари для дому" },
            { 5399, "Різні товари загального призначення" },
            { 1520, "Будівництво" },
            { 4812, "Телекомунікації" },
            { 5300, "Оптовики" },
            { 5811, "Постачальники провізії" },
            { 5722, "Побутова техніка" },
            { 5697, "Пошиття та ремонт одягу" },
            { 5921, "Продаж алкоголю" },
            { 5122, "Аптеки та ліки" },
            { 5331, "Універсальні магазини" },
            { 5451, "Молочні продукти" },
            { 5310, "Дисконтні магазини" },
            { 4900, "Комунальні послуги" }
        };

        public static string GetCategory(int mccCode)
        {
            return MccCategories.TryGetValue(mccCode, out var category) ? category : "Невідома категорія";
        }

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
