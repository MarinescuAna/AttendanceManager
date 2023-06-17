import Vue from 'vue';
import Vuetify from 'vuetify/lib/framework';
import colors from 'vuetify/lib/util/colors';

Vue.use(Vuetify);

export default new Vuetify({
  icons: {
    iconfont: 'mdi',
  },
  theme: {
    themes: {
      light: {
        orange_background:colors.orange.lighten3,
        blue_grey:colors.blueGrey.lighten4, 
        dark_button: '#000000',
        light_button: colors.orange.lighten3
      },
    },
  },
});
