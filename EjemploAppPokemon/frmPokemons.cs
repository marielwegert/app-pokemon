using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using dominio; //Agrego el proyecto "dominio" para que la clase PokemonNegocio
               //conozca las clases de ese proyecto (es una librería que yo creé).
using negocio; //Hago lo mismo que hice con librería "dominio", agrego "negocio".

namespace winform_app
{
    public partial class frmPokemons : Form
    {
        //ATRIBUTO PRIVADO q al principio no va a tener nada, mas abajo lleno 
        private List<Pokemon> ListaPokemon;
        
        //CONSTRUCTOR
        public frmPokemons()
        {
            InitializeComponent();
        }

        //EVENTO de carga de formulario: Aca voy a invocar la lectura a DB,
        //pero con metodo creado mas abajo llamado "cargar()"
        private void frmPokemons_Load(object sender, EventArgs e)
        {
            //Llamo al metodo cargar de mas abajo, que carga la grilla de pokemons:
            cargar();
            
            //Cargo el desplegable del "Campo:" para elegir filtrar una columna de la grilla:
            cboCampo.Items.Add("Número");
            cboCampo.Items.Add("Nombre");
            cboCampo.Items.Add("Descripción");

        }

        //EVENTO para cuando cambio la selección de la grilla,
        //cambia lo que cargo en la imagen
        private void dgvPokemons_SelectionChanged(object sender, EventArgs e)
        {
            //Como da error cuando hace el filtro rapido y vuelve a la grilla original,
            //pero como esta vez esta vacia la grilla, quiere transformar el null a un Pokemon
            //=> da error => hago una validacion:

            //En la grilla de Pokemons "dgvPokemons" la fila actual "CurrentRow" 
            //hay un pokemon selccionado => si es distinto del nulo,
            //quiere decir que hay una fila actual seleccionada
            //=> trata de transformarlo y cargar la imagen
            //SINO no hay que transformarlo pq se rompe.
            if (dgvPokemons.CurrentRow != null)
            {
                //Para que cambie la imagen cada vez que cambio la selección de la grilla
                //tengo que tomar el elemento que esta seleccionado en la grilla
                //"dgvPokemons"-> Pongo la grilla de Pokemons 
                //"CurrentRow" -> Fila actual que esta seleccionada
                //"DataBoundItem" -> elemento vinculado a datos
                //=> lo que le estoy diciendo acá es que de la grilla de pokemons
                //de la grilla actual, dame el objeto enlazado
                //xq sé q cada fila tiene un objeto
                //Objeto enlazado ("DataBoundItem") -> Es un objeto xq todas las clase heredan
                //de la clase madre y padre "Object" en el framework q esta orientado a objetos
                //=> Para que no maneje internamente como un objeto
                //Aclaro entre paréntesis que es del tipo Pokemon
                //y guardo todo esto en la variable llamada "seleccionado" que es del tipo Pokemon 
                Pokemon seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
                //pbxPokemon.Load(seleccionado.UrlImagen); -> como puse esto en el 
                //metodo privado "cargarImagen" => tengo que llamar el método ahora,
                //por lo tanto, lo que estoy haciendo es modularizar esa instrucción
                //en una función q tenia repetida (xq mas arriba tmb se repite)
                //para que tenga la misma tenga una "try-catch", una excepción:
                cargarImagen(seleccionado.UrlImagen);
            }
        }

