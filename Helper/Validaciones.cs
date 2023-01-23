using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace helper
{
    public class Validaciones
    {
        public bool sinEspeciales(string cadena)
        {
            foreach (var caracter in cadena)
            {
                if (!(char.IsLetter(caracter)) && !(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
        public bool soloNumeros(string cadena)
        {
            foreach (var caracter in cadena)
            {
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
    }
}
