//document.addEventListener('DOMContentLoaded', () => {

//Este seria para inciar el juego
async function iniciarJuego(tamano) {
    document.getElementById("modal").style.display = "none"; // Ocultar modal
    await solicitar_matriz(tamano); // Generar el tablero dinámicamente
};


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
        //mostrarMatriz(data); // Aquí puedes procesar la matriz

    } catch (error) {
        console.error('Error en la solicitud:', error);
    }
}

function generarTablero(size) {
    let tabla = document.getElementById("tablero");
    tabla.innerHTML = ""; // Limpiar el tablero anterior

    for (let i = 0; i < size; i++) {
        let fila = tabla.insertRow();
        for (let j = 0; j < size; j++) {
            let celda = fila.insertCell();
            celda.innerHTML = `<span id="celda-${i}-${j}" onclick="seleccionarCelda(${i},${j})">0</span>`;
            celda.style.border = "1px solid black";
            celda.style.padding = "10px";
            celda.style.textAlign = "center";
        }
    }
};


//});