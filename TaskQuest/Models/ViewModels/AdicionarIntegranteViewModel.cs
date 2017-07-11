using System.ComponentModel.DataAnnotations;

namespace TaskQuest.ViewModels
{
    public class AdicionarIntegranteViewModel
    {
    	[Required]
    	[RegularExpression("^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$")]
        public string Email { get; set; }

        [Required]
        public string GrupoId { get; set; }
    }
}