        //METODO para invocar la lectura a DB
        private void cargar()
        {
            //OBJETO de PokemonNegocio
            PokemonNegocio negocio = new PokemonNegocio();

            //Agrego un try/catch para capturar la excepcion
            //y que la app no se romoa cuando la imagen sea null
            //solo va a mostrar un cartel con el error y no se rompe
            //PERO lo de la imagen null valido en el while de PokemonNegocio del metodo listar()
            try
            {
                //dgvPokemons.DataSource -> a la grilla de datos le voy a asignar negocio.listar()
                //negocio.listar() -> va a la DB y te devuelve una lista de datos
                //DataSource -> Recibe un origen de datos y lo modela en la tabla "dgvPokemon"
                //dgvPokemons.DataSource = negocio.listar(); MAS ABAJO HAGO UN ARREGLO DE ESTO

                //COMO FUNCIONA ESTO:
                //Yo le doy la lista de objetos al DataSource
                //La grilla "dgvPokemons" agarra la lista de objetos y ve que tiene (Pokemons en este caso)
                //Mira y lee como es la estructura de la Clase Pokemon
                //Con las propiedades de la Clase Pokemon genera automaticamente las columnas con sus cabeceras
                //con sus nombres de columnas
                //Finalmente mapea, de manera automatica, cada objeto de la lista q va a recorrer de cada objeto de la lista de cada property en cada columna
                //Entonces, de esta forma, esa lista de objetos, automaticamente se tranforma en la tabla de la app.

                //Hasta ahora lo que hace la app es cargar los datos
                //=> Lo que quiero es poder seleccionar un Pokemon y que aparezca su imagen en el "pictureBox"
                //para esto se hace lo siguiente:
                //"negocio.listar()" -> Hasta ahora obtengo y directamente le doy al "DataSource"
                //=> Lo que quiero hacer ahora es guardar en una en un objeto lista para poder manipularlo
                //xq asi como traigo de la DB le doy a la grilla "dgvPokemons".

                //Guardo lo que traigo de la DB a un atributo privado:
                //Esto sirve para poder manipular los datos traidos de la DB de manera mas sencilla
                ListaPokemon = negocio.listar();
                dgvPokemons.DataSource = ListaPokemon;

                //Llamo al metodo para ocultar columnas:
                ocultarColumnas();

                //En el pictureBox voy a cargar una imagen:
                //(pongo en la sobrecarga 2 para que pueda recibir un string Url)
                //Preselecciono la imagen del primer elemento
                //y con el EVENTO "SelectionChanged" va a cambiar la imagen del pokemon
                cargarImagen(ListaPokemon[0].UrlImagen);

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }
        }

        //METODO para ocultar columnas, las q no quiero q aparezcan en la tabla ppal.
        private void ocultarColumnas()
        {
            //Ocultamos la columna Url para q solo muestre la imagen en la app
            //no se borra la columna, solo se oculta para q no se vea en la app
            dgvPokemons.Columns["UrlImagen"].Visible = false;
            //Se oculta tambien la columna "Id"
            dgvPokemons.Columns["Id"].Visible = false;
        }

        //Armo una excepcion por si la imagen se borra y no se rompa el programa
        //METODO "cargaImagen" con un parámetro "imagen"
        //lo q hace es es mostrar una imagen por defecto, esas de error
        //no muestra un cartel
        private void cargarImagen(string imagen)
        {
            //Si falla el try (q no tiene imagen)
            //entonces va al catch y muestra la imagen por defecto.
            try
            {
                pbxPokemon.Load(imagen);
            }
            catch (Exception ex)
            {

                pbxPokemon.Load("https://t3.ftcdn.net/jpg/02/48/42/64/360_F_248426448_NVKLywWqArG2ADUxDq6QprtIzsF82dMF.jpg");
            }
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            //Declaro la instancia del formulario de alta
            //llamando al constructor VACIO de la clase "frmAltaPokemon"
            frmAltaPokemon alta = new frmAltaPokemon();
            
            //Llamo la instancia del formulario de alta:
            //ShowDialog() -> No permite salir, ir a otra ventana hasta que no se termine
            //                de trabajar en la ventana "frmAltaPokemon".
            alta.ShowDialog();
            //Una vez que se agrego exitosamente el Pokemon, se cierra el formulario
            //=> Ahi voy a actualizar la carga del nuevo pokemon.
            //Hay que hacer lo mismo que se hizo en el frmPokemons_Load()
            //Pero para no repetir codigo hago un METODO privado con lo que hice en frmPokemons_Load()
            cargar();
            //=> aca se va a ver el Pokemon nuevo cargado sin tener que cerrar y volver a abrir la app.

        }

        private void btnModificar_Click(object sender, EventArgs e)
        {
            //Hace lo mismo que el boton "Agregar"
            //Pero con la diferencia que al formulario le va a pasar por parametro
            //el objeto Pokemon que se quiere modificar.
            //Para esto, se selecciona el objeto Pokemon:
            Pokemon seleccionado;
            seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;
            //=> con esto tengo el Pokemon seleccionado
            //Ahora lo q voy a hacer es pasarle por parametro al constructor de la clase "frmAltaPokemon"
            //llamando al constructor que esta sobrecargado para q reciba un Pokemon
            frmAltaPokemon modificar = new frmAltaPokemon(seleccionado);
            //=>mando a crear el formulario de alta con el pokemon que seleccione para modificar.
            modificar.ShowDialog();
            cargar();
        }

