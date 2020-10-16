cordova.define('cordova/plugin_list', function(require, exports, module) {
  module.exports = [
    {
      "id": "cordova-plugin-device.device",
      "file": "plugins/cordova-plugin-device/www/device.js",
      "pluginId": "cordova-plugin-device",
      "clobbers": [
        "device"
      ]
    },
    {
      "id": "cordova-plugin-oauth.OAuth",
      "file": "plugins/cordova-plugin-oauth/www/oauth.js",
      "pluginId": "cordova-plugin-oauth",
      "clobbers": [
        "open"
      ]
    }
  ];
  module.exports.metadata = {
    "cordova-plugin-device": "2.0.3",
    "cordova-plugin-whitelist": "1.3.4",
    "cordova-plugin-oauth": "2.0.0"
  };
});