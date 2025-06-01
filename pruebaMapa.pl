:- use_module(library(clpfd)). % Libreria para el sudoku
:- use_module(library(random)). % Libreria para los numeros aleatorios




% Entrada: No tiene
% Salidas: La matriz resulta y la matriz con pistas
% Restricciones: No tiene
% Funcionamiento: Llama a sudoku_con_pistas para generar una matriz con pistas y lo que hace es imprimir la matriz 
% original y la matriz resulta de una vez.
prueba :-
    sudoku_con_pistas(S, R),     
    writeln('Matriz Resuelta:'), maplist(writeln, R),
    writeln('Matriz con ceros (pistas):'), maplist(writeln, S).



% aqui estoy definiendo una regla de como tienen que ser sus filas y sus columnas
% Entradas: La matriz del sudoku y la lista del sudoku
% Salidas: Una matriz de 9x9 con valores totalmente asignados en cada una de sus posiciones
% Restricciones: No tiene
% Funcionamiento: En si lo que hace es crear una lista y asignar columnas, luego se pasa a una lista normal
% En donde se especifica que los valores solo podrán ser del 1 al 9 como maximo luego le aplicamos que todos los valores
% Cumplan con ser distintos y le aplicamos una transpuesta a la matriz por medio de las columnas y repetimos el proceso,
% Una vez terminado esto se pasa a revisar los bloques generados y por último asignamos valores a cada posición de la lista.

sudoku(MatrizSudoku, ListaSudoku) :- 
    length(MatrizSudoku, 9),
    maplist(same_length(MatrizSudoku), MatrizSudoku), 
    append(MatrizSudoku, ListaSudoku),
    ListaSudoku ins 1..9, 
    maplist(all_distinct, MatrizSudoku), 
    transpose(MatrizSudoku, Columns),
    maplist(all_distinct, Columns), 
    revisa_bloque(MatrizSudoku), 
    asignaValores(ListaSudoku).  


% Entradas: La matriz del sudoku 
% Salidas: Los valores distintos dentro de cada bloque de la matriz 9x9 y que estos sigan las reglas
% Restricciones: No tiene
% Funcionamiento: Primero extraera las primeras 3 filas dentro de la matriz, luego esta seguira en la recursion llamando
% ahora revisa_bloque_3x3 donde le pasamos la primera, segunda y tercera fila, una vez vuelve sigue continuando hasta que 
% el restante de la lista quede vacio como tal. 
revisa_bloque([]).
revisa_bloque([Row1, Row2, Row3 | Rest]) :- 
    revisa_bloque_3x3(Row1, Row2, Row3), 
    revisa_bloque(Rest).


% de una forma mas resumida esta va revisando la lista para que se cumpla que cada bloque de 3 filas y 3 columnas no repita valores

% Entradas: recibe tres listas 
% Salidas: el bloque de 3x3 revisado y funcional
% Restricciones: No tiene
% Funcionamiento: por cada fila revisa sus 3 columnas y luego se pide que estas sean distintas de las demás de forma que
% se respeta la regla de que cada valor sea distinto y que no pegue con sus filas y columnas aledañas, esto continua
% asi hasta que la matriz haya sido recorrida en su totalidad de forma que se asegure que el juego si es posible de jugar, por
% eso lo que hacemos es que en la recursión pasamos el resto de las filas.
revisa_bloque_3x3([], [], []).
revisa_bloque_3x3([A,B,C | F1], [D,E,F | F2], [G,H,I | F3]) :- 
    all_distinct([A,B,C,D,E,F,G,H,I]), 
    revisa_bloque_3x3(F1, F2, F3). 



% Entradas: Una lista 
% Salidas: La lista con valores del 1 al 9
% Restricciones: No tiene
% Funcionamiento: definimos una cabeza y una cola con la cual le decimos que encuentre todos los valores del 1 al 9 en la cabeza
% para lyego mezclarlos de forma aleatoria y luego los mandamos a revisar con el member para ver que si se cumpla 
% y por ultimo le asignamos el valor a la lista de forma que sus valores sean aleatorios

asignaValores([]). 
asignaValores([Var|ListaSudoku]) :-
    findall(Val, (between(1,9,Val)), Lista), 
    random_permutation(Lista, ListaAleatoria),
    member(Var, ListaAleatoria), 
    asignaValores(ListaSudoku).


