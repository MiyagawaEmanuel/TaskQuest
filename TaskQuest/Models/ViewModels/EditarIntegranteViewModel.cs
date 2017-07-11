using System.ComponentModel.DataAnnotations;

namespace TaskQuest.ViewModels
{
    public class EditarIntegranteViewModel
    {
    	[Required]
        public string UserId { get; set; }

        [Required]
        public string GrupoId { get; set; }
        
    }
}