//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//namespace Novo.Helpers
//{
//    public class Permissao : ValidationAttribute
//    {
//        protected override ValidationResult? IsValid(object value, ValidationContext validationContext)
//        {
//            if (HttpContext.User.Claims.Contains(value))
//            {
//                return new ValidationResult("Permissão insuficiente");
//            }

//            return IsValid(value, validationContext);
//        }
        
//    }
//}