% Entradas: Una variable donde se vaya a almacenar el número
% Salidas: Un número aleatorio entre 17 y 25
% Restricciones: No tiene
% Funcionamiento: Por medio del random_between le decimos que queremos un número entre 17 a 25 y que este se guarde en NumPistas

asignar_pistas(NumPistas) :-
    random_between(17, 25, NumPistas).


% Entradas: La lista de sudoku con ceros(o sea pistas) y la posicion 
% Salidas: un true hasta que encuentre la posicion correcta
% Restricciones: No tiene
% Funcionamiento: Esta busca un valor aleatorio entre 0 y 80 de forma que este se guarde en Pos
% una vez este se guarde se busca la posicion exacta de este con nth0 y se guarda en P1 luego con P1 lo que hacemos
% es compararlo para ver si es una posicion con cero sino seguimos buscando.
asignar_sugerencia(ListaSudokuConCeros, Pos) :-
    random_between(0, 80, Pos),
    nth0(Pos, ListaSudokuConCeros, P1),
    ( P1 =:= 0 -> true ; asignar_sugerencia(ListaSudokuConCeros, Pos) ).




% Entradas: Dos listas la de ceros y la que viene resuelta además de la posición
% Salidas: La lista cambiada con el valor de la posicion de la lista resuelta con la que se buscaba
% Restricciones: No tiene
% Funcionamiento: Aqui lo que hacemos es ir buscando por medio de la posicion hasta que nuestra posicion sea 1
% Si es un 1 esto le indica al programa que ya la encontramos y que puede sustituir su valor
% Sino esta continua hasta encontrar el valor de la sugerencia
generar_sugerencia([0|T], [H1|_], [H1|T], 1).
generar_sugerencia([X|T], [_|_], [X|T], 1) :- X \= 0.  
generar_sugerencia([H|T], [H1|T1], [H|T2], Posicion) :-
    Posicion > 1,
    Pos1 is Posicion - 1,
    generar_sugerencia(T, T1, T2, Pos1).


% Entradas: La posicion y la lista de posiciones guardadas
% Salidas: No tiene
% Restricciones: 
% Funcionamiento: Una vez creada la lista lo que hacemos es buscar valores aleatorios entre 0 a 8 para filas y columnas
% luego le hacemos un calculo para tenerlo en un valor común o sea para manejarlo en una lista  luego lo que hace es que si 
% esta se encuentra en la lista de usadas se sigue llamando hasta encontrar un valor que no este siendo utilizado. 
posicion_aleatoria(Pos) :-
    posicion_aleatoria(Pos, []).  

posicion_aleatoria(Pos, Usadas) :-
    random_between(0, 8, F),      
    random_between(0, 8, C),      
    Pos is F * 9 + C,             
    \+ member(Pos, Usadas).       


posicion_aleatoria(Pos, Usadas) :-
    posicion_aleatoria(Pos, Usadas).  



% Entradas: la lista vieja, la posicion, el valor y la nueva lista
% Salidas: El 0 en la posición indica
% Restricciones: No tiene
% Funcionamiento: Lo que hace es igualq que los demás vamos a ir buscando por la posicion pero esto con las listas
% con sus cabezas y colas de forma que hasta que encontremos la posicion que queremos vamos recorriendo la lista
% de forma que cuando encontremos correctamente la posicion la sustituiremos para asignar la pista.
actualizar_lista([_|T], 1, Nuevo, [Nuevo|T]).
actualizar_lista([H|T], Pos, Nuevo, [H|Resto]) :-
    Pos > 1,
    Pos1 is Pos - 1,
    actualizar_lista(T, Pos1, Nuevo, Resto).


% Entradas: Tendremos la cantidad de ceros, la lista, la lista de usadas y el resultado donde se guarda la lista modificada
% Salidas: La lista modificada con el valor del 0 asignado en su posición correctamente
% Restricciones: No tiene
% Funcionamiento: Primero inicializamos el predicado y pasamos la lista de posiciones usadas(vacia) de forma que 
%  luego lo que haremos es que buscaremos las posiciones aleatorias donde pondremos los ceros y los iremos reemplazando
%  constantemente de forma que una vez insertado se va ir reduciendo el numero de ceros a insertar hasta que este llegue a ser cero


