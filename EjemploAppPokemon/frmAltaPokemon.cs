using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using dominio;
using negocio;
using System.Configuration; //Con esta libreria puedo usar la ruta que este en "app.config"

namespace winform_app
{
    public partial class frmAltaPokemon : Form
    {
        //Creo este ATRIBUTO xq cuando toque el boton:
        //Agregar -> Se va a ejecutar el constructor vacio 
        //          => el atributo Pokemon privado va a quedar en nulo.
        //Modificar -> Se va a ejecutar el constructor sobrecargado
        //           => va a estar cargado con un pokemon q vino de la otra ventana.
        private Pokemon pokemon = null;

        //Creo este otro ATRIBUTO para poder levantar una imagen de nuestra computadora
        //"OpenFileDialog" -> Permite generar una ventana de dialogo q se va a abrir
        //y permite elegir un archivo.
        //=> El objeto "archivo" -> va a arrancar en nulo.
        //=> Al tocar el EVENTO  "btnAgregarImagen_Click" -> quiere decir que se agrego una
        //                                                   imagen en el objeto "archivo". 
        private OpenFileDialog archivo = null;

        //CONSTRUCTOR vacio
        public frmAltaPokemon()
        {
            InitializeComponent();
        }

        //Modifico el CONSTRUCTOR de arriba
        //para que reciba por parametro un pokemon del tipo Pokemon
        public frmAltaPokemon(Pokemon pokemon)
        {
            InitializeComponent();
            this.pokemon = pokemon;
            //pongo el "this" pq los 2 se llaman igual
            //this.pokemon -> Es el pokemon del ATRIBUTO de arriba
            //= pokemon -> Es el pokemon del PARAMETRO del constructor sobrecargado

            //Cambio el nombre de la ventana cuando voy a modificar un Pokemon
            //para que no diga "Nuevo Pokemon" como cuando agrego uno nuevo
            Text = "Modificar Pokemon";
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            Close(); //Cierra la venta de "frmAltaPokemon" cuando apreto Cancelar
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            //Al momento de darle click en Aceptar, se capturan los datos que se cargan
            //y hay que transformarlos en un objeto del Tipo Pokemon.
            
            //Creo un nuevo objeto del tipo PokemonNegocio
            //para poder llamar al Metodo "Agregar()" que recibe como parametro un nuevo Pokemon 
            PokemonNegocio negocio = new PokemonNegocio();

            try
            {
                //Si el pokemon:
                //Esta en nulo -> Quiere decir que se toco el btn "Agregar"
                //              => hay que generar un nuevo pokemon para q no siga en nulo. 
                //No esta en nulo -> Quiere decir que se toco el btn "Modificar"
                //                   y no hace falta generar un nuevo pokemon.

                if (pokemon == null)
                    pokemon = new Pokemon();
                
                //Cargo lo que voy cargando en los cuadros de textos(txt)
                //usando el ATRIBUTO pokemon del tipo Pokemon que cree al ppio de esta clase: 
                pokemon.Numero = int.Parse(txtNumero.Text);
                pokemon.Nombre = txtNombre.Text;
                pokemon.Descripcion = txtDescripcion.Text;
                pokemon.UrlImagen = txtUrlImagen.Text;
                //Capturo lo que voy seleccionando en los deplegables "Tipo" y "Debilidad"
                //Como "cboTipo.SelectedItem" devuelve un objet
                //=> tengo q aclarar entre parentesis que es del tipo "Elemento" 
                pokemon.Tipo = (Elemento)cboTipo.SelectedItem;
                pokemon.Debilidad = (Elemento)cboDebilidad.SelectedItem;
                //Hasta aca ya tengo el objeto "poke" cargado
                
                //Mando estos datos a la DB

                //Antes hago una validacion para saber si estoy agregando o modificando un pokemon
                //Si el pokemon es distinto de cero => estoy modificando un pokemon.
                //Sino estoy agregando un pokemon.
                if(pokemon.Id != 0)
                {
                    //Para modificar llamo a la funcion "Modificar()" de la clase PokemonNegocio
                    negocio.modificar(pokemon);
                    MessageBox.Show("Modificado exitosamente");
                }
                else
                {
                    //Para esto llamo a la funcion "Agregar()" de la clase PokemonNegocio
                    negocio.agregar(pokemon);
                    //Si todo sale bien, muestro un cartel de que se agrego un nuevo Pokemon
                    MessageBox.Show("Agregado exitosamente");
                    //Si no pudo agregar, va a salta al catch antes de mostrar el cartel
                    //Si agrego, cierra la ventana y vuelve a la ppal.
                }

                //Para guardar la imagen, si la levanto localmente tengo q hacer una validacion
                //=> Si el archivo es distinto del nulo -> quiere decir q tengo que guardar la imagen localmente
                //=> Y si la barra de texto de "UrlImagen" no contiene "http" -> quiere decir que no se puso una imagen de internet
                //=> POR LO TANTO tiene que guardar el archivo imagen de manera local:
                if(archivo != null && !(txtUrlImagen.Text.ToLower().Contains("http")))
                    File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName);
                //Toda la explicacion de que es cada cosa esta en el EVENTO "btnAgregarImagen_Clic"

                Close();
                //Esto es todo el FrontEnd, para que se conecte a la DB,
                //tengo que poner la logica de la app en el Metodo "Agregar()" 

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void frmAltaPokemon_Load(object sender, EventArgs e)
        {
            //Aca cargo los desplegables "Tipo" y "Debilidad" desde la DB

            //Creo un objeto del tipo ElementoNegocio
            ElementoNegocio elementoNegocio = new ElementoNegocio();

            try
            {
                //Voy a la DB:
                cboTipo.DataSource = elementoNegocio.listar();
                //Hago que el desplegable Tipo se precargue cuando quiero modificar un pokemon
                //=>le digo cual quiero q sea su "clave" y cual su "value"
                //para dsp decirle que value quiero que este preseleccionado
                cboTipo.ValueMember = "Id"; //elijo como valor escondido del objeto, valor clave
                cboTipo.DisplayMember = "Descripcion"; //elijo este como el value que quiero que se preseleccione, que se muestre

                //Voy a la DB:
                //No voy una sola vez a la DB asociando la misma lista a los 2 desplegables
                //xq al ser 2 desplegables distintos, si quedan asociados a la misma referencia
                //de lista, se marean los deplegables y se rompe
                //=> por mas que sea el mismo tipo de datos, hay que agregarle una lista distinta.
                //Aca ya tengo los deplegables cargados en la app.
                cboDebilidad.DataSource = elementoNegocio.listar();
                //Hago que el desplegable Debilidad se precargue como en el deplegable "Tipo"
                cboDebilidad.ValueMember = "Id"; //valor clave
                cboDebilidad.DisplayMember = "Descripcion"; //valor q se muestra


                //Precargo los datos en los controles cuando apreto el btn "Modificar"
                //Para esto hago una validacion para saber si estos datos estan precargados:
                //=>Si el pokemon es distinto del nulo quiere decir q hay un pokemon
                //p/Modificar y lo tengo que precargar:
                if (pokemon != null)
                {
                    txtNumero.Text = pokemon.Numero.ToString();
                    txtNombre.Text = pokemon.Nombre;
                    txtDescripcion.Text = pokemon.Descripcion;
                    txtUrlImagen.Text = pokemon.UrlImagen;
                    //Hago que precargue la imagen cuando se abre la ventana de modificar
                    //para esto llamo al metodo que cree mas abajo "cargarImagen"
                    cargarImagen(pokemon.UrlImagen);

                    //Como los desplegables ya estan cargados
                    //=> quiero preseleccionar un valor cuando quiero modificar un pokemon
                    //SelectedValue -> Devuelve el valor q se puso mas arriba en "ValueMember"
                    cboTipo.SelectedValue = pokemon.Tipo.Id;
                    cboDebilidad.SelectedValue = pokemon.Debilidad.Id;

                }

            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.ToString());
            }

        }

