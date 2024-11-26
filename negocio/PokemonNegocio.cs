using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient; //Declaro la utilización de esta libreria
                             //para poder crear los objetos y así establecer
                             //la conexión y lectura a la DB.
using dominio; //Agrego el proyecto "dominio" para que la clase PokemonNegocio
               //conozca las clases de ese proyecto (es una librería que yo creé).
using System.Diagnostics.Contracts;

namespace negocio
{
    public class PokemonNegocio
    {

        //CLASE POKEMON NEGOCIO -> Sirve para conectarme a la DB.
        //Aca voy a crear los métodos de acceso a datos para los pokemons.
        
        //Cada Clase tiene q tener su "Clase de Negocio"
        //-> con sus métodos de acceso a datos.


        //No conecto desde la "Clase Pokemon" xq es la clase q me define el modelo
        //de mi clase y el objeto que voy a utilizar.
        //Ahora voy a necesitar un método q haga una conexion a la DB y q realice
        //una lectura => por principio de diseño no va en la clase "Pokemon".

        //Creo un MÉTODO q lee registros de la DB
        //y devuelve varios registros pokemon q se agrupan en una lista.
        public List<Pokemon> listar()
        {
            //Creo una lista de pokemons
            List<Pokemon> lista = new List<Pokemon>();
            

            //Creo los objetos q necesito para la conexión a la DB
            //(antes creo la librería):
            //Objeto para conectarme a algún lado:
            SqlConnection conexion = new SqlConnection();
            //=> una vez q me conecte a esa "conexion", voy a necesitar 
            //realizar acciones.
            //Para realizar acciones voy a declarar el objeto "comando":
            SqlCommand comando = new SqlCommand();
            //Como resultado de la lectura q voy a realizar con la DB,
            //voy a tener un set de datos, q voy a albergar en una variable
            //llamada "lector":
            SqlDataReader lector;
            //No genero instancia pq cuando realice la lectura, voy a obtener como
            //resultado una instancia de un objeto del tipo "SlqDataReader".


            //Creo una función para poder establecer la conexión a la DB.
            //Hago un manejo de excepciones para poder crear esta lista:
            try
            {
                //Acá pongo toda la funcionalidad que pueda fallar:
                //como la lectura a DB, la transformación de datos, etc.

                //1.Para poder establecer la conexión y leer los datos:, necesito:
                //declarar ciertos objetos y configurarlos para poder establecer
                //la conexión.
                //2.Una vez que cree los objetos y la variable,
                //dsp hay que configurarlos:
                //a.Configuro la cadena de conexión
                //(q es un atributo del objeto "conexion"):
                //conexion -> Objeto del tipo "SqlConnection".
                //ConnectionString -> Cadena de conexion.
                //-Dsp del = -> Aclaro entre comillas a dónde me voy a conectar
                //"server=..." o "datasourve??=" -> Aclaro a qué servidor me voy a conectar:
                //Si es a mi motor de DB local -> copio lo que dice en "server name" en la DB.
                //Si es al motor de DB remoto -> "server=IP\\el nombre del motor de DB. 
                //-Antes de \\ -> es el nombre de la computadora ("server=MARIEL\\SQLEXPRESS")
                //Para q se haga referencia a un entorno local GENERAL pongo:
                //"server=(local)\\SQLEXPRESS" o "server=.\\SQLEXPRESS" ->los 2 son lo mismo.
                //-Dsp de \\ -> va el nombre la instancia de la DB
                //(q es SQLEXPRESS a menos q se cambie o si es otro tipo de DB q no sea express).
                //-Dsp de ; -> Aclaro a qué DB me voy a conectar como:
                //"initial catalog=..." o "database=..." -> va el nombre de la DB.
                //-Dsp del 2do ; -> Aclaro cómo me voy a conectar:
                //FORMA 1 -> Seguridad integrada: es la seguridad q estamos utilizando en "Authentication" en la DB
                //(en este caos es "windows authentication" quiere decir q utiliza las credenciales de windows),
                //funciona cuando se trabaja de manera local. "integrated security=true".
                //FORMA 2 -> "SQL Server Authentication": Para esto hay q tener un usuario y contraseña del motor de DB.
                //"integrated security=false;user=...;password=...;".
                conexion.ConnectionString = "server=.\\SQLEXPRESS; database=POKEDEX_DB; integrated security=true";
                //Hasta acá ya tengo configurado a dónde me voy a conectar.

                //b.Configuro el comando:
                //que sirve para ralizar la acción (que va a ser la lectura):
                //la lectura voy a hacer mandandole/inyectadole desde acá la sentencia sql que yo quiero ejecutar.
                //=> al comando le voy a decir de qué tipo va a ser, hay 3:
                //TIPO 1 -> Text: En el cual le voy a mandar/inyectar una sentencia sql. 
                //TIPO 2 -> StoredProedure(Procedimiento almancenado): Es la forma en la cual yo le voy a pedir q ejecute una funion q va a estar guardada en la DB.
                //TIPO 3 -> TableDirect(Enlace Directo con la Tabla): No se suele usar.
                comando.CommandType = System.Data.CommandType.Text;
                //Una vez q le digo que es del tipo texto, le voy a decir cuál es el texto.
                //texto -> va a ser la consulta sql (hacer 1ro en sql).
                //Consulta sql -> no se puede poner el * para consultar todo, tiene que ser una consulta declarando las columnas que quiero.
                //Esta es la consulta que desde mi aplicacion le voy a mandar a la DB:
                comando.CommandText = "Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id from POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id AND P.IdDebilidad = D.Id AND P.Activo = 1";
                //Después voy al comando.connection que va a ejecutar ese comando en esta conexion:
                comando.Connection = conexion;
                //=>el comando que estoy configurando en "comando.commandtext... y comando.commandtext..." se va a
                //ejecutar en la conexion de esta ultima linea que estoy estableciendo en "conexion.ConnectionString..."

                //3.Abro la conexion:
                conexion.Open();

                //4.Realizo la lectura
                //Esto me va a dar como resultado un "SqlDataReader".
                lector = comando.ExecuteReader();
                //Si todo esto funcionó bien, hasta acá tengo los datos guardado en la varible creada mas arriba.
                //Ahora tengo mi objeto SqlDataReader con los datos.
                //=>Esto me genera una tabla virtual con un puntero, q vamos a ir posicionando ahora en memoria,
                //que yo voy a trasformar en esta coleccion de objetos ("List<Pokemon> lista = new List<Pokemon>()")
                //y para poder transformar, tengo que ir leyendo la variable "lector".

                //5.Leo la varible "lector":
                //"lector.Read()"-> Si pudo leer, o sea, si hay un registro a continuacion, va a devolver TRUE
                //y entra al while y ademas de devolver TRUE, va a posicionar un puntero en la siguiente posicion,
                //=>Cuando recien arranca, va a devolver la tabla de sql vacía, con los titulos pero sin llenar cada columna
                //Cuando haga "lector.Read()" se va a fijar si hay una lectura, si la hay, va a posicionar el puntero en la 1ra fila y ademas va a devolver TRUE,
                //=> va a ingresar al while y va a empezar a cargar "Pokemon aux" con los datos del lector de ese registro
                //"aux.Numero = lector.GetInt32(0)" -> Para poner q es int tengo q conocer el tipo de dato en la DB, tienen que coincidir
                //Fijarse q algunos tipos de datos se llaman distinto en la DB y en c#.
                //GetInt32(0) -> Adentro del parentesis va el indice d la columna q estoy queriendo completar, empieza con el cero.
                //Si quiero hacer de la misma forma con las columnas Nombre y Descripcion, pongo "GetString" q en la DB el tipo de dato es "varchar".
                //"aux.Nombre = (string)lector["Nombre"]"->Otra forma, pero si no aclaro entre parenteis q es un string, devuelve un objeto
                //"lista.Add(aux)" -> A lo ultimo, agrego ese Pokemon a la lista 
                //Entonces, en cada vuelta (cada vez q haga "lector.Read()") y encuentre un siguiente elemento,
                //el puntero va a bajar de fila en la tabla que llamo de la DB y va a reutilizar la variable "aux" pero
                //va a crear una nueva instancia de Pokemon y ahi le va a ir guardando los datos de la linea q corresponda
                //en cada vuelta del while (cada vez q haya hecho read al lector) y esa referencia va a ir guardando a la lista
                //=> la lista va a ir teniendo referencia a distintos objetos.
                //Cuando no haya mas por leer, el "lector.Read()" va a dar FALSE, va a salir del while
                //y, si va todo bien, va a retornar la lista, sino va a devoler una excepcion.

                //Transforma todo a objetos
                //Copio el while del METODO "listar()"
                //PERO donde aparece "lector" pongo "datos.lector"
                while (lector.Read())
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)lector["Id"];
                    aux.Numero = lector.GetInt32(0);
                    aux.Nombre = (string)lector["Nombre"];
                    aux.Descripcion = (string)lector["Descripcion"];

                    //Hago una validacion para cuando la imagen sea null:
                    //Le digo al if que intente leer si la imagen NO esta nula
                    //(por eso niego con el signo de admiracion al ppio del if)
                    //xq cuando la imagen esta nula es cuando la  app re rompe.

                    //VALIDACION:
                    if (!(lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)lector["UrlImagen"];


                    //Cargo los datos de mi Tipo de mi Pokemon
                    //Como no cree una instancia antes en el property Tipo
                    //=>tengo q crear un nuevo objeto del tipo Elemento
                    //xq sino cuando quiera hacer "aux.Tipo.Descripcion..."
                    //sin crear esto antes, me va dar referencia nula
                    //xq no existe un objeto del tipo Elemento cargado aca. 
                    aux.Tipo = new Elemento();
                    //Tipo -> Es un objeto => tengo que aclarar q dato estoy trayendo
                    //del objeto "Tipo", que es la descripcion
                    aux.Tipo.Id = (int)lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)lector["Tipo"];

                    //Cargo los datos de mi Debilidad de mi Pokemon
                    //Pasa lo mismo que con "Tipo"
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)lector["Debilidad"];

