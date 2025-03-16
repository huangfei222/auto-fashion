/* globals bufferpm, chrome, safari */

// Put together a query string for the iframe
var buildSrc = function(data, config) {

  var src = data.local ?
    config.overlay.localendpoint :
    config.overlay.endpoint;

  var qs = '';

  config.attributes.forEach(function(attr, i){
    if ( !data[ attr.name ] ) return;
    if ( qs.length ) qs += '&';
    qs += attr.name + '=' + attr.encode( data[attr.name] );
  });

  if (qs.length) src += '?' + qs;

  return src;
};

var openPopUp = function(src, port, doneCallback, isSmallPopup) {

  // Open popups from privileged code
  port.emit('socialbee_open_popup', { src: src, isSmallPopup: isSmallPopup });

  // Bind close listener
  // Listen for when the overlay has closed itself
  bufferpm.bind("buffermessage", function(overlaydata) {
    bufferpm.unbind("buffermessage");
    setTimeout(function () {
      doneCallback(overlaydata);
    }, 0);
    window.focus();
  });
};

var shouldOpenSmallPopupForUrl = function(url) {
  var hostname = window.location.hostname;
  var pathname = window.location.pathname;

  var isTwitterIntent = /twitter.com$/.test(hostname) && /^\/intent/.test(pathname);

  if (isTwitterIntent) return true;
  else return false;
}

// Build that overlay!
// Triggered by code working from the button up
var socialBeeOverlay = function(data, config, port, doneCallback) {

  if( ! doneCallback ) doneCallback = function () {};
  if( ! config ) return;

  portCache.grabPort(port);

  var src = buildSrc(data, config);
  var domain = window.location.hostname;

  if (shouldOpenSmallPopupForUrl(window.location.href)) {
    openPopUp(src, port, doneCallback, true);
    return;
  }

  /*if (xt.options['socialbee.op.tpc-disabled'] || xt.options['socialbee.op.open-as'] === 'new-tab') {
    openPopUp(src, port, doneCallback);
    return;
  }*/

  var shouldContinue = ensureOnlyOneOverlayOpen(data, closePopup.bind(window, document, doneCallback));
  if (!shouldContinue) return;

  port.emit('socialbee_overlay_open');

  // Create the iframe and add the footer:
  var iframe = document.createElement('iframe');

  iframe.allowtransparency = 'true';
  iframe.scrolling = 'no';
  iframe.id = 'socialbee_overlay';
  iframe.name = 'socialbee_overlay';
  iframe.style.cssText = config.overlay.getCSS();
  iframe.src = xt.data.get('data/shared/socialbee-frame-container.html');

  iframe.addEventListener('load', function() {
    iframe.contentWindow.postMessage({
      src: src,
      css: config.overlay.getCSS(true),
    }, '*');
  });

  var rightCnt = createBtnContainer('right');
  //var helpButton = createHelpButton();
  var dashboardButton = createDashboardButton();

  //rightCnt.appendChild(helpButton);
  rightCnt.appendChild(dashboardButton);

  var leftCnt = createBtnContainer('left');
  var cancelButton = createCancelButton();

  //leftCnt.appendChild(cancelButton);

  document.body.appendChild(iframe);
  document.body.appendChild(rightCnt);
  document.body.appendChild(leftCnt);

  $(document).on('click', '.socialbee-btn-cancel', function() {
      closePopup(document, doneCallback);
  });

  // Bind close listener
  // Listen for when the overlay has closed itself
  bufferpm.bind('buffermessage', function(overlaydata) {
      closePopup(document, doneCallback, overlaydata);
  });

  /**
   * Listen to ESC key and close the popup when hit.
   */
  $(document).on('keyup.socialBeeOverlay', function(e) {
    if (e.keyCode == 27) {
      // When an overlay instance is hidden (but still open), don't let shortcuts close it
      if (!ensureOnlyOneOverlayOpen.isOverlayVisible()) return;

      closePopup(document, doneCallback);
    }
  });

  // Remove the loading image when we hear from the other side
  bufferpm.bind('socialbee_loaded', function(data) {
    // Send user data to background script for caching
    if (data && data.userData) port.emit('socialbee_user_data', data.userData);

    bufferpm.unbind('socialbee_loaded');
    iframe.style.cssText += 'background-image: none !important';

    $(".socialbee-btn-cancel").remove();
  });
};