        //EVENTO para cargar la imagen cuando estoy agregando un nuevo Pokemon
        private void txtUrlImagen_Leave(object sender, EventArgs e)
        {
            cargarImagen(txtUrlImagen.Text);
        }

        //METODO copiado de la clase "frmPokemon",
        //podria hacer una clase "helper" con este metodo centralizado
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

        
        //EVENTO para levantar una imagen de nuestra computadora
        //=> La imagen no va a estar en la DB sino que en una carpeta de la app
        //y la DB va a leer la imagen desde en esa carpeta.
        private void btnAgregarImagen_Click(object sender, EventArgs e)
        {
            //Para poder levantar una imagen de nuestra computadora
            //=> vamos a crear un ATRIBUTO bien arriba de esta clase.
            //"OpenFileDialog" -> Permite generar una ventana de dialogo q se va a abrir
            //                      y permite elegir un archivo.
            //=> El objeto "archivo" -> va a arrancar en nulo.
            //=> Al tocar el EVENTO  "btnAgregarImagen_Click" -> quiere decir que se agrego una 
            //                                                  imagen en el objeto "archivo". 
            archivo = new OpenFileDialog();
            //A esa ventana de dialogo que se abre, se le puede decir que tipos de archivo quiero q permita
            //Para decirle el tipo de archivo -> pongo el tipo de archivo q quiero => jpg|*.jpg y le agrego todos los tipos q quiero
            //=> Aca tengo filtrado que tipos de archivos me va a permitir ese cuadro de dialogo:
            archivo.Filter = "jpg|*.jpg|png|*.png";
            
            //Cuando abra la ventana "archivo.ShowDialog"
            //Si el resultado es "ok"
            //=> esto quiere decir q le di ok a la seleccion de imagen
            //y esto me va a permitir manejar el "archivo" que la ventana me permitio capturar: 
            if(archivo.ShowDialog() == DialogResult.OK)
            {
                //Aca le digo que en la caja de texto de la UrlImagen me guarde
                //la ruta completa del archivo q estoy seleccionando:
                txtUrlImagen.Text = archivo.FileName;
                //Para poder ver el archivo que estoy capturando, llamo al sgte metodo,
                //poniendo como parametro la ruta y el archivo completo de la imagen
                //seleccionada en la compu para que muestre la imagen:
                cargarImagen(archivo.FileName);
                //Hasta aca ya se puede buscar una imagen y que muestre en la app SIN guardar.

                //Guardo la imagen en una carpeta:
                //Para esto uso una clase estatica llamada "File"
                //(cuando pongo esta clase, bien arriba de esta clase, agrega la libreria "using System.IO;")
                //En el metodo "Copy" -> "archivo.FileName" -> 1er parametro: un archivo de origen, q le da la ruta y el archivo completo
                //                    -> "" -> 2do parametro: una ruta de destino
                //                    Creo una carpeta en el disco C de la compu xq todas las pc tienen esta ruta,
                //                    tmb puedo crear una ruta dentro del mismo directorio,
                //                    lo que no puedo hacer es poner la ruta donde esta la app,
                //                    q en mi caso esta en el disco D q no tiene todo el mundo.
                //2do parametro:
                //1-Creo una carpeta "poke-app" en el disco C de la compu (C:\poke-app\),
                //en una instalacion inicial de la app.
                //2-Voy a "app.config" q tiene una configuracion por defecto del framework,
                //donde se puede usar con conexion a DB tmb (Key="conexion-db" y value="aca pongo la conexion de la DB"; dsp levantar desde la app)
                //3-Pongo una configuracion propia de la app <appSettings>,
                //donde le agrego una clave (Key="poke-app",aca pongo un nombre q yo quiera)
                //y un value (value="C:\poke-app\", aca pongo la ruta donde va a guardar
                //las imagenes q va a leer dsp la DB).
                //4-En el 2do parametro -> Para no tener q poner la ruta del archivo por codigo,
                //sino q leer desde el archivo de configuracion "App.config" tengo que agregar
                //una Referencia en "Ensamblados", el q se llama "System.configuration".
                //"System.configuration" -> lo q hace es agregar la referencia a esa dll en la app.
                //5-Incluyo la libreria bien arriba de esta clase "Using System.configuration".
                //6-A partir de ahora puedo usar "ConfigurationManager.AppSettings[]" en el 2do parametro.
                //7-De este modo puedo leer las configuraciones que agregue en la "App.config".
                //8-Y asi se va a estar guardando la imagen en la ruta.
                //9-En el 2do parametro -> va la ruta + el nombre del archivo.
                //archivo.SafeFileName -> Nombre del archivo original.

                //File.Copy(archivo.FileName, ConfigurationManager.AppSettings["poke-app"] + archivo.SafeFileName);

                //=> Lo que hizo aca es copiar el archivo (la imagen seleccionada)
                //en la carpeta que cree en el disco "C". 
                //Como la idea es que guarde la imagen SOLO si puse aceptar
                //=>todo el renglon del "File.Copy..." voy a poner en el EVENTO "btnAceptar_Click"

                //MEJORAR cuando levanto un archivo de forma local:
                //-Decidir el nombre que se le pone a los archivos
                //(en este caso pone el mismo nombre q esta en la carpeta q saca la imagen)
                //-Si quiero cargar la misma imagen, como maneja esto, o sea, manejo de la unicidad.
                //-Si quiero modificar el pokemon y cambiar la imagen de este => tendria que pensar
                //en eliminar la imagen anterior => con la misma clase estatica File.Copy -> se puede usar File.Delete
                //File.Delete -> Hay que decir que si existe el archivo
                //File.Exists -> Buscar el archivo si es q existe => se puede preguntar si existe
                //la ruta con el archivo que le doy => si es asi, se elimina, para guardar uno nuevo
                //y no estar guardando archivos cuya ruta ya no van a estar mas en la DB xw se cambio.


            }



        }

        
    }
}
