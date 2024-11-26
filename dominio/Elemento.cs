using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Elemento
    {
        //Agrego una nueva Clase Elemento para que
        //se pueda leer de la DB la tabla Elementos

        //Propiedades
        public int Id { get; set; }
        public string Descripcion { get; set; }

        //Sobreescribo el METODO ToString
        //para que pueda mostrar el tipo del pokemon
        //y no, por defecto, de donde proviene la property
        //El metodo ToString  retorna -> "base.ToString()"
        public override string ToString()
        {
            return Descripcion;
        }

    }
}
