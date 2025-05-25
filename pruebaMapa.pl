:- use_module(library(clpfd)). % Libreria para el sudoku
:- use_module(library(random)). % Libreria para los numeros aleatorios


prueba :-
    sudoku_con_pistas(S, R),     
    writeln('Matriz Resuelta:'), maplist(writeln, R),
    writeln('Matriz con ceros (pistas):'), maplist(writeln, S).

% Asi se prueba sudoku_con_pistas(S), maplist(writeln, S).

% aqui estoy definiendo una regla de como tienen que ser sus filas y sus columnas
sudoku(MatrizSudoku, ListaSudoku) :-  % CAMBIADO a sudoku/2
    length(MatrizSudoku, 9), % Creo una lista llamada filas con 9 filas valga la redundancia
    maplist(same_length(MatrizSudoku), MatrizSudoku), % Aquí hacemos que cada elemento de la fila tenga 9 elementos
    append(MatrizSudoku, ListaSudoku), % pasamos la lista de listas a una sola lista como tal
    ListaSudoku ins 1..9, % Cada valor de esa lista solo puede tener un elemento del 1 al 9
    maplist(all_distinct, MatrizSudoku), % Todas las listas deben tener valores distintos o sea no se pueden repetir
    transpose(MatrizSudoku, Columns), % Convertimos las filas en columnas
    maplist(all_distinct, Columns), % Repetimos que ninguna columna pueda repetir sus valores
    revisa_bloque(MatrizSudoku), % Lo pasamos a un bloque lo cual hace que recorre de 3x3
    asignaValores(ListaSudoku).  % Le damos valores aleatorios

% Aqui lo recorremos de 3 filas en 3 para ir haciendo los bloques
revisa_bloque([]).
revisa_bloque([Row1, Row2, Row3 | Rest]) :- % Extraemos las primeras 3 filas
    revisa_bloque_3x3(Row1, Row2, Row3), % Aqui se las pasamos al bloque de 3x3 para que revise si los valores son distintos
    revisa_bloque(Rest). % Se vuelve a llamar de forma recursiva hasta quedar vacia

% de una forma mas resumida esta va revisando la lista para que se cumpla que cada bloque de 3 filas y 3 columnas no repita valores
revisa_bloque_3x3([], [], []).
revisa_bloque_3x3([A,B,C | F1], [D,E,F | F2], [G,H,I | F3]) :- % Una vez obtenida las 9 filas comienza a observar si sus valores son distintos
    all_distinct([A,B,C,D,E,F,G,H,I]), % Aqui compara los valores de cada uno
    revisa_bloque_3x3(F1, F2, F3). % Se vuelve a llamar por cada fila de forma recursiva hasta quedar vacio

asignaValores([]).  % recibimos una lista aleatoria 
asignaValores([Var|ListaSudoku]) :- % Var es la cabeza y ListaSudoku la cola
    findall(Val, (between(1,9,Val)), Lista), % le decimos que asigne todo los valores del 1 al 9 en y que los guarde en lista 
    random_permutation(Lista, ListaAleatoria), % Aqui le decimos que los mezclen de aleatoriamente
    member(Var, ListaAleatoria), % si cumple las reglas asigna el valor sino intenta con los demas
    asignaValores(ListaSudoku). % se llama de forma recursiva

% Esta funcion es para que asigne un valor aleatorio en las pistas siguiendo lo que dijo el profe de minimo 17 y maximo 25 
asignar_pistas(NumPistas) :-
    random_between(17, 25, NumPistas).

% Esta funcion es para que asigne un valor aleatorio en las sugerencias siguiendo lo que dijo se pide que la pista sea aleatoria
% Pide sugerencia solo en una posición donde haya un 0
asignar_sugerencia(ListaSudokuConCeros, Pos) :-
    random_between(0, 80, Pos),
    nth0(Pos, ListaSudokuConCeros, P1),
    ( P1 =:= 0 -> true ; asignar_sugerencia(ListaSudokuConCeros, Pos) ).



% Cuando la posición que queremos es la primera
% se tiene que mandar así  generar_sugerencia(ListaSudokuConCeros, ListaSudokuSolucion, ListaSudokuCerosActualizado, Posicion).
generar_sugerencia([0|T], [H1|_], [H1|T], 1).
generar_sugerencia([X|T], [_|_], [X|T], 1) :- X \= 0.  % Si en la posicion que estamos no es igual a cero seguimos
generar_sugerencia([H|T], [H1|T1], [H|T2], Posicion) :-
    Posicion > 1,
    Pos1 is Posicion - 1,
    generar_sugerencia(T, T1, T2, Pos1).


posicion_aleatoria(Pos) :-
    posicion_aleatoria(Pos, []).  % Inicia con lista vacía de posiciones usadas

posicion_aleatoria(Pos, Usadas) :-
    random_between(0, 8, F),      
    random_between(0, 8, C),      
    Pos is F * 9 + C,             
    \+ member(Pos, Usadas).       

% Caso recursivo: si la posición ya está usada, reintenta
posicion_aleatoria(Pos, Usadas) :-
    posicion_aleatoria(Pos, Usadas).  % Backtracking para generar otra posición

