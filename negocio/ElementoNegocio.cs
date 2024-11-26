using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using dominio; //Agrego el proyecto "dominio" para que la clase ElementoNegocio
               //conozca las clases de ese proyecto (es una librería que yo creé).

namespace negocio
{
    public class ElementoNegocio
    {
        //Como en la Clase PokemonNegocio Creo un MÉTODO q lea registros de la DB
        //y devuelve varios registros de los elementos de los Pokemon que se agrupan en una lista.
        //PERO para no tener que estar declarando todo lo que se hizo en el METODO LISTAR
        //de la Clase PokemonNegocio
        //(la cadena de conexion, el comando, la conexion en si, el lector)
        //=> CREO una clase que centralice toda esta conexion a la DB para no tener q repetir los comandos.

        //Todo lo que creo y configuro en el Metodo "listarPokemon" es lo que voy a hacer en "listarElemento"
        public List<Elemento> listar()
        {
            //Creo una lista de elementos de los pokemons
            List<Elemento> lista = new List<Elemento>();

            //Aca nace un objeto que tiene un lector, que tiene un comando y una conexion
            //el comando y conexion tiene instancia y, ademas tiene una cadena de conexion configurada
            AccesoDatos datos = new AccesoDatos();
            //=>Declaro UN solo objeto conexion (Acceso a datos)
            //que internamente tiene todo lo que necesito,
            //para posteriormente consumir ese objeto en el bloque try
            //y lo voy a utilizar en todos los metodos que necesite en mis clases
            //que tenga logica dde acceso a datos.
            //=>Casi todo lo que tengo en el Metodo "listar" de la Clase PokemonNegocio
            //encapsulo todo en un solo objeto para no tener que repetir el codigo cada
            //vez que necesite acceder a la DB.

            try
            {
                //Tengo que setear la consulta que quiero realizar,
                //una consulta a la tabla de Elementos de la DB para traer estos datos,
                //mando la consulta por parametro:
                datos.setearConsulta("Select Id, Descripcion From ELEMENTOS");

                //Llamo al Metodo "ejecutarLectura()" para
                //realizar la lectura y guardar en el lector
                datos.ejecutarLectura();

                //Leo la varible "lector" para Transformar todo a objetos:
                while (datos.Lector.Read())
                {
                    Elemento aux = new Elemento();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    //Guarda los objetos en la lista
                    lista.Add(aux);
                }

                //Retorno/Devuelvo la lista de objetos, de elementos de los pokemons, si todo estuvo bien
                return lista;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //Cierra el lector(q en este caso existe)
                //y va a cerrar la conexion a la DB 
                datos.cerrarConexion();
            }
        }
    }
}
