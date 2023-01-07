const { defineConfig } = require('@vue/cli-service')
module.exports = defineConfig({
  /**
   * Prevent preloading the components
   */
  chainWebpack:(config) =>{
    config.plugins.delete('prefetch')
  },
  transpileDependencies: [
    'vuetify'
  ]
})
