function lbxBorrarItem(listaOpciones, listaSelecciones) {
    var availableList = document.getElementById(listaOpciones);
    var selectedList = document.getElementById(listaSelecciones);
    var selIndex = selectedList.selectedIndex;
    if (selIndex < 0)
        return;
    availableList.appendChild(
      selectedList.options.item(selIndex))
    selectNone(selectedList, availableList);
    setSize(availableList, selectedList);
}

function lbxAgregarItem(listaOpciones, listaSelecciones) {
    var availableList = document.getElementById(listaOpciones);
    var selectedList = document.getElementById(listaSelecciones);
    var addIndex = availableList.selectedIndex;
    if (addIndex < 0)
        return;
    selectedList.appendChild(
      availableList.options.item(addIndex));
    selectNone(selectedList, availableList);
    setSize(selectedList, availableList);
}

function lbxBorrarTodos(listaOpciones, listaSelecciones) {
    var availableList = document.getElementById(listaOpciones);
    var selectedList = document.getElementById(listaSelecciones);
    var len = selectedList.length - 1;
    for (i = len; i >= 0; i--) {
        availableList.appendChild(selectedList.item(i));
    }
    selectNone(selectedList, availableList);
    setSize(selectedList, availableList);

}

function lbxAgregarTodos(listaOpciones, listaSelecciones) {
    var availableList = document.getElementById(listaOpciones);
    var selectedList = document.getElementById(listaSelecciones);
    var len = availableList.length - 1;
    for (i = len; i >= 0; i--) {
        selectedList.appendChild(availableList.item(i));
    }
    selectNone(selectedList, availableList);
    setSize(selectedList, availableList);

}

function setSize(list1, list2) {
    list1.size = getSize(list1);
    list2.size = getSize(list2);
}

function selectNone(list1, list2) {
    list1.selectedIndex = -1;
    list2.selectedIndex = -1;
    addIndex = -1;
    selIndex = -1;
}

function getSize(list) {
    /* Mozilla ignores whitespace, 
    IE doesn't - count the elements 
    in the list */
    var len = list.childNodes.length;
    var nsLen = 0;
    //nodeType returns 1 for elements
    for (i = 0; i < len; i++) {
        if (list.childNodes.item(i).nodeType == 1)
            nsLen++;
    }
    if (nsLen < 2)
        return 2;
    else
        return nsLen;
}