                    //Guarda los objetos en la lista
                    lista.Add(aux);
                }


                //6.Cierro la conexion:
                conexion.Close();

                //Retorno/Devuelvo la lista de objetos, de pokemons, si todo estuvo bien
                return lista;
            }
            catch (Exception ex)
            {

                //Retorno la lista de pokemons si todo estuvo mal
                throw ex;
            }

            

        }

        
        //Si quiero AGREGAR o MODIFICAR un Pokemon,
        //voy a necesitar una FUNCIÓN q agregue Pokemons y otra que modifique Pokemons,
        //y cada Funciòn se va a tener que conectar a la DB por separado
        
        //METODO para agregar un pokemon
        public void agregar(Pokemon nuevo)
        {
            //Aca construyo la logica para que se conecte a la DB
            //Para poder conectarme, necesito un objeto de la clase "AccesoDatos"
            AccesoDatos datos = new AccesoDatos();
            //Con este objeto ya me puedo conectar a la DB
            //Como este Metodo SOLO VA A INSERTAR registros => No necesito una lista para devolver
            
            try
            {
                //Traigo las columnas que quiero configurar cuando agregue un pokemon de SQL
                //Pero para que la consulta funcione con los datos del pokemon nuevo
                //Tengo que reemplazar los valores de Numero, Nombre y Descripcion que traje de la consulta SQL
                //FORMA 1 para reemplazar:
                //values(1, '', '',... reemplazo esto por: values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', ...
                // 1 -> " + nuevo.Numero + " -> Le estoy poniendo el nro del nuevo pokemon a la DB.
                //'' -> '" + nuevo.Nombre + "' -> Le estoy poniendo el nombre del nuevo pokemon a la DB.
                //'' -> '" + nuevo.Descripcion + "' -> Le estoy poniendo la descripcion del nuevo pokemon a la DB.
                //Lo unico que queda igual es el Activo.
                //FORMA 2 para reemplazar sin concatenar como los anteriores:
                //Como en el Pokemon recibo el objeto "Tipo" y el objeto "Debilidad" pero lo que tengo que mandar en el Insert es un Id
                //xq lo que se guarda en la tabla Pokemons es el "IdTipo" y "IdDebilidad" => hay que mandar esos Id
                //no me interesa la descripcion sino los Id para poder dar de alta ese pokemon que agrego.
                //Para IdTipo -> agrego dsp de todo lo q puse antes -> values(..., @idTipo,...)
                //Para IdDebilidad -> agrego dsp de todo lo q puse antes -> values(...,@idTipo, @idDebilidad)
                //Con el @ estoy creando una variable, le estoy diciendo a la consulta SQL que cuando se ejecute "IdTipo" y "IdDebilidad"
                //va a esxistir una variable "idTipo" y una varible "idDebilidad"
                //Para UrlImagen -> Hago lo mismo que hice con "IdTipo" y "IdDebilidad"
                datos.setearConsulta("insert into POKEMONS(Numero, Nombre, Descripcion, Activo, IdTipo, IdDebilidad, UrlImagen)values(" + nuevo.Numero + ", '" + nuevo.Nombre + "', '" + nuevo.Descripcion + "', 1, @idTipo, @idDebilidad, @urlImagen)");
                //Aca les digo que van a tener las variables "@idTipo" y "@idDebilidad", que ene realidad son parametros y les tengo que agregar al comando
                //pero no puedo hacer "comando.parametros" pq el "comando" esta encapsulado
                //=>Hay que ir a la clase "AccesoDatos" y crear un metodo que permita agregar esos parametros.
                //Llamo la funcion "SetearParametro" de la clase "AccesoDatos" para agregar los datos de @idTipo y @idDebilidad ingresados al agregar un pokemon
                //SetearParametro -> Permite cargar un nombre de un parametro
                //                   y un object value (el valor del parametro)
                datos.setearParametro("@idTipo", nuevo.Tipo.Id);
                datos.setearParametro("@idDebilidad", nuevo.Debilidad.Id);
                datos.setearParametro("@urlImagen", nuevo.UrlImagen);


                //Una vez que setee la consulta,
                //Ahora hay que Ejecutar la consulta:
                //Pero como es una Insert, no puedo llamar al Metodo "ejecutarLectura"
                //xq no devulvo un resultado sino que agrego,
                //=> tengo que hacer una ejecucion del tipo "no consulta", utilizando ExecuteNonQuery.
                datos.ejecutarAccion();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                datos.cerrarConexion();
            }
        }
        
        
        //METODO para modificar un pokemon, parecido al metodo agregar pero con algunos cambios
        public void modificar(Pokemon modificar)
        {
            //Este objeto puede ser un atributo privado de mi clase PokemonNegocio
            //para no tener que andar creeando uno nuevo en cada metodo y nacer en el constructor
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //Como ya nace seteado => entonces tengo que settear la consulta
                //Esta vez no traigo un "select" de la DB sino un "update" 
                datos.setearConsulta("update POKEMONS set Numero = @numero, Nombre = @nombre, Descripcion = @desc, UrlImagen = @img, IdTipo = @idTipo, IdDebilidad = @idDebilidad where Id = @id");
                datos.setearParametro("@numero", modificar.Numero);
                datos.setearParametro("@nombre", modificar.Nombre);
                datos.setearParametro("@desc", modificar.Descripcion);
                datos.setearParametro("img", modificar.UrlImagen);
                datos.setearParametro("idTipo", modificar.Tipo.Id);
                datos.setearParametro("idDebilidad", modificar.Debilidad.Id);
                datos.setearParametro("id", modificar.Id);

                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {

                throw ex;
            }
            finally
            {
                datos.cerrarConexion(); 
            }
               
        }


        //METODO p/ Eliminacion Fisica -> Elimina de forma permanente en la DB 
        //elimina de forma Fisica un pokemon q recibe por parametro un id del pokemon q quiera eliminar
        public void eliminar(int id)
        {
            try
            {
                //Creo la clase Acceso a Datos
                AccesoDatos datos = new AccesoDatos();

                //Seteo la consulta desde la DB 
                //La consulta va a ser "Eliminar" con "delete" xq es una Eliminacion Fisica
                //De lo que traje de la DB cambio:
                //id="nro q le puse en la consulta" -> pongo "@id". 
                datos.setearConsulta("delete from POKEMONS where id = @id");
                //Agregamos el parametro @id:
                datos.setearParametro("@id", id);

                //Agrego la accion de borrar contra la DB:
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //METODO p/ Eliminacion Logica -> Elimina de forma temporal en la DB (deja inactivo)
        //elimina de forma Logica un pokemon q recibe por parametro un id del pokemon q quiera eliminar
        //Si una persona elimina un registro sin querer en la app de forma logica
        //=> se puede programar dandole la funcionalidad a un
        //perfil "Admin" de q se pueda ir a alguna opcion en la app
        //y se pueda recuperar algun registro desde la app sin tener que ir a la DB
        //=>Asi como se filtro para q se traigan todos los pokemons activos,
        //se puede tener otro listado en una opcion especial que traiga
        //todos los pokemons inactivos y un boton, asi como el btn "Eliminar logico",
        //que diga "Activar" => este btn va a agarrar el Id seleccionado, va ir a la DB,
        //va a Activar el Pokemon y dsp cuando se vuelva al listado ppal, donde aparecen los
        //pokemones activos, va a estar el pokemon selccionado activo otra vez.
        public void eliminarLogico(int id)
        {
            try
            {
                //Creo la clase Acceso a Datos
                AccesoDatos datos = new AccesoDatos();

                //Seteo la consulta desde la DB
                //La consulta va a ser "Eliminar" con "Update" xq es una Eliminacion Logica 
                //=> tengo que cambiar la columna Activo(1) como Inactivo(0).
                //De lo que traje de la DB cambio:
                //id="nro q le puse en la consulta" -> pongo "@id". 
                datos.setearConsulta("update POKEMONS set Activo = 0 where id = @id");
                //Agregamos el parametro @id:
                datos.setearParametro("@id", id);

                //Hay que actualizar los datos (por mas q sea una actualizacion "Update"):
                datos.ejecutarAccion();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //METODO muy parecido a "listar()" 
        public List<Pokemon> filtrar(string campo, string criterio, string filtro)
        {
            //Creo una lista de pokemons:
            List<Pokemon> lista = new List<Pokemon>();
            //Creamos el objeto de acceso a datos:
            AccesoDatos datos = new AccesoDatos();

            try
            {
                //Se necesita LA MISMA consulta a la DB que se hizo en el METODO "listar()"
                //PERO le concatenamos posibles filtros de agregarle un "campo" y un "criterio"
                //=> dependiendo de lo que la persona seleccione, voy a elegir un campo (Nro, Nombre o Descripcion)
                //=> la ultima parte va a ser dinamica en la consulta
                //A LO ULTIMO DE LA CONSULTA -> agrego "And " y esto se completa con "Switch" o "If"
                string consulta = "Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id from POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id AND P.IdDebilidad = D.Id AND P.Activo = 1 And ";
                //Consulta dinamica en DB -> Con el SWITCH o el IF voy a evaluar los campos del METODO "filtrar()"
                //y dependiendo de lo que tengan, voy a terminar de completar la consulta.
                //En ambos casos se puede optimizar para no repetir la variable "filtro".
                //Se puede agregar mas busquedas en el front (en el diseño) para q sea mas especifica, depende de lo que se necesite.
                //Tanto para el SWITCH o el IF -> se escriben como en la consulta de SQL.

                //SWITCH:
                //Si el campo es:
                //= Numero -> criterio "mayor a" => se pone escrito esto "Numero > " mas la variable "filtro".
                //              y asi para el resto de las opciones de criterio.
                //= Nombre -> criterio > => se pone escrito esto "Nombre like " mas la variable "filtro".
                //              y asi para el resto de las opciones de criterio.
                //= Descripcion -> Lo mismo que en nombre
                switch (campo)
                {
                    case "Número":
                        switch (criterio)
                        {
                            case "Mayor a":
                                consulta += "Numero > " + filtro;
                                break;
                            case "Menor a":
                                consulta += "Numero < " + filtro;
                                break;
                            default:
                                consulta += "Numero = " + filtro;
                                break;
                        }
                        break;

                    case "Nombre":
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "Nombre like '" + filtro + "%' ";
                                break;
                            case "Termina con":
                                consulta += "Nombre like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "Nombre like '%" + filtro + "%'";
                                break;
                        }
                        break;

                    default:
                        switch (criterio)
                        {
                            case "Comienza con":
                                consulta += "P.Descripcion like '" + filtro + "%' ";
                                //Aca es P.Descripcion pq tiene q estar escrito como en SQL
                                break;
                            case "Termina con":
                                consulta += "P.Descripcion like '%" + filtro + "'";
                                break;
                            default:
                                consulta += "P.Descripcion like '%" + filtro + "%'";
                                break;
                        }
                        break;
                }

                ////IF -> Parecido al Switch
                //if (campo == "Número")
                //{
                //    switch (criterio)
                //    {
                //        case "Mayor a":
                //            consulta += "Numero > " + filtro;
                //            break;
                //        case "Menor a":
                //            consulta += "Numero < " + filtro;
                //            break;
                //        default:
                //            consulta += "Numero = " + filtro;
                //            break;
                //    }
                //}
                //else if (campo == "Nombre")
                //{
                //    switch (criterio)
                //    {
                //        case "Comienza con":
                //            consulta += "Nombre like '" + filtro + "%' ";
                //            break;
                //        case "Termina con":
                //            consulta += "Nombre like '%" + filtro + "'";
                //            break;
                //        default:
                //            consulta += "Nombre like '%" + filtro + "%'";
                //            break;
                //    }
                //}
                //else
                //{
                //    switch (criterio)
                //    {
                //        case "Comienza con":
                //            consulta += "P.Descripcion like '" + filtro + "%' ";
                //            break;
                //        case "Termina con":
                //            consulta += "P.Descripcion like '%" + filtro + "'";
                //            break;
                //        default:
                //            consulta += "P.Descripcion like '%" + filtro + "%'";
                //            break;
                //    }
                //}

                datos.setearConsulta(consulta);
                datos.ejecutarLectura();

                //Transforma todo a objetos
                //Copio el while del METODO "listar()"
                //PERO donde aparece "lector" pongo "datos.lector"
                while (datos.Lector.Read()) 
                {
                    Pokemon aux = new Pokemon();
                    aux.Id = (int)datos.Lector["Id"];
                    aux.Numero = datos.Lector.GetInt32(0);
                    aux.Nombre = (string)datos.Lector["Nombre"];
                    aux.Descripcion = (string)datos.Lector["Descripcion"];

                    //Hago una validacion para cuando la imagen sea null:
                    //Le digo al if que intente leer si la imagen NO esta nula
                    //(por eso niego con el signo de admiracion al ppio del if)
                    //xq cuando la imagen esta nula es cuando la  app re rompe.

                    //VALIDACION:
                    if (!(datos.Lector["UrlImagen"] is DBNull))
                        aux.UrlImagen = (string)datos.Lector["UrlImagen"];


                    //Cargo los datos de mi Tipo de mi Pokemon
                    //Como no cree una instancia antes en el property Tipo
                    //=>tengo q crear un nuevo objeto del tipo Elemento
                    //xq sino cuando quiera hacer "aux.Tipo.Descripcion..."
                    //sin crear esto antes, me va dar referencia nula
                    //xq no existe un objeto del tipo Elemento cargado aca. 
                    aux.Tipo = new Elemento();
                    //Tipo -> Es un objeto => tengo que aclarar q dato estoy trayendo
                    //del objeto "Tipo", que es la descripcion
                    aux.Tipo.Id = (int)datos.Lector["IdTipo"];
                    aux.Tipo.Descripcion = (string)datos.Lector["Tipo"];

                    //Cargo los datos de mi Debilidad de mi Pokemon
                    //Pasa lo mismo que con "Tipo"
                    aux.Debilidad = new Elemento();
                    aux.Debilidad.Id = (int)datos.Lector["IdDebilidad"];
                    aux.Debilidad.Descripcion = (string)datos.Lector["Debilidad"];

                    //Guarda los objetos en la lista
                    lista.Add(aux); 
                }

                return lista;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