insertar_pistas(N, Lista, Resultado) :-
    insertar_pistas(N, Lista, [], Resultado). 

insertar_pistas(N, Lista, Usadas, Resultado) :-
    ( N = 0 -> Resultado = Lista
    ; posicion_aleatoria(Pos, Usadas),
      actualizar_lista(Lista, Pos+1, 0, ListaActualizada),
      N1 is N - 1,
      insertar_pistas(N1, ListaActualizada, [Pos|Usadas], Resultado)
    ).

% Entradas: dos listas
% Salidas: Una matriz
% Restricciones: Si la lista es vacia devolvemos vacio, y que la fila tenga 9 elementos
% Funcionamiento: Le generamos un largo de 9 y luego dividimos la lista donde en fila tendremos los primeros
% 9 valores y luego pues el resto o sea las columnas y entonces los vamos agregando de forma que se va armando la lista
%  de listas por medio de la recursion.

crear_matriz([], []).
crear_matriz(Lista, [Fila|Resto]) :-
    length(Fila, 9),
    append(Fila, RestoLista, Lista),
    crear_matriz(RestoLista, Resto).

% Entradas: La matriz con ceros y la matriz resuelta o sea donde la vamos a guardar
% Salidas: Una matriz con pistas 
% Restricciones: No tiene
% Funcionamiento: Lo que hacemos es crear un sudoku y este le asignamos pistas, una vez hecho esto las insertamos
% y estas se guardan en ListaSudokuConCeros para asi luego crear la matriz y devolverla con el sudoku con ceros

sudoku_con_pistas(MatrizSudokuConCeros, MatrizResuelta) :-
    sudoku(MatrizResuelta, ListaSudoku),
    asignar_pistas(N),
    writeln('Número de pistas generadas (N):'), writeln(N),
    insertar_pistas(N, ListaSudoku, ListaSudokuConCeros),
    crear_matriz(ListaSudokuConCeros, MatrizSudokuConCeros).



% Entradas: la matriz con ceros, la matriz resuelta, la matriz actualizada, y donde se guarda
% Salidas: La sugerencia en el sudoku 
% Restricciones: No tiene
% Funcionamiento: Lo que hacemos es aplanar la matriz o sea la pasamos a ser lista, luego llamamos a asignar_sugerencia
% donde luego usaremos otra variable para asignar la posicion en generar_sugerencia y por ultimo ya hecha la sugerencia
% creamos la matriz y la guardamos. 
hacer_sugerencia(MatrizSudokuConCeros, MatrizSudokuSolucion, MatrizSudokuActualizado, MatrizSudokuActualizadoDividido) :-
    append(MatrizSudokuConCeros, ListaSudokuConCeros),  % Aplanar la matriz
    append(MatrizSudokuSolucion, ListaSudokuSolucion), % Aplanar la solución
    asignar_sugerencia(ListaSudokuConCeros, Pos),
    Pos1 is Pos + 1,
    generar_sugerencia(ListaSudokuConCeros, ListaSudokuSolucion, ListaSudokuActualizado, Pos1),
    crear_matriz(ListaSudokuActualizado, MatrizSudokuActualizadoDividido),
    MatrizSudokuActualizado = MatrizSudokuActualizadoDividido.




% Entradas: dos listas
% Salidas: Un true si cumplen las mismas posiciones
% Restricciones: Si las dos son iguales eso es un True 
% Funcionamiento: Lo que hace es ir comparando posición por posición de forma que revise si es verdad que estas son iguales
% así hasta que acabe.
revisa_matriz([], []).
revisa_matriz([H|T], [He|Ta]) :-
    H =:= He,              
    revisa_matriz(T, Ta).  



% Entradas: la fila, columna y la variable donde se va a guardar la posicion
% Salidas: Una posicion calculada
% Restricciones: No tiene
% Funcionamiento: Calculamos la fila por 9 y le sumamos la columna para tener el valor en una posición de una lista común
recibe_posicion(F,C, PosicionU):-
    PosicionU is F * 9 + C.


% Entradas: Una matriz, una fila, una columna, el valor a cambiar, y la variable donde se guarda si se puede insertar
% Salidas: Un True si se puede y sino un false
% Restricciones: Que la fila, la columna y el valor esten entre 1 y 9
% Funcionamiento: verificamos los valores, hacemos una lista plana, calculamos la posicion
% revisamos si en la lista esa posicion es 0 si la posicion lo es devolvemos un true sino un false


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


