/* globals key */

// requires keymaster.js

;(function () {
    // Wait for xt.options to be set
  ;(function check() {
    // If hotkey is switched on, add the buttons
    if( xt.options && xt.options['socialbee.op.key-enable'] === 'key-enable') {
      key(xt.options['socialbee.op.key-combo'], function () {
        var usesDefaultShortcut = xt.options['socialbee.op.key-combo'] === 'alt+b';
        var isShortcutPressedInSocialBeeApp = /https?:\/\/app.socialbee.io/.test(location.href);
        if (usesDefaultShortcut && isShortcutPressedInSocialBeeApp) {
          return false; // SocialBee.io already offers the alt+b shortcut to open the in-app composer
        }

        xt.port.emit("socialbee_click", {placement: 'hotkey'});
        return false;
      });
    } else {
      setTimeout(check, 50);
    }
  }());
}());
