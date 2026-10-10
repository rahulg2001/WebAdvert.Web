using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WebAdvert.Web.Models.Accounts
{
    public class LoginWithMFAModel
    {

        [Required(ErrorMessage = "Multifactor Authentication Code is required")]        
        [DataType(DataType.Text)]
        [Display(Name = "MultifactorAuthenticationCode")]
        public string MFACode { get; set; }
    }
}
