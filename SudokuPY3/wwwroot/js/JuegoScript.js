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

    // Limpiar el tablero.
    let tabla = document.getElementById("tablero");
    tabla.innerHTML = ""; 

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

}


//async function solicitar_matriz(tamano) {
//    try {
//        const dataToSend = JSON.stringify({ TamanoMatriz: tamano });
//        console.log("Datos a enviar:", dataToSend);

//        const respuesta = await fetch('/Juego/solicitarMatriz', {
//            method: 'POST',
//            headers: {
//                'Content-Type': 'application/json'
//            },
//            body: dataToSend
//        });

//        console.log("Esperando la respuesta..");
//        if (respuesta.ok) {
//            const resultado = await respuesta.json();
//            console.log("Respuesta recibida:", resultado);

//            if (resultado.matriz) {  // <-- Cambia de "matriz" a "Matriz"
//                console.log("Matriz recibida correctamente:", resultado.matriz);

//                // Recorremos la matriz correctamente
//                resultado.matriz.forEach(fila => {
//                    console.log("Fila:", fila);
//                });

//            } else {
//                console.error('Error en la respuesta:', resultado.mensaje);
//            }
//        } else {
//            console.log("La respuesta no fue la esperada, es erronea..");
//        }

//    } catch (error) {
//        console.error('Error en la solicitud:', error);
//    }
//}

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
        console.log("Matriz recibida:", data);

        // Guardamos los datos del tamaño y la matriz para usarlos mas tarde.
        sessionStorage.setItem("TableroOriginal", JSON.stringify(data));
        sessionStorage.setItem("TableroVolatil", JSON.stringify(data));
        sessionStorage.setItem("SizeTablero", tamano);


        let tableroOrigen = sessionStorage.getItem("TableroOriginal");
        console.log("Tablero: ", tableroOrigen);

        // Agregar las opciones al select.
        generarOpcionesSelect(tamano);

        // Renderizar la matriz.
        generarTablero(tamano,data);

    } catch (error) {
        console.error('Error en la solicitud:', error);
    }
}

//function generarTablero(size, data) {
//    let tabla = document.getElementById("tablero");
//    tabla.innerHTML = ""; // Limpiar el tablero anterior

//    for (let i = 0; i < size; i++) {
//        let fila = tabla.insertRow();
//        for (let j = 0; j < size; j++) {
//            let celda = fila.insertCell();
//            celda.innerHTML = `<span id="celda-${i}-${j}" onclick="seleccionarCelda(${i},${j})">0</span>`;
//            celda.style.border = "1px solid black";
//            celda.style.padding = "10px";
//            celda.style.textAlign = "center";
//        }
//    }
//};

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

    for (let i = 0; i < size; i++) {
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

    let celda = document.getElementById(`celda-${fila}-${columna}`);
    celda.innerText = valor;

    console.log(`Movimiento realizado en (${fila}, ${columna}): ${valor}`);
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

    let celda = document.getElementById(`celda-${fila}-${columna}`);
    celda.innerText = "";

    console.log(`Casilla limpiada en (${fila}, ${columna})`);
}



//});