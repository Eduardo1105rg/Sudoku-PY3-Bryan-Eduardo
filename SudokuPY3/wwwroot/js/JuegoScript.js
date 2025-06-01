//document.addEventListener('DOMContentLoaded', () => {

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function iniciarJuego(tamano) {
    document.getElementById("modal").style.display = "none"; // Ocultar modal
    await solicitar_matriz(tamano); // Generar el tablero dinámicamente

    // Iniciar el temporizador.
    iniciarTemporizador();
};

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function inicarNuevoJuego() {

    let duracion = detenerTemporizador();

    let tipoFinalizacion = "Abandono";

    // Realizar la finalizacion de la partida.
    finalizarPartidad(tipoFinalizacion, duracion);

    // Limpiar el tablero.
    let tabla = document.getElementById("tablero");
    tabla.innerHTML = ""; 

    // En esta parte iniciaria el proceso de guardar los datos de la partida.

    

    // desplegar el modal.
    document.getElementById("modal").style.display = "block";
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function reinicarJuego() {

    // Restauramos el tablero original.
    let tableroOrigen = JSON.parse(sessionStorage.getItem("TableroOriginal"));
    sessionStorage.setItem("TableroVolatil", JSON.stringify(tableroOrigen));

    // Recuperar el tamaño del tablero.
    let tamano = parseInt(sessionStorage.getItem("SizeTablero"), 10);

    // Agregar las opciones al select.
    generarOpcionesSelect(tamano);

    // Renderizar la matriz.
    generarTablero(tamano, tableroOrigen);


    // Volver a reiniciar el contador de sugerencias:

    sessionStorage.setItem("CantSugerencias", 0);

    // Reiniciar el temporizador

    detenerTemporizador();
    iniciarTemporizador();

    // Llamar a la funcion que reiniciara los datos de la partida en el controlador.
    enviarSolicitudDeReinicio();

}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function enviarSolicitudDeReinicio() {

    try {
        const response = await fetch('/Juego/ReiniciarPartida', {
            method: 'GET',

        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.json();

        return;


    } catch (error) {
        console.error("Error en la solicitud:", error);
    }



}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function verSugerencias() {
    // Revisar la cantidad de sugerencias restantes:
    let cantSugerencias = parseInt(sessionStorage.getItem("CantSugerencias"), 10);
    console.log("Can sugerencias: ", cantSugerencias);
    if (cantSugerencias != 5) {

        // Modificar el contador de susgerencias:
        sessionStorage.setItem("CantSugerencias", cantSugerencias + 1);



        // Solicitar las sugerencias al controlador.
        let datos_sugerencias = await solicitarSugerencias();

        // Recuperar el tamaño del tablero.
        let tamano = parseInt(sessionStorage.getItem("SizeTablero"), 10);

        // Guardar las matriz del jugador.
        sessionStorage.setItem("TableroVolatil", JSON.stringify(datos_sugerencias.matrizConSugerencias));

        // Renderizar el tablero.
        generarTablero(tamano, datos_sugerencias.matrizConSugerencias);

        // Esta parte se tendria que revisar las validaciones que volvieron en la consulta.


        let cantErrores = datos_sugerencias.cantErrores;

        let cantVacios = datos_sugerencias.cantVacios;

        let estadoJuego = datos_sugerencias.finalizado;




        // Mostrar los datos de las casillas.
        alert(`En el tablero actual hay ${cantErrores} campos erroneos y ${cantVacios} campos vacios.`);

        if (estadoJuego === 1) {

            // El juego ha finalizado.

            alert(`Juego finalizado, has completado el Sudoku.`);

            // registrar la finalizacion, esto deveria de ejecutarse despues de un tiempo.
            setTimeout(() => {
                let duracion = detenerTemporizador();
                let tipoFinalizacion = "Exitosa";

                // Realizar la finalización de la partida
                finalizarPartidad(tipoFinalizacion, duracion);

                // Limpiar el tablero.
                let tabla = document.getElementById("tablero");
                tabla.innerHTML = "";

                // Desplegar el modal.
                document.getElementById("modal").style.display = "block";

            }, 10000);

        }


    } else {
        alert("No se cuenta con sugerencias restantes.");
    }

    

}


/**
 * Nombre: solicitarSugerencias
 * 
 * Descripcion: Funcion fecht para solicitar las sugerencias en controlador de 'Juego', esta funcion hace un get a la ruta de la funcion de SolicitarSugerencias
 * 
 * Entradas: No posee.
 * 
 * Salidas: Los datos de las de la matriz con sugerencias y las validaciones respectivas.
 * 
 */
//async function solicitarSugerencias() {
//    try {
//        const response = await fetch('/Juego/SolicitarSugerencias', {
//            method: 'GET',

//        });

//        if (!response.ok) {
//            throw new Error(`Error HTTP: ${response.status}`);
//        }

//        const data = await response.json();
//        console.log("Matriz con sugerencias:", data);

//        // Devover los datos de la matriz recibidad.
//        return data;


//    } catch (error) {
//        console.error("Error en la solicitud:", error);
//    }

//}

async function solicitarSugerencias() {

    try {
        let tableroVolatil = JSON.parse(sessionStorage.getItem("TableroVolatil"));

        const dataToSend = JSON.stringify({ Tablero: tableroVolatil });
        console.log("Datos a enviar del tablero de la sugerencia a enviar:", dataToSend);

        const response = await fetch('/Juego/SolicitarSugerencias', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: dataToSend
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.json();
        console.log("Datos finales recibidos:", data);

        return data;

    } catch (error) {
        console.error('Error en la solicitud:', error);
    }


}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function verSolucion() {

    // Registrar la finalizacion de la partida.
    let duracion = detenerTemporizador();

    let tipoFinalizacion = "AutoSolucion";

    // Realizar la finalizacion de la partida.
    finalizarPartidad(tipoFinalizacion, duracion);


    // Restauramos el tablero original.
    let tableroOrigen = JSON.parse(sessionStorage.getItem("MatrizOrigen"));
    //sessionStorage.setItem("TableroVolatil", JSON.stringify(tableroOrigen));

    // Recuperar el tamaño del tablero.
    let tamano = parseInt(sessionStorage.getItem("SizeTablero"), 10);

    // Agregar las opciones al select.
    generarOpcionesSelect(tamano);

    // Renderizar la matriz.
    generarTablero(tamano, tableroOrigen);

    // Aqui mandariamos un mensaje para avisar que esta seria la solicion del Sodoku.
    alert("Juego terminado por autosolucion, en breve se iniciara un nuevo juego.");

    // Aqui se ejecutaria algo despues de unos 5 segundos para inicar un nuevo juego.

    setTimeout(() => {
        // Limpiar el tablero.
        let tabla = document.getElementById("tablero");
        tabla.innerHTML = "";

        // Desplegar el modal.
        document.getElementById("modal").style.display = "block";
    }, 10000); 

    //// En esta parte iniciaria el proceso de guardar los datos de la partida.







}


/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function solicitar_matriz(tamano) {
    try {
        const dataToSend = JSON.stringify({ TamanoMatriz: tamano });
        console.log("Datos a enviar:", dataToSend);

        const response = await fetch('/Juego/solicitarMatriz', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: dataToSend
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.json();
        console.log("Datos recibidos en la consulta:", data);

        let matrizResuelta = data.matrizResuelta;

        //console.log("Ttablero resuelto: ", matrizResuelta);

        let matrizJuego = data.matrizJuego;
        //console.log("Ttablero resuelto: ", matrizJuego);



        // Guardamos los datos del tamaño y la matriz para usarlos mas tarde.
        sessionStorage.setItem("MatrizOrigen", JSON.stringify(matrizResuelta));
        sessionStorage.setItem("TableroOriginal", JSON.stringify(matrizJuego));
        sessionStorage.setItem("TableroVolatil", JSON.stringify(matrizJuego));
        sessionStorage.setItem("SizeTablero", tamano);
        sessionStorage.setItem("CantSugerencias", 0);


        //let tableroOrigen = sessionStorage.getItem("TableroOriginal");
        //console.log("Tablero: ", tableroOrigen);

        // Agregar las opciones al select.
        generarOpcionesSelect(tamano);

        // Renderizar la matriz.
        generarTablero(tamano, matrizJuego);

        return;

    } catch (error) {
        console.error('Error en la solicitud:', error);
    }
}


/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function generarTablero(size, matriz) {
    let tabla = document.getElementById("tablero");
    tabla.innerHTML = ""; // Limpiar el tablero anterior

    let grupoSize = size === 9 ? 3 : 4; // Define tamaño de agrupación (3x3 en 9x9, 4x4 en 16x16)

    for (let i = 0; i < size; i++) {
        let fila = tabla.insertRow();
        for (let j = 0; j < size; j++) {
            let celda = fila.insertCell();
            let valor = matriz[i][j] === 0 ? "" : matriz[i][j]; // Si es 0, la celda queda vacia

            celda.innerHTML = `<span id="celda-${i}-${j}" onclick="seleccionarCelda(${i},${j})">${valor}</span>`;

            // Aplicar bordes oscuros en las agrupaciones
            celda.style.border = "1px solid black";
            celda.style.padding = "15px";
            celda.style.textAlign = "center";
            celda.style.fontSize = "18px";

            if (i % grupoSize === 0) celda.style.borderTop = "3px solid black";  
            if (j % grupoSize === 0) celda.style.borderLeft = "3px solid black";
            if ((i + 1) % grupoSize === 0) celda.style.borderBottom = "3px solid black";
            if ((j + 1) % grupoSize === 0) celda.style.borderRight = "3px solid black";
        }
    }
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function seleccionarCelda(fila, columna) {
    let celda = document.getElementById(`celda-${fila}-${columna}`);
    let nuevoValor = prompt("Ingrese un número:");
    celda.innerText = nuevoValor ? nuevoValor : celda.innerText; // Solo cambia si el usuario ingresa un valor
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function generarOpcionesSelect(size) {
    let filaSelect = document.querySelector("select[name='FilaSudoku']");
    let columnaSelect = document.querySelector("select[name='ColumnaSudoku']");
    let valorSelect = document.querySelector("select[name='ValorSudoku']");

    filaSelect.innerHTML = "";
    columnaSelect.innerHTML = "";
    valorSelect.innerHTML = "";

    for (let i = 1; i <= size; i++) {
        filaSelect.innerHTML += `<option value="${i}">${i}</option>`;
        columnaSelect.innerHTML += `<option value="${i}">${i}</option>`;
    }

    for (let i = 1; i <= size; i++) {
        valorSelect.innerHTML += `<option value="${i}">${i}</option>`;
    }
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function realizarMovimiento() {
    let fila = document.querySelector("select[name='FilaSudoku']").value;
    let columna = document.querySelector("select[name='ColumnaSudoku']").value;
    let valor = document.querySelector("select[name='ValorSudoku']").value;

    if (!fila || !columna || !valor) {
        alert("Seleccione todos los valores antes de realizar el movimiento.");
        return;
    }

    //let celda = document.getElementById(`celda-${fila}-${columna}`);
    //celda.innerText = valor;

    enviarMovimiento(fila, columna, valor);

    //console.log(`Movimiento realizado en (${fila}, ${columna}): ${valor}`);
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
async function enviarMovimiento(fila, columna, valor) {

    try {
        let tableroVolatil = JSON.parse(sessionStorage.getItem("TableroVolatil"));
        let matrizOrigen = JSON.parse(sessionStorage.getItem("MatrizOrigen"));

        console.log("Tablero a enviar: ", tableroVolatil, "\n");

        const dataToSend = JSON.stringify({ Fila: fila, Columna: columna, Valor: valor, Tablero: tableroVolatil, MatrizOrigen: matrizOrigen });
        //console.log("Datos a enviar de la jugada:", dataToSend);

        const response = await fetch('/Juego/jugadaSudoku', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: dataToSend
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.json();
        console.log("Datos recibido de la jugada:", data);


        // Optener los datos de la respuestas.
        let tableroJuego = data.tablero;

        let cantErrores = data.cantErrores;

        let cantVacios = data.cantVacios;

        let estadoJuego = data.finalizado;


        //// Guardamos los datos del tamaño y la matriz para usarlos mas tarde.

        sessionStorage.setItem("TableroVolatil", JSON.stringify(tableroJuego));



        //// Recuperar el tamaño del tablero.
        let tamano = parseInt(sessionStorage.getItem("SizeTablero"), 10);

        //// Renderizar la matriz.
        generarTablero(tamano, tableroJuego);


        // Mostrar los datos de las casillas.
        alert(`En el tablero actual hay ${cantErrores} campos erroneos y ${cantVacios} campos vacios.`);

        if (estadoJuego === 1) {

            // El juego ha finalizado.

            alert(`Juego finalizado, has completado el Sudoku.`);

            // registrar la finalizacion, esto deveria de ejecutarse despues de un tiempo.
            setTimeout(() => {
                let duracion = detenerTemporizador();
                let tipoFinalizacion = "Exitosa";

                // Realizar la finalización de la partida
                finalizarPartidad(tipoFinalizacion, duracion);


                // Limpiar el tablero.
                let tabla = document.getElementById("tablero");
                tabla.innerHTML = "";

                // Desplegar el modal.
                document.getElementById("modal").style.display = "block";

            }, 10000); 

        }


    } catch (error) {
        console.error('Error en la solicitud:', error);
    }



}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function limpiarCasilla() {
    let fila = document.querySelector("select[name='FilaSudoku']").value;
    let columna = document.querySelector("select[name='ColumnaSudoku']").value;

    if (!fila || !columna) {
        alert("Seleccione una fila y columna para limpiar la casilla.");
        return;
    }

    //console.log(`Casilla limpiada en (${fila}, ${columna})`);

    // Logica para editar la matriz cuando se elimina una casilla.
    let tableroVolatil = JSON.parse(sessionStorage.getItem("TableroVolatil"));
    let tableroConCeros = JSON.parse(sessionStorage.getItem("TableroOriginal"));

    let valor = tableroConCeros[fila][columna];

    if (valor === 0) {
        tableroVolatil[fila][columna] = 0; // Modificarla con un cero en esa posicion

        sessionStorage.setItem("TableroVolatil", JSON.stringify(tableroVolatil)); // Volverla a guardar.

        // Limpiar esa casilla del tablero.
        let celda = document.getElementById(`celda-${fila}-${columna}`);
        celda.innerText = "";

    } else { // Si en la original no havia un cero entonces no se puede modificar.
        alert("No se puede borrar esta casilla debido a que esta es una de las casillas por defecto.");

    }

}



let tiempoInicio;
let intervalo;

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function iniciarTemporizador() {
    console.log("Temporizador iniciado");
    tiempoInicio = Date.now(); // Guardamos el tiempo de inicio
    intervalo = setInterval(actualizarTiempo, 1000);
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function actualizarTiempo() {
    if (!tiempoInicio) {
        console.error("El temporizador no ha iniciado correctamente.");
        return;
    }

    let tiempoActual = Math.floor((Date.now() - tiempoInicio) / 1000); // 🔹 Calculamos la diferencia en segundos
    document.getElementById("temporizador").innerText = `Tiempo: ${tiempoActual} s`;
}

/**
 * Nombre:
 * 
 * Descripcion:
 * 
 * Entradas:
 * 
 * Salidas:
 * 
 */
function detenerTemporizador() {
    clearInterval(intervalo); // Detener el temporizador
    let tiempoTotal = Math.floor((Date.now() - tiempoInicio) / 1000); // Obtener duración total
    sessionStorage.setItem("TiempoDuracion", tiempoTotal);
    console.log("Tiempo total de juego:", tiempoTotal, "segundos");

    return tiempoTotal;
}


/**
 * Nombre: finalizarPartidad
 * 
 * Descripcion: Funcion para enviar los datos de finalizacion de la partidad al controlador de la ventana.
 * 
 * Entradas:
 * 
 * Salidas: No posee.
 * 
 */
async function finalizarPartidad(tipoFinalizacion, duracion) {

    try {
        let tableroVolatil = JSON.parse(sessionStorage.getItem("TableroVolatil"));

        let cantSugerencias = parseInt(sessionStorage.getItem("CantSugerencias"), 10);

        //console.log("Tablero a enviar: ", tableroVolatil, "\n");

        const dataToSend = JSON.stringify({ TipoFinalizacion: tipoFinalizacion, Tablero: tableroVolatil, SugerenciasUtilizadas: cantSugerencias, Duracion: duracion });
        console.log("Datos a enviar de la finalizacion:", dataToSend);

        const response = await fetch('/Juego/FinalizacionPartida', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json'
            },
            body: dataToSend
        });

        if (!response.ok) {
            throw new Error(`Error HTTP: ${response.status}`);
        }

        const data = await response.json();
        console.log("Datos finales recibidos:", data);

        return; 

    } catch (error) {
        console.error('Error en la solicitud:', error);
    }



}


//});