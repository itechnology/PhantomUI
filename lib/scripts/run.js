var page   = require('webpage').create(),
    system = require('system'),
    address, path;

if (system.args.length !== 3) {
    console.log(system.args);
    phantom.exit(1);
}
else {
    // wire up console.log support :: http://phantomjs.org/api/webpage/handler/on-console-message.html
    page.onConsoleMessage = function (msg) {
        console.log(msg);
    };

    address = system.args[1];
    path    = system.args[2];

    // this is the most common viewport size
    page.viewportSize = { width: 1366, height: 768 };

    // do stuff
    page.open(address, function (status) {
        page.evaluate(function () {
            // if no color is set in the page, set it to white
            if (document.body.bgColor === "") {
                document.body.bgColor = "#FFFFFF";
            }
        });

        page.render(path);
        phantom.exit(0);
    });
}