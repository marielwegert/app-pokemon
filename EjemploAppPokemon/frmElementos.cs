using dominio;
using negocio;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace winform_app
{
    public partial class frmElementos : Form
    {
        //ATRIBUTO PRIVADO q al principio no va a tener nada, mas abajo lleno 
        private List<Elemento> ListaElemento;
        
        //CONSTRUCTOR
        public frmElementos()
        {
            InitializeComponent();
        }


        //EVENTO de carga de formulario: Aca voy a invocar la lectura a DB
        private void frmElementos_Load(object sender, EventArgs e)
        {
            //Hago todo lo mismo que hice con "PokemonNegocio" para "ElementoNegocio":
            //OBJETO de PokemonNegocio
            ElementoNegocio elemento = new ElementoNegocio();
            
            ListaElemento = elemento.listar();
            dgvElementos.DataSource = ListaElemento;
        }
    }
}