// Returns true if a new overlay should be open, false if we've toggled the visibility of
// and existing overlay instead.
function ensureOnlyOneOverlayOpen(data, closePopup) {
  // State can't be saved in this script, since it gets re-executed multiple times by some browsers
  // (e.g. Firefox), so we rely on the DOM instead.
  var isOverlayOpen = function() { return !!$('#socialbee_overlay').length };

  // If the open intent comes from the Buffer toolbar button or a keyboard shortcut, toggle the
  // visibility of the overlay if it's already open, otherwise allow a new one to be open.
  if (data.placement == 'toolbar' || data.placement == 'hotkey') {
    if (!isOverlayOpen()) return true;

    $('#socialbee_overlay, .socialbee-btn-container').toggle();
    return false;
  }

  // If the open intent comes from somewhere else, discard any hidden overlay and open a new one
  closePopup();

  return true;
};

ensureOnlyOneOverlayOpen.isOverlayVisible = function() { return $('#socialbee_overlay').is(':visible') };

function closePopup(document, doneCallback, overlayData) {
    $('#socialbee_overlay, .socialbee-btn-container').remove();

    bufferpm.unbind('buffermessage');
    bufferpm.unbind('socialbee_addbutton');

    $(document).off('keyup.socialBeeOverlay');

    setTimeout(function () {
      doneCallback(overlayData);
    }, 0);

    window.focus();
}

// position = 'left' || 'right'
var createBtnContainer = function(position) {
  var container = document.createElement('div');
  container.setAttribute('class', 'socialbee-btn-container socialbee-btn-container-' + position);

  return container;
};

var createHelpButton = function() {
  var contactHref = 'https://help.socialbee.com';
  var faqHref = 'https://help.socialbee.com/';

  var button = document.createElement('a');
  button.href = contactHref;
  button.target = '_blank';
  button.setAttribute('data-faq-href', faqHref);
  button.setAttribute('class', 'socialbee-btn-help');

  var icon = document.createElement('span');
  icon.classList.add('icon');
  var iconUrl = xt.data.get('data/shared/img/buffer-help-button-icon@2x.png');
  icon.style.cssText = 'background-image: url(' + iconUrl + ') !important;';
  button.appendChild(icon);

  var text = document.createTextNode('Help');
  button.appendChild(text);

  button.addEventListener('click', function() {
    _bmq.trackAction(['overlay', 'help_button'], {
      button_action: button.href === contactHref ? 'contact' : 'faq',
    });
  }, false);

  return button;
};

var createDashboardButton = function() {
  var button = document.createElement('a');
  button.href = 'https://app.socialbee.com';
  button.target = '_blank';
  button.setAttribute('class', 'socialbee-btn-dashboard');

  var icon = document.createElement('span');
  icon.classList.add('icon');
  var iconUrl = xt.data.get('data/shared/img/logo.png');
  icon.style.cssText = 'background-image: url(' + iconUrl + ') !important;';
  button.appendChild(icon);

  var text = document.createTextNode('Go to SocialBee');
  button.appendChild(text);

  return button;
};

var createCancelButton = function() {

  var button = document.createElement('button');
  button.setAttribute('class', 'socialbee-btn-cancel');

  var text = document.createTextNode('Cancel');
  button.appendChild(text);

  return button;
};

var createUpgradeButton = function() {
  var button = document.createElement('a');
  // TODO: change to .com
  button.href = 'https://app.socialbee.io/license?utm_campaign=extensions_header&utm_medium=web';
  button.target = '_blank';
  button.setAttribute('class', 'socialbee-btn-upgrade hidden');

  var icon = document.createElement('span');
  icon.classList.add('icon');
  var iconUrl = xt.data.get('data/shared/img/buffer-awesome-button-icon@2x.png');
  icon.style.cssText = 'background-image: url(' + iconUrl + ') !important;';
  button.appendChild(icon);

  var text = document.createTextNode('Upgrade to Awesome');
  button.appendChild(text);

  return button;
};

