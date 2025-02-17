namespace FinancialAssistent.Entities
{
    public class Widgets
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public int IconID { get; set; }
        public Icons Icon { get; set; }

        public decimal Budget {  get; set; }

        public int UserInfoId { get; set; }
        public UserInfo UserInfo { get; set; }

        public decimal Expenses { get; set; }
    }
}
