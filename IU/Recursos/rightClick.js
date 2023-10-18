//******************************************************************************
// Module  :   rightClick.js
//******************************************************************************
// DISCLAIMER: This Application is provide on AS IS basis without any warranty
//******************************************************************************

var BM = 2; // button middle
var BR = 3; // button right
var msg ="MOUSE CLICK DERECHO NO ESTA HABILITADO EN LA APLICACION";

function mouseDown(e) 
{ 
  try { if (event.button==BM||event.button==BR) {return false;} }  
  catch (e) { if (e.which == BR) {return false;} } 
}
document.oncontextmenu = function() { alert(msg); return false; }
document.onmousedown   = mouseDown;
//******************************************************************************