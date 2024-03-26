grammar Constraint;

expr
    :   implyExpr
    ;

implyExpr
    :   orExpr
    |   leftOp=orExpr '->' rightOp=orExpr
    ;
    
orExpr
    :   andExpr
    |   leftOp=andExpr '|' rightOp=andExpr
    ;
    
andExpr
    :   primaryExpr
    |   leftOp=primaryExpr '&' rightOp=primaryExpr
    ;
    
primaryExpr
    :   trueExpr
    |   falseExpr
    |   parenExpr
    |   notParenExpr
    |   statement
    ;
    
trueExpr
    :   'true'
    ;
    
falseExpr
    :   'false'
    ;
    
parenExpr
    :   '(' expr ')'
    ;
    
notParenExpr
    :   '!(' expr ')'
    ;
    
statement
    :   eqValStatement
    |   notEqValStatement
    |   inListStatement
    |   notInListStatement
    ;
    
eqValStatement
    :   key=STR '=' value=val
    ;
    
notEqValStatement
    :   key=STR '!=' value=val
    ;
    
inListStatement
    :   key=STR '=' list=lst
    ;
    
notInListStatement
    :   key=STR '!=' list=lst
    ;

lst
    :   '[' values+=val (',' values+=val)* ']'
    ;

val
    :   strVal
    |   nullVal
    ;

strVal
    :   value=STR
    ;
    
nullVal
    :   NULL
    ;

// Lexer

NULL:   'null';
STR: [a-zA-Z0-9-_]+;
WS: [ \t\n\r]+ -> skip;