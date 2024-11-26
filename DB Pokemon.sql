

--Esta es una consulta general que en visual estudio no puedo poner, tiene que ser de otra forma.
Select * From ELEMENTOS

Select * From POKEMONS

--ESTO VA PARA LA CLASE "PokemonNegocio" de VS:
--=> Si quiero pedir todos los datos, tengo que poner todas las columnas.
--=> Sino pongo especialmente las columnas que quiero obtener
-- En este caso quiero obtener: Numero, Nombre, Descripcion:
Select Numero, Nombre, Descripcion From POKEMONS
--=> copio esta consulta al código de VS.

--Para obtener la url de la imagen tambien, agrego a la consulta anterior la url de la imagen de cada Pokemon
Select Numero, Nombre, Descripcion, UrlImagen From POKEMONS
--=> reemplazo esta consulta con la anterior en el código de VS.

--Borro la imagen del Pidgey para ver agregar una excepcion cuando falte una imagen
update POKEMONS set UrlImagen = '' where Numero = 15 

--Quiero traer la información del Pokemon: Numero, Nombre, Descripcion, UrlImagen (de la tabla POKEMONS) 
--y su tipo (de la tabla ELEMENTOS)
--Para esto hago una consulta con relación entre tablas o una consulta joineada:
Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo From POKEMONS P, ELEMENTOS E where P.IdTipo = E.Id

--Quiero traer la información del Pokemon (tabla POKEMONS), su tipo (tabla ELEMENTOS) y su debilidad (tabla ELEMENTOS)
--Para esto hago una consulta con relación entre tablas, como la consulta anterior,
--PERO como la debilidad sale de la misma tabla de tipo, para poder relacionar tengo que poner 
--a la tabla elementos con otro nombre para que no confunda los Id, 
--xq para la debilidad tengo q relacionar los mismos id q con el tipo
Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad From POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id AND P.IdDebilidad = D.Id

--ESTO VA PARA LA CLASE "ElementoNegocio" de VS:
--Quiero traer la información de la tabla de ELEMENTOS, el Id y la Descripcion:
Select Id, Descripcion From ELEMENTOS

--SEGUIMOS CON LO QUE VA PARA LA CLASE "PokemonNegocio" de VS:
--Hago una consulta de insercion en la DB (en SQL)
---Una forma mas corta PERO que obliga a respetar TODAS las columnas que tiene la tabla:
insert into POKEMONS values(1, '', '', '', 1, 1, 1, 1)
---Otra forma es pasandole las columnas que quiero:
insert into POKEMONS(Numero, Nombre, Descripcion, Activo)values(1, '', '', 1)

--Para que me muestre el Pokemon Carterpie a la app, agrego los datos que estan nulos (UrlImagen, IdTipo, IdDebilidad):
update POKEMONS set IdTipo = 1, IdDebilidad = 2, UrlImagen = '' where Id = 5

--Para que me muestre el Pokemon Squirtle a la app, agrego los datos que estan nulos (UrlImagen):
update POKEMONS set UrlImagen = '' where Id = 6

--Quiero llevar a VS la informacion tambien del IdTipo y IdDebilidad
Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad from POKEMONS P, ELEMENTOS E, ELEMENTOS D where P.IdTipo = E.Id AND P.IdDebilidad = D.Id

--Quiero llevar a VS la informacion que quiero modificar y ajusto en el VS
update POKEMONS set Numero = 1, Nombre = '', Descripcion = '', UrlImagen = '', IdTipo = 1, IdDebilidad = 1 where Id = 1

--Quiero llevar a VS la informacion que quiero eliminar y ajusto en el VS (ELIMINACION FISICA):
delete from POKEMONS where id = 11

--Quiero llevar a VS la informacion que quiero eliminar y ajusto en el VS (ELIMINACION LOGICA):
--Esta vez uso UPDATE pq quiero cambiar la columna Activo(1) como Inactivo(0)
update POKEMONS set Activo = 0 where id = 1

--Si todos los Pokemones esta INACTIVOS (Activo = 0) 
--=> puedo volver a activar TODOS de esta forma: 
--(si quiero activar uno en espacial nomas, le agrego a lo ultimo where Id = nro del Id del Pokemon)
update POKEMONS set Activo = 1 

--Quiero llevar a VS la informacion de la tabla POKEMONS y filtros dsp del "where":
---Para hacer busquedas en SQL por string se utiliza la palabra clave "like"
---=> Para busqueda exacta -> like 'Pikachu'
---=> Para busqueda con letras q contenga -> like '%ch%'
---=> Para busqueda q comience con alguna letra -> like 'pi%'
---=> Para busqueda q termine con alguna letra -> like '%chu'
Select P.Numero, P.Nombre, P.Descripcion, P.UrlImagen, E.Descripcion Tipo, 
D.Descripcion Debilidad, P.IdTipo, P.IdDebilidad, P.Id 
from POKEMONS P, ELEMENTOS E, ELEMENTOS D 
where P.IdTipo = E.Id 
AND P.IdDebilidad = D.Id 
AND P.Activo = 1
AND --Aca le agrego con el "SWITCH" o el "IF" en VS


