var page   = require('webpage').create(),
    system = require('system'),
    address;

if (system.args.length !== 3) {
    console.log(system.args);
    phantom.exit(1);
}
else {
    address = system.args[1];
    path    = system.args[2];
    page.viewportSize = { width: 1366, height: 768 };
    page.open(address, function (status) {
        page.render(path);
        phantom.exit(0);
    });
}