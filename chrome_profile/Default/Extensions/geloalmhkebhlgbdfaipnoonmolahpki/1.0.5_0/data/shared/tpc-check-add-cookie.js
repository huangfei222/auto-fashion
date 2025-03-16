/* globals bufferpm */

document.cookie = 'test';

if (!document.cookie) {
  bufferpm({
    target: window.parent,
    type: "socialbee_3pc_disabled",
  });
}

bufferpm({
  target: window.parent,
  type: "socialbee_3pc_done",
});
