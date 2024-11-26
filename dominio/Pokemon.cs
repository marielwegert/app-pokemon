using System;
using System.Collections.Generic;
using System.ComponentModel; //Esta libreria sirve para las ANNOTATIONS
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace dominio
{
    public class Pokemon
    {
        //CLASE BASE "POKEMON" -> Da el formato del objeto a manipular en la app.
        //Define el modelo de mi clase y el objeto que voy a utilizar.

        //No pq tenga en la DB la tabla POKEMONS y la tabla ELEMENTOS
        //=> tengo que tener si o si las clases "pokemons" y "elementos"
        //en este caso SI funciona pero no en todos los casos tiene q ser asi.

        //"IdTipo", "IdDebilidad" y "IdEvolucion" -> NO van a ser propiedades
        //de la clase SINO que son relaciones entre objetos con la característica
        //"tiene", con la relación de asociación/composición xq un objeto nace
        //de tal Tipo y nace teniendo ciertas Debilidades.

        //Propiedades la clase
        public int Id { get; set; }
        [DisplayName("Número")] //Annotation -> Pongo arriba del atributo para q cambie el nombre de la columna
        public int Numero { get; set; }
        public string Nombre { get; set; }
        [DisplayName("Descripción")]
        public string Descripcion { get; set; }

        //Agregamos la propiedad UrlImagen para poder obtener ese valor (la url) 
        public string UrlImagen { get; set; }

        //Propiedad -> El Tipo del Pokemon es un objeto del tipo Elemento
        //La propiedad ésta NO es una Composición xq no creé antes un constructor.
        //En el caso que cree un constructor, pase por parámetro, al momento
        //de crear el Pokemon, el Tipo del Pokemon
        //=> la Composición sería -> El Pokemón TIENE un Elemento, nace con este.
        public Elemento Tipo { get; set; }

        //Propiedad "Debilidad" pasa lo mismo que con Propiedad "Tipo"
        public Elemento Debilidad { get; set; }

        //ANNOTATIONS -> Sirve para validaciones, formato de fecha, darle un nombre a la columna


    }
}
