using System.ComponentModel.DataAnnotations;

namespace Sheltos.ViewModel.Card
{
    public class CardListViewModel
    {
        public int Id { get; set; }

        public string CardName { get; set; }
       
        public string CardNumber { get; set; }
       
        public string CVV { get; set; }

        public string CardType { get; set; } // debit, credit
        [DataType(DataType.Date)]
        public DateTime Expiry { get; set; }
        public bool IsDefault { get; set; }

    }
}
