using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //Incluyo esta libreria para poder conectarme a la DB

namespace negocio
{
    public class AccesoDatos
    {
        //Esta clase centraliza toda la conexion a la DB para no tener q repetir los comandos.
        //Hago algo parecido que hice en el Metodo "listar" de la Clase PokemonNegocio.

        //Declaro ATRIBUTOS (los objetos q necesito para establecer una conexion
        //Esta no es una PROPIEDAD q se acceda desde otro lado
        private SqlConnection conexion; //Declaro una varible vacia
        private SqlCommand comando;
        private SqlDataReader lector;
        //Para poder leer el atributo privado "lector" desde el exterior
        //creo la PROPIEDAD del mismo:
        public SqlDataReader Lector
        {
            get { return lector; }
        }


        //Creo un CONSTRUCTOR AccesoDatos
        public AccesoDatos()
        {
            //En el constructor le paso la cadena de conexion
            //Cadena de conexion -> es la misma que esta en el Metodo "listar" de la Clase
            //                      PokemonNegocio "conexion.ConnectionString =..."
            
            //En el Metodo "listar" de la Clase PokemonNegocio
            //creo el conection con un Constructor vacio "SqlConnection conexion = new SqlConnection();"
            //y dsp le asigno la Propiedad "conexion.ConnectionString =...".

            //Aca lo que hago es al momento de crear el Constructor,
            //le paso por parametro la cadena de conexion (esta sobrecargado el Constructor).
            conexion = new SqlConnection("server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true");
            
            //Para hacer una consulta o una accion contra la DB declaro un comando:
            comando = new SqlCommand();



        }

        //Hago un METODO "SetearConsulta" para configurar el comando
        //donde pongo el "commandType" y "commandText"
        //del Metodo "listar" de la Clase PokemonNegocio
        public void setearConsulta(string consulta)
        {
            comando.CommandType = System.Data.CommandType.Text;
            comando.CommandText = consulta;
        }

        //Hago un METODO para abrir y darle la conexion y ejecutar,
        //y esta ejecucion va a devolver un lector
        public void ejecutarLectura()
        {
            //=> Este metodo realiza la lectura y lo guarda en el lector
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                lector = comando.ExecuteReader();
                //ExecuteReader -> Se utiliza para ejecutar declaraciones SELECT
                //                 y recuperar un conjunto de resultados, devulve lo q hay en DB.
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
        }

        //METODO para ejecucion del tipo "no consulta"
        public void ejecutarAccion()
        {
            comando.Connection = conexion;
            try
            {
                conexion.Open();
                comando.ExecuteNonQuery();
                //ExecuteNonQuery -> Ejecuta instrucciones SQL sin devolver ningún conjunto
                //de resultados. Se puede utilizar para crear objetos de DB o modificar
                //datos en una DB ejecutando instrucciones INSERT, UPDATE o DELETE. 

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //METODO para validar esas variables @idTipo y @idDebilidad de la clase "PokemonNegocio"
        //que yo cree en la consulta embebida, en el string,
        //Le agrego esos parametros a conexion, como hice en los metodos anteriores
        public void setearParametro(string nombre, object valor)
        {
            //AgregarConValor -> Permite cargar un nombre de un parametro
            //                   y un object value (el valor del parametro) 
            comando.Parameters.AddWithValue(nombre, valor);
        }


        //METODO para cerrar la conexion
        public void cerrarConexion()
        {
            if(lector != null) //Si realice una lectura y tengo el lector
                               //(a veces lo voy a hacer y a veces no)
                lector.Close(); //=> el lector tambien hay que cerrarlo 

            conexion.Close();
        }


    }
}
