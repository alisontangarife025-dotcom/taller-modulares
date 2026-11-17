using System;

class Program
{
   
    // DATOS DEL INVENTARIO 
    const int MAX_PRODUCTOS = 20;
    static string[] codigos = new string[MAX_PRODUCTOS];
    static string[] nombres = new string[MAX_PRODUCTOS];
    static double[] precios = new double[MAX_PRODUCTOS];
    static int[] existencias = new int[MAX_PRODUCTOS];
    static string[] categorias = new string[MAX_PRODUCTOS];
    static int cantidadProductos = 0;
    static double totalVendidoDia = 0;

    // DATOS ACADEMICOS 
    const int MAX_ESTUDIANTES = 25;
    static string[] codigosEst = new string[MAX_ESTUDIANTES];
    static string[] nombresEst = new string[MAX_ESTUDIANTES];
    static double[,] notas = new double[MAX_ESTUDIANTES, 3];
    static double[] asistencias = new double[MAX_ESTUDIANTES];
    static int cantidadEstudiantes = 0;

    
    // DATOS DE PRODUCCION (10 operarios x 5 dias)
    const int MAX_OPERARIOS = 10;
    const int DIAS = 5;
    static string[] codigosOp = new string[MAX_OPERARIOS];
    static string[] nombresOp = new string[MAX_OPERARIOS];
    static double[,] produccion = new double[MAX_OPERARIOS, DIAS];
    static bool[] diaRegistrado = new bool[DIAS];
    static int cantidadOperarios = 0;

    static void Main(string[] args)
    {
        int opcion;
        do
        {
            Console.WriteLine("\n===== TALLER DE ALGORITMOS MODULARES =====");
            Console.WriteLine("1. Sistema de inventario para cafeteria");
            Console.WriteLine("2. Sistema academico de notas");
            Console.WriteLine("3. Sistema de control de produccion");
            Console.WriteLine("0. Salir");
            opcion = LeerEntero("Seleccione una opcion: ", 0, 3);

            switch (opcion)
            {
                case 1:
                    MenuInventario();
                    break;
                case 2:
                    MenuAcademico();
                    break;
                case 3:
                    MenuProduccion();
                    break;
                case 0:
                    Console.WriteLine("Programa finalizado.");
                    break;
            }
        } while (opcion != 0);
    }

    // FUNCIONES Y PROCEDIMIENTOS GENERALES

    static int LeerEntero(string mensaje, int minimo, int maximo)
    {
        int valor;
        bool correcto;
        do
        {
            Console.Write(mensaje);
            correcto = int.TryParse(Console.ReadLine(), out valor);
            if (!correcto || valor < minimo || valor > maximo)
                Console.WriteLine("Dato invalido. Intente nuevamente.");
        } while (!correcto || valor < minimo || valor > maximo);

        return valor;
    }

    static double LeerDecimal(string mensaje, double minimo, double maximo)
    {
        double valor;
        bool correcto;
        do
        {
            Console.Write(mensaje);
            correcto = double.TryParse(Console.ReadLine(), out valor);
            if (!correcto || valor < minimo || valor > maximo)
                Console.WriteLine("Dato invalido. Intente nuevamente.");
        } while (!correcto || valor < minimo || valor > maximo);

        return valor;
    }

    static string LeerTexto(string mensaje)
    {
        string valor;
        do
        {
            Console.Write(mensaje);
            valor = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(valor))
                Console.WriteLine("El dato no puede quedar vacio.");
        } while (string.IsNullOrWhiteSpace(valor));

