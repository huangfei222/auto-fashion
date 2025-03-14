console.log("background script loaded");
chrome.runtime.onInstalled.addListener(function(details) {
  if (details.reason === "install") {
    chrome.tabs.create({
      url: "https://image-upscaler.pro/welcome"
    });
  }
  chrome.contextMenus.create({
    id: "image-upscaler",
    title: "Image Upscaler",
    contexts: ["image"]
  });
});
chrome.action.onClicked.addListener(function() {
  chrome.tabs.create({
    url: "https://image-upscaler.pro/upscale"
  });
});
chrome.runtime.setUninstallURL("https://image-upscaler.pro/uninstall");
chrome.contextMenus.onClicked.addListener((info, tab) => {
  if (info.menuItemId === "image-upscaler") {
    chrome.tabs.create({
      url: `https://image-upscaler.pro/upscale?source=${info.srcUrl}`
    });
  }
});
