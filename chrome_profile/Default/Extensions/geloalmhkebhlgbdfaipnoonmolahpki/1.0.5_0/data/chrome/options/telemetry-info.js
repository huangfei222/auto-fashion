// Remember that the popup was shown to user
localStorage.setItem('socialbee.op.firefox-disable-data-collection', 'null');

$(document).on('ready', function() {
  $('.buttons-container button').on('click', function(e) {
    var $button = $(e.target);
    var choice = $button.attr('data-choice');
    var settingValue = choice === 'enable' ? 'no' : 'yes';

    localStorage.setItem('socialbee.op.firefox-disable-data-collection', settingValue);
    window.close();
  });
});