        private void btnEliminarFisico_Click(object sender, EventArgs e)
        {
            //EVENTO para eliminar un pokemon de forma fisica (elimina en la DB)
            //Llamo al METODO eliminar:
            eliminar();
        }

        private void btnEliminarLogico_Click(object sender, EventArgs e)
        {
            //EVENTO para eliminar un pokemon de forma logica
            //Eliminacion Logica -> Elimina de forma temporal en la DB, deja inactivo el Pokemon.
            //Llamo al METODO eliminar, avisando que la bandera del metodo esta en true:
            eliminar(true);
        }

        //METODO para eliminar un Pokemon
        //bool logico = false -> Aca le estoy mandando un valor opcional 
        //=> si no le mando un parametro, va a tomar como falso por defecto.
        private void eliminar(bool logico = false)
        {
            //Creo un objeto del tipo "PokemonNegocio"
            PokemonNegocio negocio = new PokemonNegocio();
            //Para tener el id del pokemon que quiero eliminar, selecciono le objeto pokemon:
            Pokemon seleccionado;
            try
            {
                //Hago una validacion para preguntar si la presona quiere relmente eliminar el pokemon:
                //=> Guardo en una variable lo que devuelve el metodo 'show'
                DialogResult respuesta = MessageBox.Show("¿De verdad queres eliminar el Pokemon?", "Eliminando", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                //=>Una vez que se pregunto si se quiere borrar o no,
                //se va a guardar en la varible un resultado del cuadro de dialogo "Si No"
                //Si tiene un si -> elimina el Pokemon.
                //Si tiene un No -> esto no se ejecuta.
                if (respuesta == DialogResult.Yes)
                {
                    //Con esto tengo el Pokemon seleccionado que quiero eliminar.
                    seleccionado = (Pokemon)dgvPokemons.CurrentRow.DataBoundItem;

                    //Aca llamamos al EVENTO "btnEliminarFisico_Click" o "btnEliminarLogico_Click"
                    //dependiendo del valor de la bandera que puse en este metodo "eliminar(bool logico = false)"
                    //=> Si logico = true -> LLamo al evento Eliminar Logico
                    //=> Si logico = false -> LLamo al evento Eliminar Fisico
                    if (logico)
                        negocio.eliminarLogico(seleccionado.Id);
                    else
                        negocio.eliminar(seleccionado.Id);

                    //Actualizo la grilla, qpara cuando elimine el pokemon, vuelva a la grilla y ya elimine el pokemon
                    cargar();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }


        //Filtro rapido -> Busca sin ir a la DB, xq al buscar rápido no esta bueno q vaya
        //                 y vuelva de la DB todo el tiempo mientras busca,
        //                 PERO se puede hacer una busqueda mas especifica como el filtro avanzado.
        //Filtro avanzado -> Busca en la DB.
        //EVENTO para buscar CON FILTRO RAPIDO:
        //Cada vez q se toque una tecla en la caja de texto de la busqueda
        //=> se va a ir actualizando el filtro de la grilla 
        private void txtFiltro_TextChanged(object sender, EventArgs e)
        {
            //Para realizar el filtro de busqueda se usa el ATRIBUTO "ListaPokemon" 
            //"ListaPokemon" -> Se carga cuando arranca el formulario o se refresca cada vez q hay alguna accion
            //Filtro -> Se toma lo que hay en la caja de texto y se aplica un filtro sobre la lista

            //=>Creo una lista de pokemons vacia (mas abajo se llena):
            //no le genero una instancia a la lista pq la voy a obtener de un filtro q voy a aplicar
            List<Pokemon> listaFiltrada;

            //Armo una varible filtro:
            string filtro = txtFiltro.Text;

            //FORMAS DE HACER EL IF:
            // 1- "if (filtro != "")" -> Si el filtro es distinto de vacio, sin cant caracteres.
            // 2- "if (filtro.Length >= 2)" -> Si el filtro tiene 2 o mas caracteres, o sea,
            //Si se quiere filtrar algo => se filtra y devulve "listaFiltrada"
            //con los pokemons que quiero buscar.
            //Sino no hay filtro => en "listaFiltrada" se pone la lista original 
            //sin ningun tipo de filtro.
            if (filtro.Length >= 2)
            {
                //Lleno la lista con el METODO de la list "listaPokemon" (q es una coleccion)
                //"find" -> Devuelve un objeto q encuentre en la lista, segun los parametros q yo le de.
                //"findAll" -> Devuelve todos los objetos q se correspondan, q matcheen, con una clave de busqueda q yo le voy a dar.
                //=> Esto seria una filtro rapido -> xq yo voy a utilizar la lista q tengo en este momento,
                //la coleccion, para hacer un filtro sobre ella misma
                //y devolver una "listaFiltrada" q voy a utilizar para mostrar la informacion en el formulario, en el DataGridView.
                //"FindAll()" -> Le paso un parametro especial llamado "lamda expression",
                //q en este caso le pongo "x", pero le puedo poner cualquier nombre.
                //x => x.Nombre == txtFiltro.Text -> Con esta expresion, el "FindAll",
                //esto es como un forEach/ciclo, q recorre la "listaPokemon" y en c/vuelta
                //va a evaluar si el nombre del objeto "x" es igual al filtro yo le di en la caja de texto
                //=> si es igual lo va a devolver y guardar en la "listaFiltrada".
                //ToLower() -> Cambia todo a minuscula y compara asi durante la busqueda
                //             para que no diferencie la mayuscula de la minuscula. 
                //Contains(filtro.ToLower())) -> Al METODO "Contains" le paso la cadena q estoy buscando
                //"filtro.ToLower()" => esto va a devolver verdadero o falso si la cadena "filtro.ToLower()"
                //esta contenida en la otra cadena "Nombre.ToLower()", o sea, si lo que viene en el campo
                //de "filtro" esta contenido en el "Nombre" que estoy analizando 
                //=> puedo buscar una parte del nombre del pokemon sin tener que poner el nombre completo.
                //|| x.Tipo.Descripcion.ToLower().Contains(filtro.ToLower()) -> Al agregar esto hago que busque
                //por el Nombre del Pokemon o el Tipo del Pokemon.
                //=> Esto va a filtrar y armar una lista con todos los Pokemons cuyo "Nombre" coincida 
                //o (||) cuyo "Tipo" coincida
                //=> Al ser un condicionante esto, puedo seguir agregando || otras columnas.
                listaFiltrada = ListaPokemon.FindAll(x => x.Nombre.ToLower().Contains(filtro.ToLower()) || x.Tipo.Descripcion.ToLower().Contains(filtro.ToLower()));
                //Esto devuelve una lista y guarda en la variable "listaFiltrada".
            }
            else
            {
                //Sino no hay filtro => en "listaFiltrada" se pone la lista original
                //sin ningun tipo de filtro.
                listaFiltrada = ListaPokemon;
            }

            //=> Una vez q tengo la list "listaFiltada",
            //Primero limpio el DataGridView:
            dgvPokemons.DataSource = null;
            //Dsp actualizo, le digo el nuevo origen:
            dgvPokemons.DataSource = listaFiltrada;
            //Desde aca tengo el filtro funcionando.

            //Armo un METODO aparte para ocultar las columnas "Id" y "UrlImagen".
            //=> Llamo al metodo para ocultar columnas:
            ocultarColumnas();
        }

        //METODO para validar filtro de busqueda avanzado
        //Es un bool pq cuando llamo al metodo en otro lado =>
        //voy a poner q si true valide sino que no valide
        //POR LO TANTO, cuando no haya nada seleccionado en "campo"
        //va a mostrar el cartel y cuando no, no muestra nada pq hay algo sleccionado
        //y tiene q ser asi.
        private bool validarFiltro()
        {
            //Aca quiero validar q el desplegable "Campo" ("cboCampo")
            //y el desplegable "Criterio" ("cboCriterio") esta cargado:
            //Para esto pregunto:
            //SI "Campo" no tiene ninguna selccion => devuelve -1.
            //=> quiere decir que "cboCampo.SelectedItem.ToString();" del EVENTO
            //"btnFiltro_Click" no esta seleccionado.
            //SINO devuelve 0 => tiene algun elemento seleccionado.
            //Devuelve 0 -> xq es como q tiene un vector adentro y los elementos
            //de estos se cuentan a partir del cero (1er elemento del vector).
            if (cboCampo.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el campo para filtrar.");
                return true;
            }
            //Hago if para "criterio" igual que el de "campo":
            if(cboCriterio.SelectedIndex < 0)
            {
                MessageBox.Show("Por favor seleccione el criterio para filtrar.");
                return true;
            }

            //Si en el desplegable "Campo" se selecciona "Número", pregunto: 
            if (cboCampo.SelectedItem.ToString() == "Número")
            {
                //Primero -> Si el campo esta nulo o vacío:
                if (string.IsNullOrEmpty(txtFiltroAvanzado.Text))
                {
                    //Si es asi, muestra este cartel:
                    MessageBox.Show("Por favor cargar un número en el Filtro avanzado.");
                    return true;
                }

                //Segundo -> Ai el campo no está vacío, tmp puede ser texto:
                //=> Si se ingresó un nro en la caja de texto, esta todo bien para seguir.
                if (!(soloNumeros(txtFiltroAvanzado.Text)))
                {
                    //=> Si no se ingresó un nro (el metodo "soloNumeros" retorna un falso)
                    //muestro el mensaje y cancelo para q se siga.
                    MessageBox.Show("Por favor cagar sólo números.");
                    return true;
                } 
            }

            return false;
        }
        
        //METODO para que una caja de texto reciba solo números
        private bool soloNumeros(string cadena)
        {
            //Va a recorrer la cadena ingresada y va a devolver true o false
            foreach (char caracter in cadena)
            {
                //En la cadena se va a analizar si cada elemento es una letra:
                //SI NO es nro el caracter => retorna un falso
                //SI ES nro el caracter => retorna un true
                if (!(char.IsNumber(caracter)))
                    return false;
            }
            return true;
        }
        
        
        //Filtro rapido -> Busca sin ir a la DB.  
        //Filtro avanzado -> Busca en la DB => permite filtrar por campo y por criterio.
        //EVENTO para buscar CON FILTRO AVANZADO
        private void btnFiltro_Click(object sender, EventArgs e)
        {
            //Armo una variable "negocio" del tipo "PokemonNegocio" que devuelve una lista:
            PokemonNegocio negocio = new PokemonNegocio();

            
            try
            {
                //ANTES de hacer algo, hay que validar que estos campos
                //que se van a agregar ahora, se completen para que realice
                //la busqueda avanzada.
                //=> llamo al METODO para validar los filtros: 
                //SI "validarFiltro()" da true => mostrar el mensaje de "seleccionar campo"
                if (validarFiltro())
                    return;
                //Como este EVENTO "btnFiltro_Click" es void no devulve nada
                //=> el return de este if no devulve nada
                //=>lo que va a hacer es cancelar la ejecución de este evento.
                
                //Si el if da falso, sigue la ejecucion de este evento:
                //Para poder ir a la DB y filtrar, tengo que capturar 
                //lo que diga en "Campo", "Criterio" y "Filtro avanzado"
                string campo = cboCampo.SelectedItem.ToString();
                string criterio = cboCriterio.SelectedItem.ToString();
                string filtro = txtFiltroAvanzado.Text;

                dgvPokemons.DataSource = negocio.filtrar(campo, criterio, filtro);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
            



        }

        //EVENTO del desplegable del "Campo"
        private void cboCampo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //Creo una variable donde guardo el elemento selccionado del desplegable de "Campo": 
            string opcion = cboCampo.SelectedItem.ToString();

            //Guardo en el desplegable sgte de "Criterio":
            //-Si la varible "opcion" es igual a "Número"
            //=> que en el deplegable aparezca q sea "Mayor a", "Menor a" o "Igual a".
            //-Sino la variable "opcion" es alguna de las otras opciones, q son texto
            //=> que en el deplegable aparezca q sea "Comienza con", "Termina con" o "Contiene"
            if (opcion == "Número")
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Mayor a");
                cboCriterio.Items.Add("Menor a");
                cboCriterio.Items.Add("Igual a");
            }
            else
            {
                cboCriterio.Items.Clear();
                cboCriterio.Items.Add("Comienza con");
                cboCriterio.Items.Add("Termina con");
                cboCriterio.Items.Add("Contiene");
            }
        }
    }
}