% Esta es la regla para poner 0 en la posicion que indicamos
% En general en nuevo es el valor o sea 0 y en Pos donde vamos a querer que se cambie el valor lo que hace es 
% ir bajando hasta llegar a la posicion que se quiere cambiar
actualizar_lista([_|T], 1, Nuevo, [Nuevo|T]).
actualizar_lista([H|T], Pos, Nuevo, [H|Resto]) :-
    Pos > 1,
    Pos1 is Pos - 1,
    actualizar_lista(T, Pos1, Nuevo, Resto).


% Aqui es donde haremos que se inserten los 0 dependiendo de la cantidad de pistas que se tengan
insertar_pistas(N, Lista, Resultado) :-
    insertar_pistas(N, Lista, [], Resultado). % Llamamos a la que guarda las posiciones usadas

insertar_pistas(N, Lista, Usadas, Resultado) :-
    ( N = 0 -> Resultado = Lista
    ; posicion_aleatoria(Pos, Usadas),
      actualizar_lista(Lista, Pos+1, 0, ListaActualizada),
      N1 is N - 1,
      insertar_pistas(N1, ListaActualizada, [Pos|Usadas], Resultado)
    ).

% Hacemos la lista como una matriz pues cortamos por fila y las hacemos sublistas con 9 elementos
crear_matriz([], []).
crear_matriz(Lista, [Fila|Resto]) :-
    length(Fila, 9),
    append(Fila, RestoLista, Lista),
    crear_matriz(RestoLista, Resto).

% primero generamos el tablero original del sudoku completo, luego llamamos a asignarPistas para que nos dé la cantidad corredta
% por ultimo insertamos las pistas y generamos el otro mapa de sudoku
sudoku_con_pistas(MatrizSudokuConCeros, MatrizResuelta) :-
    sudoku(MatrizResuelta, ListaSudoku),
    asignar_pistas(N),
    writeln('Número de pistas generadas (N):'), writeln(N),
    insertar_pistas(N, ListaSudoku, ListaSudokuConCeros),
    crear_matriz(ListaSudokuConCeros, MatrizSudokuConCeros).

% Ahora, para hacer la sugerencia (ejemplo):
hacer_sugerencia(MatrizSudokuConCeros, MatrizSudokuSolucion, MatrizSudokuActualizado, MatrizSudokuActualizadoDividido) :-
    append(MatrizSudokuConCeros, ListaSudokuConCeros),  % Aplanar la matriz
    append(MatrizSudokuSolucion, ListaSudokuSolucion), % Aplanar la solución
    asignar_sugerencia(ListaSudokuConCeros, Pos),
    Pos1 is Pos + 1,
    generar_sugerencia(ListaSudokuConCeros, ListaSudokuSolucion, ListaSudokuActualizado, Pos1),
    crear_matriz(ListaSudokuActualizado, MatrizSudokuActualizadoDividido),
    MatrizSudokuActualizado = MatrizSudokuActualizadoDividido.





revisa_matriz([], []).
revisa_matriz([H|T], [He|Ta]) :-
    H =:= He,              
    revisa_matriz(T, Ta).  



% Esto es para cuando el usuario me de la fila y la columna calcular la posicion en la que voy a necesitar que se cambie
recibe_posicion(F,C, PosicionU):-
    PosicionU is F * 9 + C.




% Aqui lo trabajo en posicion que comienza en 1 o sea fila 1 columna 1

puede_insertar(Matriz, Fila, Col, Valor, Resultado) :-
    between(1, 9, Fila), between(1, 9, Col),     
    between(1, 9, Valor),                         
    flatten(Matriz, ListaPlana),                  
    Pos is (Fila-1)*9 + (Col-1),                 
    

    (   nth0(Pos, ListaPlana, 0)                 
    ->  (   var(Resultado)                       
         -> intercambio_valor(Pos, ListaPlana, Valor, ListaModificada),
            crear_matriz(ListaModificada, Resultado)
         ;  Resultado == true                    
        )
    ;   false                                    
    ).


% Esto todavia no lo usamos 
asignar_valor(Lista, Fila, Col, Valor, ListaModificada) :-
    between(1, 9, Fila), between(1, 9, Col),      
    Pos is (Fila-1)*9 + (Col-1),                  
    intercambio_valor(Pos, Lista, Valor, ListaModificada). 


% Entradas: Indice, Lista, ValorCambio, ListaCambiada
% Salidas: la lista cambiada
% Funcionamiento: En si agarra una lista y dependiendo del indice las corta por la mitad dejando solo al indice por fuera
% de forma que luego con la nueva lista agrega los dos que corto mas el nuevo valor de forma que se agrega a esa nueva lista
% como si fuera un reemplazo 
% Restricciones: No aplica
intercambio_valor(Indice, Lista, ValorCambio, ListaCambiada) :-
    length(Sublista, Indice),
    append(Sublista, [_|Resto], Lista),
    append(Sublista, [ValorCambio|Resto], ListaCambiada).



pistas_en_matriz(ListaSudoku, MatrizSudokuConCeros) :-
    asignar_pistas(N),
    insertar_pistas(N, ListaSudoku, ListaSudokuConCeros),
    crear_matriz(ListaSudokuConCeros, MatrizSudokuConCeros).