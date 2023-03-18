import Vue from 'vue'
import App from './App.vue'
import router from './router'
import store from './store'
import vuetify from './plugins/vuetify'
import storeHelper from './store/store-helper'
import VueCookies from 'vue-cookies';
import { ValidationProvider } from 'vee-validate/dist/vee-validate.full.esm';
import {ValidationObserver} from 'vee-validate'
import "@/plugins/vue-toastification";

/**
 * This is used for validations reasons.
 */
Vue.component('ValidationProvider', ValidationProvider);
Vue.component('ValidationObserver', ValidationObserver);

Vue.config.productionTip = false

/**
 * Create a component that is entirely decoupled from the DOM or the rest of the app.
 * All that exists on it are its instance methods.
 * This allow the communications between non-parent components
 */
export const EventBus = new Vue();

/**
 * Allow using cookies for storing the access and refresh tokens
 */
Vue.use(VueCookies,{expires:"7d"});

/**
 * Store initialization
 */
storeHelper.init(store);

/**
 * Other plugins
 */
new Vue({
  router,
  store,
  vuetify,
  render: h => h(App)
}).$mount('#app')
