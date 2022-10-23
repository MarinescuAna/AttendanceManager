import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import axios from "axios";
import storeHelper from './store/store-helper'
import VueCookies from 'vue-cookies';
import { ValidationProvider } from 'vee-validate/dist/vee-validate.full.esm';
import {ValidationObserver} from 'vee-validate'

Vue.component('ValidationProvider', ValidationProvider);
Vue.component('ValidationObserver', ValidationObserver);

Vue.config.productionTip = false

Vue.use(VueCookies);



axios.defaults.baseURL='https://localhost:44347/api/'

storeHelper.init(store);

new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
