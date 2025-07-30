using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Sheltos.ViewModel.Card
{
    public class CardViewModel
    {

        public List<CardListViewModel> Cards { get; set; } = new();
       public CardListViewModel NewCard { get; set; } 
    }
}