% Entradas: una lista, fila, columna, valor a cambiar y la lista donde se va a guardar el resultado
% Salidas: El valor cambiado en la lista y posicion pedida
% Restricciones: Revisa si la fila y columna tienen valores de 1 a 9
% Funcionamiento: Revisamos las filas y columnas, calculamos la posicion y llamamos a intercambio_valor para hacer la asignacion
% correspondiente.
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


% Entradas: una lista y una matriz del sudoku con ceros
% Salidas: Una matriz con pistas
% Funcionamiento: Lllamos a asignar pistas, las insertamos en la lista y las guardamos en la matriz de sudoku cojceros
% Restricciones: No aplica
pistas_en_matriz(ListaSudoku, MatrizSudokuConCeros) :-
    asignar_pistas(N),
    insertar_pistas(N, ListaSudoku, ListaSudokuConCeros),
    crear_matriz(ListaSudokuConCeros, MatrizSudokuConCeros).

% Entradas: fila, columna, valor, lista con ceros, y la variable o sea el resultado
% Salidas: un True si se puede o un False si no se puede
% Funcionamiento: calculamos un indice o sea la posicion revisamos la posicion y si es un 0 eso quiere decir
% que si se pude ingresar un valor en la posicion y devolvemos un true si no un false.
% Restricciones: Revisamos que los valores esten entre 1 y 9 en fila, columna y valor
verifica_posicion(Fila, Col, Valor, ListaConCeros, Variable) :-
    between(1, 9, Fila),
    between(1, 9, Col),
    between(1, 9, Valor),
    Indice is (Fila - 1) * 9 + (Col - 1),
    nth0(Indice, ListaConCeros, PosicionValo),
    (PosicionValo =:= 0 -> Variable = true ; Variable = false).



% Entradas: dos listas y un contador
% Salidas: La cantidad de erorres
% Funcionamiento: Comparamos elemento a elemento para ver si son distintos(Solo no cuenta si un valor es 0) de esta forma si son
% distintos vamos aumentando el contador y al final lo devolvemos
% Restricciones: si las dos son vacias devolvemos 0 errores 
cantidad_errores([], [], 0).
cantidad_errores([CZ|TL], [CZ2|TL2], C) :-
    ( CZ2 =\= 0, CZ2 =\= CZ ->
        cantidad_errores(TL, TL2, C1),
        C is C1 + 1
    ;
        cantidad_errores(TL, TL2, C)
    ).

% Entradas: una lista y un contador
% Salidas: La cantidad de posiciones vacias
% Funcionamiento: si el valor es un cero entonces hacemos la recursion y agregamos un 1 en el contador
% Restricciones: si la lista es vacia devolvemos un cero
cantidad_vacios([], 0).
cantidad_vacios([CZ|TL], C) :-
    ( CZ =:= 0 ->
        cantidad_vacios(TL, C1),
        C is C1 + 1
    ;
        cantidad_vacios(TL, C)
    ).

% Entradas:La cantidad de erorres, cantidad de vacios y el valor del resultado
% Salidas: un True si los errores y vacios son 0 sino un false
% Funcionamiento: Comparamos si los errores y los vacios son iguales a cero si es asi a Valor le damos un True sino un false
% Restricciones:No aplica
final_sudoku(CantidadErrores, CantidadVacios, Valor) :-
    (CantidadErrores =:= 0, CantidadVacios =:= 0 ->
        Valor = true
    ;
        Valor = false
    ).

% Entradas:La lista original, la lista de ceros, dos contadores uno para errores y otro para vacios y por ultimo una variable
% Salidas: los valors de los contadores y el valor de la variable
% Funcionamiento: Aqui en general aplicamos contar errores, contar vacios y los comparamos de una vez para ver si termino el juego
% cuando hace la comparación al final guarda los valores en las variables
% Restricciones:No aplica
juego_final(ListaOriginal, ListaConCeros, C, C1, Valor) :-
    cantidad_errores(ListaOriginal, ListaConCeros, C),
    cantidad_vacios(ListaConCeros, C1),
    final_sudoku(C, C1, Valor).

