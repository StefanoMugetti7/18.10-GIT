function registerLink(obj) {
    var link = document.createElement('link');
    link.href = obj.href;
    link.rel = obj.rel;
    link.type = obj.type;
    document.getElementsByTagName('head')[0].appendChild(link);
}

Sys.Application.notifyScriptLoaded();