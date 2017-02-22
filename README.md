# UProtector

Es una libreria creada con Microsoft .NET v4.0 y compilada con el nombre `JclunaOficial.UProtector.dll`, el cual contiene funciones que permiten generar contraseñas aleatorias, así como encriptar y desencriptar strings o arreglos de bytes mediante el algoritmo Rijndael.

Las funciones son publicas (`public`) y compartidas (`static`) para que se puedan consumir sin necesidad de crear una instancia de la clase `UProtector`; adicionalmente se pueden accesar a las funciones como metodos extendidos (`extension method`) con los tipos de datos `string` y `byte[]`.

### Requerimientos

* **Microsoft Visual Studio Community 2015**, _de preferencia trabajar con la actualización más reciente del IDE_.
* **Microsoft .NET v4.0+**, _seguramente incluido en VS2015 pero probablemente se quiera compilar usando la linea de comandos_.
* **Xamarin Studio**, _en caso de querer compilar en otra plataforma (Linux o Mac), aunque el compilado con Windows se puede consumir en otras plataformas_.

El proyecto esta creado con VS2015 Community y .NET v4.0, pero se pueden copiar los bytes (código) a otros IDEs para compilar y generar la libreria, prosupuesto configurando los ajustes necesarios para tener el producto final.

### Compilado

Si solo quieres la versión compilada de la libreria, entonces la puedes descargar haciendo [click aquí](https://www.dropbox.com/sh/q63joylv24spg39/AAAnbXqRnGNJ-SJGRvodZu97a?dl=0 "Dropbox"). Vas a encontrar todas las versiones que se han generado (Release y Debug); recomiendo uses la más reciente.
