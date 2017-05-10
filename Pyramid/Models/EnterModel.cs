using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Pyramid.Tools;

namespace Pyramid.Models
{
    public class EnterModel
    {
        [Display(Name = "Электронный адрес")]
        [RegularExpression(@"^[\w-]+(\.[\w-]+)*@([a-z0-9-]+(\.[a-z0-9-]+)*?\.[a-z]{2,6}|(\d{1,3}\.){3}\d{1,3})(:\d{4})?$", ErrorMessage = "Введите корректный E-Mail")]
        [StringLength(1000)]
        [Required]
        public string Email { get; set; }

        [Display(Name = "Логин")]
        [StringLength(1000)]
        [Required]
        public string Login { get; set; }

        [Display(Name = "Пароль")]
        [Required]
        public string Password { get; set; }

        public static bool VerifyPassword(EnterModel model, out string message)
        {
            bool res = false;
            if (model.Login == "admin")
            {

                var set = DBFirstDAL.UserDAL.DALToEntity(DBFirstDAL.UserDAL.GetByLogin(model.Login.Trim()));
                if (set == null)
                {
                    message = "Указанного пользователя нет в системе.";
                    //var hassh = Cryptography.GetHash(model.Password.Trim());

                    return false;
                }

                var hash = Cryptography.GetHash(model.Password.Trim());
                res = StringComparer.Ordinal.Compare(hash, set.Password) == 0;
            }
            message = res ? "Вы успешно авторизировались в системе." : "Введенные Вами электронный адрес или пароль неверны.";
            return res;

        }
    }

}