        return valor.Trim();
    }

    // CASO 1: INVENTARIO DE LA CAFETERIA

    static void MenuInventario()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- MENU DE INVENTARIO ---");
            Console.WriteLine("1. Registrar producto");
            Console.WriteLine("2. Realizar venta");
            Console.WriteLine("3. Actualizar inventario (agregar unidades)");
            Console.WriteLine("4. Mostrar productos agotados");
            Console.WriteLine("5. Mostrar total vendido del dia");
            Console.WriteLine("6. Mostrar todos los productos");
            Console.WriteLine("0. Volver al menu principal");
            opcion = LeerEntero("Seleccione una opcion: ", 0, 6);

            switch (opcion)
            {
                case 1: RegistrarProducto(); break;
                case 2: RealizarVenta(); break;
                case 3: ActualizarInventario(); break;
                case 4: MostrarAgotados(); break;
                case 5: MostrarTotalVendido(); break;
                case 6: MostrarProductos(); break;
            }
        } while (opcion != 0);
    }

    static void RegistrarProducto()
    {
        if (cantidadProductos == MAX_PRODUCTOS)
        {
            Console.WriteLine("No hay cupos disponibles para mas productos.");
            return;
        }

        string codigo = LeerTexto("Codigo: ");
        if (BuscarProducto(codigo) != -1)
        {
            Console.WriteLine("Ya existe un producto con ese codigo.");
            return;
        }

        codigos[cantidadProductos] = codigo;
        nombres[cantidadProductos] = LeerTexto("Nombre: ");
        precios[cantidadProductos] = LeerDecimal("Precio unitario: ", 0.01, 1000000000);
        existencias[cantidadProductos] = LeerEntero("Cantidad disponible: ", 0, 1000000);
        categorias[cantidadProductos] = LeerCategoria();

        cantidadProductos++;
        Console.WriteLine("Producto registrado correctamente.");
    }

    static string LeerCategoria()
    {
        int opcion;
        Console.WriteLine("Categoria: 1. Bebidas  2. Panaderia  3. Snacks");
        opcion = LeerEntero("Seleccione la categoria: ", 1, 3);

        if (opcion == 1) return "Bebidas";
        if (opcion == 2) return "Panaderia";
        return "Snacks";
    }

    // Funcion: devuelve la posicion del producto o -1 si no existe.
    static int BuscarProducto(string codigo)
    {
        for (int i = 0; i < cantidadProductos; i++)
        {
            if (codigos[i].Equals(codigo, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    static void RealizarVenta()
    {
        string codigo = LeerTexto("Codigo del producto que desea vender: ");
        int posicion = BuscarProducto(codigo);

        if (posicion == -1)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        int unidades = LeerEntero("Cantidad que desea vender: ", 1, 1000000);
        if (unidades > existencias[posicion])
        {
            Console.WriteLine("No hay suficientes unidades. Disponibles: " + existencias[posicion]);
            return;
        }

        double subtotal = precios[posicion] * unidades;
        existencias[posicion] -= unidades;
        totalVendidoDia += subtotal;

        Console.WriteLine("Venta realizada. Total: $" + subtotal.ToString("F2"));
    }

    static void ActualizarInventario()
    {
        string codigo = LeerTexto("Codigo del producto: ");
        int posicion = BuscarProducto(codigo);

        if (posicion == -1)
        {
            Console.WriteLine("Producto no encontrado.");
            return;
        }

        int unidades = LeerEntero("Unidades que desea agregar: ", 1, 1000000);
        existencias[posicion] += unidades;
        Console.WriteLine("Inventario actualizado. Nueva existencia: " + existencias[posicion]);
    }

    static void MostrarAgotados()
    {
        bool encontro = false;
        Console.WriteLine("\nPRODUCTOS AGOTADOS");
        for (int i = 0; i < cantidadProductos; i++)
        {
            if (existencias[i] == 0)
            {
                Console.WriteLine(codigos[i] + " - " + nombres[i]);
                encontro = true;
            }
        }

        if (!encontro)
            Console.WriteLine("No hay productos agotados.");
    }

    static void MostrarTotalVendido()
    {
        Console.WriteLine("Total vendido del dia: $" + totalVendidoDia.ToString("F2"));
    }

    static void MostrarProductos()
    {
        if (cantidadProductos == 0)
        {
            Console.WriteLine("Todavia no hay productos registrados.");
            return;
        }

        Console.WriteLine("\nCODIGO | NOMBRE | PRECIO | EXISTENCIAS | CATEGORIA");
        for (int i = 0; i < cantidadProductos; i++)
        {
            Console.WriteLine(codigos[i] + " | " + nombres[i] + " | $" +
                precios[i].ToString("F2") + " | " + existencias[i] + " | " + categorias[i]);
        }
    }

    // CASO 2: CONTROL ACADEMICO DE NOTAS

    static void MenuAcademico()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- MENU ACADEMICO ---");
            Console.WriteLine("1. Registrar estudiante y sus notas");
            Console.WriteLine("2. Mostrar definitivas y estados");
            Console.WriteLine("3. Mostrar promedio general del grupo");
            Console.WriteLine("4. Mostrar mejor y peor promedio");
            Console.WriteLine("0. Volver al menu principal");
            opcion = LeerEntero("Seleccione una opcion: ", 0, 4);

            switch (opcion)
            {
                case 1: RegistrarEstudiante(); break;
                case 2: MostrarDefinitivas(); break;
                case 3: MostrarPromedioGrupo(); break;
                case 4: MostrarMejorYPeor(); break;
            }
        } while (opcion != 0);
    }

    static void RegistrarEstudiante()
    {
        if (cantidadEstudiantes == MAX_ESTUDIANTES)
        {
            Console.WriteLine("Se alcanzo el limite de 25 estudiantes.");
            return;
        }

        string codigo = LeerTexto("Codigo del estudiante: ");
        if (BuscarEstudiante(codigo) != -1)
        {
            Console.WriteLine("Ese codigo ya esta registrado.");
            return;
        }

        codigosEst[cantidadEstudiantes] = codigo;
        nombresEst[cantidadEstudiantes] = LeerTexto("Nombre: ");

        for (int j = 0; j < 3; j++)
            notas[cantidadEstudiantes, j] = LeerDecimal("Nota parcial " + (j + 1) + " (0.0 a 5.0): ", 0, 5);

        asistencias[cantidadEstudiantes] =
            LeerDecimal("Porcentaje de asistencia (0 a 100): ", 0, 100);

        cantidadEstudiantes++;
        Console.WriteLine("Estudiante registrado correctamente.");
    }

    // Funcion: busca un estudiante por codigo.
    static int BuscarEstudiante(string codigo)
    {
        for (int i = 0; i < cantidadEstudiantes; i++)
        {
            if (codigosEst[i].Equals(codigo, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    // Funcion: calcula el promedio de las tres notas del estudiante.
    static double CalcularDefinitiva(int posicion)
    {
        return (notas[posicion, 0] + notas[posicion, 1] + notas[posicion, 2]) / 3.0;
    }

    // Funcion: determina el estado con los criterios definidos para este ejercicio.
    static string DeterminarEstado(double definitiva, double asistencia)
    {
        // Criterio asumido porque el enunciado no fija los rangos:
        // Aprobado: definitiva >= 3.0 y asistencia >= 80%.
        // Habilita: definitiva entre 2.0 y 2.99 y asistencia >= 80%.
        // Reprueba: definitiva < 2.0 o asistencia menor al 80%.
        if (asistencia < 80 || definitiva < 2.0)
            return "Reprueba";
        if (definitiva >= 3.0)
            return "Aprobado";
        return "Habilita";
    }

    static void MostrarDefinitivas()
    {
        if (cantidadEstudiantes == 0)
        {
            Console.WriteLine("No hay estudiantes registrados.");
            return;
        }

        Console.WriteLine("\nCODIGO | NOMBRE | DEFINITIVA | ASISTENCIA | ESTADO");
        for (int i = 0; i < cantidadEstudiantes; i++)
        {
            double definitiva = CalcularDefinitiva(i);
            string estado = DeterminarEstado(definitiva, asistencias[i]);
            Console.WriteLine(codigosEst[i] + " | " + nombresEst[i] + " | " +
                definitiva.ToString("F2") + " | " + asistencias[i].ToString("F1") +
                "% | " + estado);
        }
    }

    static void MostrarPromedioGrupo()
    {
        if (cantidadEstudiantes == 0)
        {
            Console.WriteLine("No hay estudiantes registrados.");
            return;
        }

        double suma = 0;
        for (int i = 0; i < cantidadEstudiantes; i++)
            suma += CalcularDefinitiva(i);

        Console.WriteLine("Promedio general del grupo: " +
            (suma / cantidadEstudiantes).ToString("F2"));
    }

    static void MostrarMejorYPeor()
    {
        if (cantidadEstudiantes == 0)
        {
            Console.WriteLine("No hay estudiantes registrados.");
            return;
        }

        int mejor = 0;
        int peor = 0;

        for (int i = 1; i < cantidadEstudiantes; i++)
        {
            if (CalcularDefinitiva(i) > CalcularDefinitiva(mejor))
                mejor = i;
            if (CalcularDefinitiva(i) < CalcularDefinitiva(peor))
                peor = i;
        }

        Console.WriteLine("Mejor promedio: " + nombresEst[mejor] + " - " +
            CalcularDefinitiva(mejor).ToString("F2"));
        Console.WriteLine("Peor promedio: " + nombresEst[peor] + " - " +
            CalcularDefinitiva(peor).ToString("F2"));
    }

    // CASO 3: CONTROL DE PRODUCCION

    static void MenuProduccion()
    {
        int opcion;
        do
        {
            Console.WriteLine("\n--- MENU DE PRODUCCION ---");
            Console.WriteLine("1. Registrar operarios");
            Console.WriteLine("2. Ingresar produccion de un dia");
            Console.WriteLine("3. Mostrar produccion semanal por operario");
            Console.WriteLine("4. Identificar mejor operario de la semana");
            Console.WriteLine("5. Mostrar promedio general de produccion");
            Console.WriteLine("6. Detectar dias de baja produccion");
            Console.WriteLine("0. Volver al menu principal");
            opcion = LeerEntero("Seleccione una opcion: ", 0, 6);

            switch (opcion)
            {
                case 1: RegistrarOperarios(); break;
                case 2: IngresarProduccionDiaria(); break;
                case 3: MostrarProduccionSemanal(); break;
                case 4: MostrarMejorOperario(); break;
                case 5: MostrarPromedioProduccion(); break;
                case 6: DetectarDiasBajaProduccion(); break;
            }
        } while (opcion != 0);
    }

    static void RegistrarOperarios()
    {
        if (cantidadOperarios == MAX_OPERARIOS)
        {
            Console.WriteLine("Ya se registraron los 10 operarios permitidos.");
            return;
        }

        int nuevos = LeerEntero("Cuantos operarios desea registrar? ",
            1, MAX_OPERARIOS - cantidadOperarios);

        for (int i = 0; i < nuevos; i++)
        {
            string codigo = LeerTexto("Codigo del operario: ");
            if (BuscarOperario(codigo) != -1)
            {
                Console.WriteLine("Codigo repetido. Ingrese nuevamente este operario.");
                i--;
                continue;
            }

            codigosOp[cantidadOperarios] = codigo;
            nombresOp[cantidadOperarios] = LeerTexto("Nombre: ");
            cantidadOperarios++;
        }

        Console.WriteLine("Registro de operarios terminado.");
    }

    static int BuscarOperario(string codigo)
    {
        for (int i = 0; i < cantidadOperarios; i++)
        {
            if (codigosOp[i].Equals(codigo, StringComparison.OrdinalIgnoreCase))
                return i;
        }
        return -1;
    }

    static void IngresarProduccionDiaria()
    {
        if (cantidadOperarios == 0)
        {
            Console.WriteLine("Primero debe registrar operarios.");
            return;
        }

        int dia = LeerEntero("Ingrese el dia (1 a 5): ", 1, 5) - 1;
        if (diaRegistrado[dia])
        {
            Console.WriteLine("Ese dia ya tiene datos. Se reemplazaran los valores anteriores.");
        }

        for (int i = 0; i < cantidadOperarios; i++)
        {
            produccion[i, dia] = LeerDecimal(
                "Produccion de " + nombresOp[i] + " en el dia " + (dia + 1) + ": ",
                0, 1000000000);
        }

        diaRegistrado[dia] = true;
        Console.WriteLine("Produccion del dia guardada.");
    }

    // Funcion: suma la produccion de los cinco dias para un operario.
    static double CalcularProduccionSemanal(int posicion)
    {
        double suma = 0;
        for (int j = 0; j < DIAS; j++)
            suma += produccion[posicion, j];

        return suma;
    }

    static int ContarDiasRegistrados()
    {
        int contador = 0;
        for (int j = 0; j < DIAS; j++)
        {
            if (diaRegistrado[j])
                contador++;
        }
        return contador;
    }

    static void MostrarProduccionSemanal()
    {
        if (cantidadOperarios == 0)
        {
            Console.WriteLine("No hay operarios registrados.");
            return;
        }

        Console.WriteLine("\nCODIGO | OPERARIO | PRODUCCION SEMANAL");
        for (int i = 0; i < cantidadOperarios; i++)
        {
            Console.WriteLine(codigosOp[i] + " | " + nombresOp[i] + " | " +
                CalcularProduccionSemanal(i).ToString("F2"));
        }

        if (ContarDiasRegistrados() < DIAS)
            Console.WriteLine("Aviso: faltan dias por registrar; los totales son parciales.");
    }

    static void MostrarMejorOperario()
    {
        if (cantidadOperarios == 0)
        {
            Console.WriteLine("No hay operarios registrados.");
            return;
        }

        int mejor = 0;
        for (int i = 1; i < cantidadOperarios; i++)
        {
            if (CalcularProduccionSemanal(i) > CalcularProduccionSemanal(mejor))
                mejor = i;
        }

        Console.WriteLine("Mejor operario: " + nombresOp[mejor] +
            " - Produccion: " + CalcularProduccionSemanal(mejor).ToString("F2"));

        if (ContarDiasRegistrados() < DIAS)
            Console.WriteLine("El resultado es parcial porque aun faltan dias.");
    }

    static void MostrarPromedioProduccion()
    {
        if (cantidadOperarios == 0)
        {
            Console.WriteLine("No hay operarios registrados.");
            return;
        }

        int dias = ContarDiasRegistrados();
        if (dias == 0)
        {
            Console.WriteLine("Primero debe ingresar la produccion de algun dia.");
            return;
        }

        double suma = 0;
        for (int i = 0; i < cantidadOperarios; i++)
        {
            for (int j = 0; j < DIAS; j++)
            {
                if (diaRegistrado[j])
                    suma += produccion[i, j];
            }
        }

        double promedio = suma / (cantidadOperarios * dias);
        Console.WriteLine("Promedio general por operario y por dia: " +
            promedio.ToString("F2"));
    }

    static void DetectarDiasBajaProduccion()
    {
        int dias = ContarDiasRegistrados();
        if (cantidadOperarios == 0 || dias == 0)
        {
            Console.WriteLine("Registre operarios y produccion antes de consultar.");
            return;
        }

        double totalSemana = 0;
        double[] totalesPorDia = new double[DIAS];

        for (int j = 0; j < DIAS; j++)
        {
            if (diaRegistrado[j])
            {
                for (int i = 0; i < cantidadOperarios; i++)
                {
                    totalesPorDia[j] += produccion[i, j];
                }
                totalSemana += totalesPorDia[j];
            }
        }

        double promedioDiario = totalSemana / dias;
        bool encontro = false;

        Console.WriteLine("Promedio de produccion total por dia: " +
            promedioDiario.ToString("F2"));

        for (int j = 0; j < DIAS; j++)
        {
            if (diaRegistrado[j] && totalesPorDia[j] < promedioDiario)
            {
                Console.WriteLine("Dia " + (j + 1) + ": " +
                    totalesPorDia[j].ToString("F2") + " (baja produccion)");
                encontro = true;
            }
        }

        if (!encontro)
            Console.WriteLine("No se detectaron dias por debajo del promedio.");
    }
}