// getOverlayConfig returns the configuration object for use by socialBeeOverlay
var getOverlayConfig = function(postData){

  var config = {};

  // Set this to true for using a local server while testing
  config.local = false;

  var segments = window.location.pathname.split('/');

  config.pocketWeb = ( window.location.host.indexOf('getpocket') !== -1 && segments[2] === 'read' );
  config.isYoutubeVideo = ( window.location.host.indexOf('.youtube.com') !== -1 && segments[1] === 'watch' );
  config.isImgur = ( window.location.host.indexOf('imgur.com') !== -1 && segments[1] === 'gallery' );

  // Specification for gathering data for the overlay
  config.attributes = [
    {
      name: "url",
      get: function (cb) {
        if(config.pocketWeb){
          var li = document.getElementsByClassName('original')[0];
          var link = li.getElementsByTagName('a')[0].href;
          cb(link);
        }
        else{
          cb(window.location.href);
        }
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "text",
      get: function (cb) {
        var selectedText = document.getSelection().toString();
        var quoteChars;

        if (selectedText) {
          // If quotes surround the selected text, strip them away
          quoteChars = ['"', '“', '”', '\'', '‘', '’', '«', '»'];
          if (quoteChars.indexOf(selectedText[0]) != -1 && quoteChars.indexOf(selectedText[selectedText.length - 1]) != -1) {
            selectedText = selectedText.slice(1, selectedText.length - 1);
          }

          return cb('“' + selectedText + '”');
        }

        if (config.pocketWeb){
          var headline = document.querySelectorAll('.reader_head h1')[0];
          var title = headline && headline.textContent;
          return cb(title);
        }

        // In some situations (e.g. YT's autoplay feature), the page is updated via xhr, the
        // title attribute too, but not the og:title meta tag: default to retrieving the title
        // of the page from the title attr for YouTube videos, and format it a bit
        if (config.isYoutubeVideo) {
          var title = document.title.replace('- YouTube', '');
          return cb(title);
        }

        // Similarly to YouTube above, in some situations (e.g. navigating from one picture to
        // another), the page is updated via xhr, the title attribute too, but not the
        // og:title meta tag: let's default to retrieving the title of the page from the title attr
        // for YouTube videos, and format it a bit
        if (config.isImgur) {
          var title = document.title.replace('- Imgur', '');
          return cb(title);
        }

        var ogTitle = document.head && document.head.querySelector('meta[property="og:title"]');
        if (ogTitle && ogTitle.content && ogTitle.content.length) {
          return cb(ogTitle.content);
        }

        cb(document.title);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "retweeted_tweet_id",
      get: function (cb) {
        cb(postData.retweeted_tweet_id);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "retweeted_user_id",
      get: function (cb) {
        cb(postData.retweeted_user_id);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "retweeted_user_name",
      get: function (cb) {
        cb(postData.retweeted_user_name);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "retweeted_user_display_name",
      get: function (cb) {
        cb(postData.retweeted_user_display_name);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "retweet_comment",
      get: function (cb) {
        cb(postData.retweet_comment);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "picture",
      get: function (cb) {
        cb(postData.image);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "embed",
      get: function (cb) {
        cb(postData.embed);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "local",
      get: function (cb) {
        cb(config.local);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "version",
      get: function (cb) {
        cb(postData.version);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "placement",
      get: function (cb) {
        if( postData.placement ) cb(postData.placement);
        else cb('general');
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    },
    {
      name: "disable-telemetry",
      get: function (cb) {
        cb(postData['disable-telemetry']);
      },
      encode: function (val) {
        return encodeURIComponent(val);
      }
    }
  ];

  var loadingImg = xt.data.get('data/shared/img/loader.gif');
  config.overlay = {
    // TODO: change to .com
    endpoint: "https://app.socialbee.io/add/",
    localendpoint: "https://dev6.app.socialbee.io/add/",
    getCSS: function (forDeepestFrame) {
      return [
        'border: none !important;',
        'height: 100% !important;',
        'width: 100% !important;',
        forDeepestFrame ? '' : 'position: fixed !important;',
        'z-index: 999999999999 !important;',
        'top:0 !important;',
        'left:0 !important;',
        'display: block !important;',
        'max-width: 100% !important;',
        'max-height: 100% !important;',
        'padding: 0 !important;',
        forDeepestFrame ? '' : 'background: rgba(51, 51, 51, 0.6) url(' + loadingImg +') no-repeat center center !important;',
        forDeepestFrame ? '' : 'background-size: 80px !important;'
      ].join('');
    }
  };

  return config;
};

// Method for handling the async firing of the cb
var executeAfter = function(done, count, args, cb) {
  if(done === count) {
    setTimeout(function(){
      cb.apply( null, args );
    }, 0);
  }
};


// Asynchronously gather data about the page and from embedded sources,
// like Twitter or Facebook. Currently the async is a bit over the top,
// and not used, but if we need async down the line, it's there.
var getData = function (postData, cb) {
  var config = getOverlayConfig( postData );
  var count = config.attributes.length;
  var done = 0;
  var data = {};

  config.attributes.forEach(function(attr, i){
    attr.get(function(d){
      done += 1;
      data[ attr.name ] = d;
      executeAfter(done, count, [ data, config ], cb);
    });
  });
};



// bufferData is triggered by the socialbee_click listener in
// the buffer-browser-embed file, where it's passed a port
// to communicate with the extension and data sent from the
// background page.
var socialbeeData = function (port, postData) {

  if (window.top !== window) return;

  // Transform the data somewhat, and then create an overlay.
  // When it's done, fire socialbee_done back to the extension
  var createOverlay = function (data, config) {
    if( data.embed ) {
      if( typeof data.embed === "object" ) {
        for( var i in data.embed ) {
          if( data.embed.hasOwnProperty(i) ) {
            data[i] = data.embed[i];
          }
        }
        if( data.embed.text && !data.embed.url ) {
          data.url = null;
        }
        data.embed = null;
      } else {
        data.text = data.embed;
        data.url = null;
        data.embed = null;
      }
    }
    socialBeeOverlay(data, config, port, function (overlaydata) {
      port.emit("socialbee_done", overlaydata);
    });
  };

  // It all starts here.
  // createOverlay is the callback that should fire after getData has
  // gathered all the necessaries
  getData(postData, createOverlay);
};

// Cache for the port to avoid passing it around in function calls
var portCache = (function() {
  var _port;

  var exposed = {
    // Executed at the very beginning of socialBeeOverlay()'s execution to
    // update the cached port
    grabPort: function(port) {
      _port = port;
    },

    getPort: function() { return _port }
  };

  return exposed;
})();

// _bmq exposes the same API here as it does in background scripts, but
// here it only takes care of passing this data to the background script's
// _bmq where it will be effectively taken care of.
var _bmq = (function() {
  var _availableMethods = ['push', 'trackAction'];

  var _passForward = function(methodName) {
    var payload = {
      methodName: methodName,
      args: Array.prototype.slice.call(arguments, 1)
    };

    portCache.getPort().emit('buffer_tracking', payload);
  };

  var exposed = {};

  // Expose all _availableMethods
  _availableMethods.forEach(function(methodName) {
    exposed[methodName] = _passForward.bind(null, methodName);
  });

  return exposed;
}());

// Get some user data asynchronously; the callback will be run once immediately if such data has already
// been cached by the background script, and once shortly after displaying the overlay when the cache
// has been refreshed from the user data in the iframe
getExtensionUserData = function(cb) {
  portCache.getPort().on('socialbee_user_data', cb);
};

// On buffer.com/add, listen to messages coming from the page straight away: don't
// wait for the overlay being open, since we're already on the share dialog's new tab.
var isBufferShareTab = (
  document.location.href.indexOf('https://app.socialbee.io/add') === 0 ||
  document.location.href.indexOf('https://dev6.app.socialbee.io/add') === 0
);

if (isBufferShareTab) {
  bufferpm.bind('buffermessage', function() {
    // Close popup from privileged code
    if (window.chrome) chrome.runtime.sendMessage({ type: 'socialbee_close_popup' });
  });
}
